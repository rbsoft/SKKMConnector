using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Позиция сохранённого чека
/// </summary>
public sealed class CheckItem
{
    /// <summary>
    /// Название.
    /// </summary>
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Количество товара.
    /// </summary>
    [JsonPropertyName("Quantity")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Цена позиции.
    /// </summary>
    [JsonPropertyName("Price")]
    public decimal Price { get; set; }

    /// <summary>
    /// Сумма с учётом скидки.
    /// </summary>
    [JsonPropertyName("Sum")]
    public decimal Sum { get; set; }

    /// <summary>
    /// Отдел.
    /// </summary>
    [JsonPropertyName("Department")]
    public int? Department { get; set; }

    /// <summary>
    /// Фискальный режим.
    /// </summary>
    [JsonPropertyName("IsFiscal")]
    public bool IsFiscal { get; set; }

    /// <summary>
    /// Ставка НДС.
    /// </summary>
    [JsonPropertyName("TaxValue")]
    public int TaxValue { get; set; }

    /// <summary>
    /// Сумма НДС.
    /// </summary>
    [JsonPropertyName("TaxSum")]
    public decimal TaxSum { get; set; }

    /// <summary>
    /// Признак способа расчёта
    /// </summary>
    [JsonPropertyName("PaymentMode")]
    public int PaymentMode { get; set; }

    /// <summary>
    /// Признак способа расчёта
    /// </summary>
    [JsonIgnore]
    public SignMethodCalculation SignMethodCalculation => (SignMethodCalculation)PaymentMode;

    /// <summary>
    /// Признак предмета расчёта (тег 1030 / 1212).
    /// </summary>
    [JsonPropertyName("ItemType")]
    public int ItemType { get; set; }

    /// <summary>
    /// Признак предмета расчёта
    /// </summary>
    [JsonIgnore]
    public SignCalculationObject SignCalculationObject => (SignCalculationObject)ItemType;

    /// <summary>
    /// Сумма акциза с учётом копеек, включённая в стоимость предмета расчёта.
    /// </summary>
    [JsonPropertyName("ExciseAmount")]
    public decimal? ExciseAmount { get; set; }

    /// <summary>
    /// Мера количества предмета расчёта (тег 2108)
    /// </summary>
    [JsonPropertyName("MeasureOfQuantity")]
    public MeasureOfQuantity? MeasureOfQuantity { get; set; }

    /// <summary>
    /// Скидка (&gt;0) или наценка (&lt;0).
    /// </summary>
    [JsonPropertyName("DiscountInfoValue")]
    public decimal DiscountInfoValue { get; set; }

    /// <summary>
    /// Дополнительный реквизит предмета расчёта.
    /// </summary>
    [JsonPropertyName("AdditionalAttribute")]
    public string? AdditionalAttribute { get; set; }

    /// <summary>
    /// Код маркировки верхнего уровня.
    /// </summary>
    [JsonPropertyName("MarkingCode")]
    public string? MarkingCodeRaw { get; set; }

    /// <summary>
    /// Код товара.
    /// </summary>
    [JsonPropertyName("ProductCode")]
    public string? ProductCode { get; set; }

    /// <summary>
    /// Код товара (маркировки).
    /// </summary>
    [JsonPropertyName("GoodCodeData")]
    public GoodCodeData? GoodCodeData { get; set; }

    /// <summary>
    /// Отраслевой реквизит позиции.
    /// </summary>
    [JsonPropertyName("IndustryAttribute")]
    public Industry? IndustryAttribute { get; set; }

    /// <summary>
    /// Параметры проверки кода маркировки позиции.
    /// </summary>
    [JsonPropertyName("ImcParams")]
    public ImcParams? ImcParams { get; set; }

    /// <summary>
    /// Нефискальная строка чека (текст, разделитель, штрихкод, картинка). У товара — null.
    /// </summary>
    [JsonPropertyName("PrintLine")]
    public PrintFormLine? PrintLine { get; set; }

    /// <summary>
    /// Код маркировки.
    /// </summary>
    [JsonIgnore]
    public string? MarkingCode => GoodCodeData?.MarkingCode ?? MarkingCodeRaw;

    /// <summary>
    /// Отраслевой реквизит: идентификатор ФОИВ.
    /// </summary>
    [JsonIgnore]
    public string? IndustryIdentifierFoiv => IndustryAttribute?.IdentifierFoiv;

    /// <summary>
    /// Отраслевой реквизит: дата документа-основания.
    /// </summary>
    [JsonIgnore]
    public string? IndustryDocumentDate => IndustryAttribute?.DocumentDate;

    /// <summary>
    /// Отраслевой реквизит: номер документа-основания.
    /// </summary>
    [JsonIgnore]
    public string? IndustryDocumentNumber => IndustryAttribute?.DocumentNumber;

    /// <summary>
    /// Отраслевой реквизит: значение.
    /// </summary>
    [JsonIgnore]
    public string? IndustryAttributeValue => IndustryAttribute?.AttributeValue;

    /// <summary>
    /// Проверка КМ выполнялась.
    /// </summary>
    [JsonIgnore]
    public bool ImcCheckFlag => ImcParams?.ItemInfoCheckResult?.ImcCheckFlag ?? false;

    /// <summary>
    /// Результат проверки КМ.
    /// </summary>
    [JsonIgnore]
    public bool ImcCheckResult => ImcParams?.ItemInfoCheckResult?.ImcCheckResult ?? false;

    /// <summary>
    /// Корректность статуса информации о КМ.
    /// </summary>
    [JsonIgnore]
    public bool ImcStatusInfo => ImcParams?.ItemInfoCheckResult?.ImcStatusInfo ?? false;

    /// <summary>
    /// Корректность планируемого статуса товара.
    /// </summary>
    [JsonIgnore]
    public bool ImcEstimatedStatusCorrect => ImcParams?.ItemInfoCheckResult?.ImcEstimatedStatusCorrect ?? false;
}
