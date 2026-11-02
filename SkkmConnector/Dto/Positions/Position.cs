namespace RBSoftSkkm;

/// <summary>
/// Таблица строк документа <see cref="SkkmConnector.Positions"/>.
/// </summary>
public sealed class PositionTable : List<Position>
{
    /// <summary>
    /// Добавляет пустую строку и возвращает её — так добавляются штрихкод, картинка и разделительная линия.
    /// </summary>
    public Position Add()
    {
        var row = new Position();
        Add(row);
        return row;
    }
}

/// <summary>
/// Строка документа — одна строка таблицы <see cref="SkkmConnector.Positions"/>.
/// Вид строки определяется заполненными колонками:
/// <para>
/// FiscalString = true — фискальная строка (товар, услуга), её добавляет <see cref="SkkmConnector.AddPosition"/>;
/// </para>
/// <para>
/// PictureValue — картинка;
/// </para>
/// <para>
/// BarcodeValue — штрихкод;
/// </para>
/// <para>
/// lineStyle — разделительная линия;
/// </para>
/// <para>
/// иначе — текст (Text), его добавляет <see cref="SkkmConnector.AddText"/>.
/// </para>
/// </summary>
public sealed class Position
{
    // ── Фискальная строка

    /// <summary>
    /// Признак фискальной строки (товар, услуга).
    /// </summary>
    public bool FiscalString { get; set; }

    /// <summary>
    /// Наименование товара или услуги.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Количество.
    /// </summary>
    public decimal Quantity { get; set; } = 1;

    /// <summary>
    /// Цена единицы с учётом скидок и наценок.
    /// </summary>
    public decimal PriceWithDiscount { get; set; }

    /// <summary>
    /// Сумма позиции с учётом скидок и наценок.
    /// </summary>
    public decimal SumWithDiscount { get; set; }

    /// <summary>
    /// Ставка НДС.
    /// </summary>
    public TaxRate Tax { get; set; }

    /// <summary>
    /// Признак предмета расчёта.
    /// </summary>
    public SignCalculationObject? SignCalculationObject { get; set; }

    /// <summary>
    /// Признак способа расчёта.
    /// </summary>
    public SignMethodCalculation? SignMethodCalculation { get; set; }

    /// <summary>
    /// Мера количества предмета расчёта (ФФД 1.2).
    /// </summary>
    public MeasureOfQuantity? MeasureOfQuantity { get; set; }

    /// <summary>
    /// Единица измерения предмета расчёта.
    /// </summary>
    public string? MeasurementUnit { get; set; }

    /// <summary>
    /// Дополнительный реквизит предмета расчёта.
    /// </summary>
    public string? AdditionalAttribute { get; set; }

    /// <summary>
    /// Код маркировки (КИЗ) как есть. Передаётся в данных кода товара (GoodCodeData.Barcode).
    /// </summary>
    public string? Cis { get; set; }

    /// <summary>
    /// Считанный штрихкод маркировки в Base64. Передаётся в данных кода товара (GoodCodeData.MarkingCode).
    /// </summary>
    public string? Cis_Base64 { get; set; }

    /// <summary>
    /// Тип товара.
    /// </summary>
    public string? CommodityGroup { get; set; }

    /// <summary>
    /// Код страны происхождения.
    /// </summary>
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// Номер таможенной декларации.
    /// </summary>
    public string? CustomsDeclaration { get; set; }

    /// <summary>
    /// Отдел (секция).
    /// </summary>
    public int Department { get; set; }

    /// <summary>
    /// Сумма скидки.
    /// </summary>
    public decimal DiscountSum { get; set; }

    /// <summary>
    /// Сумма акциза.
    /// </summary>
    public decimal? ExciseAmount { get; set; }

    /// <summary>
    /// Дробное количество маркированного товара: знаменатель.
    /// </summary>
    public int FractionalQuantityDenominator { get; set; }

    /// <summary>
    /// Дробное количество маркированного товара: числитель.
    /// </summary>
    public int FractionalQuantityNumerator { get; set; }

    /// <summary>
    /// GTIN (код товара).
    /// </summary>
    public string? GTIN { get; set; }

    /// <summary>
    /// Серийный номер товара.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Тип маркировки: «02» — мех, «05» — табак, «1520» — обувь.
    /// </summary>
    public string? StampType { get; set; }

    /// <summary>
    /// Отраслевой реквизит: идентификатор проверки кода маркировки (для онлайн-проверки — параметры UUID и Time).
    /// </summary>
    public string? IndustryAttributeAttributeValue { get; set; }

    /// <summary>
    /// Отраслевой реквизит: дата документа-основания в формате «DD.MM.YYYY».
    /// </summary>
    public string? IndustryAttributeDocumentDate { get; set; }

    /// <summary>
    /// Отраслевой реквизит: номер документа-основания.
    /// </summary>
    public string? IndustryAttributeDocumentNumber { get; set; }

    /// <summary>
    /// Отраслевой реквизит: идентификатор ФОИВ (тег 1262).
    /// </summary>
    public string? IndustryAttributeIdentifierFoiv { get; set; }

    /// <summary>
    /// Признак агента по предмету расчёта.
    /// </summary>
    public AgentType? SignSubjectCalculationAgent { get; set; }

    /// <summary>
    /// Операция платёжного агента.
    /// </summary>
    public string? PayingAgentOperation { get; set; }

    /// <summary>
    /// Телефоны платёжного агента.
    /// </summary>
    public string[]? PayingAgentPhone { get; set; }

    /// <summary>
    /// Телефоны оператора по приёму платежей (тег 1074).
    /// </summary>
    public string[]? ReceivePaymentsOperatorPhone { get; set; }

    /// <summary>
    /// Адрес оператора перевода (тег 1005).
    /// </summary>
    public string? MoneyTransferOperatorAddress { get; set; }

    /// <summary>
    /// Наименование оператора перевода (тег 1026).
    /// </summary>
    public string? MoneyTransferOperatorName { get; set; }

    /// <summary>
    /// Телефоны оператора перевода (тег 1075).
    /// </summary>
    public string[]? MoneyTransferOperatorPhone { get; set; }

    /// <summary>
    /// ИНН оператора перевода (тег 1016).
    /// </summary>
    public string? MoneyTransferOperatorVATIN { get; set; }

    /// <summary>
    /// Наименование поставщика.
    /// </summary>
    public string? VendorDataName { get; set; }

    /// <summary>
    /// Телефоны поставщика.
    /// </summary>
    public string[]? VendorDataPhones { get; set; }

    /// <summary>
    /// ИНН поставщика.
    /// </summary>
    public string? VendorDataVatin { get; set; }

    /// <summary>
    /// Сумма НДС.
    /// </summary>
    public decimal TaxSum { get; set; }

    // ── Текст

    /// <summary>
    /// Текст. Для текстовой строки — сама строка (поддерживаются префиксы [big], [bold], [center], [QR], [line] и др.);
    /// для фискальной строки — текст, который печатается после неё.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Шрифт текстовой строки.
    /// </summary>
    public PrintFont? Font { get; set; }

    /// <summary>
    /// Выравнивание текстовой строки.
    /// </summary>
    public PrintAlignment? Alignment { get; set; }

    /// <summary>
    /// Шрифт текста, который печатается после фискальной строки.
    /// </summary>
    public PrintFont? TextFont { get; set; }

    /// <summary>
    /// Выравнивание текста, который печатается после фискальной строки.
    /// </summary>
    public PrintAlignment? TextAlignment { get; set; }

    // ── Разделительная линия

    /// <summary>
    /// Стиль разделительной строки. Если задан, строка печатается как разделительная линия.
    /// </summary>
    public LineStyle? lineStyle { get; set; }

    // ── Штрихкод

    /// <summary>
    /// Тип штрихкода.
    /// </summary>
    public BarcodeType BarcodeType { get; set; } = BarcodeType.QR;

    /// <summary>
    /// Значение штрихкода в текстовом виде или в Base64 (Base64 коннектор определяет сам).
    /// </summary>
    public string? BarcodeValue { get; set; }

    /// <summary>
    /// Выравнивание штрихкода.
    /// </summary>
    public PrintAlignment BarcodeAlignment { get; set; } = PrintAlignment.Center;

    // ── Картинка

    /// <summary>
    /// Картинка в Base64 (BMP или PNG).
    /// </summary>
    public string? PictureValue { get; set; }

    /// <summary>
    /// Выравнивание картинки.
    /// </summary>
    public PictureAlignment PictureAlignment { get; set; } = PictureAlignment.Center;

}
