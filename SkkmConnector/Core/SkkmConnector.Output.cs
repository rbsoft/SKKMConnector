using System.Reflection;

namespace RBSoftSkkm;

// Выходные свойства
public sealed partial class SkkmConnector
{
    // ── Результат вызова: Success, Code, Description ──

    // Успех последнего вызова (Success из ответа). Наружу — результатом метода и ErrorCode.
    private bool _ok;

    /// <summary>Код ошибки сервера. 0 — нет ошибки.</summary>
    public int ErrorCode { get; private set; }

    /// <summary>Текст ошибки сервера.</summary>
    public string ErrorDescription { get; private set; } = "";

    // ── Таблицы ──

    /// <summary>
    /// Таблица «ключ — значение»: поля ответа сервера (поле Result).
    /// После Get…List — основные поля каждого документа с ключом «[номер] Имя» — ReadDocuments.
    /// </summary>
    public IReadOnlyList<ReportValue> ReportValues { get; private set; } = [];

    /// <summary>
    /// Счётчики по типам операций
    /// после <see cref="GetTotals"/> и отчётов о смене — за смену, после <see cref="GetOverAll"/> — необнуляемые.
    /// Строка есть только для операции, которую прислал сервер.
    /// </summary>
    public IReadOnlyList<Counter> Counter { get; private set; } = [];

    /// <summary>Список ККТ — GetDeviceList, GetDeviceListByPool.</summary>
    public DeviceListResponse[] DeviceList { get; private set; } = [];

    /// <summary>Лицензии ККТ — ReadDevice.</summary>
    public IReadOnlyList<KktLicense> KktLicenses { get; private set; } = [];

    // ── Документ и смена: ReadDocument, ReadOperationResult, ReadOutputParameters, ReadShiftTotal ──

    /// <summary>
    /// Идентификатор документа (docId). В ответе — id созданного документа. На входе — id для запросов
    /// по документу, а в <see cref="PrintCheckCorrection105"/> — id для повторной печати (передаётся,
    /// только если задан явно, а не остался от прошлого ответа).
    /// </summary>
    public string DocId
    {
        get => _docId;
        set { _docId = value; _docIdFromResponse = false; }
    }

    private string _docId = "";

    // true — DocId записан из ответа сервера; сбрасывается, когда DocId присваивают снаружи.
    private bool _docIdFromResponse;

    /// <summary>DocId, заданный явно (не оставшийся от прошлого ответа), или null.</summary>
    private string? ExplicitDocId => !_docIdFromResponse && _docId.Length > 0 ? _docId : null;

    /// <summary>Фискальный признак документа.</summary>
    public string FiscalSign { get; private set; } = "";

    /// <summary>Дата и время документа по часам ФН.</summary>
    public System.DateTime FiscalDatetime { get; private set; }

    /// <summary>Номер смены.</summary>
    public int ShiftNumber { get; set; }

    /// <summary>Состояние смены.</summary>
    public ShiftState? ShiftState { get; private set; }

    /// <summary>Номер фискального документа (ФД).</summary>
    public int CheckNumber { get; set; }

    /// <summary>Номер последнего фискального документа (из выходных параметров смены и отчётов).</summary>
    public int LastCheckNumber { get; private set; }

    /// <summary>Номер чека за смену.</summary>
    public int CheckNumberInShift { get; private set; }

    /// <summary>Количество кассовых чеков за смену.</summary>
    public int NumberOfChecks { get; private set; }

    /// <summary>Количество документов за смену.</summary>
    public int NumberOfDocuments { get; private set; }

    /// <summary>Количество чеков и чеков коррекции со всеми признаками расчётов (выходные параметры документа смены).</summary>
    public int DocumentsCounter { get; private set; }

    /// <summary>Номер чека закрытия смены.</summary>
    public int ShiftClosingCheckNumber { get; private set; }

    /// <summary>Остаток наличных в денежном ящике.</summary>
    public decimal CashBalance { get; private set; }

    /// <summary>Необнуляемая сумма продаж — GetOverAll.</summary>
    public decimal NonZeroSum { get; private set; }

    /// <summary>Сумма внесений за смену.</summary>
    public decimal ShiftIncomeSum { get; private set; }

    /// <summary>Количество внесений за смену.</summary>
    public int ShiftIncomeCount { get; private set; }

    /// <summary>Сумма выемок за смену.</summary>
    public decimal ShiftOutcomeSum { get; private set; }

    /// <summary>Количество выемок за смену.</summary>
    public int ShiftOutcomeCount { get; private set; }

    /// <summary>Номер денежного ящика.</summary>
    public int DrawerNumber { get; private set; }

    /// <summary>Документ обработан успешно.</summary>
    public bool IsProcessed { get; private set; }

    /// <summary>Версия ФФД документа.</summary>
    public string FnFFDVersion { get; private set; } = "";

    /// <summary>Тип документа.</summary>
    public CheckType? TaskType { get; private set; }

    /// <summary>Пул документа.</summary>
    public string PoolId { get; private set; } = "";

    /// <summary>Код результата обработки документа.</summary>
    public int ResultCode { get; private set; }

    /// <summary>Описание результата обработки документа.</summary>
    public string ResultDescription { get; private set; } = "";

    // ── Задание в очереди: GetTaskStatus ──

    /// <summary>Статус отправки задания на печать.</summary>
    public QueueTaskStatus? SentToPrint { get; private set; }

    /// <summary>Позиция задания в очереди.</summary>
    public int NumberInQueue { get; private set; }

    /// <summary>Размер очереди.</summary>
    public int QueueSize { get; private set; }

    /// <summary>
    /// Дата и время: постановки задания в обработку (GetTaskStatus) или формирования документа
    /// (выходные параметры смены и отчётов — открытие и закрытие смены, X-отчёт, отчёт о расчётах).
    /// </summary>
    public System.DateTime? DateTime { get; private set; }

    // ── ФН и организация: ReadHeader, блок Fn в ReadKktInfo, ReadWarnings, ReadBacklog ──

    /// <summary>Номер ФН.</summary>
    public string FnSerialNumber { get; private set; } = "";

    /// <summary>Регистрационный номер ККТ (РНМ).</summary>
    public string RnNumber { get; private set; } = "";

    /// <summary>Адрес сайта ФНС.</summary>
    public string FnFnsUrl { get; private set; } = "";

    /// <summary>Срок действия ФН.</summary>
    public string FnValidityDate { get; private set; } = "";

    /// <summary>Остаток ресурса ФН в днях.</summary>
    public int ResourcesFn { get; private set; }

    /// <summary>Название организации.</summary>
    public string OrganizationInfo { get; private set; } = "";

    /// <summary>ИНН организации.</summary>
    public string FnVatin { get; private set; } = "";

    /// <summary>Наименование ОФД.</summary>
    public string FnOfdName { get; private set; } = "";

    /// <summary>ИНН ОФД.</summary>
    public string FnOfdVatin { get; private set; } = "";

    /// <summary>Адрес расчётов.</summary>
    public string FnSaleAddress { get; private set; } = "";

    /// <summary>Место расчётов.</summary>
    public string FnSaleLocation { get; private set; } = "";

    /// <summary>Критическая ошибка ФН.</summary>
    public bool FnWarningsIsCriticalError { get; private set; }

    /// <summary>Память ФН переполнена.</summary>
    public bool FnWarningsIsMemoryOverflow { get; private set; }

    /// <summary>Требуется срочная замена ФН.</summary>
    public bool FnWarningsIsNeedReplacement { get; private set; }

    /// <summary>Превышено время ожидания ответа от ОФД.</summary>
    public bool FnWarningsIsOfdTimeout { get; private set; }

    /// <summary>Исчерпан ресурс ФН.</summary>
    public bool FnWarningsIsResourceExhausted { get; private set; }

    /// <summary>Количество непереданных в ОФД документов.</summary>
    public long BacklogDocumentsCounter { get; private set; }

    /// <summary>Номер первого непереданного документа.</summary>
    public long BacklogDocumentFirstNumber { get; private set; }

    /// <summary>Дата и время первого непереданного документа.</summary>
    public System.DateTime? BacklogDocumentFirstDateTime { get; private set; }

    // ── Статус ККТ: ReadStatus ──

    /// <summary>ФН присутствует.</summary>
    public bool IsFnPresent { get; private set; }

    /// <summary>ФН в состоянии ошибки.</summary>
    public bool IsFnError { get; private set; }

    /// <summary>Информационная система маркировки недоступна.</summary>
    public bool IsIsmDisconnected { get; private set; }

    /// <summary>Оператор фискальных данных недоступен.</summary>
    public bool IsOfdDisconnected { get; private set; }

    /// <summary>Смена открыта.</summary>
    public bool IsShiftOpened { get; private set; }

    /// <summary>Смена истекла.</summary>
    public bool IsShiftExpired { get; private set; }

    /// <summary>Денежный ящик открыт.</summary>
    public bool IsDrawerOpened { get; private set; }

    /// <summary>Наличие чековой ленты.</summary>
    public bool IsCheckPaperPresent { get; private set; }

    /// <summary>Наличие контрольной ленты.</summary>
    public bool IsControlPaperPresent { get; private set; }

    /// <summary>Сумма выручки за смену.</summary>
    public decimal TotalSum { get; private set; }

    /// <summary>Время открытия смены.</summary>
    public System.DateTime? OpenShiftDate { get; private set; }

    /// <summary>Время на Сервере ККМ: время получения статуса (GetStatus) или документа (печать, отчёты, получение документа).</summary>
    public System.DateTime ServerTime { get; private set; }

    /// <summary>Время в часах ККТ.</summary>
    public System.DateTime DeviceTime { get; private set; }

    /// <summary>Описание ошибки устройства по данным драйвера.</summary>
    public string ErrorCodeDescription { get; private set; } = "";

    /// <summary>Статус лицензии Сервера ККМ.</summary>
    public LicenseStatus? LicenseStatus { get; private set; }

    // ── ККТ и драйвер: ReadDevice, блок Driver в ReadKktInfo, GetLineLength ──

    /// <summary>Модель ККТ.</summary>
    public string DeviceModel { get; private set; } = "";

    /// <summary>Заводской номер ККТ.</summary>
    public string DeviceSerialNumber { get; private set; } = "";

    /// <summary>Версия прошивки ККТ.</summary>
    public string DeviceFirmwareVersion { get; private set; } = "";

    /// <summary>Версия конфигурации прошивки ККТ.</summary>
    public string DeviceConfigurationVersion { get; private set; } = "";

    /// <summary>Версия ФФД.</summary>
    public string DeviceFfdVersion { get; private set; } = "";

    /// <summary>Версия ФФД ФН.</summary>
    public string DeviceFnFfdVersion { get; private set; } = "";

    /// <summary>Тип устройства.</summary>
    public DeviceClass? DeviceClass { get; private set; }

    /// <summary>Часовая зона ККТ.</summary>
    public CheckTimeZone? DeviceTimeZone { get; private set; }

    /// <summary>Фискальный режим ККТ.</summary>
    public bool DeviceIsFiscal { get; private set; }

    /// <summary>Ширина строки чека в символах.</summary>
    public int DeviceLineLength { get; private set; }

    /// <summary>Ширина печатной области в пикселях.</summary>
    public int DeviceLineLengthPixels { get; private set; }

    /// <summary>Тип драйвера.</summary>
    public string DriverType { get; private set; } = "";

    /// <summary>Версия драйвера.</summary>
    public string DriverVersion { get; private set; } = "";

    /// <summary>Производитель драйвера.</summary>
    public string DriverVendor { get; private set; } = "";

    // ── Сервер: Ping, GetVersion ──

    /// <summary>Версия Сервера ККМ.</summary>
    public string ServerVersion { get; private set; } = "";

    /// <summary>Наименование продукта Сервера ККМ.</summary>
    public string Product { get; private set; } = "";

    // ── Маркировка: RequestKM, GetProcessingKMResult, VerifyMarking ──

    /// <summary>Связь с ОИСМ есть на момент запроса.</summary>
    public bool IsmConnected { get; private set; }

    /// <summary>Формат кода маркировки корректен.</summary>
    public bool FormatChecking { get; private set; }

    /// <summary>Код маркировки проверен фискальным накопителем.</summary>
    public bool Checking { get; private set; }

    /// <summary>Результат проверки кода маркировки фискальным накопителем положительный.</summary>
    public bool CheckingResult { get; private set; }

    /// <summary>Штрихкод после приведения к виду со спецсимволами GS.</summary>
    public string MarkingBarcode { get; private set; } = "";

    /// <summary>Итог проверки кода маркировки в ОИСМ (поле Result).</summary>
    public bool KmResult { get; private set; }

    /// <summary>Статус информации о коде маркировки (тег 2109).</summary>
    public int? StatusInfo { get; private set; }

    /// <summary>Код обработки запроса (тег 2105).</summary>
    public int HandleCode { get; private set; }

    /// <summary>Статус получения результата от ОИСМ: 0 — получен, 1 — ещё нет, 2 — не может быть получен.</summary>
    public int RequestStatus { get; private set; }

    /// <summary>Результат совместной онлайн-проверки одного кода.</summary>
    public CodeMarkInfo? CodeMarkInfo { get; private set; }

    /// <summary>Код результата обращения к сервису проверки кодов.</summary>
    public int MarkingAnswerCode { get; private set; }

    /// <summary>Описание результата проверки кодов.</summary>
    public string MarkingDescription { get; private set; } = "";

    /// <summary>Идентификатор запроса в сервисе проверки.</summary>
    public string ReqId { get; private set; } = "";

    /// <summary>Время проверки, Unix-время в миллисекундах.</summary>
    public long ReqTimestamp { get; private set; }

    /// <summary>Статус локального модуля.</summary>
    public LocalModuleStatus? MarkingModuleStatus { get; private set; }

    /// <summary>Нужно скачать или обновить локальную базу модуля.</summary>
    public bool RequiresDownload { get; private set; }

    /// <summary>Время последнего обновления локальной базы. 0 — нет данных.</summary>
    public long MarkingLastUpdate { get; private set; }

    /// <summary>Время последней синхронизации с ГИС МТ. 0 — нет данных.</summary>
    public long MarkingLastSync { get; private set; }

    /// <summary>Проверка выполнена офлайн через локальный модуль.</summary>
    public bool IsCheckedOffline { get; private set; }

    /// <summary>Версия локального модуля.</summary>
    public string MarkingModuleVersion { get; private set; } = "";

    /// <summary>Идентификатор экземпляра локального модуля.</summary>
    public string MarkingModuleId { get; private set; } = "";

    /// <summary>Адрес локального модуля, к которому было обращение.</summary>
    public string MarkingServiceUrl { get; private set; } = "";

    /// <summary>Результаты проверки по каждому коду.</summary>
    public IReadOnlyList<CodeMarkInfo> MarkingCodes { get; private set; } = [];

    // Выходные свойства — все свойства с private set и DocId, ShiftNumber, CheckNumber, CashAmount (вход и выход).
    // Перед разбором каждого ответа все они сбрасываются — новое свойство с private set попадает сюда само.
    private static readonly PropertyInfo[] ResponseProperties = typeof(SkkmConnector).GetProperties()
        .Where(p => p.Name is nameof(DocId) or nameof(ShiftNumber) or nameof(CheckNumber) or nameof(CashAmount) || p.SetMethod is { IsPrivate: true })
        .ToArray();

    /// <summary>
    /// Сброс выходных свойств
    /// </summary>
    private void ResetResponse()
    {
        foreach (var p in ResponseProperties)
            p.SetValue(this, EmptyValue(p.PropertyType));
    }

    private static object? EmptyValue(Type type)
    {
        if (type == typeof(string))
            return "";
        if (type.IsArray)
            return Array.CreateInstance(type.GetElementType()!, 0);
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReadOnlyList<>))
            return Array.CreateInstance(type.GetGenericArguments()[0], 0);
        return null;
    }
}
