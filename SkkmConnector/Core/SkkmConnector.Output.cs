namespace RBSoftSkkm;

public sealed partial class SkkmConnector
{
    /// <summary>
    /// Успех последнего вызова.
    /// </summary>
    public bool Ok { get; private set; }

    /// <summary>
    /// Код ошибки сервера. 0 — нет ошибки.
    /// </summary>
    public int ErrorCode { get; private set; }

    /// <summary>
    /// Текст ошибки сервера.
    /// </summary>
    public string ErrorDescription { get; private set; } = "";

    /// <summary>
    /// Список устройств.
    /// </summary>
    public DeviceListResponse[] Devices { get; private set; } = [];

    /// <summary>
    /// Данные кассы.
    /// </summary>
    internal DataKkt? Kkt { get; private set; }

    /// <summary>
    /// Состояние ККМ.
    /// </summary>
    internal KktStatus? Status { get; private set; }

    /// <summary>
    /// Статус смены.
    /// </summary>
    internal ResponseCurrentStatus? ShiftStatus { get; private set; }

    /// <summary>
    /// Итоги смены.
    /// </summary>
    internal ResShiftTotal? ShiftTotals { get; private set; }

    /// <summary>
    /// Необнуляемые итоги за всё время работы (X/Z-отчёт).
    /// </summary>
    internal OverallTotals? OverallTotals { get; private set; }

    /// <summary>
    /// Остаток наличных.
    /// </summary>
    public decimal CashBalance { get; private set; }

    /// <summary>
    /// Список картинок.
    /// </summary>
    public List<Picture> Pictures { get; private set; } = [];

    /// <summary>
    /// Ширина строки чека в символах.
    /// </summary>
    public int LineLength { get; private set; }

    /// <summary>
    /// Ширина печатной области в пикселях.
    /// </summary>
    public int LineLengthPixels { get; private set; }

    /// <summary>
    /// Необнуляемая сумма продаж.
    /// </summary>
    public decimal NonZeroSum { get; private set; }

    /// <summary>
    /// Результат локальной проверки КМ.
    /// </summary>
    internal RequestKmResult? MarkingCheck { get; private set; }

    /// <summary>
    /// Результат проверки КМ в ОИСМ.
    /// </summary>
    internal ProcessingKmResult? MarkingProcessing { get; private set; }

    /// <summary>
    /// Документ.
    /// </summary>
    internal CheckDocument? Check { get; private set; }

    /// <summary>
    /// Заголовок последнего документа:
    /// </summary>
    internal DocumentHeader? DocumentHeader => Check?.DocumentHeader;

    /// <summary>
    /// Список документов.
    /// </summary>
    public IReadOnlyList<Check> Checks { get; private set; } = [];

    /// <summary>
    /// Список слипов.
    /// </summary>
    public IReadOnlyList<Check> Slips => Checks;

    /// <summary>
    /// Список документов внесения.
    /// </summary>
    public IReadOnlyList<Check> CashIns => Checks;

    /// <summary>
    /// Список чеков коррекции ФФД 1.0.5.
    /// </summary>
    public IReadOnlyList<Check> Corrections105 => Checks;

    /// <summary>
    /// Список чеков коррекции ФФД 1.2.
    /// </summary>
    public IReadOnlyList<Check> Corrections120 => Checks;

    /// <summary>
    /// Статус задания.
    /// </summary>
    internal ResponseTaskStatus? TaskStatus { get; private set; }

    /// <summary>
    /// Печатная форма.
    /// </summary>
    public IReadOnlyList<PrintLine> PrintForm { get; private set; } = [];

    /// <summary>
    /// Список отчётов.
    /// </summary>
    public ShiftListItem[] Shifts { get; private set; } = [];

    /// <summary>
    /// Версия сервера.
    /// </summary>
    public string ServerVersion { get; private set; } = "";

    /// <summary>
    /// Наименование продукта сервера ККМ (из Ping).
    /// </summary>
    public string ServerProduct { get; private set; } = "";

    /// <summary>
    /// Список пулов.
    /// </summary>
    public string[] Pools { get; private set; } = [];

    /// <summary>
    /// Очередь печати.
    /// </summary>
    public QueueItem[] Queue { get; private set; } = [];

    /// <summary>
    /// Состояние задания очереди.
    /// </summary>
    internal QueueTaskState? QueueTask { get; private set; }

    /// <summary>
    /// Операция.
    /// </summary>
    internal DeviceTaskInfo? Operation { get; private set; }

    /// <summary>
    /// История операции.
    /// </summary>
    public IReadOnlyList<OperationHistoryEntry> OperationHistory { get; private set; } = [];

    /// <summary>
    /// TLV операции.
    /// </summary>
    public string OperationTlv { get; private set; } = "";

    /// <summary>
    /// Коды маркировки операции.
    /// </summary>
    public OperationKmRow[] OperationKm { get; private set; } = [];

    /// <summary>
    /// Связанные операции.
    /// </summary>
    public IReadOnlyList<RelatedOperation> RelatedOperations { get; private set; } = [];

    /// <summary>
    /// Список операций.
    /// </summary>
    public OperationListItem[] Operations { get; private set; } = [];

    /// <summary>
    /// Шаблон печати: имя, тип и строки.
    /// </summary>
    internal PrintTemplate? PrintTemplate { get; private set; }

    /// <summary>
    /// Строки текущего печатного шаблона: либо полученные методом <see cref="GetTemplate"/>,
    /// либо собранные через <see cref="AddText(string, PrintFont, PrintAlignment)"/> /
    /// <see cref="AddBarcode"/> / <see cref="AddSeparatorLine"/> / <see cref="AddPicture"/>.
    /// </summary>
    public IReadOnlyList<PrintLine> TemplateLines
        => (PrintTemplate?.Lines.Count > 0
                ? (IEnumerable<PrintTemplateLine>)PrintTemplate.Lines
                : BuildPrintLines(_positions))
            .Select(x => new PrintLine(x))
            .ToList();

    /// <summary>
    /// Имена шаблонов печати на сервере.
    /// </summary>
    public string[] Templates { get; private set; } = [];

    /// <summary>
    /// Имена шаблонов печати на сервере. То же, что Templates.
    /// </summary>
    public string[] TemplateNames => Templates;

    /// <summary>
    /// Шаблон чека.
    /// </summary>
    internal CheckTemplate? CheckTemplate { get; private set; }

    /// <summary>
    /// Документ шаблона чека: позиции, оплаты, тип чека, СНО.
    /// </summary>
    internal CheckTemplateDocument? CheckTemplateDocument { get; private set; }

    /// <summary>
    /// Список шаблонов чека: имя и тип чека.
    /// </summary>
    public CheckTemplateListItem[] CheckTemplates { get; private set; } = [];

    /// <summary>
    /// Документ фискализации.
    /// </summary>
    internal FiscalizationDocument? FiscalizationDocument { get; private set; }

    /// <summary>
    /// Список фискализаций.
    /// </summary>
    public FiscalizationDocument[] Fiscalizations { get; private set; } = [];

    /// <summary>
    /// Результат проверки маркировки.
    /// </summary>
    internal MarkingVerifyResult? MarkingVerify { get; private set; }

    /// <summary>
    /// Картинка в Base64.
    /// </summary>
    public string PictureBase64Result { get; private set; } = "";

    #region Выход: скаляры фискального ответа (перенесено из CheckInput)

    // Документы / смены

    /// <summary>
    /// Идентификатор документа (docId)
    /// </summary>
    public string DocumentId { get; set; } = "";

    /// <summary>
    /// Фискальный признак документа
    /// </summary>
    public string FiscalSign { get; set; } = "";

    /// <summary>
    /// Номер смены
    /// </summary>
    public int ShiftNumber { get; set; }

    /// <summary>
    /// Номер фискального документа (ФД)
    /// </summary>
    public int CheckNumber { get; set; }

    /// <summary>
    /// Номер чека за смену
    /// </summary>
    public int CheckNumberInShift { get; set; }

    /// <summary>
    /// Регистрационный номер ККТ (РНМ)
    /// </summary>
    public string RnNumber { get; set; } = "";

    /// <summary>
    /// Адрес сайта ФНС
    /// </summary>
    public string FnsUrl { get; set; } = "";

    /// <summary>
    /// Время на сервере ККМ
    /// </summary>
    public string ServerDateTime { get; set; } = "";

    /// <summary>
    /// Дата и время документа по часам ФН 
    /// </summary>
    public string FiscalDateTime { get; set; } = "";

    /// <summary>
    /// Время ККТ
    /// </summary>
    public string DeviceDateTime { get; set; } = "";

    /// <summary>
    /// Время ПК (компьютера сервера ККМ) на момент получения статуса. Заполняется из статуса ККТ.
    /// </summary>
    public DateTime ComputerTime { get; set; }

    /// <summary>
    /// Время в часах ККТ на момент получения статуса. Заполняется из статуса ККТ.
    /// </summary>
    public DateTime DeviceTime { get; set; }
    /// <summary>
    /// Состояние смены.
    /// </summary>
    public ShiftState? CurrentShiftState { get; set; }

    /// <summary>
    /// Очередь непереданных в ОФД документов (структура-хранилище).
    /// </summary>
    private Backlog? _backlog;

    /// <summary>
    /// Срок действия ФН.
    /// </summary>
    public string FnValidityDate { get; set; } = "";

    /// <summary>
    /// Остаток ресурса ФН в днях.
    /// </summary>
    public int FnDaysResources { get; set; }

    /// <summary>
    /// Номер ФН
    /// </summary>
    public string FnNumber { get; set; } = "";

    /// <summary>
    /// ФН присутствует
    /// </summary>
    public bool IsFnPresent { get; set; }

    /// <summary>
    /// Фискальный режим ККТ. Выходное поле: заполняется из последнего ответа, где этот признак есть —
    /// информации о ККТ (<see cref="GetKktInfo"/>), статуса (<see cref="GetStatus"/>) или фискализации.
    /// Для привязки к конкретному источнику используйте <see cref="StatusIsFiscal"/> / <see cref="CheckIsFiscal"/>.
    /// </summary>
    public bool IsFiscal { get; set; }

    /// <summary>
    /// Предупреждения ФН из ответа.
    /// </summary>
    internal Warnings? FnWarnings { get; set; }

    /// <summary>
    /// Критическая ошибка ФН.
    /// </summary>
    public bool FnWarningsIsCriticalError => FnWarnings?.CriticalError ?? false;

    /// <summary>
    /// Память ФН переполнена.
    /// </summary>
    public bool FnWarningsIsMemoryOverflow => FnWarnings?.MemoryOverflow ?? false;

    /// <summary>
    /// Требуется срочная замена ФН.
    /// </summary>
    public bool FnWarningsIsNeedReplacement => FnWarnings?.NeedReplacement ?? false;

    /// <summary>
    /// Превышено время ожидания ответа от ОФД.
    /// </summary>
    public bool FnWarningsIsOfdTimeout => FnWarnings?.OfdTimeout ?? false;

    /// <summary>
    /// Исчерпан ресурс ФН.
    /// </summary>
    public bool FnWarningsIsResourceExhausted => FnWarnings?.ResourceExhausted ?? false;

    /// <summary>
    /// Количество документов в смене (из фискального ответа).
    /// </summary>
    public int ShiftDocumentsCount { get; set; }

    /// <summary>
    /// Номер чека закрытия смены (из фискального ответа).
    /// </summary>
    public int ShiftClosingCheckNumber { get; set; }

    /// <summary>
    /// Модель ККТ (из фискального ответа).
    /// </summary>
    public string DeviceModel { get; set; } = "";

    /// <summary>
    /// Заводской номер ККТ (из фискального ответа).
    /// </summary>
    public string DeviceSerialNumber { get; set; } = "";

    /// <summary>
    /// Версия прошивки ККТ (из фискального ответа).
    /// </summary>
    public string DeviceFirmwareVersion { get; set; } = "";

    /// <summary>
    /// Версия конфигурации прошивки ККТ (из фискального ответа).
    /// </summary>
    public string DeviceConfigurationVersion { get; set; } = "";

    /// <summary>
    /// Версия ФФД (из фискального ответа).
    /// </summary>
    public string DeviceFfdVersion { get; set; } = "";

    /// <summary>
    /// Версия ФФД ФН (из фискального ответа).
    /// </summary>
    public string FnFfdVersion { get; set; } = "";

    /// <summary>
    /// Тип (класс) устройства (из фискального ответа).
    /// </summary>
    public DeviceClass DeviceClass { get; set; }

    /// <summary>
    /// Часовая зона устройства (из фискального ответа).
    /// </summary>
    public int DeviceTimeZone { get; set; }

    #endregion

    /// <summary>
    /// Сбрасывает ВСЕ данные ответа (плоские поля и объекты), чтобы результат
    /// каждого запроса отражал только сам себя и не оставался от предыдущего вызова.
    /// Не трогает вход запроса (для него — <see cref="Clear"/>) и данные подключения.
    /// </summary>
    private void ResetResponse()
    {
        // Плоские скаляры ответа
        FiscalSign = "";
        FnNumber = "";
        ShiftNumber = 0;
        CheckNumber = 0;
        CheckNumberInShift = 0;
        RnNumber = "";
        FnsUrl = "";
        DocumentId = "";
        ServerDateTime = "";
        FiscalDateTime = "";
        DeviceDateTime = "";
        ComputerTime = default;
        DeviceTime = default;
        CurrentShiftState = null;
        _backlog = null;
        FnValidityDate = "";
        FnDaysResources = 0;
        IsFnPresent = false;
        IsFiscal = false;
        FnWarnings = null;
        CashBalance = 0;
        NonZeroSum = 0;
        ShiftDocumentsCount = 0;
        ShiftClosingCheckNumber = 0;
        LineLength = 0;
        LineLengthPixels = 0;
        DeviceModel = "";
        DeviceSerialNumber = "";
        DeviceFirmwareVersion = "";
        DeviceConfigurationVersion = "";
        DeviceFfdVersion = "";
        FnFfdVersion = "";
        DeviceClass = default;
        DeviceTimeZone = 0;

        // Объекты ответа
        Check = null;
        Checks = [];
        Shifts = [];
        Status = null;
        ShiftStatus = null;
        ShiftTotals = null;
        OverallTotals = null;
        Kkt = null;
        Devices = [];
        Pictures = [];
        PictureBase64Result = "";
        TaskStatus = null;
        PrintForm = [];
        Pools = [];
        Queue = [];
        QueueTask = null;
        Operation = null;
        OperationHistory = [];
        OperationTlv = "";
        OperationKm = [];
        RelatedOperations = [];
        Operations = [];
        PrintTemplate = null;
        Templates = [];
        CheckTemplate = null;
        CheckTemplateDocument = null;
        CheckTemplates = [];
        FiscalizationDocument = null;
        Fiscalizations = [];
        MarkingCheck = null;
        MarkingProcessing = null;
        MarkingVerify = null;
        ServerVersion = "";
        ServerProduct = "";
    }
}
