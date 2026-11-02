using System.Globalization;
using System.Text.Json;

namespace RBSoftSkkm;

public sealed partial class SkkmConnector
{
    /// <summary>
    /// Данные ККТ: Device (GetKktInfo), DeviceInfo / deviceInfo (документы, смены, чеки).
    /// </summary>
    private void ReadDevice(JsonElement d)
    {
        if (!d.Exists())
            return;
        DeviceModel = d.Str("Model");
        DeviceSerialNumber = d.Str("SerialNumber");
        DeviceFirmwareVersion = d.Str("FirmwareVersion");
        DeviceConfigurationVersion = d.Str("ConfigurationVersion");
        DeviceFfdVersion = d.Str("FfdVersion");
        DeviceFnFfdVersion = d.Str("FnFfdVersion");
        DeviceClass = d.Enum<DeviceClass>("DeviceClass");
        DeviceTimeZone = d.Enum<CheckTimeZone>("TimeZone");
        DeviceIsFiscal = d.Bool("IsFiscal");
        KktLicenses = d.Get("KktLicenses").Items().Select(l => new KktLicense
        {
            Number = l.Int("Number"),
            Name = l.Str("Name"),
            ValidFrom = l.Date("ValidFrom"),
            ValidUntil = l.Date("ValidUntil"),
            UnitVersion = l.Str("UnitVersion"),
            Description = l.Str("Description"),
            IsActive = l.Bool("IsActive"),
        }).ToArray();

        // Ширину строки в GetKktInfo присылает и статус ККТ — нулём из устройства не затираем.
        if (d.Int("LineLength") > 0)
            DeviceLineLength = d.Int("LineLength");
        if (d.Int("LineLengthPixels") > 0)
            DeviceLineLengthPixels = d.Int("LineLengthPixels");
    }

    /// <summary>
    /// Статус ККТ: ответ GetStatus и блок Status в GetKktInfo.
    /// </summary>
    private void ReadStatus(JsonElement s)
    {
        ShiftNumber = s.Int("ShiftNumber");
        CheckNumber = s.Int("DocNumber");
        CheckNumberInShift = s.Int("DocNumberInShift");
        IsShiftOpened = s.Bool("IsShiftOpened");
        IsShiftExpired = s.Bool("IsShiftExpired");
        ShiftState = IsShiftExpired ? RBSoftSkkm.ShiftState.Expired
            : IsShiftOpened ? RBSoftSkkm.ShiftState.Opened
            : RBSoftSkkm.ShiftState.Closed;
        CashBalance = s.Dec("CashSum");
        TotalSum = s.Dec("TotalSum");
        OpenShiftDate = s.DateOrNull("OpenShiftTime");
        ServerTime = s.Date("ComputerTime");
        DeviceTime = s.Date("DeviceTime");
        DeviceLineLength = s.Int("LineLength");
        DeviceLineLengthPixels = s.Int("LineLengthPixels");
        DeviceIsFiscal = s.Bool("IsFiscal");
        IsFnPresent = s.Bool("IsFnPresent");
        IsFnError = s.Bool("IsFnError");
        IsIsmDisconnected = s.Bool("IsIsmDisconnected");
        IsOfdDisconnected = s.Bool("IsOfdDisconnected");
        IsDrawerOpened = s.Bool("IsDrawerOpened");
        IsCheckPaperPresent = s.Bool("IsCheckPaperPresent");
        IsControlPaperPresent = s.Bool("IsControlPaperPresent");
        ErrorCodeDescription = s.Str("ErrorCodeDescription");
        LicenseStatus = s.Enum<LicenseStatus>("LicenseStatus");
        ReadWarnings(s.Get("Warnings"));
        ReadBacklog(s.Get("Backlog"));
    }

    /// <summary>
    /// Предупреждения ФН: блок Warnings (статус, документ) или FnWarnings (результат операции).
    /// </summary>
    private void ReadWarnings(JsonElement w)
    {
        if (!w.Exists())
            return;
        FnWarningsIsCriticalError = w.Bool("CriticalError");
        FnWarningsIsMemoryOverflow = w.Bool("MemoryOverflow");
        FnWarningsIsNeedReplacement = w.Bool("NeedReplacement");
        FnWarningsIsOfdTimeout = w.Bool("OfdTimeout");
        FnWarningsIsResourceExhausted = w.Bool("ResourceExhausted");
    }

    /// <summary>
    /// Очередь непереданных в ОФД документов.
    /// </summary>
    private void ReadBacklog(JsonElement b)
    {
        if (!b.Exists())
            return;
        BacklogDocumentsCounter = b.Long("DocumentsCounter");
        BacklogDocumentFirstNumber = b.Long("DocumentFirstNumber");
        BacklogDocumentFirstDateTime = b.Date("DocumentFirstDateTime");
    }

    /// <summary>
    /// GetKktInfo: версия сервера, статус, ККТ, ФН, драйвер.
    /// </summary>
    private void ReadKktInfo(JsonElement r)
    {
        ServerVersion = r.Str("ServerVersion");
        if (r.Get("Status").Exists())
            ReadStatus(r.Get("Status"));
        ReadDevice(r.Get("Device"));

        var fn = r.Get("Fn");
        FnSerialNumber = fn.Str("SerialNumber");
        RnNumber = fn.Str("RnNumber");
        FnFnsUrl = fn.Str("FnsUrl");
        if (fn.Date("ValidityDate") is var validity && validity != default)
            FnValidityDate = validity.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        OrganizationInfo = fn.Str("OrganizationName");
        FnVatin = fn.Str("Vatin");
        FnOfdName = fn.Get("Ofd").Str("Name");
        FnOfdVatin = fn.Get("Ofd").Str("Vatin");
        FnSaleAddress = fn.Str("SaleAddress");
        FnSaleLocation = fn.Str("SaleLocation");

        var driver = r.Get("Driver");
        DriverType = driver.Str("Type");
        DriverVersion = driver.Str("Version");
        DriverVendor = driver.Str("Vendor");
    }

    /// <summary>
    /// Выходные параметры смены и отчёта: outputParameters (результат операции), OutputParameters (документ).
    /// Номера, которых в блоке нет (0), не затирают уже прочитанные.
    /// </summary>
    private void ReadOutputParameters(JsonElement o)
    {
        if (!o.Exists())
            return;
        if (o.Enum<ShiftState>("ShiftState") is { } shiftState)
            ShiftState = shiftState;
        if (o.Int("ShiftNumber") > 0)
            ShiftNumber = o.Int("ShiftNumber");
        if (o.Int("CheckNumber") > 0)
            CheckNumber = LastCheckNumber = o.Int("CheckNumber");
        if (o.Int("NumberOfChecks") > 0)
            NumberOfChecks = o.Int("NumberOfChecks");
        if (o.Int("NumberOfDocuments") > 0)
            NumberOfDocuments = o.Int("NumberOfDocuments");
        DocumentsCounter = o.Int("DocumentsCounter");
        DateTime = o.DateOrNull("DateTime");
        if (o.Int("ShiftClosingCheckNumber") > 0)
            ShiftClosingCheckNumber = o.Int("ShiftClosingCheckNumber");
        if (o.Int("ResourcesFn") > 0)
            ResourcesFn = o.Int("ResourcesFn");
        if (o.Str("FnValidityDate").Length > 0)
            FnValidityDate = o.Str("FnValidityDate");
        CashBalance = o.Dec("CashBalance");
        ReadBacklog(o.Get("Backlog"));
        ReadWarnings(o.Get("FnWarnings").Exists() ? o.Get("FnWarnings") : o.Get("Warnings"));
    }

    /// <summary>
    /// Шапка документа: организация, РНМ, сайт ФНС, ОФД.
    /// </summary>
    private void ReadHeader(JsonElement h)
    {
        OrganizationInfo = h.Str("OrganizationInfo");
        FnVatin = h.Str("Vatin");
        RnNumber = h.Str("RnNumber");
        FnFnsUrl = h.Str("FnsUrl");
        FnOfdName = h.Str("OfdOrganizationName");
        FnOfdVatin = h.Str("OfdVatin");
    }

    /// <summary>
    /// Итоги смены: счётчики, внесения, выемки (GetTotals, Z-отчёт).
    /// </summary>
    private void ReadShiftTotal(JsonElement t)
    {
        Counter = ReadCounters(t.Get("Counters"));
        ShiftIncomeSum = t.Get("ShiftIncome").Dec("Sum");
        ShiftIncomeCount = t.Get("ShiftIncome").Int("Count");
        ShiftOutcomeSum = t.Get("ShiftOutcome").Dec("Sum");
        ShiftOutcomeCount = t.Get("ShiftOutcome").Int("Count");
    }

    /// <summary>
    /// Таблица Counter: строка — на каждую операцию, которую прислал сервер.
    /// </summary>
    private static IReadOnlyList<Counter> ReadCounters(JsonElement c)
    {
        var rows = new List<Counter>();
        void Add(string operation, JsonElement data, JsonElement correction)
        {
            if (!data.Exists() && !correction.Exists())
                return;
            var payments = data.Get("Payments");
            rows.Add(new Counter
            {
                Operation = operation,
                Count = data.Int("Count"),
                Sum = data.Dec("Sum"),
                SalesCorrectionCount = correction.Int("Count"),
                SalesCorrectionSum = correction.Dec("Sum"),
                Cash = payments.Dec("Cash"),
                Electronically = payments.Dec("Electronically"),
                Prepaid = payments.Dec("Prepaid"),
                Credit = payments.Dec("Credit"),
                Barter = payments.Dec("Barter"),
                DiscountSum = data.Get("Discount").Dec("Sum"),
                DiscountCount = data.Get("Discount").Int("Count"),
                AddingSum = data.Get("Adding").Dec("Sum"),
                AddingCount = data.Get("Adding").Int("Count"),
            });
        }
        Add("Приход", c.Get("Sales"), c.Get("SalesCorrection"));
        Add("Возврат прихода", c.Get("SalesReturn"), c.Get("SalesReturnCorrection"));
        Add("Расход", c.Get("Purchases"), c.Get("PurchasesCorrection"));
        Add("Возврат расхода", c.Get("PurchasesReturn"), c.Get("PurchasesReturnCorrection"));
        return rows;
    }

    /// <summary>
    /// Документ: чек, отчёт, внесение, слип и т. д. (GET документа, POST cashin / cashout / cash/open / slip).
    /// </summary>
    private void ReadDocument(JsonElement d)
    {
        ServerTime = d.Date("Date");
        FiscalDatetime = d.Date("FiscalDate");
        ReadOutputParameters(d.Get("OutputParameters"));

        DocId = d.Str("DocId");
        FiscalSign = d.Str("FiscalSign");
        FnSerialNumber = d.Str("Fn");
        TaskType = d.Enum<CheckType>("TaskType");
        PoolId = d.Str("PoolId");
        ResultCode = d.Int("ResultCode");
        ResultDescription = d.Str("ResultDescription");
        IsProcessed = d.Bool("Processed");
        FnFFDVersion = d.Str("FfdVersion");
        DrawerNumber = d.Int("DrawerNumber");
        ServerVersion = d.Str("ServerVersion");
        FnSaleAddress = d.Str("SaleAddress");
        FnSaleLocation = d.Str("SaleLocation");
        ReadHeader(d.Get("DocumentHeader"));
        ReadDevice(d.Get("DeviceInfo"));
        ReadShiftTotal(d.Get("ShiftTotal"));

        // Данные коррекции — в те же свойства, что задаются для печати чека коррекции.
        var correction = d.Get("CorrectionData");
        if (correction.Exists())
        {
            CorrectionDataType = correction.Enum<CorrectionTypes>("Type");
            CorrectionDataDescription = correction.Str("Description");
            CorrectionDataDate = correction.DateOrNull("Date");
            CorrectionDataNumber = correction.Str("Number");
        }

        // Номера и наличные уже могли прийти из выходных параметров отчёта — не затираем нулями.
        if (d.Int("ShiftNumber") > 0)
            ShiftNumber = d.Int("ShiftNumber");
        if (d.Int("DocNumber") > 0)
            CheckNumber = d.Int("DocNumber");
        if (d.Int("DocNumberInShift") > 0)
            CheckNumberInShift = d.Int("DocNumberInShift");
        if (d.Get("CashSum").Exists())
            CashBalance = d.Dec("CashSum");

        // Тело чека и коррекции: оплаты, налоги, кассир, СНО — как их прислал сервер.
        if (d.Get("DeviceName").Exists())
            DeviceName = d.Str("DeviceName");
        if (d.Get("TerminalId").Exists())
            TerminalId = d.Str("TerminalId");
        if (d.Get("CashierName").Exists())
            CashierName = d.Str("CashierName");
        if (d.Get("CashierVatin").Exists())
            CashierVatin = d.Str("CashierVatin");
        if (d.Get("Electronically").Exists())
            IsElectronically = d.Bool("Electronically");
        if (d.Enum<TaxSystem>("TaxType") is { } taxType)
            TaxVariant = taxType;
        else if (d.Enum<TaxSystem>("TaxVariant") is { } taxVariant)
            TaxVariant = taxVariant;

        var payments = d.Get("Payments");
        if (payments.Exists())
        {
            Cash = Pay(payments, "Cash", "Cash");
            ElectronicPayment = Pay(payments, "Electronic", "ElectronicPayment");
            AdvancePayment = Pay(payments, "PrePaid", "AdvancePayment");
            Credit = Pay(payments, "Credit", "Credit");
            CashProvision = Pay(payments, "Barter", "CashProvision");
        }

        void Tax(string name, Action<decimal> set)
        {
            if (d.Get(name).Exists())
                set(d.Dec(name));
        }
        Tax("TaxSumNone", v => SumTaxNone = v);
        Tax("TaxSum0", v => SumTax0 = v);
        Tax("TaxSum5", v => SumTax5 = v);
        Tax("TaxSum7", v => SumTax7 = v);
        Tax("TaxSum10", v => SumTax10 = v);
        Tax("TaxSum18", v => SumTax18 = v);
        Tax("TaxSum20", v => SumTax20 = v);
        Tax("TaxSum22", v => SumTax22 = v);
        Tax("TaxSum105", v => SumTax105 = v);
        Tax("TaxSum107", v => SumTax107 = v);
        Tax("TaxSum110", v => SumTax110 = v);
        Tax("TaxSum118", v => SumTax118 = v);
        Tax("TaxSum120", v => SumTax120 = v);
        Tax("TaxSum122", v => SumTax122 = v);
    }

    private static decimal Pay(JsonElement payments, string stored, string request)
    {
        var value = payments.Get(stored);
        return value.Exists() ? payments.Dec(stored) : payments.Dec(request);
    }

    /// <summary>
    /// ReportValues списка (Get…List): по документу — его основные поля с ключом «[номер] Имя»,
    /// имена — как у свойств коннектора для одного документа. Вложенные блоки (строки, ККТ, итоги) не выводятся.
    /// Номер смены и ФД документа смены — из выходных параметров, если их нет в самом документе.
    /// </summary>
    private void ReadDocuments(JsonElement list)
    {
        var rows = new List<ReportValue>();
        var index = 0;
        foreach (var d in list.Items())
        {
            var output = d.Get("OutputParameters");
            var header = d.Get("DocumentHeader");
            void Add(string name, JsonElement value, string description, string? text = null)
            {
                if (value.Exists())
                    rows.Add(new ReportValue { Key = $"[{index}] {name}", Value = text ?? value.Text(), Description = description });
            }
            JsonElement NonZero(JsonElement value, JsonElement fallback)
                => !fallback.Exists() || value.Exists() && value.Text() is not ("" or "0") ? value : fallback;

            Add("DocId", d.Get("DocId"), "Идентификатор документа");
            Add("TaskType", d.Get("TaskType"), "Тип документа", d.Enum<CheckType>("TaskType")?.ToString());
            Add("ServerTime", d.Get("Date"), "Время на сервере ККМ");
            Add("FiscalDatetime", d.Get("FiscalDate"), "Дата и время документа по часам ФН");
            Add("ShiftNumber", NonZero(d.Get("ShiftNumber"), output.Get("ShiftNumber")), "Номер смены");
            Add("CheckNumber", NonZero(d.Get("DocNumber"), output.Get("CheckNumber")), "Номер фискального документа (ФД)");
            Add("CheckNumberInShift", d.Get("DocNumberInShift"), "Номер чека за смену");
            Add("FiscalSign", d.Get("FiscalSign"), "Фискальный признак документа");
            Add("FnSerialNumber", d.Get("Fn"), "Номер ФН");
            Add("RnNumber", header.Get("RnNumber"), "Регистрационный номер ККТ (РНМ)");
            Add("OrganizationInfo", header.Get("OrganizationInfo"), "Организация");
            Add("FnFFDVersion", d.Get("FfdVersion"), "Версия ФФД документа");
            Add("FnSaleAddress", d.Get("SaleAddress"), "Адрес расчётов");
            Add("FnSaleLocation", d.Get("SaleLocation"), "Место расчётов");
            Add("DeviceName", d.Get("DeviceName"), "Имя устройства");
            Add("TerminalId", d.Get("TerminalId"), "Идентификатор рабочего места");
            Add("PoolId", d.Get("PoolId"), "Пул");
            Add("ResultCode", d.Get("ResultCode"), "Код результата");
            Add("ResultDescription", d.Get("ResultDescription"), "Описание результата");
            Add("IsProcessed", d.Get("Processed"), "Успешная операция");
            index++;
        }
        ReportValues = rows;
    }

    /// <summary>
    /// Документ внесения или выемки: общие поля документа и фактическая сумма операции (Sum) — в CashAmount.
    /// </summary>
    private void ReadCashDocument(JsonElement d)
    {
        ReadDocument(d);
        CashAmount = d.Dec("Sum");
    }

    /// <summary>
    /// Общие поля результата операции: открытие и закрытие смены, отчёты, чеки.
    /// </summary>
    private void ReadOperationResult(JsonElement r)
    {
        DocId = r.Str("docId");
        ShiftNumber = r.Int("shiftNumber");
        FnSerialNumber = r.Str("fnNumber");
        RnNumber = r.Str("rnNumber");
        FnFnsUrl = r.Str("fnsUrl");
        ServerTime = r.Date("datetime");
        FiscalDatetime = r.Date("fiscalDatetime");
    }

    /// <summary>
    /// Результат чека и чеков коррекции.
    /// </summary>
    private void ReadFiscalResult(JsonElement r)
    {
        ReadOperationResult(r);
        FiscalSign = r.Str("fiscalSign");
        CheckNumber = r.Int("fiscalNumber");
        ReadDevice(r.Get("deviceInfo"));
    }

    /// <summary>
    /// Результат открытия и закрытия смены, X-отчёта, отчёта о расчётах.
    /// </summary>
    private void ReadShiftResult(JsonElement r)
    {
        ReadOperationResult(r);
        if (r.Get("shiftTotal").Exists())
            ReadShiftTotal(r.Get("shiftTotal"));
        ReadOutputParameters(r.Get("outputParameters"));
        ReadDevice(r.Get("deviceInfo"));
    }

    /// <summary>
    /// Очередь: в ReportValues только эти поля, с описанием.
    /// DocState и QueueState остаются кодами.
    /// </summary>
    private void ReadQueueTask(JsonElement r)
    {
        if (r.Str("DocId").Length > 0)
            DocId = r.Str("DocId");
        NumberInQueue = r.Int("NumberInQueue");
        ResultCode = r.Int("ResultCode");
        ResultDescription = r.Str("ResultDescription");
        if (r.Str("FiscalSign").Length > 0)
            FiscalSign = r.Str("FiscalSign");
        var date = r.Date("Date");
        ReportValues =
        [
            QueueRow("DocId", r.Str("DocId"), "Идентификатор документа"),
            QueueRow("DocState", r.Get("DocState").Text(), "Код состояния документа"),
            QueueRow("QueueState", r.Get("QueueState").Text(), "Код состояния очереди"),
            QueueRow("NumberInQueue", r.Get("NumberInQueue").Text(), "Позиция задания в очереди на момент запроса"),
            QueueRow("ResultCode", r.Get("ResultCode").Text(), "Код результата"),
            QueueRow("ResultDescription", r.Str("ResultDescription"), "Описание результата"),
            QueueRow("Date", date == default ? r.Str("Date") : date.ToString("dd.MM.yyyy HH:mm:ss"), "Дата и время последнего изменения статуса задачи"),
            QueueRow("FiscalSign", r.Str("FiscalSign"), "Фискальный признак документа"),
            QueueRow("PrintStatusDescription", r.Str("PrintStatusDescription"), "Описание текущего этапа обработки задания"),
        ];
    }

    /// <summary>Ответ RequestKM: локальная проверка и, если сервер её приложил, онлайн-проверка кода.</summary>
    private void ReadRequestKm(JsonElement r)
    {
        IsmConnected = r.Bool("ISMConnected");
        FormatChecking = r.Bool("FormatChecking");
        Checking = r.Bool("Checking");
        CheckingResult = r.Bool("CheckingResult");
        MarkingBarcode = r.Str("Barcode");
        CodeMarkInfo = ReadCodeMark(r.Get("CodeMarkInfo"));
    }

    /// <summary>Ответ GetProcessingKMResult.</summary>
    private void ReadProcessingKm(JsonElement r)
    {
        if (r.Str("Guid").Length > 0)
            Guid = r.Str("Guid");
        KmResult = r.Bool("Result");
        ResultCode = r.Int("ResultCode");
        StatusInfo = OptionalInt(r, "StatusInfo");
        HandleCode = r.Int("HandleCode");
        RequestStatus = r.Int("RequestStatus");
        CodeMarkInfo = ReadCodeMark(r.Get("CodeMarkInfo"));
    }

    /// <summary>Ответ VerifyMarking, VerifyMarkingTsPiot и VerifyMarkingLmcz.</summary>
    private void ReadMarkingVerify(JsonElement r)
    {
        MarkingAnswerCode = r.Int("code");
        MarkingDescription = r.Str("description");
        ReqId = r.Str("reqId");
        ReqTimestamp = r.Long("reqTimestamp");
        MarkingModuleStatus = r.Enum<LocalModuleStatus>("status");
        RequiresDownload = r.Bool("requiresDownload");
        MarkingLastUpdate = r.Long("lastUpdate");
        MarkingLastSync = r.Long("lastSync");
        IsCheckedOffline = r.Bool("isCheckedOffline");
        MarkingModuleVersion = r.Str("version");
        MarkingModuleId = r.Str("inst");
        MarkingServiceUrl = r.Str("serviceUrl");
        MarkingCodes = r.Get("codes").Items().Select(ReadCodeMark).Where(mark => mark != null).Cast<CodeMarkInfo>().ToArray();
    }

    private static CodeMarkInfo? ReadCodeMark(JsonElement e)
    {
        if (e.ValueKind != JsonValueKind.Object)
            return null;
        return new CodeMarkInfo
        {
            Cis = e.Str("cis"),
            DataMatrix = e.Str("dataMatrix"),
            Valid = e.Bool("valid"),
            PrintView = e.Str("printView"),
            GroupIds = e.Get("groupIds").Items().Select(item => int.TryParse(item.Text(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) ? id : 0).ToArray(),
            Verified = e.Bool("verified"),
            Realizable = e.Bool("realizable"),
            Utilised = e.Bool("utilised"),
            Found = e.Bool("found"),
            ExpireDate = e.Str("expireDate"),
            VariableExpirations = Raw(e, "variableExpirations"),
            ProductionDate = e.Str("productionDate"),
            ProductWeight = OptionalInt(e, "productWeight"),
            PrVetDocument = e.Str("prVetDocument"),
            IsOwner = OptionalBool(e, "isOwner"),
            ErrorCode = OptionalInt(e, "errorCode"),
            ServiceCode = e.Str("code"),
            Message = e.Str("message"),
            IsTracking = e.Bool("isTracking"),
            Sold = e.Bool("sold"),
            Gtin = e.Str("gtin"),
            PackageType = e.Str("packageType"),
            Parent = e.Str("parent"),
            ProducerInn = e.Str("producerInn"),
            Mrp = OptionalInt(e, "mrp"),
            GrayZone = e.Bool("grayZone"),
            Smp = OptionalInt(e, "smp"),
            IsBlocked = e.Bool("isBlocked"),
            IsGreyGtin = e.Bool("isGreyGtin"),
            Ogvs = e.Get("ogvs").Items().Select(item => item.Text()).ToArray(),
            InnerUnitCount = OptionalInt(e, "innerUnitCount"),
            SoldUnitCount = OptionalInt(e, "soldUnitCount"),
            ProductionSerialNumber = e.Str("productionSerialNumber"),
            ProductionBatchNumber = e.Str("productionBatchNumber"),
            FactorySerialNumber = e.Str("factorySerialNumber"),
            PackageQuantity = OptionalInt(e, "packageQuantity"),
            EliminationState = OptionalInt(e, "eliminationState"),
        };
    }

    private static int? OptionalInt(JsonElement e, string name)
        => e.Get(name).ValueKind == JsonValueKind.Number ? e.Int(name) : null;

    private static bool? OptionalBool(JsonElement e, string name)
    {
        var value = e.Get(name);
        return value.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => null,
        };
    }

    private static string Raw(JsonElement e, string name)
    {
        var value = e.Get(name);
        return value.ValueKind is JsonValueKind.Object or JsonValueKind.Array ? value.GetRawText() : value.Text();
    }

    private static ReportValue QueueRow(string key, string value, string description)
        => new() { Key = key, Value = value, Description = description };

    /// <summary>
    /// Таблица ReportValues: поле Result, развёрнутое в строки «ключ — значение»
    /// </summary>
    private static List<ReportValue> ToReportValues(JsonElement? result)
    {
        var rows = new List<ReportValue>();
        void Add(string key, JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Object)
                foreach (var property in value.EnumerateObject())
                    Add(key.Length == 0 ? property.Name : $"{key}_{property.Name}", property.Value);
            else if (value.ValueKind == JsonValueKind.Array)
            {
                var index = 0;
                foreach (var item in value.EnumerateArray())
                    Add($"{key}_{index++}", item);
            }
            else if (key.Length > 0)
                rows.Add(new ReportValue { Key = key, Value = EnumText(key, value) });
        }
        if (result is { } value && value.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
            Add("", value);
        return rows;
    }

    // Имя поля JSON (последний сегмент ключа ReportValues) → перечисление коннектора.
    private static readonly Dictionary<string, Type> ReportEnums = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ShiftState"] = typeof(ShiftState),
        ["TaskType"] = typeof(CheckType),
        ["PaymentType"] = typeof(CheckType),
        ["SentToPrint"] = typeof(QueueTaskStatus),
        ["DeviceClass"] = typeof(DeviceClass),
        ["TimeZone"] = typeof(CheckTimeZone),
        ["Driver"] = typeof(DeviceType),
        ["LicenseStatus"] = typeof(LicenseStatus),
        ["ConfirmationType"] = typeof(KmConfirmationType),
        ["PlannedStatus"] = typeof(MarkingPlannedStatus),
        ["PaymentMethod"] = typeof(ElectronicPaymentMethod),
        ["SignCalculationObject"] = typeof(SignCalculationObject),
        ["SignMethodCalculation"] = typeof(SignMethodCalculation),
        ["MeasureOfQuantity"] = typeof(MeasureOfQuantity),
        ["AgentSign"] = typeof(AgentType),
        ["SignSubjectCalculationAgent"] = typeof(AgentType),
        ["TaxVariant"] = typeof(TaxSystem),
        ["lineStyle"] = typeof(LineStyle),
    };

    /// <summary>Число известного перечисления — его имя (2 у ShiftState → Opened). Остальное — как в JSON.</summary>
    private static string EnumText(string key, JsonElement value)
    {
        var text = value.Text();
        if (value.ValueKind != JsonValueKind.Number)
            return text;
        var name = key[(key.LastIndexOf('_') + 1)..];
        if (!ReportEnums.TryGetValue(name, out var type) || !int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
            return text;
        return Enum.IsDefined(type, number) ? Enum.GetName(type, number) ?? text : text;
    }
}

/// <summary>
/// Чтение полей ответа: <c>s.Int("ShiftNumber")</c>, <c>s.Get("Warnings")</c>, <c>list.Items()</c>.
/// </summary>
internal static class JsonRead
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    /// <summary>Вложенное поле по имени без учёта регистра; нет поля — пустой элемент.</summary>
    public static JsonElement Get(this JsonElement e, string name)
    {
        if (e.ValueKind == JsonValueKind.Object)
            foreach (var property in e.EnumerateObject())
                if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
                    return property.Value;
        return default;
    }

    public static bool Exists(this JsonElement e) => e.ValueKind is not (JsonValueKind.Undefined or JsonValueKind.Null);

    public static IEnumerable<JsonElement> Items(this JsonElement e)
        => e.ValueKind == JsonValueKind.Array ? e.EnumerateArray() : [];

    /// <summary>Значение текстом: строка как есть, число как в ответе, логическое — True / False.</summary>
    public static string Text(this JsonElement e) => e.ValueKind switch
    {
        JsonValueKind.String => e.GetString() ?? "",
        JsonValueKind.Number => e.GetRawText(),
        JsonValueKind.True => "True",
        JsonValueKind.False => "False",
        _ => "",
    };

    public static string Str(this JsonElement e, string name) => e.Get(name).Text();

    public static decimal Dec(this JsonElement e, string name)
        => decimal.TryParse(e.Str(name), NumberStyles.Float, Inv, out var value) ? value : 0;

    public static int Int(this JsonElement e, string name) => (int)e.Dec(name);

    public static long Long(this JsonElement e, string name) => (long)e.Dec(name);

    public static bool Bool(this JsonElement e, string name) => e.Get(name).ValueKind == JsonValueKind.True;

    /// <summary>Дата из ответа: ISO ("2026-10-29T10:29:11") или "yyyyMMddHHmmss".</summary>
    public static System.DateTime Date(this JsonElement e, string name)
        => System.DateTime.TryParse(e.Str(name), Inv, DateTimeStyles.None, out var value)
            || System.DateTime.TryParseExact(e.Str(name), "yyyyMMddHHmmss", Inv, DateTimeStyles.None, out value) ? value : default;

    public static System.DateTime? DateOrNull(this JsonElement e, string name) => e.Get(name).Exists() ? e.Date(name) : null;

    public static T? Enum<T>(this JsonElement e, string name) where T : struct, System.Enum
        => int.TryParse(e.Str(name), NumberStyles.Integer, Inv, out var value) ? (T)System.Enum.ToObject(typeof(T), value) : null;
}
