using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Сведения о коде маркировки.
/// </summary>
public sealed class CodeMarkInfo
{
    /// <summary>
    /// Полный код маркировки (КиЗ).
    /// </summary>
    [JsonPropertyName("cis")]
    public string Cis { get; set; } = "";

    /// <summary>
    /// Признак валидности структуры кода.
    /// </summary>
    [JsonPropertyName("valid")]
    public bool Valid { get; set; }

    /// <summary>
    /// Код маркировки без крипто-подписи.
    /// </summary>
    [JsonPropertyName("printView")]
    public string PrintView { get; set; } = "";

    /// <summary>
    /// Идентификаторы товарных групп.
    /// </summary>
    [JsonPropertyName("groupIds")]
    public int[] GroupIds { get; set; } = [];

    /// <summary>
    /// Результат криптографической проверки кода.
    /// </summary>
    [JsonPropertyName("verified")]
    public bool Verified { get; set; }

    /// <summary>
    /// Признак статуса «В обороте».
    /// </summary>
    [JsonPropertyName("realizable")]
    public bool Realizable { get; set; }

    /// <summary>
    /// Признак нанесения кода на упаковку.
    /// </summary>
    [JsonPropertyName("utilised")]
    public bool Utilised { get; set; }

    /// <summary>
    /// Признак наличия кода в ГИС МТ.
    /// </summary>
    [JsonPropertyName("found")]
    public bool Found { get; set; }

    /// <summary>
    /// Код ошибки проверки.
    /// </summary>
    [JsonPropertyName("errorCode")]
    public int ErrorCode { get; set; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = "";

    /// <summary>
    /// Признак старта прослеживаемости в товарной группе.
    /// </summary>
    [JsonPropertyName("isTracking")]
    public bool IsTracking { get; set; }

    /// <summary>
    /// Признак того, что товар с данным кодом уже продан.
    /// </summary>
    [JsonPropertyName("sold")]
    public bool Sold { get; set; }

    /// <summary>
    /// Код товара (GTIN).
    /// </summary>
    [JsonPropertyName("gtin")]
    public string GTIN { get; set; } = "";

    /// <summary>
    /// Тип упаковки.
    /// </summary>
    [JsonPropertyName("packageType")]
    public string PackageType { get; set; } = "";

    /// <summary>
    /// ИНН производителя.
    /// </summary>
    [JsonPropertyName("producerInn")]
    public string ProducerInn { get; set; } = "";

    /// <summary>
    /// Признак нахождения продукции в «серой зоне».
    /// </summary>
    [JsonPropertyName("grayZone")]
    public bool GrayZone { get; set; }

    /// <summary>
    /// Признак блокировки кода по решению ОГВ.
    /// </summary>
    [JsonPropertyName("isBlocked")]
    public bool IsBlocked { get; set; }

    /// <summary>
    /// Признак некорректного (незарегистрированного) GTIN.
    /// </summary>
    [JsonPropertyName("isGreyGtin")]
    public bool IsGreyGtin { get; set; }

    /// <summary>
    /// Органы государственной власти, установившие блокировку.
    /// </summary>
    [JsonPropertyName("ogvs")]
    public string[] Ogvs { get; set; } = [];

    /// <summary>
    /// Ёмкость КИГУ (количество потенциальных вложений).
    /// </summary>
    [JsonPropertyName("packageQuantity")]
    public int PackageQuantity { get; set; }

    /// <summary>
    /// Полный код DataMatrix.
    /// </summary>
    [JsonPropertyName("dataMatrix")]
    public string? DataMatrix { get; set; }

    /// <summary>
    /// Срок годности.
    /// </summary>
    [JsonPropertyName("expireDate")]
    public DateTime? ExpireDate { get; set; }

    /// <summary>
    /// Данные о сроках годности переменного веса.
    /// </summary>
    [JsonPropertyName("variableExpirations")]
    public object? VariableExpirations { get; set; }

    /// <summary>
    /// Дата производства.
    /// </summary>
    [JsonPropertyName("productionDate")]
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// Вес продукции.
    /// </summary>
    [JsonPropertyName("productWeight")]
    public int? ProductWeight { get; set; }

    /// <summary>
    /// Идентификатор ветеринарного документа.
    /// </summary>
    [JsonPropertyName("prVetDocument")]
    public Guid? PrVetDocument { get; set; }

    /// <summary>
    /// Признак владельца кода.
    /// </summary>
    [JsonPropertyName("isOwner")]
    public bool? IsOwner { get; set; }

    /// <summary>
    /// Код сервиса проверки.
    /// </summary>
    [JsonPropertyName("code")]
    public string? ServiceCode { get; set; }

    /// <summary>
    /// Максимальная розничная цена (МРЦ).
    /// </summary>
    [JsonPropertyName("mrp")]
    public int? Mrp { get; set; }

    /// <summary>
    /// Сумма минимального расчёта (СМП).
    /// </summary>
    [JsonPropertyName("smp")]
    public int? Smp { get; set; }

    /// <summary>
    /// Количество вложенных единиц.
    /// </summary>
    [JsonPropertyName("innerUnitCount")]
    public int? InnerUnitCount { get; set; }

    /// <summary>
    /// Количество проданных вложенных единиц.
    /// </summary>
    [JsonPropertyName("soldUnitCount")]
    public int? SoldUnitCount { get; set; }

    /// <summary>
    /// Серийный номер производства.
    /// </summary>
    [JsonPropertyName("productionSerialNumber")]
    public string? ProductionSerialNumber { get; set; }

    /// <summary>
    /// Номер производственной партии.
    /// </summary>
    [JsonPropertyName("productionBatchNumber")]
    public string? ProductionBatchNumber { get; set; }

    /// <summary>
    /// Заводской серийный номер.
    /// </summary>
    [JsonPropertyName("factorySerialNumber")]
    public string? FactorySerialNumber { get; set; }
}
