using RBSoftSkkm.Internal;
using System.Text.Json;

namespace RBSoftSkkm;

public sealed partial class SkkmConnector
{
    /// <summary>
    /// Очищает входные данные чека/операции и результат прошлого вызова.
    /// </summary>
    public void Clear()
    {
        PaymentType = CheckType.Sale;
        IsElectronically = false;
        IsOperationOnline = false;
        TimeZone = null;
        TextBefore = "";
        TextAfter = "";
        SaleLocation = "";
        SaleAddress = "";
        SenderEmail = "";
        AdditionalAttribute = "";
        _positions.Clear();
        Cash = 0;
        ElectronicPayment = 0;
        AdvancePayment = 0;
        Credit = 0;
        CashProvision = 0;
        ElectronicPaymentAmount = 0;
        ElectronicPaymentMethod = default;
        ElectronicPaymentIdentifiers = "";
        ElectronicPaymentAdditionalInformation = "";
        ElectronicPayments.Clear();
        
        CustomerInfo = "";
        CustomerVatin = "";
        CustomerEmail = "";
        CustomerPhone = "";
        CustomerDateOfBirth = "";
        CustomerDocumentTypeCode = "";
        CustomerDocumentData = "";
        CustomerCitizenship = "";
        CustomerAddress = "";

        AgentSign = null;
        PayingAgentOperation = "";
        PayingAgentPhone = null;
        ReceivePaymentsOperatorPhone = null;
        MoneyTransferOperatorPhone = null;
        MoneyTransferOperatorName = "";
        MoneyTransferOperatorAddress = "";
        MoneyTransferOperatorVatin = "";
        VendorName = "";
        VendorPhones = null;
        VendorVatin = "";

        IndustryIdentifierFoiv = "";
        IndustryAttributeDocumentDate = "";
        IndustryAttributeDocumentNumber = "";
        IndustryAttributeValue = "";
        UserAttributeName = "";
        UserAttributeValue = "";
        OperationalAttributeDateTime = "";
        OperationalAttributeOperationId = null;
        OperationalAttributeData = "";

        CorrectionType = CorrectionTypes.Самостоятельно;
        CorrectionDescription = "";
        CorrectionDate = default;
        CorrectionNumber = "";
        CorrectionSumTaxNone = 0;
        CorrectionSumTax0 = 0;
        CorrectionSumTax5 = 0;
        CorrectionSumTax7 = 0;
        CorrectionSumTax10 = 0;
        CorrectionSumTax18 = 0;
        CorrectionSumTax20 = 0;
        CorrectionSumTax22 = 0;
        CorrectionSumTax105 = 0;
        CorrectionSumTax107 = 0;
        CorrectionSumTax110 = 0;
        CorrectionSumTax118 = 0;
        CorrectionSumTax120 = 0;
        CorrectionSumTax122 = 0;

        CashAmount = 0;
        TextForPrint = "";
        PictureAlignment = PictureAlignment.Center;

        MarkingCode = "";
        PlannedStatus = MarkingPlannedStatus.Sold;
        MarkingQuantity = 1;
        MeasureOfQuantity = default;
        FractionalQuantityNumerator = 0;
        FractionalQuantityDenominator = 0;
        NotSendToServer = false;
        WaitForResult = false;
        RequestKmGuid = "";
        ConfirmationType = default;
        MarkingCodes.Clear();

        TemplateName = "";
        TemplateType = default;

        Ok = false;
        ErrorCode = 0;
        ErrorDescription = "";
        Result = default;

        // Сброс всех данных ответа
        ResetResponse();
    }

    /// <summary>
    /// Полная очистка, но дополнительно сбрасывает данные кассира и СНО
    /// </summary>
    public void ClearAll()
    {
        Clear();
        CashierName = "";
        CashierVatin = "";
        TaxVariant = TaxSystem.Osn;
    }

    /// <summary>
    /// Очищает предыдущий чек и задаёт только тип операции для чека. 
    /// </summary>
    public void NewCheck(CheckType paymentType)
        {
            Clear();
            PaymentType = paymentType;
    }

    /// <summary>
    /// Очищает предыдущий чек и задаёт обязательные поля для чека - имя кассира,
    /// тип операции и систему налогообложения.
    /// </summary>
    public void NewCheck(string cashierName, CheckType paymentType, TaxSystem taxVariant)
    {
        Clear();
        CashierName = cashierName;
        PaymentType = paymentType;
        TaxVariant = taxVariant;
    }

    /// <summary>
    /// Очищает предыдущий чек и задаёт тип операции и данные коррекции
    /// </summary>
    public void NewCheckCorrection(
        CheckType paymentType,
        CorrectionTypes correctionType,
        string correctionDescription,
        DateTime correctionDate,
        string correctionNumber = "0")
    {
        Clear();
        PaymentType = paymentType;
        CorrectionType = correctionType;
        CorrectionDescription = correctionDescription;
        CorrectionDate = correctionDate;
        CorrectionNumber = correctionNumber;
    }

    /// <summary>
    /// Очищает предыдущий чек и помимо обязательных полей чека задаёт данные коррекции
    /// </summary>
    public void NewCheckCorrection(
        string cashierName,
        CheckType paymentType,
        TaxSystem taxVariant,
        CorrectionTypes correctionType,
        string correctionDescription,
        DateTime correctionDate,
        string correctionNumber = "0")
    {
        Clear();
        CashierName = cashierName;
        PaymentType = paymentType;
        TaxVariant = taxVariant;
        CorrectionType = correctionType;
        CorrectionDescription = correctionDescription;
        CorrectionDate = correctionDate;
        CorrectionNumber = correctionNumber;
    }

    /// <summary>
    /// Очищает предыдущий шаблон печати и задаёт имя и тип нового.
    /// Дальше добавляйте строки теми же методами, что и в чек:
    /// <see cref="AddText(string, PrintFont, PrintAlignment)"/>, <see cref="AddBarcode"/>,
    /// <see cref="AddSeparatorLine"/>, <see cref="AddPicture"/>.
    /// Имя — уникальный идентификатор шаблона на сервере, пробелы в нём не допускаются.
    /// </summary>
    public void NewTemplate(string name, PrintTemplateType type = PrintTemplateType.Advertisement)
    {
        Clear();
        TemplateName = name;
        TemplateType = type;
    }

    /// <summary>
    /// Очищает предыдущий шаблон чека и задаёт его имя.
    /// затем <see cref="AddCheckTemplate()"/>.
    /// </summary>
    public void NewCheckTemplate(string name)
    {
        Clear();
        TemplateName = name;
    }

    /// <summary>
    /// Очищает предыдущий шаблон чека, задаёт имя и тип операции.
    /// </summary>
    public void NewCheckTemplate(string name, CheckType paymentType)
    {
        NewCheck(paymentType);
        TemplateName = name;
    }

    /// <summary>
    /// Очищает предыдущий шаблон чека и задаёт имя, кассира, тип операции и СНО.
    /// </summary>
    public void NewCheckTemplate(string name, string cashierName, CheckType paymentType, TaxSystem taxVariant)
    {
        NewCheck(cashierName, paymentType, taxVariant);
        TemplateName = name;
    }

    /// <summary>
    /// Период отбора для списков отчётов, чеков и операций: даты «с» и «по».
    /// </summary>
    public void FromTo(DateTime from, DateTime to)
    {
        ShiftsFrom = from;
        ShiftsTo = to;
    }

    /// <summary>
    /// Проверка доступности сервера ККМ. Не требует передачи ключа доступа (api_key)
    /// </summary>
    public async Task<bool> Ping()
    {
        await Get("ping");
        if (Result.ValueKind == JsonValueKind.Object)
        {
            if (Result.TryGetProperty("product", out var product))
                ServerProduct = product.GetString() ?? "";
            if (Result.TryGetProperty("version", out var version))
                ServerVersion = version.GetString() ?? "";
        }
        return Ok;
    }

    /// <summary>
    /// Получение списка зарегистрированных ККТ
    /// </summary>
    public async Task<bool> GetDeviceList()
    {
        await Get("kkt/list");
        Devices = ReadResult<DeviceListResponse[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Получение подробной информации об устройстве ККТ
    /// </summary>
    public async Task<bool> GetKktInfo()
    {
        await Get($"kkt?{DeviceQuery}");
        Kkt = ReadResult<DataKkt>();
        if (Kkt == null)
            return Ok;
        ServerVersion = Kkt.ServerVersion ?? "";
        ApplyStatus(Kkt.Status);
        ApplyDeviceInfo(Kkt.Device);
        ApplyFnInfo(Kkt.Fn);
        return Ok;
    }

    /// <summary>
    /// Получение расширенного статуса ККТ
    /// </summary>
    public async Task<bool> GetStatus()
    {
        await Get($"kkt/status?{DeviceQuery}");
        ApplyStatus(ReadResult<KktStatus>());
        return Ok;
    }

    /// <summary>
    /// Получение краткого статуса смены и очереди ОФД
    /// </summary>
    public async Task<bool> GetShiftStatus()
    {
        await Get($"kkt/shift/status?{DeviceQuery}");
        ShiftStatus = ReadResult<ResponseCurrentStatus>();
        if (ShiftStatus == null)
            return Ok;
        ShiftNumber = ShiftStatus.ShiftNumber;
        CheckNumber = ShiftStatus.CheckNumber;
        CurrentShiftState = ShiftStatus.ShiftState;
        _backlog = ShiftStatus.Backlog;
        return Ok;
    }

    /// <summary>
    /// Открытие кассовой смены
    /// </summary>
    public async Task<bool> OpenShift()
    {
        await Post("shift/open", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Закрытие кассовой смены (Z-отчёт)
    /// </summary>
    public async Task<bool> CloseShift()
    {
        await Post("shift/z", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Формирование X-отчёта (без закрытия смены)
    /// </summary>
    public async Task<bool> ReportX()
    {
        await Post("shift/x", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Формирование отчёта о текущем состоянии расчётов
    /// </summary>
    public async Task<bool> ReportSettlement()
    {
        await Post("report/settlement", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Возвращает X-отчёт по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetReportX(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("shift/x");
        return Ok;
    }

    /// <summary>
    /// Возвращает Z-отчёт по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetReportZ(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("shift/z");
        return Ok;
    }

    /// <summary>
    /// Возвращает результат открытия смены по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetOpenShift(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("shift/open");
        return Ok;
    }

    /// <summary>
    /// Возвращает отчёт о состоянии расчётов по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetReportSettlement(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("report/settlement");
        return Ok;
    }

    /// <summary>
    /// Получение необнуляемых (накопительных) счётчиков ККТ
    /// </summary>
    public async Task<bool> GetOverAll()
    {
        await Get($"kkt/counters/overall?{DeviceQuery}");
        OverallTotals = ReadResult<OverallTotals>();
        NonZeroSum = OverallTotals?.Counters?.Sales?.Sum ?? 0;
        return Ok;
    }

    /// <summary>
    /// Получение максимальной ширины строки чека устройства
    /// </summary>
    public async Task<bool> GetLineLength()
    {
        await Get($"kkt/lineLength?{DeviceQuery}");
        var length = ReadResult<LineLengthV2>();
        if (length == null)
            return Ok;
        LineLength = length.LineLength;
        LineLengthPixels = length.LineLengthPixels;
        return Ok;
    }

    /// <summary>
    /// Получение счётчиков за смену
    /// </summary>
    public async Task<bool> GetTotals()
    {
        await Get($"kkt/counters/shift?{DeviceQuery}");
        ShiftTotals = ReadResult<ResShiftTotal>();
        return Ok;
    }

    /// <summary>
    /// Получение списка Z-отчётов за период
    /// </summary>
    public async Task<bool> GetShiftList()
    {
        var extra = ReportType > 0 ? $"reportType={ReportType}" : null;
        await GetReportList("shift/z/list", extra);
        return Ok;
    }

    /// <summary>
    /// Получение списка открытий смен за период
    /// </summary>
    public async Task<bool> GetOpenShiftList()
    {
        await GetReportList("shift/open/list");
        return Ok;
    }

    /// <summary>
    /// Получение списка X-отчётов за период
    /// </summary>
    public async Task<bool> GetReportXList()
    {
        await GetReportList("shift/x/list");
        return Ok;
    }

    /// <summary>
    /// Список отчётов о состоянии расчётов по устройству за период
    /// </summary>
    public async Task<bool> GetReportSettlementList()
    {
        await GetReportList("report/settlement/list");
        return Ok;
    }

    /// <summary>
    /// Печать кассового чека
    /// </summary>
    public async Task<bool> PrintCheck()
    {
        if (!ValidateCheck())
            return Ok;
        await Post("check", CheckBody());
        return Ok;
    }

    /// <summary>
    /// Асинхронно поставить фискальный чек в очередь печати
    /// </summary>
    public async Task<bool> PrintCheckAsync()
    {
        if (!ValidateCheck())
            return Ok;
        await Post("check/async", CheckBody());
        return Ok;
    }

    /// <summary>
    /// Печать чека коррекции для ФФД 1.2
    /// </summary>
    public async Task<bool> PrintCheckCorrection120()
    {
        if (!ValidateCorrection120())
            return Ok;
        await Post("correction120", Correction120Body());
        return Ok;
    }

    /// <summary>
    /// Асинхронно печатает чек коррекции для ФФД 1.2
    /// </summary>
    public async Task<bool> PrintCheckCorrection120Async()
    {
        if (!ValidateCorrection120())
            return Ok;
        await Post("correction120/async", Correction120Body());
        return Ok;
    }

    /// <summary>
    /// Печать чека коррекции для ФФД 1.0.5
    /// </summary>
    public async Task<bool> PrintCheckCorrection105()
    {
        if (!ValidateCorrection105())
            return Ok;
        await Post("correction105", Correction105Body());
        return Ok;
    }

    /// <summary>
    /// Асинхронно ставит печать чека коррекции для ФФД 1.0.5.
    /// </summary>
    public async Task<bool> PrintCheckCorrection105Async()
    {
        if (!ValidateCorrection105())
            return Ok;
        await Post("correction105/async", Correction105Body());
        return Ok;
    }

    /// <summary>
    /// Возвращает чек коррекции ФФД 1.2 по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetCorrection120(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("correction120");
        return Ok;
    }

    /// <summary>
    /// Получение списка чеков коррекции ФФД 1.2
    /// </summary>
    public async Task<bool> GetCorrection120List()
    {
        await GetCheckList("correction120/list");
        return Ok;
    }

    /// <summary>
    /// Возвращает чек коррекции ФФД 1.0.5 по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetCorrection105(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("correction105");
        return Ok;
    }

    /// <summary>
    /// Получение списка чеков коррекции ФФД 1.0.5
    /// </summary>
    public async Task<bool> GetCorrection105List()
    {
        await GetCheckList("correction105/list");
        return Ok;
    }

    /// <summary>
    /// Возвращает статус выполнения задания по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetTaskStatus(string documentId)
    {
        DocumentId = documentId;
        await Get($"task/status?{IdQuery}");
        TaskStatus = ReadResult<ResponseTaskStatus>();
        if (TaskStatus == null)
            return Ok;
        if (!string.IsNullOrEmpty(TaskStatus.FiscalSign))
            FiscalSign = TaskStatus.FiscalSign!;
        if (TaskStatus.DocNumber > 0)
            CheckNumber = TaskStatus.DocNumber;
        if (TaskStatus.ShiftNumber > 0)
            ShiftNumber = TaskStatus.ShiftNumber;
        if (!string.IsNullOrEmpty(TaskStatus.DocId))
            DocumentId = TaskStatus.DocId!;
        return Ok;
    }

    /// <summary>
    /// Возвращает результат операции по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetCheck(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("check");
        return Ok;
    }

    /// <summary>
    /// Получение фискального признака (ФП) по номеру фискального документа (ФД)
    /// </summary>
    public async Task<bool> GetFiscalSign(int docNumber)
    {
        CheckNumber = docNumber;
        await Get($"check/fiscalSign?docNumber={CheckNumber}&{DeviceQuery}");
        if (Ok && Result.ValueKind == JsonValueKind.String)
            FiscalSign = Result.GetString() ?? "";
        return Ok;
    }

    /// <summary>
    /// Печать копии чека по идентификатору документа
    /// </summary>
    public async Task<bool> PrintCheckCopy()
    {
       
        await Post("check/copy", new CheckbaseParameters { DeviceName = DeviceName, DocId = DocumentId });
        return Ok;
    }

    /// <summary>
    /// Печать копии последнего чека).
    /// </summary>
    public async Task<bool> PrintLastCheckCopy()
    {
        await Post($"check/copy/last?{DeviceQuery}");
        return Ok;
    }

    /// <summary>
    /// Возвращает печатную форму документа по его идентификатору (docId)
    /// </summary>
    public async Task<bool> GetPrintForm(string documentId)
    {
        DocumentId = documentId;
        await Get($"task/form?{IdQuery}");
        PrintForm = (ReadResult<PrintFormLine[]>() ?? []).Select(x => new PrintLine(x)).ToList();
        return Ok;
    }

    /// <summary>
    /// Регистрация операции внесения наличных в денежный ящик. 
    /// </summary>
    public async Task<bool> CashIn()
    {
        await Post("cashin", CashBody());
        ApplyDocument(ReadResult<CheckDocument>());
        return Ok;
    }

    /// <summary>
    /// Регистрация операции выемки наличных из денежного ящика.
    /// </summary>
    public async Task<bool> CashOut()
    {
        await Post("cashout", CashBody());
        ApplyDocument(ReadResult<CheckDocument>());
        return Ok;
    }

    /// <summary>
    /// Открытие денежного ящика
    /// </summary>
    public async Task<bool> OpenCashdrawer()
    {
        await Post("cash/open", CheckBase());
        ApplyDocument(ReadResult<CheckDocument>());
        return Ok;
    }

    /// <summary>
    /// Получение остатка наличных в денежном ящике
    /// </summary>
    public async Task<bool> GetCash()
    {
        await Get($"cash?{DeviceQuery}");
        CashBalance = ReadResult<CashSum>()?.Sum ?? 0;
        return Ok;
    }

    /// <summary>
    /// Возвращает результат операции внесения наличных по идентификатору операции (docId)
    /// </summary>
    public async Task<bool> GetCashIn(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("cashin");
        return Ok;
    }

    /// <summary>
    /// Получение списка операций внесения наличных по имени устройства
    /// </summary>
    public async Task<bool> GetCashInList()
    {
        await GetCheckList("cashin/list");
        return Ok;
    }

    /// <summary>
    /// Возвращает результат операции выемки наличных по идентификатору операции (docId)
    /// </summary>
    public async Task<bool> GetCashOut(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("cashout");
        return Ok;
    }

    /// <summary>
    /// Загрузка изображения из файла в выбранную ККТ. Файл должен быть BMP или PNG —
    /// коннектор сам проверяет формат, кодирует в Base64 и отправляет.
    /// Имя картинки на сервере — <see cref="PictureId"/>, а если он не задан — имя файла без расширения.
    /// </summary>
    public async Task<bool> SendPicture(string filePath)
    {
        if (!LoadPicture(filePath, out var base64))
            return Ok;

        await Post("picture", new UploadPicture
        {
            DeviceName = DeviceName,
            PictureName = string.IsNullOrEmpty(PictureId) ? Path.GetFileNameWithoutExtension(filePath) : PictureId,
            Base64 = base64,
            Alignment = (int)PictureAlignment
        });
        return Ok;
    }

    /// <summary>
    /// Получение списка изображений
    /// </summary>
    public async Task<bool> GetPictureList()
    {
        await Get($"picture/list?{DeviceQuery}");
        Pictures = ReadResult<List<Picture>>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Открытие сессии регистрации (проверки) кодов маркировки на ККТ
    /// </summary>
    public async Task<bool> OpenSessionRegistrationKM()
    {
        await Post("marking/session/open", new CheckbaseParameters { DeviceName = DeviceName });
        return Ok;
    }

    /// <summary>
    /// Закрытие сессии регистрации (проверки) кодов маркировки на ККТ
    /// </summary>
    public async Task<bool> CloseSessionRegistrationKM()
    {
        await Post("marking/session/close", new CheckbaseParameters { DeviceName = DeviceName });
        return Ok;
    }

    /// <summary>
    /// Локальная проверка кода маркировки на ККТ (ФФД 1.2)
    /// </summary>
    public async Task<bool> RequestKM()
    {
        if (string.IsNullOrWhiteSpace(RequestKmGuid))
            RequestKmGuid = Guid.NewGuid().ToString();

        await Post("marking/km/request", new RequestKmParameters
        {
            DeviceName = DeviceName,
            RequestKM = new RequestKm
            {
                Guid = RequestKmGuid,
                NotSendToServer = NotSendToServer,
                WaitForResult = WaitForResult,
                MarkingCode = MarkingCode,
                PlannedStatus = (int)PlannedStatus,
                Quantity = MarkingQuantity,
                MeasureOfQuantity = (int)MeasureOfQuantity,
                FractionalQuantityNumerator = FractionalQuantityNumerator > 0 ? FractionalQuantityNumerator : null,
                FractionalQuantityDenominator = FractionalQuantityDenominator > 0 ? FractionalQuantityDenominator : null
            }
        });
        MarkingCheck = ReadResult<RequestKmResult>();
        return Ok;
    }

    /// <summary>
    /// Получение результата проверки кода маркировки в ОИСМ
    /// </summary>
    public async Task<bool> GetProcessingKMResult()
    {
        await Get($"marking/km/result?{DeviceQuery}");
        MarkingProcessing = ReadResult<ProcessingKmResult>();
        if (!string.IsNullOrWhiteSpace(MarkingProcessing?.Guid))
            RequestKmGuid = MarkingProcessing!.Guid!;
        return Ok;
    }

    /// <summary>
    /// Подтверждение, будет ли ранее проверенный код маркировки фактически включён в документ реализации. Действительно только в рамках открытой сессии регистрации
    /// </summary>
    public async Task<bool> ConfirmKM()
    {
        await Post("marking/km/confirm", new RequestConfirmKm
        {
            DeviceName = DeviceName,
            GUID = RequestKmGuid,
            ConfirmationType = (int)ConfirmationType
        });
        return Ok;
    }

    /// <summary>
    /// Печать нефискального документа.
    /// </summary>
    public async Task<bool> PrintSlip()
    {
        await Post("slip", SlipBody());
        ApplyDocument(ReadResult<CheckDocument>());
        return Ok;
    }

    /// <summary>
    /// Асинхронно поставить нефискальный документ в очередь печати
    /// </summary>
    public async Task<bool> PrintSlipAsync()
    {
        await Post("slip/async", SlipBody());
        return Ok;
    }

    /// <summary>
    /// Версия сервера ККМ.
    /// </summary>
    public async Task<bool> GetVersion()
    {
        await Get("version");
        if (Result.ValueKind == JsonValueKind.String)
            ServerVersion = Result.GetString() ?? "";
        else if (Result.ValueKind == JsonValueKind.Object && Result.TryGetProperty("ServerVersion", out var version))
            ServerVersion = version.GetString() ?? "";
        else
            ServerVersion = Result.ToString();
        return Ok;
    }

    /// <summary>
    /// Перезагрузка кассы.
    /// </summary>
    public async Task<bool> RebootDevice()
    {
        await Post("kkt/reboot", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Список пулов устройств.
    /// </summary>
    public async Task<bool> GetPoolList()
    {
        await Get("pool/list");
        Pools = ReadResult<string[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Список касс в пуле.
    /// </summary>
    public async Task<bool> GetDeviceListByPool()
    {
        await Get($"kkt/list/byPool?pool={Uri.EscapeDataString(PoolName)}");
        Devices = ReadResult<DeviceListResponse[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Асинхронное открытие смены.
    /// </summary>
    public async Task<bool> OpenShiftAsync()
    {
        await Post("shift/open/async", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Асинхронное закрытие смены.
    /// </summary>
    public async Task<bool> CloseShiftAsync()
    {
        await Post("shift/z/async", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Асинхронный X-отчёт.
    /// </summary>
    public async Task<bool> ReportXAsync()
    {
        await Post("shift/x/async", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Асинхронный отчёт о состоянии расчётов.
    /// </summary>
    public async Task<bool> ReportSettlementAsync()
    {
        await Post("report/settlement/async", CheckBase());
        return Ok;
    }

    /// <summary>
    /// Асинхронное внесение наличных.
    /// </summary>
    public async Task<bool> CashInAsync()
    {
        await Post("cashin/async", CashBody());
        return Ok;
    }

    /// <summary>
    /// Асинхронная выемка наличных.
    /// </summary>
    public async Task<bool> CashOutAsync()
    {
        await Post("cashout/async", CashBody());
        return Ok;
    }

    /// <summary>
    /// Список чеков за период и смену. Номер смены передаётся параметром, период (даты «с»/«по»)
    /// </summary>
    public async Task<bool> GetCheckList(int shiftNumber)
    {
        ShiftNumber = shiftNumber;
        await Get($"check/list?{DeviceQuery}&{DateQuery(ShiftsFrom, ShiftsTo)}&shift={ShiftNumber}");
        Checks = (ReadResult<CheckDocument[]>() ?? []).Select(x => new Check(x)).ToList();
        return Ok;
    }

    /// <summary>
    /// Печать копии чека по данным фискального накопителя.
    /// </summary>
    public async Task<bool> PrintCheckCopyFn()
    {
        await Post("check/copy/fn", new CheckCopyFnParameters
        {
            DeviceName = DeviceName,
            FnNumber = FnNumber,
            FiscalSign = FiscalSign,
            DocNumber = CheckNumber
        });
        return Ok;
    }

    /// <summary>
    /// Получение слипа по идентификатору документа.
    /// </summary>
    public async Task<bool> GetSlip(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("slip");
        return Ok;
    }

    /// <summary>
    /// Список слипов по кассе.
    /// </summary>
    public async Task<bool> GetSlipList()
    {
        await GetCheckList("slip/list");
        return Ok;
    }

    /// <summary>
    /// Получение картинки по имени.
    /// </summary>
    public async Task<bool> GetPicture(string pictureId)
    {
        PictureId = pictureId;
        await Get($"picture?{DeviceQuery}&id={Uri.EscapeDataString(PictureId)}");
        if (Ok && Result.ValueKind == JsonValueKind.String)
            PictureBase64Result = Result.GetString() ?? "";
        return Ok;
    }

    /// <summary>
    /// Удаление картинки.
    /// </summary>
    public async Task<bool> DeletePicture()
    {
        await Delete($"picture?{DeviceQuery}&id={Uri.EscapeDataString(PictureId)}");
        return Ok;
    }

    /// <summary>
    /// Создание шаблона печати.
    /// </summary>
    public async Task<bool> AddTemplate()
    {
        if (!ValidateTemplate())
            return Ok;
        await Post("template", TemplateBody());
        if (Ok && Result.ValueKind == JsonValueKind.String)
            TemplateName = Result.GetString() ?? TemplateName;
        return Ok;
    }

    /// <summary>
    /// Изменение шаблона печати по его имени. Строки задаются так же, как при создании
    /// </summary>
    public async Task<bool> UpdateTemplate()
    {
        if (!ValidateTemplate())
            return Ok;
        await Put("template", TemplateBody());
        return Ok;
    }

    /// <summary>
    /// Удаление шаблона печати. Имя берётся из <see cref="TemplateName"/>.
    /// </summary>
    public async Task<bool> DeleteTemplate()
    {
        if (!ValidateTemplateName(TemplateName, "шаблона печати"))
            return Ok;
        await Delete($"template?id={Uri.EscapeDataString(TemplateName)}");
        return Ok;
    }

    /// <summary>
    /// Удаление шаблона печати по имени.
    /// </summary>
    public async Task<bool> DeleteTemplate(string name)
    {
        TemplateName = name;
        await DeleteTemplate();
        return Ok;
    }

    /// <summary>
    /// Список имён шаблонов печати.
    /// </summary>
    public async Task<bool> GetTemplateList()
    {
        await Get("template/list");
        Templates = ReadResult<string[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Получение шаблона печати по имени. 
    /// </summary>
    public async Task<bool> GetTemplate(string name)
    {
        TemplateName = name;
        await Get($"template?name={Uri.EscapeDataString(TemplateName)}");
        ApplyTemplate(ReadResult<PrintTemplate>());
        return Ok;
    }

    /// <summary>
    /// Создание шаблона чека.
    /// </summary>
    public async Task<bool> AddCheckTemplate()
    {
        if (!ValidateCheckTemplate(TemplateName))
            return Ok;
        await Post("checkTemplate", CheckTemplateBody(TemplateName));
        return Ok;
    }

    /// <summary>
    /// Создание шаблона чека под указанным именем.
    /// </summary>
    public async Task<bool> AddCheckTemplate(string name)
    {
        TemplateName = name;
        await AddCheckTemplate();
        return Ok;
    }

    /// <summary>
    /// Изменение шаблона чека. Обязательно указываем имя шаблона
    /// </summary>
    public async Task<bool> UpdateCheckTemplate()
    {
        if (!ValidateCheckTemplate(TemplateName))
            return Ok;
        await Put("checkTemplate", CheckTemplateBody(TemplateName));
        return Ok;
    }

    /// <summary>
    /// Изменение шаблона чека под указанным именем.
    /// </summary>
    public async Task<bool> UpdateCheckTemplate(string name)
    {
        TemplateName = name;
        await UpdateCheckTemplate();
        return Ok;
    }

    /// <summary>
    /// Удаление шаблона чека. Имя берётся из <see cref="TemplateName"/>.
    /// </summary>
    public async Task<bool> DeleteCheckTemplate()
    {
        if (!ValidateTemplateName(TemplateName, "шаблона чека"))
            return Ok;
        await Delete($"checkTemplate?id={Uri.EscapeDataString(TemplateName)}");
        return Ok;
    }

    /// <summary>
    /// Удаление шаблона чека по имени.
    /// </summary>
    public async Task<bool> DeleteCheckTemplate(string name)
    {
        TemplateName = name;
        await DeleteCheckTemplate();
        return Ok;
    }

    /// <summary>
    /// Список шаблонов чека: имя шаблона и тип чека.
    /// </summary>
    public async Task<bool> GetCheckTemplateList()
    {
        await Get("checkTemplate/list");
        CheckTemplates = ReadResult<CheckTemplateListItem[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Получение шаблона чека по имени
    /// </summary>
    public async Task<bool> GetCheckTemplate(string name)
    {
        TemplateName = name;
        await Get($"checkTemplate?id={Uri.EscapeDataString(TemplateName)}");
        ApplyCheckTemplate(ReadResult<CheckTemplate>());
        return Ok;
    }

    /// <summary>
    /// Состояние очереди печати.
    /// </summary>
    public async Task<bool> GetQueue()
    {
        await Get("queue");
        Queue = ReadResult<QueueItem[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Состояние задания в очереди.
    /// </summary>
    public async Task<bool> GetQueueTask(string taskId)
    {
        QueueTaskId = taskId;
        await Get($"queue/task?taskId={Uri.EscapeDataString(QueueTaskId)}");
        QueueTask = ReadResult<QueueTaskState>();
        return Ok;
    }

    /// <summary>
    /// История обработки задания в очереди.
    /// </summary>
    public async Task<bool> GetQueueTaskHistory(string taskId)
    {
        QueueTaskId = taskId;
        await Get($"queue/task/history?taskId={Uri.EscapeDataString(QueueTaskId)}");
        QueueTask = ReadResult<QueueTaskState>();
        if (QueueTask != null)
            OperationHistory = QueueTask.History
                .Select(h => new OperationHistoryEntry(new OperationHistoryItem
                {
                    Time = h.Time,
                    State = h.State,
                    Description = h.Description,
                    Info = h.Info
                }))
                .ToList();
        return Ok;
    }

    /// <summary>
    /// Отмена задания в очереди.
    /// </summary>
    public async Task<bool> CancelQueueTask()
    {
        await Delete($"queue/task?taskId={Uri.EscapeDataString(QueueTaskId)}");
        return Ok;
    }

    /// <summary>
    /// Проверка кода маркировки через внешний сервис.
    /// </summary>
    public async Task<bool> VerifyMarking()
    {
        await Post("marking/km/verify", new MarkingCodesRequest
        {
            DeviceName = DeviceName,
            Codes = MarkingCodes.ToList()
        });
        MarkingVerify = ReadResult<MarkingVerifyResult>();
        return Ok;
    }

    /// <summary>
    /// Проверка кода маркировки через ТС ПИоТ.
    /// </summary>
    public async Task<bool> VerifyMarkingTsPiot()
    {
        await Post("marking/km/tspiot/verify", new MarkingCodesRequest
        {
            DeviceName = DeviceName,
            Codes = MarkingCodes.ToList()
        });
        MarkingVerify = ReadResult<MarkingVerifyResult>();
        return Ok;
    }

    /// <summary>
    /// Проверка кода маркировки через ЛМ ЧЗ.
    /// </summary>
    public async Task<bool> VerifyMarkingLmcz()
    {
        await Post("marking/km/lmcz/verify", new MarkingCodesRequest
        {
            DeviceName = DeviceName,
            Codes = MarkingCodes.ToList()
        });
        MarkingVerify = ReadResult<MarkingVerifyResult>();
        return Ok;
    }

    /// <summary>
    /// Фискализация кассы.
    /// </summary>
    public async Task<bool> Fiscalization()
    {
        await Post("fiscalization", FiscalizationBody());
        return Ok;
    }

    /// <summary>
    /// Асинхронная фискализация кассы.
    /// </summary>
    public async Task<bool> FiscalizationAsync()
    {
        await Post("fiscalization/async", FiscalizationBody());
        return Ok;
    }

    /// <summary>
    /// Результат фискализации по идентификатору документа.
    /// </summary>
    public async Task<bool> GetFiscalization(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("fiscalization");
        FiscalizationDocument = ReadResult<FiscalizationDocument>();
        if (FiscalizationDocument != null)
        {
            if (!string.IsNullOrEmpty(FiscalizationDocument.DocId))
                DocumentId = FiscalizationDocument.DocId;
            if (!string.IsNullOrEmpty(FiscalizationDocument.FiscalSign))
                FiscalSign = FiscalizationDocument.FiscalSign;
            if (!string.IsNullOrEmpty(FiscalizationDocument.RnNumber))
                RnNumber = FiscalizationDocument.RnNumber;
            if (FiscalizationDocument.ShiftNumber > 0)
                ShiftNumber = FiscalizationDocument.ShiftNumber;
            if (FiscalizationDocument.DocNumber > 0)
                CheckNumber = FiscalizationDocument.DocNumber;
            IsFiscal = FiscalizationDocument.IsFiscal;
        }
        return Ok;
    }

    /// <summary>
    /// Список операций фискализации по кассе.
    /// </summary>
    public async Task<bool> GetFiscalizationList()
    {
        await Get($"fiscalization/list?{DeviceQuery}");
        Fiscalizations = ReadResult<FiscalizationDocument[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Последняя операция из базы. <c>tasktype</c> — <see cref="PaymentType"/> (<see cref="CheckType"/>), <c>isProcessed</c> — <see cref="IsProcessed"/>.
    /// </summary>
    public async Task<bool> GetOperationLast()
    {
        var processed = IsProcessed ? "true" : "false";
        await Get($"operation/last?tasktype={(int)PaymentType}&isProcessed={processed}");
        ApplyOperation(ReadResult<DeviceTaskInfo>());
        return Ok;
    }

    /// <summary>
    /// Операция по идентификатору документа.
    /// </summary>
    public async Task<bool> GetOperation(string documentId)
    {
        DocumentId = documentId;
        await Get($"operation?{DocIdQuery}");
        ApplyOperation(ReadResult<DeviceTaskInfo>());
        return Ok;
    }

    /// <summary>
    /// История операции по идентификатору документа.
    /// </summary>
    public async Task<bool> GetOperationHistory(string documentId)
    {
        DocumentId = documentId;
        await Get($"operation/history?{DocIdQuery}");
        OperationHistory = (ReadResult<OperationHistoryItem[]>() ?? [])
            .Select(h => new OperationHistoryEntry(h))
            .ToList();
        return Ok;
    }

    /// <summary>
    /// TLV-данные операции.
    /// </summary>
    public async Task<bool> GetOperationTlv(string documentId)
    {
        DocumentId = documentId;
        await Get($"operation/tlv?{DocIdQuery}");
        if (Ok && Result.ValueKind == JsonValueKind.String)
            OperationTlv = Result.GetString() ?? "";
        return Ok;
    }

    /// <summary>
    /// Данные маркировки операции.
    /// </summary>
    public async Task<bool> GetOperationKm(string documentId)
    {
        DocumentId = documentId;
        await Get($"operation/km?{DocIdQuery}");
        OperationKm = ReadResult<OperationKmRow[]>() ?? [];
        return Ok;
    }

    /// <summary>
    /// Связанные операции.
    /// </summary>
    public async Task<bool> GetOperationRelated(string documentId)
    {
        DocumentId = documentId;
        await Get($"operation/related?{DocIdQuery}");
        RelatedOperations = (ReadResult<DeviceTaskInfo[]>() ?? []).Select(x => new RelatedOperation(x)).ToList();
        return Ok;
    }

    /// <summary>
    /// Список операций за период.
    /// </summary>
    public async Task<bool> GetOperationList()
    {
        await Get($"operation/list?{DateQuery(ShiftsFrom, ShiftsTo)}");
        Operations = ReadResult<OperationListItem[]>() ?? [];
        return Ok;
    }

    private FiscalizationRequest FiscalizationBody()
    {
        var body = new FiscalizationRequest
        {
            DeviceName = DeviceName,
            RnNumber = FiscalizationRnNumber,
            TaxationSystems = FiscalizationTaxationSystems is { Length: > 0 } sno
                ? string.Join(",", sno.Select(s => (int)s))
                : null,
            Vatin = FiscalizationVatin,
            CompanyName = FiscalizationCompanyName,
            Fn = FiscalizationFn,
            FfdVersionKkt = FiscalizationFfdVersionKkt,
            FfdVersionFn = FiscalizationFfdVersionFn,
            RegistrationLabelCodes = FiscalizationRegistrationLabelCodes,
            OfdAddress = FiscalizationOfdAddress,
            OfdPort = FiscalizationOfdPort,
            AutomaticNumber = FiscalizationAutomaticNumber,
            SenderEmail = FiscalizationSenderEmail,
            ReasonCode = FiscalizationReasonCode,
            IsmHost = FiscalizationIsmHost,
            IsmPort = FiscalizationIsmPort,
            FnsUrl = FiscalizationFnsUrl,
            OfdVatin = FiscalizationOfdVatin,
            OfdName = FiscalizationOfdName,
            AgentTypes = FiscalizationAgentTypes is { Length: > 0 } agents
                ? string.Join(",", agents.Select(a => (int)a))
                : null,
            IsBsoSign = FiscalizationIsBsoSign,
            IsMarking = FiscalizationIsMarking,
            IsPawnshop = FiscalizationIsPawnshop,
            IsAssurance = FiscalizationIsAssurance,
            IsAutomatic = FiscalizationIsAutomatic,
            IsVending = FiscalizationIsVending,
            IsAutomaticPrinter = FiscalizationIsAutomaticPrinter,
            IsOnline = FiscalizationIsOnline,
            IsLottery = FiscalizationIsLottery,
            IsGambling = FiscalizationIsGambling,
            IsExcisable = FiscalizationIsExcisable,
            IsService = FiscalizationIsService,
            IsEncrypted = FiscalizationIsEncrypted,
            IsOffline = FiscalizationIsOffline,
            IsCateringServices = FiscalizationIsCateringServices,
            IsWholesaleTrade = FiscalizationIsWholesaleTrade
        };
        FillBase(body);
        return body;
    }
}
