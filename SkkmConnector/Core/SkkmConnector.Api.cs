using RBSoftSkkm.Internal;

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
        return await Call<PingResult>(HttpMethod.Get, "ping", null, ping =>
        {
            ServerProduct = ping.Product ?? "";
            ServerVersion = ping.Version ?? "";
        });
    }

    /// <summary>
    /// Получение списка зарегистрированных ККТ
    /// </summary>
    public async Task<bool> GetDeviceList()
    {
        return await Call<DeviceListResponse[]>(HttpMethod.Get, "kkt/list", null, devices => Devices = devices);
    }

    /// <summary>
    /// Получение подробной информации об устройстве ККТ
    /// </summary>
    public async Task<bool> GetKktInfo()
    {
        return await Call<DataKkt>(HttpMethod.Get, $"kkt?{DeviceQuery}", null, kkt =>
        {
            Kkt = kkt;
            ServerVersion = kkt.ServerVersion ?? "";
            ApplyStatus(kkt.Status);
            ApplyDeviceInfo(kkt.Device);
            ApplyFnInfo(kkt.Fn);
        });
    }

    /// <summary>
    /// Получение расширенного статуса ККТ
    /// </summary>
    public async Task<bool> GetStatus()
    {
        return await Call<KktStatus>(HttpMethod.Get, $"kkt/status?{DeviceQuery}", null, ApplyStatus);
    }

    /// <summary>
    /// Получение краткого статуса смены и очереди ОФД
    /// </summary>
    public async Task<bool> GetShiftStatus()
    {
        return await Call<ResponseCurrentStatus>(HttpMethod.Get, $"kkt/shift/status?{DeviceQuery}", null, status =>
        {
            ShiftStatus = status;
            ShiftNumber = status.ShiftNumber;
            CheckNumber = status.CheckNumber;
            CurrentShiftState = status.ShiftState;
            _backlog = status.Backlog;
        });
    }

    /// <summary>
    /// Открытие кассовой смены
    /// </summary>
    public async Task<bool> OpenShift()
    {
        return await Call<OpenShiftResult>(HttpMethod.Post, "shift/open", CheckBase(), ApplyOpenShiftResult);
    }

    /// <summary>
    /// Закрытие кассовой смены (Z-отчёт)
    /// </summary>
    public async Task<bool> CloseShift()
    {
        return await Call<ReportResult>(HttpMethod.Post, "shift/z", CheckBase(), ApplyReportResult);
    }

    /// <summary>
    /// Формирование X-отчёта (без закрытия смены)
    /// </summary>
    public async Task<bool> ReportX()
    {
        return await Call<ReportResult>(HttpMethod.Post, "shift/x", CheckBase(), ApplyReportResult);
    }

    /// <summary>
    /// Формирование отчёта о текущем состоянии расчётов
    /// </summary>
    public async Task<bool> ReportSettlement()
    {
        return await Call<ReportResult>(HttpMethod.Post, "report/settlement", CheckBase(), ApplyReportResult);
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
    public async Task<bool> GetCloseShift(string documentId)
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
        return await Call<OverallTotals>(HttpMethod.Get, $"kkt/counters/overall?{DeviceQuery}", null, totals =>
        {
            OverallTotals = totals;
            NonZeroSum = totals.Counters?.Sales?.Sum ?? 0;
            if (totals.CashDrawer != null)
                CashBalance = totals.CashDrawer.Sum;
        });
    }

    /// <summary>
    /// Получение максимальной ширины строки чека устройства
    /// </summary>
    public async Task<bool> GetLineLength()
    {
        return await Call<LineLengthV2>(HttpMethod.Get, $"kkt/lineLength?{DeviceQuery}", null, length =>
        {
            LineLength = length.LineLength;
            LineLengthPixels = length.LineLengthPixels;
        });
    }

    /// <summary>
    /// Получение счётчиков за смену
    /// </summary>
    public async Task<bool> GetTotals()
    {
        return await Call<ResShiftTotal>(HttpMethod.Get, $"kkt/counters/shift?{DeviceQuery}", null, totals =>
        {
            ShiftTotals = totals;
            if (totals.ShiftNumber > 0)
                ShiftNumber = (int)totals.ShiftNumber;
            if (totals.CashDrawer != null)
                CashBalance = totals.CashDrawer.Sum;
        });
    }

    /// <summary>
    /// Получение списка Z-отчётов за период
    /// </summary>
    public async Task<bool> GetCloseShiftList()
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
        return await Call<FiscalParamsResult>(HttpMethod.Post, "check", CheckBody(), ApplyFiscalParams);
    }

    /// <summary>
    /// Асинхронно поставить фискальный чек в очередь печати
    /// </summary>
    public async Task<bool> PrintCheckAsync()
    {
        if (!ValidateCheck())
            return Ok;
        return await PostTask("check/async", CheckBody());
    }

    /// <summary>
    /// Печать чека коррекции для ФФД 1.2
    /// </summary>
    public async Task<bool> PrintCheckCorrection120()
    {
        if (!ValidateCorrection120())
            return Ok;
        return await Call<FiscalParamsResult>(HttpMethod.Post, "correction120", Correction120Body(), ApplyFiscalParams);
    }

    /// <summary>
    /// Асинхронно печатает чек коррекции для ФФД 1.2
    /// </summary>
    public async Task<bool> PrintCheckCorrection120Async()
    {
        if (!ValidateCorrection120())
            return Ok;
        return await PostTask("correction120/async", Correction120Body());
    }

    /// <summary>
    /// Печать чека коррекции для ФФД 1.0.5
    /// </summary>
    public async Task<bool> PrintCheckCorrection105()
    {
        if (!ValidateCorrection105())
            return Ok;
        return await Call<FiscalParamsResult>(HttpMethod.Post, "correction105", Correction105Body(), ApplyFiscalParams);
    }

    /// <summary>
    /// Асинхронно ставит печать чека коррекции для ФФД 1.0.5.
    /// </summary>
    public async Task<bool> PrintCheckCorrection105Async()
    {
        if (!ValidateCorrection105())
            return Ok;
        return await PostTask("correction105/async", Correction105Body());
    }

    /// <summary>
    /// Возвращает чек коррекции ФФД 1.2 по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetCheckCorrection120(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("correction120");
        return Ok;
    }

    /// <summary>
    /// Получение списка чеков коррекции ФФД 1.2
    /// </summary>
    public async Task<bool> GetCheckCorrection120List()
    {
        await GetCheckList("correction120/list");
        return Ok;
    }

    /// <summary>
    /// Возвращает чек коррекции ФФД 1.0.5 по идентификатору документа (docId)
    /// </summary>
    public async Task<bool> GetCheckCorrection105(string documentId)
    {
        DocumentId = documentId;
        await GetDocumentById("correction105");
        return Ok;
    }

    /// <summary>
    /// Получение списка чеков коррекции ФФД 1.0.5
    /// </summary>
    public async Task<bool> GetCheckCorrection105List()
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
        return await Call<ResponseTaskStatus>(HttpMethod.Get, $"task/status?{IdQuery}", null, status =>
        {
            TaskStatus = status;
            if (!string.IsNullOrEmpty(status.FiscalSign))
                FiscalSign = status.FiscalSign!;
            if (status.DocNumber > 0)
                CheckNumber = status.DocNumber;
            if (status.ShiftNumber > 0)
                ShiftNumber = status.ShiftNumber;
            if (!string.IsNullOrEmpty(status.DocId))
                DocumentId = status.DocId!;
        });
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
        return await Call<string>(HttpMethod.Get, $"check/fiscalSign?docNumber={CheckNumber}&{DeviceQuery}", null, sign =>
        {
            if (Ok)
                FiscalSign = sign;
        });
    }

    /// <summary>
    /// Печать копии чека по идентификатору документа
    /// </summary>
    public async Task<bool> PrintCheckCopy()
    {
        return await Execute(HttpMethod.Post, "check/copy", new CheckbaseParameters { DeviceName = DeviceName, DocId = DocumentId });
    }

    /// <summary>
    /// Печать копии последнего чека).
    /// </summary>
    public async Task<bool> PrintLastCheckCopy()
    {
        return await Execute(HttpMethod.Post, $"check/copy/last?{DeviceQuery}");
    }

    /// <summary>
    /// Возвращает печатную форму документа по его идентификатору (docId)
    /// </summary>
    public async Task<bool> GetPrintForm(string documentId)
    {
        DocumentId = documentId;
        return await Call<PrintFormLine[]>(HttpMethod.Get, $"task/form?{IdQuery}", null,
            lines => PrintForm = lines.Select(x => new PrintLine(x)).ToList());
    }

    /// <summary>
    /// Регистрация операции внесения наличных в денежный ящик. 
    /// </summary>
    public async Task<bool> CashIn()
    {
        return await Call<CheckDocument>(HttpMethod.Post, "cashin", CashBody(), ApplyCheckBaseDoc);
    }

    /// <summary>
    /// Регистрация операции выемки наличных из денежного ящика.
    /// </summary>
    public async Task<bool> CashOut()
    {
        return await Call<CheckDocument>(HttpMethod.Post, "cashout", CashBody(), ApplyCheckBaseDoc);
    }

    /// <summary>
    /// Открытие денежного ящика
    /// </summary>
    public async Task<bool> OpenCashdrawer()
    {
        return await Call<CheckDocument>(HttpMethod.Post, "cash/open", CheckBase(), ApplyCheckBaseDoc);
    }

    /// <summary>
    /// Получение остатка наличных в денежном ящике
    /// </summary>
    public async Task<bool> GetCash()
    {
        return await Call<CashSum>(HttpMethod.Get, $"cash?{DeviceQuery}", null, cash => CashBalance = cash.Sum);
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

        return await Execute(HttpMethod.Post, "picture", new UploadPicture
        {
            DeviceName = DeviceName,
            PictureName = string.IsNullOrEmpty(PictureId) ? Path.GetFileNameWithoutExtension(filePath) : PictureId,
            Base64 = base64,
            Alignment = (int)PictureAlignment
        });
    }

    /// <summary>
    /// Получение списка изображений
    /// </summary>
    public async Task<bool> GetPictureList()
    {
        return await Call<List<Picture>>(HttpMethod.Get, $"picture/list?{DeviceQuery}", null, pictures => Pictures = pictures);
    }

    /// <summary>
    /// Открытие сессии регистрации (проверки) кодов маркировки на ККТ
    /// </summary>
    public async Task<bool> OpenSessionRegistrationKM()
    {
        return await Execute(HttpMethod.Post, "marking/session/open", new CheckbaseParameters { DeviceName = DeviceName });
    }

    /// <summary>
    /// Закрытие сессии регистрации (проверки) кодов маркировки на ККТ
    /// </summary>
    public async Task<bool> CloseSessionRegistrationKM()
    {
        return await Execute(HttpMethod.Post, "marking/session/close", new CheckbaseParameters { DeviceName = DeviceName });
    }

    /// <summary>
    /// Локальная проверка кода маркировки на ККТ (ФФД 1.2)
    /// </summary>
    public async Task<bool> RequestKM()
    {
        if (string.IsNullOrWhiteSpace(RequestKmGuid))
            RequestKmGuid = Guid.NewGuid().ToString();

        return await Call<RequestKmResult>(HttpMethod.Post, "marking/km/request", new RequestKmParameters
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
        }, result => MarkingCheck = result);
    }

    /// <summary>
    /// Получение результата проверки кода маркировки в ОИСМ
    /// </summary>
    public async Task<bool> GetProcessingKMResult()
    {
        return await Call<ProcessingKmResult>(HttpMethod.Get, $"marking/km/result?{DeviceQuery}", null, result =>
        {
            MarkingProcessing = result;
            if (!string.IsNullOrWhiteSpace(result.Guid))
                RequestKmGuid = result.Guid!;
        });
    }

    /// <summary>
    /// Подтверждение, будет ли ранее проверенный код маркировки фактически включён в документ реализации. Действительно только в рамках открытой сессии регистрации
    /// </summary>
    public async Task<bool> ConfirmKM()
    {
        return await Execute(HttpMethod.Post, "marking/km/confirm", new RequestConfirmKm
        {
            DeviceName = DeviceName,
            GUID = RequestKmGuid,
            ConfirmationType = (int)ConfirmationType
        });
    }

    /// <summary>
    /// Печать нефискального документа.
    /// </summary>
    public async Task<bool> PrintSlip()
    {
        return await Call<CheckDocument>(HttpMethod.Post, "slip", SlipBody(), ApplyCheckBaseDoc);
    }

    /// <summary>
    /// Асинхронно поставить нефискальный документ в очередь печати
    /// </summary>
    public async Task<bool> PrintSlipAsync()
    {
        return await PostTask("slip/async", SlipBody());
    }

    /// <summary>
    /// Версия сервера ККМ.
    /// </summary>
    public async Task<bool> GetVersion()
    {
        return await Call<ServerVersionResult>(HttpMethod.Get, "version", null, version => ServerVersion = version.ServerVersion ?? "");
    }

    /// <summary>
    /// Перезагрузка кассы.
    /// </summary>
    public async Task<bool> RebootDevice()
    {
        return await Execute(HttpMethod.Post, "kkt/reboot", CheckBase());
    }

    /// <summary>
    /// Список пулов устройств.
    /// </summary>
    public async Task<bool> GetPoolList()
    {
        return await Call<string[]>(HttpMethod.Get, "pool/list", null, pools => Pools = pools);
    }

    /// <summary>
    /// Список касс в пуле.
    /// </summary>
    public async Task<bool> GetDeviceListByPool()
    {
        // Сервер возвращает имена касс пула (List<string>).
        return await Call<string[]>(HttpMethod.Get, $"kkt/list/byPool?pool={Uri.EscapeDataString(PoolName)}", null,
            names => Devices = names.Select(name => new DeviceListResponse { DeviceName = name }).ToArray());
    }

    /// <summary>
    /// Асинхронное открытие смены.
    /// </summary>
    public async Task<bool> OpenShiftAsync()
    {
        return await PostTask("shift/open/async", CheckBase());
    }

    /// <summary>
    /// Асинхронное закрытие смены.
    /// </summary>
    public async Task<bool> CloseShiftAsync()
    {
        return await PostTask("shift/z/async", CheckBase());
    }

    /// <summary>
    /// Асинхронный X-отчёт.
    /// </summary>
    public async Task<bool> ReportXAsync()
    {
        return await PostTask("shift/x/async", CheckBase());
    }

    /// <summary>
    /// Асинхронный отчёт о состоянии расчётов.
    /// </summary>
    public async Task<bool> ReportSettlementAsync()
    {
        return await PostTask("report/settlement/async", CheckBase());
    }

    /// <summary>
    /// Асинхронное внесение наличных.
    /// </summary>
    public async Task<bool> CashInAsync()
    {
        return await PostTask("cashin/async", CashBody());
    }

    /// <summary>
    /// Асинхронная выемка наличных.
    /// </summary>
    public async Task<bool> CashOutAsync()
    {
        return await PostTask("cashout/async", CashBody());
    }

    /// <summary>
    /// Список чеков за период и смену. Номер смены передаётся параметром, период (даты «с»/«по»)
    /// </summary>
    public async Task<bool> GetCheckList(int shiftNumber)
    {
        ShiftNumber = shiftNumber;
        return await Call<CheckDocument[]>(HttpMethod.Get, $"check/list?{DeviceQuery}&{DateQuery(ShiftsFrom, ShiftsTo)}&shift={ShiftNumber}", null, ApplyCheckList);
    }

    /// <summary>
    /// Печать копии чека из ФН по <see cref="DocumentId"/> (Guid или ФП чека). Только на ККТ исходного чека.
    /// </summary>
    public async Task<bool> PrintCheckCopyFn()
    {
        return await Execute(HttpMethod.Post, "check/copy/fn", new CheckbaseParameters { DeviceName = DeviceName, DocId = DocumentId });
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
        return await Call<string>(HttpMethod.Get, $"picture?{DeviceQuery}&id={Uri.EscapeDataString(PictureId)}", null, picture =>
        {
            if (Ok)
                PictureBase64Result = picture;
        });
    }

    /// <summary>
    /// Удаление картинки.
    /// </summary>
    public async Task<bool> DeletePicture()
    {
        return await Execute(HttpMethod.Delete, $"picture?{DeviceQuery}&id={Uri.EscapeDataString(PictureId)}");
    }

    /// <summary>
    /// Создание шаблона печати.
    /// </summary>
    public async Task<bool> AddTemplate()
    {
        if (!ValidateTemplate())
            return Ok;
        return await Call<string>(HttpMethod.Post, "template", TemplateBody(), name =>
        {
            if (Ok)
                TemplateName = name;
        });
    }

    /// <summary>
    /// Изменение шаблона печати по его имени. Строки задаются так же, как при создании
    /// </summary>
    public async Task<bool> UpdateTemplate()
    {
        if (!ValidateTemplate())
            return Ok;
        return await Execute(HttpMethod.Put, "template", TemplateBody());
    }

    /// <summary>
    /// Удаление шаблона печати. Имя берётся из <see cref="TemplateName"/>.
    /// </summary>
    public async Task<bool> DeleteTemplate()
    {
        if (!ValidateTemplateName(TemplateName, "шаблона печати"))
            return Ok;
        return await Execute(HttpMethod.Delete, $"template?id={Uri.EscapeDataString(TemplateName)}");
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
        return await Call<string[]>(HttpMethod.Get, "template/list", null, templates => Templates = templates);
    }

    /// <summary>
    /// Получение шаблона печати по имени. 
    /// </summary>
    public async Task<bool> GetTemplate(string name)
    {
        TemplateName = name;
        return await Call<PrintTemplate>(HttpMethod.Get, $"template?name={Uri.EscapeDataString(TemplateName)}", null, ApplyTemplate);
    }

    /// <summary>
    /// Создание шаблона чека.
    /// </summary>
    public async Task<bool> AddCheckTemplate()
    {
        if (!ValidateCheckTemplate(TemplateName))
            return Ok;
        return await Execute(HttpMethod.Post, "checkTemplate", CheckTemplateBody(TemplateName));
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
        return await Execute(HttpMethod.Put, "checkTemplate", CheckTemplateBody(TemplateName));
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
        return await Execute(HttpMethod.Delete, $"checkTemplate?id={Uri.EscapeDataString(TemplateName)}");
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
        return await Call<CheckTemplateListItem[]>(HttpMethod.Get, "checkTemplate/list", null, templates => CheckTemplates = templates);
    }

    /// <summary>
    /// Получение шаблона чека по имени
    /// </summary>
    public async Task<bool> GetCheckTemplate(string name)
    {
        TemplateName = name;
        return await Call<CheckTemplate>(HttpMethod.Get, $"checkTemplate?id={Uri.EscapeDataString(TemplateName)}", null, ApplyCheckTemplate);
    }

    /// <summary>
    /// Состояние очереди печати.
    /// </summary>
    public async Task<bool> GetQueue()
    {
        return await Call<QueueItem[]>(HttpMethod.Get, "queue", null, queue => Queue = queue);
    }

    /// <summary>
    /// Состояние задания в очереди.
    /// </summary>
    public async Task<bool> GetQueueTask(string taskId)
    {
        QueueTaskId = taskId;
        return await Call<QueueTaskState>(HttpMethod.Get, $"queue/task?taskId={Uri.EscapeDataString(QueueTaskId)}", null, task => QueueTask = task);
    }

    /// <summary>
    /// История обработки задания в очереди.
    /// </summary>
    public async Task<bool> GetQueueTaskHistory(string taskId)
    {
        QueueTaskId = taskId;
        return await Call<QueueTaskState>(HttpMethod.Get, $"queue/task/history?taskId={Uri.EscapeDataString(QueueTaskId)}", null, task =>
        {
            QueueTask = task;
            OperationHistory = task.History
                .Select(h => new OperationHistoryEntry(new OperationHistoryItem
                {
                    Time = h.Time,
                    State = h.State,
                    Description = h.Description,
                    Info = h.Info
                }))
                .ToList();
        });
    }

    /// <summary>
    /// Отмена задания в очереди.
    /// </summary>
    public async Task<bool> CancelQueueTask()
    {
        return await Execute(HttpMethod.Delete, $"queue/task?taskId={Uri.EscapeDataString(QueueTaskId)}");
    }

    /// <summary>
    /// Проверка кода маркировки через внешний сервис.
    /// </summary>
    public async Task<bool> VerifyMarking()
    {
        return await Call<MarkingVerifyResult>(HttpMethod.Post, "marking/km/verify", new MarkingCodesRequest
        {
            DeviceName = DeviceName,
            Codes = MarkingCodes.ToList()
        }, result => MarkingVerify = result);
    }

    /// <summary>
    /// Проверка кода маркировки через ТС ПИоТ.
    /// </summary>
    public async Task<bool> VerifyMarkingTsPiot()
    {
        return await Call<MarkingVerifyResult>(HttpMethod.Post, "marking/km/tspiot/verify", new MarkingCodesRequest
        {
            DeviceName = DeviceName,
            Codes = MarkingCodes.ToList()
        }, result => MarkingVerify = result);
    }

    /// <summary>
    /// Проверка кода маркировки через ЛМ ЧЗ.
    /// </summary>
    public async Task<bool> VerifyMarkingLmcz()
    {
        return await Call<MarkingVerifyResult>(HttpMethod.Post, "marking/km/lmcz/verify", new MarkingCodesRequest
        {
            DeviceName = DeviceName,
            Codes = MarkingCodes.ToList()
        }, result => MarkingVerify = result);
    }

    /// <summary>
    /// Фискализация кассы.
    /// </summary>
    public async Task<bool> Fiscalization()
    {
        return await Call<FiscalizationDoc>(HttpMethod.Post, "fiscalization", FiscalizationBody(), ApplyCheckBaseDoc);
    }

    /// <summary>
    /// Асинхронная фискализация кассы.
    /// </summary>
    public async Task<bool> FiscalizationAsync()
    {
        return await PostTask("fiscalization/async", FiscalizationBody());
    }

    /// <summary>
    /// Результат фискализации по идентификатору документа.
    /// </summary>
    public async Task<bool> GetFiscalization(string documentId)
    {
        DocumentId = documentId;
        return await Call<FiscalizationDoc>(HttpMethod.Get, $"fiscalization?{IdQuery}", null, doc =>
        {
            ApplyCheckBaseDoc(doc);
            FiscalizationDocument = new FiscalizationDocument
            {
                OperationType = doc.OperationType,
                RnNumber = doc.RnNumber ?? "",
                TaxationSystems = doc.TaxationSystems ?? "",
                Vatin = doc.Vatin ?? "",
                CompanyName = doc.CompanyName ?? "",
                FfdVersionKkt = doc.FfdVersionKkt ?? "",
                FfdVersionFn = doc.FfdVersionFn ?? "",
                IsFiscal = doc.IsFiscal,
                DocId = doc.DocId ?? "",
                DeviceName = doc.DeviceName ?? "",
                ShiftNumber = doc.ShiftNumber,
                DocNumber = doc.DocNumber,
                FiscalSign = doc.FiscalSign ?? ""
            };
            if (!string.IsNullOrEmpty(doc.RnNumber))
                RnNumber = doc.RnNumber!;
            IsFiscal = doc.IsFiscal;
        });
    }

    /// <summary>
    /// Список операций фискализации по кассе.
    /// </summary>
    public async Task<bool> GetFiscalizationList()
    {
        return await Call<FiscalizationDocument[]>(HttpMethod.Get, $"fiscalization/list?{DeviceQuery}", null, list => Fiscalizations = list);
    }

    /// <summary>
    /// Последняя операция из базы. <c>tasktype</c> — <see cref="PaymentType"/> (<see cref="CheckType"/>), <c>isProcessed</c> — <see cref="IsProcessed"/>.
    /// </summary>
    public async Task<bool> GetOperationLast()
    {
        var processed = IsProcessed ? "true" : "false";
        return await Call<DeviceTaskInfo>(HttpMethod.Get, $"operation/last?tasktype={(int)PaymentType}&isProcessed={processed}", null, ApplyOperation);
    }

    /// <summary>
    /// Операция по идентификатору документа.
    /// </summary>
    public async Task<bool> GetOperation(string documentId)
    {
        DocumentId = documentId;
        return await Call<DeviceTaskInfo>(HttpMethod.Get, $"operation?{DocIdQuery}", null, ApplyOperation);
    }

    /// <summary>
    /// История операции по идентификатору документа.
    /// </summary>
    public async Task<bool> GetOperationHistory(string documentId)
    {
        DocumentId = documentId;
        return await Call<OperationHistoryItem[]>(HttpMethod.Get, $"operation/history?{DocIdQuery}", null,
            history => OperationHistory = history.Select(h => new OperationHistoryEntry(h)).ToList());
    }

    /// <summary>
    /// TLV-данные операции.
    /// </summary>
    public async Task<bool> GetOperationTlv(string documentId)
    {
        DocumentId = documentId;
        return await Call<string>(HttpMethod.Get, $"operation/tlv?{DocIdQuery}", null, tlv =>
        {
            if (Ok)
                OperationTlv = tlv;
        });
    }

    /// <summary>
    /// Данные маркировки операции.
    /// </summary>
    public async Task<bool> GetOperationKm(string documentId)
    {
        DocumentId = documentId;
        return await Call<OperationKmRow[]>(HttpMethod.Get, $"operation/km?{DocIdQuery}", null, rows => OperationKm = rows);
    }

    /// <summary>
    /// Связанные операции.
    /// </summary>
    public async Task<bool> GetOperationRelated(string documentId)
    {
        DocumentId = documentId;
        return await Call<DeviceTaskInfo[]>(HttpMethod.Get, $"operation/related?{DocIdQuery}", null,
            related => RelatedOperations = related.Select(x => new RelatedOperation(x)).ToList());
    }

    /// <summary>
    /// Список операций за период.
    /// </summary>
    public async Task<bool> GetOperationList()
    {
        return await Call<OperationListItem[]>(HttpMethod.Get, $"operation/list?{DateQuery(ShiftsFrom, ShiftsTo)}", null, operations => Operations = operations);
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
