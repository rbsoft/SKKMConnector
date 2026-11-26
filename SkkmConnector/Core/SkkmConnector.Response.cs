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
        DeviceLineLength = d.IntOr("LineLength", DeviceLineLength);
        DeviceLineLengthPixels = d.IntOr("LineLengthPixels", DeviceLineLengthPixels);
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
        if (IsShiftExpired)
            ShiftState = RBSoftSkkm.ShiftState.Expired;
        else if (IsShiftOpened)
            ShiftState = RBSoftSkkm.ShiftState.Opened;
        else
            ShiftState = RBSoftSkkm.ShiftState.Closed;
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
        if (s.Get("ErrorCode").Exists())
            ErrorCode = s.Int("ErrorCode");
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
        var validityDate = fn.Date("ValidityDate");
        if (validityDate != default)
            FnValidityDate = validityDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
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
        ShiftState = o.Enum<ShiftState>("ShiftState") ?? ShiftState;
        ShiftNumber = o.IntOr("ShiftNumber", ShiftNumber);
        CheckNumber = o.IntOr("CheckNumber", CheckNumber);
        LastCheckNumber = o.IntOr("CheckNumber", LastCheckNumber);
        NumberOfChecks = o.IntOr("NumberOfChecks", NumberOfChecks);
        NumberOfDocuments = o.IntOr("NumberOfDocuments", NumberOfDocuments);
        DocumentsCounter = o.Int("DocumentsCounter");
        DateTime = o.DateOrNull("DateTime");
        ShiftClosingCheckNumber = o.IntOr("ShiftClosingCheckNumber", ShiftClosingCheckNumber);
        ResourcesFn = o.IntOr("ResourcesFn", ResourcesFn);
        FnValidityDate = o.StrOr("FnValidityDate", FnValidityDate);
        CashBalance = o.Dec("CashBalance");
        ReadBacklog(o.Get("Backlog"));
        if (o.Get("FnWarnings").Exists())
            ReadWarnings(o.Get("FnWarnings"));
        else
            ReadWarnings(o.Get("Warnings"));
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
    /// Итоги смены: счётчики, внесения, выемки (GetTotals, документ смены).
    /// </summary>
    private void ReadShiftTotal(JsonElement t)
    {
        Counter = ReadCounters(t.Get("Counters"));
        ReadShiftIncomeOutcome(t);
    }

    /// <summary>
    /// Внесения и выемки за смену: ShiftIncome и ShiftOutcome.
    /// </summary>
    private void ReadShiftIncomeOutcome(JsonElement t)
    {
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
        AddCounter(rows, "Приход", c.Get("Sales"), c.Get("SalesCorrection"));
        AddCounter(rows, "Возврат прихода", c.Get("SalesReturn"), c.Get("SalesReturnCorrection"));
        AddCounter(rows, "Расход", c.Get("Purchases"), c.Get("PurchasesCorrection"));
        AddCounter(rows, "Возврат расхода", c.Get("PurchasesReturn"), c.Get("PurchasesReturnCorrection"));
        return rows;
    }

    /// <summary>
    /// Строка счётчиков одной операции. Нет ни операции, ни её коррекции — строка не добавляется.
    /// </summary>
    private static void AddCounter(List<Counter> rows, string operation, JsonElement data, JsonElement correction)
    {
        if (!data.Exists() && !correction.Exists())
            return;

        // Скидка и надбавка коррекции затирают скидку и надбавку операции.
        var discount = data.Get("Discount");
        if (correction.Get("Discount").Exists())
            discount = correction.Get("Discount");
        var adding = data.Get("Adding");
        if (correction.Get("Adding").Exists())
            adding = correction.Get("Adding");

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
            DiscountSum = discount.Dec("Sum"),
            DiscountCount = discount.Int("Count"),
            AddingSum = adding.Dec("Sum"),
            AddingCount = adding.Int("Count"),
        });
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
        IsProcessed = d.Bool("IsProcessed");
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
        ShiftNumber = d.IntOr("ShiftNumber", ShiftNumber);
        CheckNumber = d.IntOr("DocNumber", CheckNumber);
        CheckNumberInShift = d.IntOr("DocNumberInShift", CheckNumberInShift);
        if (d.Get("CashSum").Exists())
            CashBalance = d.Dec("CashSum");

        // Тело чека и коррекции: оплаты, налоги, кассир, СНО — как их прислал сервер.
        if (d.Get("DeviceName").Exists())
            DeviceName = d.Str("DeviceName");
        if (d.Get("TerminalId").Exists())
            TerminalId = d.Str("TerminalId");
        if (d.Get("CashierName").Exists())
            CashierName = d.Str("CashierName");
        else if (d.Get("DocumentHeader").Get("Cashier").Exists())
            CashierName = d.Get("DocumentHeader").Str("Cashier");
        if (d.Get("CashierVatin").Exists())
            CashierVatin = d.Str("CashierVatin");
        if (d.Get("Electronically").Exists())
            IsElectronically = d.Bool("Electronically");
        TaxVariant = d.Enum<TaxSystem>("TaxType") ?? d.Enum<TaxSystem>("TaxVariant") ?? TaxVariant;

        var payments = d.Get("Payments");
        if (payments.Exists())
        {
            Cash = Pay(payments, "Cash", "Cash");
            ElectronicPayment = Pay(payments, "Electronic", "ElectronicPayment");
            AdvancePayment = Pay(payments, "PrePaid", "AdvancePayment");
            Credit = Pay(payments, "Credit", "Credit");
            CashProvision = Pay(payments, "Barter", "CashProvision");
        }

        SumTaxNone = TaxSum(d, "TaxSumNone", SumTaxNone);
        SumTax0 = TaxSum(d, "TaxSum0", SumTax0);
        SumTax5 = TaxSum(d, "TaxSum5", SumTax5);
        SumTax7 = TaxSum(d, "TaxSum7", SumTax7);
        SumTax10 = TaxSum(d, "TaxSum10", SumTax10);
        SumTax18 = TaxSum(d, "TaxSum18", SumTax18);
        SumTax20 = TaxSum(d, "TaxSum20", SumTax20);
        SumTax22 = TaxSum(d, "TaxSum22", SumTax22);
        SumTax105 = TaxSum(d, "TaxSum105", SumTax105);
        SumTax107 = TaxSum(d, "TaxSum107", SumTax107);
        SumTax110 = TaxSum(d, "TaxSum110", SumTax110);
        SumTax118 = TaxSum(d, "TaxSum118", SumTax118);
        SumTax120 = TaxSum(d, "TaxSum120", SumTax120);
        SumTax122 = TaxSum(d, "TaxSum122", SumTax122);
    }

    /// <summary>
    /// Сумма оплаты. В сохранённом документе поле называется иначе, чем в запросе печати (PrePaid и AdvancePayment).
    /// </summary>
    private static decimal Pay(JsonElement payments, string stored, string request)
    {
        if (payments.Get(stored).Exists())
            return payments.Dec(stored);
        return payments.Dec(request);
    }

    /// <summary>
    /// Сумма НДС коррекции 1.05. Нет поля в ответе — остаётся <paramref name="current"/>.
    /// </summary>
    private static decimal? TaxSum(JsonElement d, string name, decimal? current)
    {
        if (d.Get(name).Exists())
            return d.Dec(name);
        return current;
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
    }

    /// <summary>
    /// Открытие смены
    /// </summary>
    private void ReadOpenShift(JsonElement r)
    {
        ReadOperationResult(r);
        ReadDevice(r.Get("deviceInfo"));
    }

    /// <summary>
    /// X-отчёт и отчёт о расчётах
    /// </summary>
    private void ReadReport(JsonElement r)
    {
        ReadOperationResult(r);
        ReadOutputParameters(r.Get("outputParameters"));
    }

    /// <summary>
    /// Z-отчёт
    /// </summary>
    private void ReadCloseShift(JsonElement r)
    {
        ReadOperationResult(r);
        Counter = ReadCounters(r.Get("overallTotals").Get("Counters"));
        ReadShiftIncomeOutcome(r.Get("shiftTotal"));
        ReadDevice(r.Get("deviceInfo"));
    }

    /// <summary>
    /// Очередь: в ReportValues только эти поля, с описанием.
    /// DocState и QueueState остаются кодами.
    /// </summary>
    private void ReadQueueTask(JsonElement r)
    {
        DocId = r.StrOr("DocId", DocId);
        NumberInQueue = r.Int("NumberInQueue");
        ResultCode = r.Int("ResultCode");
        ResultDescription = r.Str("ResultDescription");
        FiscalSign = r.StrOr("FiscalSign", FiscalSign);
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

    /// <summary>Ответ RequestKM</summary>
    private void ReadRequestKm(JsonElement r)
    {
        IsmConnected = r.Bool("ISMConnected");
        FormatChecking = r.Bool("FormatChecking");
        Checking = r.Bool("Checking");
        CheckingResult = r.Bool("CheckingResult");
        if (r.Get("Code").Exists())
            ErrorCode = r.Int("Code");
        if (r.Get("Description").Exists())
            ErrorDescription = r.Str("Description");
    }

    /// <summary>Ответ GetProcessingKMResult.</summary>
    private void ReadProcessingKm(JsonElement r)
    {
        Guid = r.StrOr("Guid", Guid);
        KmResult = r.Bool("Result");
        ResultCode = r.Int("ResultCode");
        StatusInfo = OptionalInt(r, "StatusInfo");
        HandleCode = r.Int("HandleCode");
        RequestStatus = r.Int("RequestStatus");
        if (IsObject(r.Get("CodeMarkInfo")))
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
        MarkingCodes = r.Get("codes").Items().Where(IsObject).Select(ReadCodeMark).ToArray();
    }

    private static bool IsObject(JsonElement e) => e.ValueKind == JsonValueKind.Object;

    /// <summary>
    /// Результат проверки одного кода маркировки.
    /// </summary>
    private static CodeMarkInfo ReadCodeMark(JsonElement e)
    {
        return new CodeMarkInfo
        {
            Cis = e.Str("cis"),
            DataMatrix = e.Str("dataMatrix"),
            Valid = e.Bool("valid"),
            PrintView = e.Str("printView"),
            GroupIds = e.Get("groupIds").Items().Select(GroupId).ToArray(),
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

    /// <summary>Номер товарной группы. Не целое число — 0.</summary>
    private static int GroupId(JsonElement item)
    {
        if (int.TryParse(item.Text(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            return id;
        return 0;
    }

    /// <summary>Число из ответа. Нет поля или не число — null.</summary>
    private static int? OptionalInt(JsonElement e, string name)
    {
        if (e.Get(name).ValueKind == JsonValueKind.Number)
            return e.Int(name);
        return null;
    }

    /// <summary>Логическое из ответа. Нет поля или не true / false — null.</summary>
    private static bool? OptionalBool(JsonElement e, string name)
    {
        var kind = e.Get(name).ValueKind;
        if (kind == JsonValueKind.True)
            return true;
        if (kind == JsonValueKind.False)
            return false;
        return null;
    }

    /// <summary>Объект или массив — JSON как есть, остальное — текстом.</summary>
    private static string Raw(JsonElement e, string name)
    {
        var value = e.Get(name);
        if (value.ValueKind == JsonValueKind.Object || value.ValueKind == JsonValueKind.Array)
            return value.GetRawText();
        return value.Text();
    }

    private static ReportValue QueueRow(string key, string value, string description)
        => new() { Key = key, Value = value, Description = description };

    /// <summary>
    /// Список чеков: строка «Чек N», затем поля чека.
    /// </summary>
    private void ReadCheckList(JsonElement r)
    {
        var rows = new List<ReportValue>();
        var number = 1;
        foreach (var check in r.Items())
        {
            rows.Add(new ReportValue { Key = $"Чек {number++}" });
            foreach (var field in check.EnumerateObject())
                rows.Add(new ReportValue
                {
                    Key = field.Name,
                    Value = EnumText(field.Name, field.Value),
                    Description = ReportFieldDescriptions.ForKey(field.Name),
                });
        }
        ReportValues = rows;
    }

    /// <summary>
    /// Таблица ReportValues: поле Result, развёрнутое в строки «ключ — значение»
    /// </summary>
    private static List<ReportValue> ToReportValues(JsonElement result)
    {
        var rows = new List<ReportValue>();
        AddReportValues(rows, "", result);
        return rows;
    }

    /// <summary>
    /// Значение — строкой с ключом; объект и массив — их поля, ключ дополняется: «DeviceInfo Model», «Positions [0] Name».
    /// </summary>
    private static void AddReportValues(List<ReportValue> rows, string key, JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in value.EnumerateObject())
                AddReportValues(rows, ChildKey(key, property.Name), property.Value);
        }
        else if (value.ValueKind == JsonValueKind.Array)
        {
            var index = 0;
            foreach (var item in value.EnumerateArray())
            {
                AddReportValues(rows, ChildKey(key, $"[{index}]"), item);
                index++;
            }
        }
        else if (key.Length > 0)
        {
            rows.Add(new ReportValue
            {
                Key = key,
                Value = EnumText(key, value),
                Description = ReportFieldDescriptions.ForKey(key),
            });
        }
    }

    private static string ChildKey(string key, string part) => key.Length == 0 ? part : $"{key} {part}";

    /// <summary>Последняя часть ключа ReportValues: у «DeviceInfo Model» — «Model».</summary>
    internal static string LastKeyPart(string key) => key[(key.LastIndexOf(' ') + 1)..];

    // Имя поля JSON (последний сегмент ключа ReportValues) и перечисление коннектора.
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
        ["TaxType"] = typeof(TaxSystem),
        ["lineStyle"] = typeof(LineStyle),
    };

    /// <summary>Число известного перечисления — его имя (у ShiftState значение 2 даёт Opened). Остальное — как в JSON.</summary>
    private static string EnumText(string key, JsonElement value)
    {
        var text = value.Text();
        if (value.ValueKind != JsonValueKind.Number)
            return text;
        if (!ReportEnums.TryGetValue(LastKeyPart(key), out var type))
            return text;
        if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
            return text;
        if (!Enum.IsDefined(type, number))
            return text;
        return Enum.GetName(type, number) ?? text;
    }
}

/// <summary>
/// Чтение полей ответа: <c>s.Int("ShiftNumber")</c>, <c>s.Get("Warnings")</c>, <c>list.Items()</c>.
/// </summary>
internal static class JsonRead
{
    private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

    /// <summary>Вложенное поле по имени без учёта регистра. Нет поля — пустой элемент.</summary>
    public static JsonElement Get(this JsonElement e, string name)
    {
        if (e.ValueKind != JsonValueKind.Object)
            return default;
        // Обычно имя совпадает точно: такой поиск не создаёт строк для имён полей.
        if (e.TryGetProperty(name, out var exact))
            return exact;
        foreach (var property in e.EnumerateObject())
            if (property.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return property.Value;
        return default;
    }

    /// <summary>Поле есть в ответе и не равно null.</summary>
    public static bool Exists(this JsonElement e) => e.ValueKind != JsonValueKind.Undefined && e.ValueKind != JsonValueKind.Null;

    /// <summary>Элементы массива. Не массив — пустой список.</summary>
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

    /// <summary>Строка из ответа. Нет поля — пустая строка.</summary>
    public static string Str(this JsonElement e, string name) => e.Get(name).Text();

    /// <summary>Число из ответа. Нет поля или не число — 0.</summary>
    public static decimal Dec(this JsonElement e, string name)
    {
        var field = e.Get(name);
        // Число читается сразу, без промежуточной строки. Строку с числом ("12.5") разбираем как текст.
        if (field.ValueKind == JsonValueKind.Number && field.TryGetDecimal(out var number))
            return number;
        return decimal.TryParse(field.Text(), NumberStyles.Float, Inv, out var value) ? value : 0;
    }

    public static int Int(this JsonElement e, string name) => (int)e.Dec(name);

    public static long Long(this JsonElement e, string name) => (long)e.Dec(name);

    /// <summary>Логическое из ответа. Нет поля — false.</summary>
    public static bool Bool(this JsonElement e, string name) => e.Get(name).ValueKind == JsonValueKind.True;

    /// <summary>Дата из ответа: ISO ("2026-10-29T10:29:11") или "yyyyMMddHHmmss". Не дата — пустая дата.</summary>
    public static DateTime Date(this JsonElement e, string name)
    {
        var text = e.Str(name);
        if (DateTime.TryParse(text, Inv, DateTimeStyles.None, out var date))
            return date;
        if (DateTime.TryParseExact(text, "yyyyMMddHHmmss", Inv, DateTimeStyles.None, out date))
            return date;
        return default;
    }

    /// <summary>Дата из ответа. Нет поля — null.</summary>
    public static DateTime? DateOrNull(this JsonElement e, string name) => e.Get(name).Exists() ? e.Date(name) : null;

    /// <summary>Перечисление по числу из ответа. Нет поля или не число — null.</summary>
    public static T? Enum<T>(this JsonElement e, string name) where T : struct, System.Enum
    {
        if (!int.TryParse(e.Str(name), NumberStyles.Integer, Inv, out var number))
            return null;
        return (T)System.Enum.ToObject(typeof(T), number);
    }

    /// <summary>Строка из ответа. Нет поля или она пустая — <paramref name="current"/>.</summary>
    public static string StrOr(this JsonElement e, string name, string current)
    {
        var value = e.Str(name);
        return value != "" ? value : current;
    }

    /// <summary>Число из ответа. Нет поля или оно не больше нуля — <paramref name="current"/>.</summary>
    public static int IntOr(this JsonElement e, string name, int current)
    {
        var value = e.Int(name);
        return value > 0 ? value : current;
    }
}
