using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Фискальная строка чека (товар / услуга). Основные поля: Name, Quantity, Price, Sum,
/// Tax, SignMethodCalculation, SignCalculationObject; при необходимости Marking, Agent, Vendor.
/// </summary>
public sealed class FiscalLine : Position
{
    /// <summary>
    /// Наименование товара
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Код товара
    /// </summary>
    public string? ProductCode { get; set; }

    /// <summary>
    /// Количество товара
    /// </summary>
    public decimal Quantity { get; set; } = 1;

    /// <summary>
    /// Цена единицы товара с учетом скидок/наценок
    /// </summary>
    [JsonPropertyName("PriceWithDiscount")]
    public decimal Price { get; set; }

    /// <summary>
    /// Конечная сумма по позиции чека с учетом всех скидок/наценок
    /// </summary>
    [JsonPropertyName("SumWithDiscount")]
    public decimal Sum { get; set; }

    /// <summary>
    /// Сумма скидок и наценок
    /// </summary>
    public decimal DiscountSum { get; set; }

    /// <summary>
    /// Ставка НДС предмета расчёта.
    /// </summary>
    public TaxRate Tax { get; set; }

    /// <summary>
    /// Сумма НДС за предмет расчета
    /// </summary>
    public decimal TaxSum { get; set; }

    /// <summary>
    /// Отдел, по которому ведется продажа
    /// </summary>
    public int Department { get; set; }

    /// <summary>
    /// Признак способа расчёта.
    /// </summary>
    public SignMethodCalculation? SignMethodCalculation { get; set; }

    /// <summary>
    /// Признак предмета расчёта.
    /// </summary>
    public SignCalculationObject? SignCalculationObject { get; set; }

    /// <summary>
    /// Мера количества предмета расчёта.
    /// </summary>
    public MeasureOfQuantity? MeasureOfQuantity { get; set; }

    /// <summary>
    /// Сумма акциза с учетом копеек
    /// </summary>
    public decimal? ExciseAmount { get; set; }

    /// <summary>
    /// Цифровой код страны происхождения товара
    /// </summary>
    public string? CountryOfOrigin { get; set; }

    /// <summary>
    /// Регистрационный номер таможенной декларации
    /// </summary>
    public string? CustomsDeclaration { get; set; }

    /// <summary>
    /// Признак агента по предмету расчёта.
    /// </summary>
    [JsonPropertyName("SignSubjectCalculationAgent")]
    public AgentType? AgentSign { get; set; }

    /// <summary>
    /// Операция платёжного агента.
    /// </summary>
    [JsonIgnore]
    public string? PayingAgentOperation { get; set; }

    /// <summary>
    /// Телефон(ы) платёжного агента.
    /// </summary>
    [JsonIgnore]
    public string[]? PayingAgentPhone { get; set; }

    /// <summary>
    /// Телефон(ы) оператора по приёму платежей.
    /// </summary>
    [JsonIgnore]
    public string[]? ReceivePaymentsOperatorPhone { get; set; }

    /// <summary>
    /// Телефон(ы) оператора перевода.
    /// </summary>
    [JsonIgnore]
    public string[]? MoneyTransferOperatorPhone { get; set; }

    /// <summary>
    /// Наименование оператора перевода.
    /// </summary>
    [JsonIgnore]
    public string? MoneyTransferOperatorName { get; set; }

    /// <summary>
    /// Адрес оператора перевода.
    /// </summary>
    [JsonIgnore]
    public string? MoneyTransferOperatorAddress { get; set; }

    /// <summary>
    /// ИНН оператора перевода.
    /// </summary>
    [JsonIgnore]
    public string? MoneyTransferOperatorVatin { get; set; }

    /// <summary>
    /// Данные агента, собираемые из плоских полей позиции (wire: AgentData).
    /// Если ни одно поле не заполнено — не сериализуется.
    /// </summary>
    [JsonPropertyName("AgentData")]
    public Agent? AgentData => BuildAgent();

    /// <summary>
    /// Собирает объект агента из плоских полей позиции.
    /// Возвращает null, если не заполнено ни одно поле (пустой агент не передаётся).
    /// </summary>
    private Agent? BuildAgent()
    {
        if (string.IsNullOrEmpty(PayingAgentOperation)
            && (PayingAgentPhone == null || PayingAgentPhone.Length == 0)
            && (ReceivePaymentsOperatorPhone == null || ReceivePaymentsOperatorPhone.Length == 0)
            && (MoneyTransferOperatorPhone == null || MoneyTransferOperatorPhone.Length == 0)
            && string.IsNullOrEmpty(MoneyTransferOperatorName)
            && string.IsNullOrEmpty(MoneyTransferOperatorAddress)
            && string.IsNullOrEmpty(MoneyTransferOperatorVatin))
            return null;

        return new Agent
        {
            PayingAgentOperation = PayingAgentOperation,
            PayingAgentPhone = PayingAgentPhone,
            ReceivePaymentsOperatorPhone = ReceivePaymentsOperatorPhone,
            MoneyTransferOperatorPhone = MoneyTransferOperatorPhone,
            MoneyTransferOperatorName = MoneyTransferOperatorName,
            MoneyTransferOperatorAddress = MoneyTransferOperatorAddress,
            MoneyTransferOperatorVatin = MoneyTransferOperatorVatin
        };
    }

    /// <summary>
    /// Наименование поставщика.
    /// </summary>
    [JsonIgnore]
    public string? VendorName { get; set; }

    /// <summary>
    /// Телефон(ы) поставщика.
    /// </summary>
    [JsonIgnore]
    public string[]? VendorPhones { get; set; }

    /// <summary>
    /// ИНН поставщика.
    /// </summary>
    [JsonIgnore]
    public string? VendorVatin { get; set; }

    /// <summary>
    /// Данные поставщика, собираемые из плоских полей позиции (wire: Vendor).
    /// Если ни одно поле не заполнено — не сериализуется.
    /// </summary>
    [JsonPropertyName("Vendor")]
    public Vendor? VendorData => BuildVendor();

    /// <summary>
    /// Код маркировки товара. Строка в любом формате (base64, CIS, GTIN)
    /// </summary>
    [JsonPropertyName("MarkingCode")]
    public string? Marking { get; set; }

    /// <summary>
    /// Числитель дробного количества (частичное выбытие маркированного товара).
    /// </summary>
    [JsonIgnore]
    public int FractionalQuantityNumerator { get; set; }

    /// <summary>
    /// Знаменатель дробного количества (частичное выбытие маркированного товара).
    /// </summary>
    [JsonIgnore]
    public int FractionalQuantityDenominator { get; set; }

    /// <summary>
    /// Дробное количество, собираемое из плоских полей позиции (wire: FractionalQuantity).
    /// Если числитель и знаменатель равны 0 — не сериализуется.
    /// </summary>
    [JsonPropertyName("FractionalQuantity")]
    public FractionalQuantity? FractionalData => BuildFractional();

    /// <summary>
    /// Код товарной номенклатуры: код контрольной марки в кодировке Base64.
    /// Если не задан — сервер использует <see cref="Marking"/>.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeMarkingCode { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: GTIN.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeGtin { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: тип маркировки ("02" — мех, "05" — табак, "1520" — обувь).
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeStampType { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: КиЗ.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeStamp { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: серийный номер.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeSerialNumber { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: штрихкод.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeBarcode { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: код неидентифицированного формата в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeNotIdentified { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: EAN-8 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeEan8 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: EAN-13 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeEan13 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: ITF-14 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeItf14 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: GS10 (без маркировки) в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeGs10 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: GS1 (с маркировкой) в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeGs1M { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: короткий код маркировки в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeKmk { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: КиЗ мехового изделия.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeMi { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: ЕГАИС-2.0 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeEgais20 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: ЕГАИС-3.0 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeEgais30 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: код формата Ф.1 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeF1 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: код формата Ф.2 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeF2 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: код формата Ф.3 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeF3 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: код формата Ф.4 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeF4 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: код формата Ф.5 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeF5 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры: код формата Ф.6 в кодировке Base64.
    /// </summary>
    [JsonIgnore]
    public string? GoodCodeF6 { get; set; }

    /// <summary>
    /// Код товарной номенклатуры, собираемый из плоских полей GoodCode* (wire: GoodCodeData).
    /// Если ни одно поле не заполнено — не сериализуется.
    /// </summary>
    [JsonPropertyName("GoodCodeData")]
    public CommodityNomenclatureCode? GoodCodeData => BuildGoodCode();

    /// <summary>
    /// Идентификатор ФОИВ отраслевого реквизита.
    /// </summary>
    [JsonIgnore]
    public string? IndustryIdentifierFoiv { get; set; }

    /// <summary>
    /// Дата документа-основания отраслевого реквизита.
    /// </summary>
    [JsonIgnore]
    public string? IndustryAttributeDocumentDate { get; set; }

    /// <summary>
    /// Номер документа-основания отраслевого реквизита.
    /// </summary>
    [JsonIgnore]
    public string? IndustryAttributeDocumentNumber { get; set; }

    /// <summary>
    /// Значение отраслевого реквизита.
    /// </summary>
    [JsonIgnore]
    public string? IndustryAttributeValue { get; set; }

    /// <summary>
    /// Отраслевой реквизит, собираемый из плоских полей позиции (wire: IndustryAttribute).
    /// Если ни одно поле не заполнено — не сериализуется.
    /// </summary>
    [JsonPropertyName("IndustryAttribute")]
    public Industry? IndustryData => BuildIndustry();

    /// <summary>
    /// Дополнительный реквизит предмета расчета
    /// </summary>
    public string? AdditionalAttribute { get; set; }

    /// <summary>
    /// Собирает данные поставщика из плоских полей позиции (null, если ничего не заполнено).
    /// </summary>
    private Vendor? BuildVendor()
    {
        if (string.IsNullOrEmpty(VendorName)
            && string.IsNullOrEmpty(VendorVatin)
            && (VendorPhones == null || VendorPhones.Length == 0))
            return null;

        return new Vendor
        {
            Name = VendorName,
            Phones = VendorPhones,
            Vatin = VendorVatin
        };
    }

    /// <summary>
    /// Собирает дробное количество из плоских полей позиции.
    /// Дробь передаётся только когда заданы обе части (числитель и знаменатель больше нуля) —
    /// сервер применяет дробное количество лишь при Numerator > 0 и Denominator > 0, иначе игнорирует.
    /// </summary>
    private FractionalQuantity? BuildFractional()
    {
        if (FractionalQuantityNumerator <= 0 || FractionalQuantityDenominator <= 0)
            return null;

        return new FractionalQuantity
        {
            Numerator = FractionalQuantityNumerator,
            Denominator = FractionalQuantityDenominator
        };
    }

    /// <summary>
    /// Собирает код товарной номенклатуры из плоских полей позиции (null, если ничего не заполнено).
    /// </summary>
    private CommodityNomenclatureCode? BuildGoodCode()
    {
        var code = new CommodityNomenclatureCode
        {
            MarkingCode = NullIfEmpty(GoodCodeMarkingCode),
            Gtin = NullIfEmpty(GoodCodeGtin),
            StampType = NullIfEmpty(GoodCodeStampType),
            Stamp = NullIfEmpty(GoodCodeStamp),
            SerialNumber = NullIfEmpty(GoodCodeSerialNumber),
            Barcode = NullIfEmpty(GoodCodeBarcode),
            NotIdentified = NullIfEmpty(GoodCodeNotIdentified),
            EAN8 = NullIfEmpty(GoodCodeEan8),
            EAN13 = NullIfEmpty(GoodCodeEan13),
            ITF14 = NullIfEmpty(GoodCodeItf14),
            GS10 = NullIfEmpty(GoodCodeGs10),
            GS1M = NullIfEmpty(GoodCodeGs1M),
            KMK = NullIfEmpty(GoodCodeKmk),
            MI = NullIfEmpty(GoodCodeMi),
            EGAIS20 = NullIfEmpty(GoodCodeEgais20),
            EGAIS30 = NullIfEmpty(GoodCodeEgais30),
            F1 = NullIfEmpty(GoodCodeF1),
            F2 = NullIfEmpty(GoodCodeF2),
            F3 = NullIfEmpty(GoodCodeF3),
            F4 = NullIfEmpty(GoodCodeF4),
            F5 = NullIfEmpty(GoodCodeF5),
            F6 = NullIfEmpty(GoodCodeF6)
        };

        return code.MarkingCode == null && code.Gtin == null && code.StampType == null
            && code.Stamp == null && code.SerialNumber == null && code.Barcode == null
            && code.NotIdentified == null && code.EAN8 == null && code.EAN13 == null
            && code.ITF14 == null && code.GS10 == null && code.GS1M == null
            && code.KMK == null && code.MI == null && code.EGAIS20 == null
            && code.EGAIS30 == null && code.F1 == null && code.F2 == null
            && code.F3 == null && code.F4 == null && code.F5 == null && code.F6 == null
            ? null
            : code;
    }

    private static string? NullIfEmpty(string? value) => string.IsNullOrEmpty(value) ? null : value;

    /// <summary>
    /// Собирает отраслевой реквизит из плоских полей позиции (null, если ничего не заполнено).
    /// </summary>
    private Industry? BuildIndustry()
    {
        if (string.IsNullOrEmpty(IndustryIdentifierFoiv)
            && string.IsNullOrEmpty(IndustryAttributeDocumentDate)
            && string.IsNullOrEmpty(IndustryAttributeDocumentNumber)
            && string.IsNullOrEmpty(IndustryAttributeValue))
            return null;

        return new Industry
        {
            IdentifierFoiv = IndustryIdentifierFoiv,
            DocumentDate = IndustryAttributeDocumentDate,
            DocumentNumber = IndustryAttributeDocumentNumber,
            AttributeValue = IndustryAttributeValue
        };
    }
}
