using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Документ шаблона чека в том виде, в каком его хранит сервер
/// </summary>
public sealed class CheckTemplateDocument
{
    /// <summary>
    /// Тип чека
    /// </summary>
    [JsonPropertyName("TaskType")]
    public CheckType PaymentType { get; set; }

    /// <summary>
    /// Система налогообложения
    /// </summary>
    [JsonPropertyName("TaxType")]
    public TaxSystem TaxVariant { get; set; }

    /// <summary>
    /// Часовая зона.
    /// </summary>
    [JsonPropertyName("TimeZone")]
    public CheckTimeZone TimeZone { get; set; }

    /// <summary>
    /// Признак применения ККТ при расчёте в безналичном порядке в сети «Интернет».
    /// </summary>
    [JsonPropertyName("OperationOnline")]
    public bool OperationOnline { get; set; }

    /// <summary>
    /// Замена системы налогообложения настройками ККТ.
    /// </summary>
    [JsonPropertyName("IsReplaceTax")]
    public bool IsReplaceTax { get; set; }

    /// <summary>
    /// Формирование чека только в электронном виде (без печати на бумаге).
    /// </summary>
    [JsonPropertyName("Electronically")]
    public bool Electronically { get; set; }

    /// <summary>
    /// Фискальный документ.
    /// </summary>
    [JsonPropertyName("IsFiscal")]
    public bool IsFiscal { get; set; }

    /// <summary>
    /// Подтверждён в ФН.
    /// </summary>
    [JsonPropertyName("TrustedInFn")]
    public bool TrustedInFn { get; set; }

    /// <summary>
    /// Сумма с учётом скидки.
    /// </summary>
    [JsonPropertyName("Sum")]
    public decimal Sum { get; set; }

    /// <summary>
    /// Сдача.
    /// </summary>
    [JsonPropertyName("Change")]
    public decimal Change { get; set; }

    /// <summary>
    /// Номер документа «Уведомление о реализации МТ», в который включаются данные чека.
    /// </summary>
    [JsonPropertyName("MtNumber")]
    public int MtNumber { get; set; }

    /// <summary>
    /// Ошибка при печати бумажной формы чека.
    /// </summary>
    [JsonPropertyName("PrintError")]
    public bool PrintError { get; set; }

    /// <summary>
    /// Позиции шаблона.
    /// </summary>
    [JsonPropertyName("CheckItems")]
    public CheckItem[] CheckItems { get; set; } = [];

    /// <summary>
    /// Способы оплаты.
    /// </summary>
    [JsonPropertyName("Payments")]
    public CheckPayments? Payments { get; set; }

    /// <summary>
    /// Сведения о покупателе.
    /// </summary>
    [JsonPropertyName("CustomerDetail")]
    public CheckCustomer? CustomerDetail { get; set; }

    /// <summary>
    /// Дополнительный реквизит чека (тег 1192).
    /// </summary>
    [JsonPropertyName("AdditionalAttribute")]
    public string? AdditionalAttribute { get; set; }

    /// <summary>
    /// Адрес расчётов.
    /// </summary>
    [JsonPropertyName("SaleAddress")]
    public string? SaleAddress { get; set; }

    /// <summary>
    /// Место расчётов.
    /// </summary>
    [JsonPropertyName("SaleLocation")]
    public string? SaleLocation { get; set; }

    /// <summary>
    /// Email отправителя чека.
    /// </summary>
    [JsonPropertyName("SenderEmail")]
    public string? SenderEmail { get; set; }

    /// <summary>
    /// ФИО кассира.
    /// </summary>
    [JsonPropertyName("CashierName")]
    public string? CashierName { get; set; }

    /// <summary>
    /// ИНН кассира.
    /// </summary>
    [JsonPropertyName("CashierVatin")]
    public string? CashierVatin { get; set; }

    /// <summary>
    /// Данные коррекции, если шаблон — чек коррекции.
    /// </summary>
    [JsonPropertyName("CorrectionData")]
    public CorrectionData? CorrectionData { get; set; }
}
