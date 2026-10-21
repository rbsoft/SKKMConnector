using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Документ с сервера (сервер: CheckBase и наследники — чек, коррекция, отчёт, внесение/выемка, слип).
/// Поля всех типов документов в одной модели; у конкретного документа незаполненные поля остаются по умолчанию.
/// </summary>
public class CheckDocument
{
    /// <summary>
    /// Позиции чека.
    /// </summary>
    [JsonPropertyName("CheckItems")]
    public CheckItem[]? CheckItems { get; set; }

    /// <summary>
    /// Подтверждён в ФН.
    /// </summary>
    [JsonPropertyName("TrustedInFn")]
    public bool TrustedInFn { get; set; }

    /// <summary>
    /// Признак замены ставки НДС (замещение налога).
    /// </summary>
    [JsonPropertyName("IsReplaceTax")]
    public bool IsReplaceTax { get; set; }

    /// <summary>
    /// Фискальный документ.
    /// </summary>
    [JsonPropertyName("IsFiscal")]
    public bool IsFiscal { get; set; }

    /// <summary>
    /// Сдача.
    /// </summary>
    [JsonPropertyName("Change")]
    public decimal Change { get; set; }

    /// <summary>
    /// Сумма с учётом скидки. Для внесения и выемки — фактическая сумма операции.
    /// </summary>
    [JsonPropertyName("Sum")]
    public decimal Sum { get; set; }

    /// <summary>
    /// Остаток наличных в денежном ящике (внесение, выемка).
    /// </summary>
    [JsonPropertyName("CashSum")]
    public decimal? CashSum { get; set; }

    /// <summary>
    /// Номер денежного ящика (открытие ящика).
    /// </summary>
    [JsonPropertyName("DrawerNumber")]
    public int DrawerNumber { get; set; }

    /// <summary>
    /// Признак применения ККТ при осуществлении расчёта в безналичном порядке в сети «Интернет».
    /// </summary>
    [JsonPropertyName("OperationOnline")]
    public bool OperationOnline { get; set; }

    /// <summary>
    /// Номер телефона или электронная почта клиента.
    /// </summary>
    [JsonPropertyName("ClientContact")]
    public string? ClientContact { get; set; }

    /// <summary>
    /// Сведения о покупателе (клиенте).
    /// </summary>
    [JsonPropertyName("CustomerDetail")]
    public CheckCustomer? CustomerDetail { get; set; }

    /// <summary>
    /// Данные для отображения QR-кода чека.
    /// </summary>
    [JsonPropertyName("QrData")]
    public QrCheckData? QrData { get; set; }

    /// <summary>
    /// Оплаты.
    /// </summary>
    [JsonPropertyName("Payments")]
    public CheckPayments? Payments { get; set; }

    /// <summary>
    /// Детализация безналичных оплат.
    /// </summary>
    [JsonPropertyName("ElectronicPayments")]
    public ElectronicPayment[]? ElectronicPayments { get; set; }

    /// <summary>
    /// Заголовок документа.
    /// </summary>
    [JsonPropertyName("DocumentHeader")]
    public DocumentHeader? DocumentHeader { get; set; }

    /// <summary>
    /// Строки напечатанного документа (нефискальный документ).
    /// </summary>
    [JsonPropertyName("Lines")]
    public PrintFormLine[]? Lines { get; set; }

    /// <summary>
    /// Регистрация чека без печати на ленте.
    /// </summary>
    [JsonPropertyName("Electronically")]
    public bool Electronically { get; set; }

    /// <summary>
    /// Код налогообложения (СНО): 0 — ОСН, 1 — УСН, 2 — УСНД_Р, 3 — ЕНВД, 4 — ЕСН, 5 — ПСН.
    /// </summary>
    [JsonPropertyName("TaxType")]
    public int TaxType { get; set; }

    /// <summary>
    /// Часовая зона: 0 — авто; 1 — МСК-1 / UTC+2; … 11 — МСК+9 / UTC+12.
    /// </summary>
    [JsonPropertyName("TimeZone")]
    public int TimeZone { get; set; }

    /// <summary>
    /// Данные коррекции (чеки коррекции 1.2 и 1.05).
    /// </summary>
    [JsonPropertyName("CorrectionData")]
    public CorrectionData? CorrectionData { get; set; }

    /// <summary>
    /// Дополнительный реквизит чека (тег 1192).
    /// </summary>
    [JsonPropertyName("AdditionalAttribute")]
    public string? AdditionalAttribute { get; set; }

    /// <summary>
    /// Номер сессии. Используется для GET check/list.
    /// </summary>
    [JsonPropertyName("ShiftNumber")]
    public int ShiftNumber { get; set; }

    /// <summary>
    /// Номер фискального документа.
    /// </summary>
    [JsonPropertyName("DocNumber")]
    public int DocNumber { get; set; }

    /// <summary>
    /// Номер фискального документа за смену.
    /// </summary>
    [JsonPropertyName("DocNumberInShift")]
    public int DocNumberInShift { get; set; }

    /// <summary>
    /// Фискальный признак документа.
    /// </summary>
    [JsonPropertyName("FiscalSign")]
    public string? FiscalSign { get; set; }

    /// <summary>
    /// Серийный номер фискального накопителя.
    /// </summary>
    [JsonPropertyName("Fn")]
    public string? Fn { get; set; }

    /// <summary>
    /// Время регистрации операции по часам ККМ.
    /// </summary>
    [JsonPropertyName("FiscalDate")]
    public DateTime FiscalDate { get; set; }

    /// <summary>
    /// Имя кассира.
    /// </summary>
    [JsonPropertyName("CashierName")]
    public string? CashierName { get; set; }

    /// <summary>
    /// ИНН кассира.
    /// </summary>
    [JsonPropertyName("CashierVatin")]
    public string? CashierVatin { get; set; }

    /// <summary>
    /// Адрес проведения расчётов.
    /// </summary>
    [JsonPropertyName("SaleAddress")]
    public string? SaleAddress { get; set; }

    /// <summary>
    /// Место проведения расчётов.
    /// </summary>
    [JsonPropertyName("SaleLocation")]
    public string? SaleLocation { get; set; }

    /// <summary>
    /// Версия ФФД.
    /// </summary>
    [JsonPropertyName("FfdVersion")]
    public string? FfdVersion { get; set; }

    /// <summary>
    /// Структура значений тегов документа.
    /// </summary>
    [JsonPropertyName("Tlv")]
    public string? Tlv { get; set; }

    /// <summary>
    /// Тип чека (<see cref="CheckType"/>).
    /// </summary>
    [JsonPropertyName("TaskType")]
    public CheckType TaskType { get; set; }

    /// <summary>
    /// Идентификатор документа.
    /// </summary>
    [JsonPropertyName("DocId")]
    public string? DocId { get; set; }

    /// <summary>
    /// Идентификатор исходного документа (у копий и связанных операций).
    /// </summary>
    [JsonPropertyName("BaseDocId")]
    public string? BaseDocId { get; set; }

    /// <summary>
    /// Дата создания документа.
    /// </summary>
    [JsonPropertyName("Date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Идентификатор терминала, с которого пришёл документ.
    /// </summary>
    [JsonPropertyName("TerminalId")]
    public string? TerminalId { get; set; }

    /// <summary>
    /// Имя устройства.
    /// </summary>
    [JsonPropertyName("DeviceName")]
    public string? DeviceName { get; set; }

    /// <summary>
    /// Пул, который назначен чеку.
    /// </summary>
    [JsonPropertyName("PoolId")]
    public string? PoolId { get; set; }

    /// <summary>
    /// Результат обработки.
    /// </summary>
    [JsonPropertyName("ResultCode")]
    public int ResultCode { get; set; }

    /// <summary>
    /// Описание результата.
    /// </summary>
    [JsonPropertyName("ResultDescription")]
    public string? ResultDescription { get; set; }

    /// <summary>
    /// Признак удачного завершения обработки.
    /// </summary>
    [JsonPropertyName("Processed")]
    public bool Processed { get; set; }

    /// <summary>
    /// Версия сервера ККМ.
    /// </summary>
    [JsonPropertyName("ServerVersion")]
    public string? ServerVersion { get; set; }

    /// <summary>
    /// Сведения о ККТ на момент документа.
    /// </summary>
    [JsonPropertyName("DeviceInfo")]
    public Device? DeviceInfo { get; set; }

    /// <summary>
    /// Сменные итоги (X/Z-отчёт).
    /// </summary>
    [JsonPropertyName("ShiftTotal")]
    public ResShiftTotal? ShiftTotal { get; set; }

    /// <summary>
    /// Необнуляемые итоги за всё время работы (X/Z-отчёт).
    /// </summary>
    [JsonPropertyName("OverallTotals")]
    public OverallTotals? OverallTotals { get; set; }

    /// <summary>
    /// Состояние обмена с ОФД (открытие смены, X/Z-отчёт).
    /// </summary>
    [JsonPropertyName("OfdStatus")]
    public OfdStatus? OfdStatus { get; set; }

    /// <summary>
    /// Выходные параметры документа (открытие смены, X/Z-отчёт, отчёт о расчётах).
    /// </summary>
    [JsonPropertyName("OutputParameters")]
    public FiscalOutputParameters? OutputParameters { get; set; }

    /// <summary>
    /// Адрес электронной почты отправителя чека.
    /// </summary>
    [JsonPropertyName("SenderEmail")]
    public string? SenderEmail { get; set; }

    /// <summary>
    /// Строки заголовка документа.
    /// </summary>
    [JsonPropertyName("Before")]
    public PrintFormLine[]? Before { get; set; }

    /// <summary>
    /// Строки подвала документа.
    /// </summary>
    [JsonPropertyName("After")]
    public PrintFormLine[]? After { get; set; }

    /// <summary>
    /// Количество аннулирований (X/Z-отчёт).
    /// </summary>
    [JsonPropertyName("AnullatesCount")]
    public int AnullatesCount { get; set; }

    /// <summary>
    /// Сумма НДС 0% (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum0")]
    public decimal TaxSum0 { get; set; }

    /// <summary>
    /// Сумма НДС 5% (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum5")]
    public decimal TaxSum5 { get; set; }

    /// <summary>
    /// Сумма НДС 7% (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum7")]
    public decimal TaxSum7 { get; set; }

    /// <summary>
    /// Сумма НДС 10% (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum10")]
    public decimal TaxSum10 { get; set; }

    /// <summary>
    /// Сумма НДС 18% (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum18")]
    public decimal TaxSum18 { get; set; }

    /// <summary>
    /// Сумма НДС 20% (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum20")]
    public decimal TaxSum20 { get; set; }

    /// <summary>
    /// Сумма НДС 22% (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum22")]
    public decimal TaxSum22 { get; set; }

    /// <summary>
    /// Сумма без НДС (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSumNone")]
    public decimal TaxSumNone { get; set; }

    /// <summary>
    /// Сумма НДС 5/105 (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum105")]
    public decimal TaxSum105 { get; set; }

    /// <summary>
    /// Сумма НДС 7/107 (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum107")]
    public decimal TaxSum107 { get; set; }

    /// <summary>
    /// Сумма НДС 10/110 (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum110")]
    public decimal TaxSum110 { get; set; }

    /// <summary>
    /// Сумма НДС 18/118 (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum118")]
    public decimal TaxSum118 { get; set; }

    /// <summary>
    /// Сумма НДС 20/120 (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum120")]
    public decimal TaxSum120 { get; set; }

    /// <summary>
    /// Сумма НДС 22/122 (коррекция 1.05).
    /// </summary>
    [JsonPropertyName("TaxSum122")]
    public decimal TaxSum122 { get; set; }
}
