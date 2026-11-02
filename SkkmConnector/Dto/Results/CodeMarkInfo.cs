namespace RBSoftSkkm;

/// <summary>
/// Результат проверки одного кода маркировки (CodeMarkInfo и элемент codes).
/// </summary>
public sealed class CodeMarkInfo
{
    /// <summary>Полный КИ/КиЗ.</summary>
    public string Cis { get; init; } = "";

    /// <summary>PNG DataMatrix в Base64.</summary>
    public string DataMatrix { get; init; } = "";

    /// <summary>Структура КИ/КиЗ валидна.</summary>
    public bool Valid { get; init; }

    /// <summary>КИ/КиЗ без криптоподписи.</summary>
    public string PrintView { get; init; } = "";

    /// <summary>Товарные группы.</summary>
    public int[] GroupIds { get; init; } = [];

    /// <summary>Криптопроверка пройдена.</summary>
    public bool Verified { get; init; }

    /// <summary>Статус «В обороте».</summary>
    public bool Realizable { get; init; }

    /// <summary>КМ нанесён на упаковку.</summary>
    public bool Utilised { get; init; }

    /// <summary>КМ найден.</summary>
    public bool Found { get; init; }

    /// <summary>Срок годности.</summary>
    public string ExpireDate { get; init; } = "";

    /// <summary>Вариативные сроки годности.</summary>
    public string VariableExpirations { get; init; } = "";

    /// <summary>Дата производства.</summary>
    public string ProductionDate { get; init; } = "";

    /// <summary>Вес, г.</summary>
    public int? ProductWeight { get; init; }

    /// <summary>Ветеринарный документ.</summary>
    public string PrVetDocument { get; init; } = "";

    /// <summary>КМ принадлежит ИНН из запроса.</summary>
    public bool? IsOwner { get; init; }

    /// <summary>Код ошибки проверки КМ. 0 — ошибки нет.</summary>
    public int? ErrorCode { get; init; }

    /// <summary>Сервисный код элемента.</summary>
    public string ServiceCode { get; init; } = "";

    /// <summary>Текст ошибки.</summary>
    public string Message { get; init; } = "";

    /// <summary>Прослеживаемость в товарной группе включена.</summary>
    public bool IsTracking { get; init; }

    /// <summary>Товар продан.</summary>
    public bool Sold { get; init; }

    /// <summary>GTIN.</summary>
    public string Gtin { get; init; } = "";

    /// <summary>Тип упаковки.</summary>
    public string PackageType { get; init; } = "";

    /// <summary>КИ агрегата.</summary>
    public string Parent { get; init; } = "";

    /// <summary>ИНН производителя.</summary>
    public string ProducerInn { get; init; } = "";

    /// <summary>Максимальная розничная цена, коп.</summary>
    public int? Mrp { get; init; }

    /// <summary>Серая зона.</summary>
    public bool GrayZone { get; init; }

    /// <summary>Единая минимальная цена, коп.</summary>
    public int? Smp { get; init; }

    /// <summary>КМ заблокирован.</summary>
    public bool IsBlocked { get; init; }

    /// <summary>Некорректный GTIN.</summary>
    public bool IsGreyGtin { get; init; }

    /// <summary>Органы, установившие блокировку.</summary>
    public string[] Ogvs { get; init; } = [];

    /// <summary>Количество единиц, объём или вес во вложении.</summary>
    public int? InnerUnitCount { get; init; }

    /// <summary>Продано или возвращено.</summary>
    public int? SoldUnitCount { get; init; }

    /// <summary>Номер серии.</summary>
    public string ProductionSerialNumber { get; init; } = "";

    /// <summary>Номер партии.</summary>
    public string ProductionBatchNumber { get; init; } = "";

    /// <summary>Заводской номер.</summary>
    public string FactorySerialNumber { get; init; } = "";

    /// <summary>Ёмкость групповой упаковки.</summary>
    public int? PackageQuantity { get; init; }

    /// <summary>Выбытие, допускающее продажу.</summary>
    public int? EliminationState { get; init; }
}
