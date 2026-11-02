namespace RBSoftSkkm;

// Публичные методы — запросы к Серверу ККМ
public sealed partial class SkkmConnector
{
    // ── Сервер и ККТ ──

    /// <summary>Проверка доступности сервера ККМ. Не требует передачи ключа доступа (api_key).</summary>
    public Task<bool> Ping() => Call(HttpMethod.Get, "ping", null, r =>
    {
        Product = r.Str("product");
        ServerVersion = r.Str("version");
    });

    /// <summary>Версия сервера ККМ.</summary>
    public Task<bool> GetVersion() => Call(HttpMethod.Get, "version", null, r => ServerVersion = r.Str("ServerVersion"));

    /// <summary>Получение списка зарегистрированных ККТ.</summary>
    public Task<bool> GetDeviceList() => Call(HttpMethod.Get, "kkt/list", null, r =>
        DeviceList = r.Items().Select(d => new DeviceListResponse
        {
            DeviceName = d.Str("DeviceName"),
            Driver = d.Enum<DeviceType>("Driver") ?? default,
            Pool = d.Str("Pool"),
            DeviceStatusDescription = d.Str("DeviceStatusDescription"),
        }).ToArray());

    /// <summary>Список касс в пуле (сервер возвращает имена касс).</summary>
    public Task<bool> GetDeviceListByPool() => Call(HttpMethod.Get, $"kkt/list/byPool?pool={Uri.EscapeDataString(Pool)}", null, r =>
        DeviceList = r.Items().Select(name => new DeviceListResponse { DeviceName = name.Text() }).ToArray());

    /// <summary>Получение подробной информации об устройстве ККТ.</summary>
    public Task<bool> GetKktInfo() => Call(HttpMethod.Get, $"kkt?{DeviceQuery}", null, ReadKktInfo);

    /// <summary>Получение расширенного статуса ККТ.</summary>
    public Task<bool> GetStatus() => Call(HttpMethod.Get, $"kkt/status?{DeviceQuery}", null, ReadStatus);

    /// <summary>Получение краткого статуса смены и очереди ОФД.</summary>
    public Task<bool> GetShiftStatus() => Call(HttpMethod.Get, $"kkt/shift/status?{DeviceQuery}", null, r =>
    {
        ShiftNumber = r.Int("ShiftNumber");
        CheckNumber = r.Int("CheckNumber");
        ShiftState = r.Enum<ShiftState>("ShiftState");
        ReadBacklog(r.Get("Backlog"));
    });

    /// <summary>Получение максимальной ширины строки чека устройства.</summary>
    public Task<bool> GetLineLength() => Call(HttpMethod.Get, $"kkt/lineLength?{DeviceQuery}", null, r =>
    {
        DeviceLineLength = r.Int("LineLength");
        DeviceLineLengthPixels = r.Int("LineLengthPixels");
    });

    /// <summary>Получение счётчиков за смену.</summary>
    public Task<bool> GetTotals() => Call(HttpMethod.Get, $"kkt/counters/shift?{DeviceQuery}", null, r =>
    {
        ReadShiftTotal(r);
        if (r.Int("ShiftNumber") > 0)
            ShiftNumber = r.Int("ShiftNumber");
        if (r.Get("CashDrawer").Exists())
            CashBalance = r.Get("CashDrawer").Dec("Sum");
    });

    /// <summary>Получение необнуляемых (накопительных) счётчиков ККТ.</summary>
    public Task<bool> GetOverAll() => Call(HttpMethod.Get, $"kkt/counters/overall?{DeviceQuery}", null, r =>
    {
        Counter = ReadCounters(r.Get("Counters"));
        NonZeroSum = r.Get("Counters").Get("Sales").Dec("Sum");
        if (r.Get("CashDrawer").Exists())
            CashAmount = r.Get("CashDrawer").Dec("Sum"); // наличные в ящике после GetOverAll — в CashAmount
    });

    // ── Смена и отчёты ──

    /// <summary>Открытие кассовой смены.</summary>
    public Task<bool> OpenShift() => Call(HttpMethod.Post, "shift/open", CheckBase(), ReadShiftResult);

    /// <summary>Асинхронное открытие смены.</summary>
    public Task<bool> OpenShiftAsync() => PostTask("shift/open/async", CheckBase());

    /// <summary>Закрытие кассовой смены (Z-отчёт).</summary>
    public Task<bool> CloseShift() => Call(HttpMethod.Post, "shift/z", CheckBase(), ReadShiftResult);

    /// <summary>Асинхронное закрытие смены.</summary>
    public Task<bool> CloseShiftAsync() => PostTask("shift/z/async", CheckBase());

    /// <summary>Формирование X-отчёта (без закрытия смены).</summary>
    public Task<bool> ReportX() => Call(HttpMethod.Post, "shift/x", CheckBase(), ReadShiftResult);

    /// <summary>Асинхронный X-отчёт.</summary>
    public Task<bool> ReportXAsync() => PostTask("shift/x/async", CheckBase());

    /// <summary>Формирование отчёта о текущем состоянии расчётов.</summary>
    public Task<bool> ReportSettlement() => Call(HttpMethod.Post, "report/settlement", CheckBase(), ReadShiftResult);

    /// <summary>Асинхронный отчёт о состоянии расчётов.</summary>
    public Task<bool> ReportSettlementAsync() => PostTask("report/settlement/async", CheckBase());

    /// <summary>Возвращает результат открытия смены по идентификатору документа (docId).</summary>
    public Task<bool> GetOpenShiftResult() => GetDocument("shift/open");

    /// <summary>Возвращает Z-отчёт по идентификатору документа (docId).</summary>
    public Task<bool> GetCloseShiftResult() => GetDocument("shift/z");

    /// <summary>Возвращает X-отчёт по идентификатору документа (docId).</summary>
    public Task<bool> GetReportXResult() => GetDocument("shift/x");

    /// <summary>Возвращает отчёт о состоянии расчётов по идентификатору документа (docId).</summary>
    public Task<bool> GetReportSettlementResult() => GetDocument("report/settlement");

    /// <summary>Получение списка открытий смен за период.</summary>
    public Task<bool> GetOpenShiftList() => GetReportList("shift/open/list");

    /// <summary>Получение списка Z-отчётов за период.</summary>
    public Task<bool> GetCloseShiftList() => GetReportList("shift/z/list");

    /// <summary>Получение списка X-отчётов за период.</summary>
    public Task<bool> GetReportXList() => GetReportList("shift/x/list");

    /// <summary>Список отчётов о состоянии расчётов по устройству за период.</summary>
    public Task<bool> GetReportSettlementList() => GetReportList("report/settlement/list");

    // ── Чеки ──

    /// <summary>Печать кассового чека.</summary>
    public Task<bool> PrintCheck() => Call(HttpMethod.Post, "check", CheckBody(), ReadFiscalResult);

    /// <summary>Асинхронно поставить фискальный чек в очередь печати.</summary>
    public Task<bool> PrintCheckAsync() => PostTask("check/async", CheckBody());

    /// <summary>Возвращает результат операции по идентификатору документа (docId).</summary>
    public Task<bool> GetCheck() => GetDocument("check");

    /// <summary>Список чеков за период и смену. Номер смены — ShiftNumber, период — FromDate / ToDate.</summary>
    public Task<bool> GetCheckList() => GetDocumentList($"check/list?{DeviceQuery}&{DateQuery(FromDate, ToDate)}&shift={ShiftNumber}");

    /// <summary>Получение фискального признака (ФП) по номеру фискального документа (ФД).</summary>
    public Task<bool> GetFiscalSign() => Call(HttpMethod.Get, $"check/fiscalSign?docNumber={CheckNumber}&{DeviceQuery}", null, r =>
    {
        if (_ok)
            FiscalSign = r.Text();
    });

    /// <summary>Печать копии чека по идентификатору документа.</summary>
    public Task<bool> PrintCheckCopy() => Call(HttpMethod.Post, "check/copy", Obj(("DeviceName", DeviceName), ("DocId", DocId)));

    /// <summary>Печать копии последнего чека.</summary>
    public Task<bool> PrintCheckCopyLast() => Call(HttpMethod.Post, $"check/copy/last?{DeviceQuery}");

    // ── Чеки коррекции ──

    /// <summary>Печать чека коррекции для ФФД 1.2.</summary>
    public Task<bool> PrintCheckCorrection120() => Call(HttpMethod.Post, "correction120", CheckBody(), ReadFiscalResult);

    /// <summary>Асинхронно печатает чек коррекции для ФФД 1.2.</summary>
    public Task<bool> PrintCheckCorrection120Async() => PostTask("correction120/async", CheckBody());

    /// <summary>Печать чека коррекции для ФФД 1.0.5.</summary>
    public Task<bool> PrintCheckCorrection105() => Call(HttpMethod.Post, "correction105", Correction105Body(), ReadFiscalResult);

    /// <summary>Асинхронно ставит печать чека коррекции для ФФД 1.0.5.</summary>
    public Task<bool> PrintCheckCorrection105Async() => PostTask("correction105/async", Correction105Body());

    /// <summary>Возвращает чек коррекции ФФД 1.2 по идентификатору документа (docId).</summary>
    public Task<bool> GetCorrection120() => GetDocument("correction120");

    /// <summary>Получение списка чеков коррекции ФФД 1.2.</summary>
    public Task<bool> GetCorrection120List() => GetDocumentList($"correction120/list?{DeviceQuery}");

    /// <summary>Возвращает чек коррекции ФФД 1.0.5 по идентификатору документа (docId).</summary>
    public Task<bool> GetCorrection105() => GetDocument("correction105");

    /// <summary>Получение списка чеков коррекции ФФД 1.0.5.</summary>
    public Task<bool> GetCorrection105List() => GetDocumentList($"correction105/list?{DeviceQuery}");

    // ── Деньги ──

    /// <summary>Регистрация операции внесения наличных в денежный ящик.</summary>
    public Task<bool> CashIn() => Call(HttpMethod.Post, "cashin", CashBody(), ReadCashDocument);

    /// <summary>Асинхронное внесение наличных.</summary>
    public Task<bool> CashInAsync() => PostTask("cashin/async", CashBody());

    /// <summary>Регистрация операции выемки наличных из денежного ящика.</summary>
    public Task<bool> CashOut() => Call(HttpMethod.Post, "cashout", CashBody(), ReadCashDocument);

    /// <summary>Асинхронная выемка наличных.</summary>
    public Task<bool> CashOutAsync() => PostTask("cashout/async", CashBody());

    /// <summary>Возвращает результат операции внесения наличных по идентификатору операции (docId).</summary>
    public Task<bool> GetCashIn() => GetDocument("cashin", ReadCashDocument);

    /// <summary>Возвращает результат операции выемки наличных по идентификатору операции (docId).</summary>
    public Task<bool> GetCashOut() => GetDocument("cashout", ReadCashDocument);

    /// <summary>Открытие денежного ящика.</summary>
    public Task<bool> OpenCashdrawer() => Call(HttpMethod.Post, "cash/open", CheckBase(), ReadDocument);

    /// <summary>Получение остатка наличных в денежном ящике.</summary>
    public Task<bool> GetCash() => Call(HttpMethod.Get, $"cash?{DeviceQuery}", null, r => CashBalance = r.Dec("Sum"));

    // ── Нефискальный документ ──

    /// <summary>Печать нефискального документа.</summary>
    public Task<bool> PrintSlip() => Call(HttpMethod.Post, "slip", SlipBody(), ReadDocument);

    /// <summary>Асинхронно поставить нефискальный документ в очередь печати.</summary>
    public Task<bool> PrintSlipAsync() => PostTask("slip/async", SlipBody());

    /// <summary>Получение слипа по идентификатору документа.</summary>
    public Task<bool> GetSlip() => GetDocument("slip");

    /// <summary>Список слипов по кассе.</summary>
    public Task<bool> GetSlipList() => GetDocumentList($"slip/list?{DeviceQuery}");

    // ── Маркировка ──

    /// <summary>Открытие сессии регистрации (проверки) кодов маркировки на ККТ.</summary>
    public Task<bool> OpenSessionRegistrationKM() => Call(HttpMethod.Post, "marking/session/open", Obj(("DeviceName", DeviceName)));

    /// <summary>Закрытие сессии регистрации (проверки) кодов маркировки на ККТ.</summary>
    public Task<bool> CloseSessionRegistrationKM() => Call(HttpMethod.Post, "marking/session/close", Obj(("DeviceName", DeviceName)));

    /// <summary>Локальная проверка кода маркировки на ККТ (ФФД 1.2). Код запроса (Guid) каждый раз новый.</summary>
    public Task<bool> RequestKM()
    {
        Guid = System.Guid.NewGuid().ToString();
        return Call(HttpMethod.Post, "marking/km/request", RequestKmBody(), ReadRequestKm);
    }

    /// <summary>Получение результата проверки кода маркировки в ОИСМ.</summary>
    public Task<bool> GetProcessingKMResult() => Call(HttpMethod.Get, $"marking/km/result?{DeviceQuery}", null, ReadProcessingKm);

    /// <summary>
    /// Подтверждение, будет ли ранее проверенный код маркировки фактически включён в документ реализации.
    /// Действительно только в рамках открытой сессии регистрации.
    /// </summary>
    public Task<bool> ConfirmKM() => Call(HttpMethod.Post, "marking/km/confirm", Obj(
        ("DeviceName", DeviceName), ("GUID", Guid), ("ConfirmationType", (int)ConfirmationType)));

    /// <summary>Проверка кода маркировки через внешний сервис.</summary>
    public Task<bool> VerifyMarking() => Call(HttpMethod.Post, "marking/km/verify", Obj(("DeviceName", DeviceName), ("Codes", CodeList())), ReadMarkingVerify);

    /// <summary>Проверка кода маркировки через ТС ПИоТ.</summary>
    public Task<bool> VerifyMarkingTsPiot() => Call(HttpMethod.Post, "marking/km/tspiot/verify", Obj(("DeviceName", DeviceName), ("Codes", CodeList())), ReadMarkingVerify);

    /// <summary>Проверка кода маркировки через ЛМ ЧЗ.</summary>
    public Task<bool> VerifyMarkingLmcz() => Call(HttpMethod.Post, "marking/km/lmcz/verify", Obj(("DeviceName", DeviceName), ("Codes", CodeList())), ReadMarkingVerify);

    // ── Задания и очередь ──

    /// <summary>Возвращает статус выполнения задания по идентификатору документа (docId).</summary>
    public Task<bool> GetTaskStatus() => Call(HttpMethod.Get, $"task/status?{IdQuery}", null, r =>
    {
        DateTime = r.Date("Date");
        SentToPrint = r.Enum<QueueTaskStatus>("SentToPrint");
        NumberInQueue = r.Int("NumberInQueue");
        QueueSize = r.Int("QueueSize");
        TaskType = r.Enum<CheckType>("TaskType");
        PoolId = r.Str("PoolId");
        ResultCode = r.Int("ResultCode");
        ResultDescription = r.Str("ResultDescription");
        ReadHeader(r.Get("DocumentHeader"));
        DeviceSerialNumber = r.Get("DocumentHeader").Str("SerialNumber");
        FnSerialNumber = r.Get("DocumentHeader").Str("Fn");
        if (r.Str("FiscalSign").Length > 0)
            FiscalSign = r.Str("FiscalSign");
        if (r.Int("DocNumber") > 0)
            CheckNumber = r.Int("DocNumber");
        if (r.Int("ShiftNumber") > 0)
            ShiftNumber = r.Int("ShiftNumber");
        if (r.Str("DocId").Length > 0)
            DocId = r.Str("DocId");
    });

    /// <summary>Состояние задания в очереди. В ReportValues девять строк статуса, не весь JSON задания.</summary>
    public Task<bool> GetQueueTask() => Call(HttpMethod.Get, $"queue/task?taskId={Uri.EscapeDataString(DocId)}", null, ReadQueueTask);
}
