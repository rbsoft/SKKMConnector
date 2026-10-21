namespace RBSoftSkkm;

public sealed partial class SkkmConnector
{


    /// <summary>
    /// Начало периода отбора отчётов, чеков и операций
    /// </summary>
    public DateTime ShiftsFrom { get; set; } = DateTime.Today.AddDays(-7);

    /// <summary>
    /// Конец периода отбора отчётов, чеков и операций
    /// </summary>
    public DateTime ShiftsTo { get; set; } = DateTime.Today;

    // Наличные

    /// <summary>
    /// Сумма внесения или выемки
    /// </summary>
    public decimal CashAmount { get; set; }

    // Картинки

    /// <summary>
    /// Выравнивание изображения при печати. 
    /// </summary>
    public PictureAlignment PictureAlignment { get; set; } = PictureAlignment.Center;

    // Слип

    /// <summary>
    /// Текст нефискального документа.
    /// </summary>
    public string TextForPrint { get; set; } = "";

    // Шаблоны

    /// <summary>
    /// Имя шаблона печати или шаблона чека.
    /// </summary>
    public string TemplateName { get; set; } = "";

    /// <summary>
    /// Тип печатного шаблона
    /// </summary>
    public PrintTemplateType TemplateType { get; set; }

    // Чек

    /// <summary>
    /// Тип чека / задания.
    /// </summary>
    public CheckType PaymentType { get; set; } = CheckType.Sale;

    /// <summary>
    /// Только обработанные операции. 
    /// </summary>
    public bool IsProcessed { get; set; }

    /// <summary>
    /// Система налогообложения. 
    /// </summary>
    public TaxSystem TaxVariant { get; private set; } = TaxSystem.Osn;

    /// <summary>
    /// Задать систему налогообложения на смену.
    /// </summary>
    public void SetTaxType(TaxSystem taxVariant) => TaxVariant = taxVariant;

    /// <summary>
    /// Часовая зона.
    /// </summary>
    public CheckTimeZone? TimeZone { get; set; }

    /// <summary>
    /// Чек только в электронном виде (без печати на бумаге).
    /// true — не печатать; для обычной печати оставляйте false.
    /// </summary>
    public bool IsElectronically { get; set; }

    /// <summary>
    /// Текст для печати перед товарной частью
    /// </summary>
    public string TextBefore { get; set; } = "";

    /// <summary>
    /// Текст для печати после товарной части чека
    /// </summary>
    public string TextAfter { get; set; } = "";

    /// <summary>
    /// Место проведения расчётов
    /// </summary>
    public string SaleLocation { get; set; } = "";

    /// <summary>
    /// Адрес проведения расчётов
    /// </summary>
    public string SaleAddress { get; set; } = "";

    /// <summary>
    /// Адрес электронной почты отправителя чека
    /// </summary>
    public string SenderEmail { get; set; } = "";

    /// <summary>
    /// Признак применения ККТ при осуществлении расчета в безналичном порядке в сети «Интернет»
    /// </summary>
    public bool IsOperationOnline { get; set; }

    /// <summary>
    /// Дополнительный реквизит чека (БСО), тег 1192
    /// </summary>
    public string AdditionalAttribute { get; set; } = "";

    // Отраслевой реквизит чека (тег 1261)

    /// <summary>
    /// Идентификатор ФОИВ отраслевого реквизита чека.
    /// </summary>
    public string IndustryIdentifierFoiv { get; set; } = "";

    /// <summary>
    /// Дата документа-основания отраслевого реквизита чека.
    /// </summary>
    public string IndustryAttributeDocumentDate { get; set; } = "";

    /// <summary>
    /// Номер документа-основания отраслевого реквизита чека.
    /// </summary>
    public string IndustryAttributeDocumentNumber { get; set; } = "";

    /// <summary>
    /// Значение отраслевого реквизита чека.
    /// </summary>
    public string IndustryAttributeValue { get; set; } = "";

    // Дополнительный реквизит пользователя (тег 1084)

    /// <summary>
    /// Наименование дополнительного реквизита пользователя.
    /// </summary>
    public string UserAttributeName { get; set; } = "";

    /// <summary>
    /// Значение дополнительного реквизита пользователя.
    /// </summary>
    public string UserAttributeValue { get; set; } = "";

    // Операционный реквизит чека (тег 1270)

    /// <summary>
    /// Дата и время операции операционного реквизита чека.
    /// </summary>
    public string OperationalAttributeDateTime { get; set; } = "";

    /// <summary>
    /// Идентификатор операции операционного реквизита чека.
    /// </summary>
    public int? OperationalAttributeOperationId { get; set; }

    /// <summary>
    /// Данные операции операционного реквизита чека.
    /// </summary>
    public string OperationalAttributeData { get; set; } = "";

    // Информация о безналичной оплаты

    /// <summary>
    /// Сумма безналичной оплаты по транзакции. Если 0 и остальные поля пусты — детализация не передаётся.
    /// </summary>
    public decimal ElectronicPaymentAmount { get; set; }

    /// <summary>
    /// Признак способа безналичной оплаты.
    /// </summary>
    public ElectronicPaymentMethod ElectronicPaymentMethod {  get; set; }

    /// <summary>
    /// Идентификаторы безналичной оплаты.
    /// </summary>
    public string ElectronicPaymentIdentifiers { get; set; } = "";

    /// <summary>
    /// Дополнительные сведения о безналичной оплате.
    /// </summary>
    public string ElectronicPaymentAdditionalInformation { get; set; } = "";

    /// <summary>
    /// Детализация нескольких безналичных оплат. Если список не пуст, в чек уходит он,
    /// а не одна оплата из полей ElectronicPaymentAmount / Method / Identifiers / AdditionalInformation.
    /// </summary>
    public List<ElectronicPayment> ElectronicPayments { get; } = [];

    /// <summary>
    /// Признак агента
    /// </summary>
    public AgentType? AgentSign { get; set; }

    /// <summary>
    /// Операция платёжного агента.
    /// </summary>
    public string PayingAgentOperation { get; set; } = "";

    /// <summary>
    /// Телефон(ы) платёжного агента.
    /// </summary>
    public string[]? PayingAgentPhone { get; set; }

    /// <summary>
    /// Телефон(ы) оператора по приёму платежей.
    /// </summary>
    public string[]? ReceivePaymentsOperatorPhone { get; set; }

    /// <summary>
    /// Телефон(ы) оператора перевода.
    /// </summary>
    public string[]? MoneyTransferOperatorPhone { get; set; }

    /// <summary>
    /// Наименование оператора перевода.
    /// </summary>
    public string MoneyTransferOperatorName { get; set; } = "";

    /// <summary>
    /// Адрес оператора перевода.
    /// </summary>
    public string MoneyTransferOperatorAddress { get; set; } = "";

    /// <summary>
    /// ИНН оператора перевода.
    /// </summary>
    public string MoneyTransferOperatorVatin { get; set; } = "";

    /// <summary>
    /// Наименование поставщика.
    /// </summary>
    public string VendorName { get; set; } = "";

    /// <summary>
    /// Телефон(ы) поставщика.
    /// </summary>
    public string[]? VendorPhones { get; set; }

    /// <summary>
    /// ИНН поставщика.
    /// </summary>
    public string VendorVatin { get; set; } = "";

    /// <summary>
    /// Наименование организации или фамилия, имя, отчество (при наличии)
    /// </summary>
    public string? CustomerInfo { get; set; }

    /// <summary>
    /// ИНН покупателя
    /// </summary>
    public string? CustomerVatin { get; set; }

    /// <summary>
    /// Электронная почта покупателя
    /// </summary>
    public string? CustomerEmail { get; set; }
    
    /// <summary>
    /// Номер телефона покупателя
    /// </summary>
    public string? CustomerPhone { get; set; }

    /// <summary>
    /// Дата рождения покупателя (клиента) в формате "DD.MM.YYYY"
    /// </summary>
    public string? CustomerDateOfBirth {  get; set; }

    /// <summary>
    /// Числовой код страны, гражданином которой является покупатель (клиент).
    /// Код страны указывается в соответствии с Общероссийским классификатором стран мира ОКСМ.
    /// </summary>
    public string? CustomerCitizenship { get; set;  }

    /// <summary>
    /// Числовой код вида документа, удостоверяющего личность (ФФД, Таблица 116)
    /// </summary>
    public string? CustomerDocumentTypeCode { get; set; }

    /// <summary>
    /// Данные документа, удостоверяющего личность
    /// </summary>
    public string? CustomerDocumentData { get; set; }

    /// <summary>
    /// Адрес покупателя (клиента)
    /// </summary>
    public string? CustomerAddress { get; set; }

    /// <summary>
    /// Сумма наличной оплаты.
    /// </summary>
    public decimal Cash { get; set; }

    /// <summary>
    /// Сумма безналичными средствами.
    /// </summary>
    public decimal ElectronicPayment { get; set; }

    /// <summary>
    /// Сумма предоплатой (зачётом аванса).
    /// </summary>
    public decimal AdvancePayment { get; set; }

    /// <summary>
    /// Сумма постоплатой (в кредит).
    /// </summary>
    public decimal Credit { get; set; }

    /// <summary>
    /// Сумма встречным предоставлением.
    /// </summary>
    public decimal CashProvision { get; set; }

    /// <summary>
    /// Позиции чека
    /// </summary>
    private readonly List<Position> _positions = new();

    /// <summary>
    /// Добавление фискальной строки (товара, услуги) в чек.
    /// Обязательные поля задаются параметрами: наименование, количество, сумма со скидкой и ставка НДС;
    /// признак предмета расчёта и признак способа расчёта (если не заданы, применяются
    /// значения ККТ по умолчанию). Цена за единицу вычисляется как сумма, делённая на количество.
    /// </summary>
    public FiscalLine AddPosition(
        string name,
        decimal quantity,
        decimal sum,
        TaxRate tax,
        SignCalculationObject? signCalculationObject = null,
        SignMethodCalculation? signMethodCalculation = null)
    {
        var line = new FiscalLine
        {
            Name = name,
            Quantity = quantity,
            Sum = sum,
            Price = quantity != 0 ? sum / quantity : sum,
            Tax = tax,
            SignCalculationObject = signCalculationObject,
            SignMethodCalculation = signMethodCalculation
        };
        _positions.Add(line);
        return line;
    }

    /// <summary>
    /// Добавление безналичной оплаты.
    /// Для нескольких безналичных оплат вызвать несколько раз (каждая попадает в список
    /// ElectronicPayments). Для одной оплаты можно также использовать плоские поля
    /// ElectronicPaymentAmount / Method / Identifiers / AdditionalInformation.
    /// </summary>
    public ElectronicPayment AddElectronicPayment(
        decimal amount,
        ElectronicPaymentMethod? paymentMethod = null,
        string? identifiers = null,
        string? additionalInformation = null)
    {
        var payment = new ElectronicPayment
        {
            Amount = amount,
            PaymentMethod = paymentMethod,
            Identifiers = identifiers,
            AdditionalInformation = additionalInformation,
        };
        ElectronicPayments.Add(payment);
        return payment;
    }

    /// <summary>
    /// Добавление текстовой строки в чек или печатный шаблон.
    /// </summary>
    public TextLine AddText(
        string text,
        PrintFont font = PrintFont.Normal,
        PrintAlignment alignment = PrintAlignment.Left)
    {
        var line = new TextLine { Text = text, Font = font, Alignment = alignment };
        _positions.Add(line);
        return line;
    }

    /// <summary>
    /// Добавление текстовой строки из двух частей: левая прижимается к левому краю,
    /// правая — к правому (например, «Итого» и сумма). Перенос при этом отключается.
    /// </summary>
    public TextLine AddText(string left, string right, PrintFont font = PrintFont.Normal)
    {
        var line = new TextLine
        {
            Text = left,
            LineRight = right,
            Font = font,
            Wrap = false
        };
        _positions.Add(line);
        return line;
    }

    /// <summary>
    /// Добавление строки штрихкода в чек или печатный шаблон.
    /// Высота и ширина штриха задаются в точках; печать текста действует только для одномерных.
    /// </summary>
    public BarcodeLine AddBarcode(
        BarcodeType type,
        string value,
        PrintAlignment alignment = PrintAlignment.Center,
        int height = 0,
        int barWidth = 0,
        BarcodePrintText printText = BarcodePrintText.None)
    {
        var line = new BarcodeLine
        {
            Type = type,
            Barcode = value,
            Alignment = alignment,
            Height = height,
            BarWidth = barWidth,
            PrintText = printText
        };
        _positions.Add(line);
        return line;
    }

    /// <summary>
    /// Добавление разделительной линии в чек или печатный шаблон. Стиль по умолчанию — сплошная линия.
    /// </summary>
    public SeparatorLine AddSeparatorLine(LineStyle lineStyle = LineStyle.Solid)
    {
        var line = new SeparatorLine { LineStyle = lineStyle };
        _positions.Add(line);
        return line;
    }

    /// <summary>
    /// Добавление изображения из файла в чек или печатный шаблон.
    /// Файл (BMP или PNG) читается и кодируется в Base64; ширина и высота — в точках (0 — размер изображения).
    /// </summary>
    public PictureLine AddPicture(
        string filePath,
        PictureAlignment alignment = PictureAlignment.Center,
        int width = 0,
        int height = 0)
    {
        if (!LoadPicture(filePath, out var base64))
            return new PictureLine { Alignment = alignment };

        return AddPictureBase64(base64, alignment, width, height);
    }

    /// <summary>
    /// Добавление изображения в Base64 в чек или печатный шаблон.
    /// </summary>
    internal PictureLine AddPictureBase64(
        string valueBase64,
        PictureAlignment alignment = PictureAlignment.Center,
        int width = 0,
        int height = 0)
    {
        var line = new PictureLine
        {
            Value = valueBase64,
            Alignment = alignment,
            Width = width > 0 ? width : null,
            Height = height > 0 ? height : null
        };
        _positions.Add(line);
        return line;
    }

    // Коррекция

    /// <summary>
    /// Тип коррекции
    /// </summary>
    public CorrectionTypes CorrectionType { get; set; } = CorrectionTypes.Самостоятельно;

    /// <summary>
    /// Описание коррекции.
    /// </summary>
    public string CorrectionDescription { get; set; } = "";

    /// <summary>
    /// Дата совершения корректируемого расчёта.
    /// </summary>
    public DateTime CorrectionDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Номер предписания налогового органа (для типа коррекции «По предписанию»).
    /// </summary>
    public string CorrectionNumber { get; set; } = "";

    // Суммы НДС по ставкам для чека коррекции ФФД 1.05 (плоские поля).

    /// <summary>
    /// Сумма расчёта без НДС (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTaxNone { get; set; }

    /// <summary>
    /// Сумма НДС 0% (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax0 { get; set; }

    /// <summary>
    /// Сумма НДС 5% (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax5 { get; set; }

    /// <summary>
    /// Сумма НДС 7% (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax7 { get; set; }

    /// <summary>
    /// Сумма НДС 10% (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax10 { get; set; }

    /// <summary>
    /// Сумма НДС 18% (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax18 { get; set; }

    /// <summary>
    /// Сумма НДС 20% (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax20 { get; set; }

    /// <summary>
    /// Сумма НДС 22% (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax22 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 10/110 (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax110 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 18/118 (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax118 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 20/120 (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax120 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 22/122 (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax122 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 5/105 (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax105 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 7/107 (коррекция 1.05).
    /// </summary>
    public decimal CorrectionSumTax107 { get; set; }

    // Маркировка (вход)

    /// <summary>
    /// Код маркировки в кодировке Base64
    /// </summary>
    public string MarkingCode { get; set; } = "";

    /// <summary>
    /// Планируемый статус товара.
    /// </summary>
    public MarkingPlannedStatus PlannedStatus { get; set; } = MarkingPlannedStatus.Sold;

    /// <summary>
    /// Количество товара
    /// </summary>
    public decimal MarkingQuantity { get; set; } = 1;

    /// <summary>
    /// Мера количества предмета расчёта.
    /// </summary>
    public MeasureOfQuantity MeasureOfQuantity { get; set; }

    /// <summary>
    /// Числитель дробного количества товара
    /// </summary>
    public int FractionalQuantityNumerator { get; set; }

    /// <summary>
    /// Знаменатель дробного количества товара.
    /// </summary>
    public int FractionalQuantityDenominator { get; set; }

    /// <summary>
    /// Не отправлять результат проверки на сервер ОИСМ
    /// </summary>
    public bool NotSendToServer { get; set; }

    /// <summary>
    /// Признак ожидания ответа ОИСМ
    /// </summary>
    public bool WaitForResult { get; set; }

    /// <summary>
    /// Уникальный код запроса КМ
    /// </summary>
    public string RequestKmGuid { get; set; } = "";

    /// <summary>
    /// Признак подтверждения кода маркировки.
    /// </summary>
    public KmConfirmationType ConfirmationType { get; set; }

    // Параметры отдельных операций: отчёты, очередь, картинки, маркировка, фискализация.

    /// <summary>
    /// Тип отчёта для списка Z-отчётов.
    /// </summary>
    public int ReportType { get; set; }

    /// <summary>
    /// Идентификатор задания в очереди печати.
    /// </summary>
    public string QueueTaskId { get; set; } = "";

    /// <summary>
    /// Имя картинки или шаблона.
    /// </summary>
    public string PictureId { get; set; } = "";

    /// <summary>
    /// Коды маркировки для проверки.
    /// </summary>
    public List<string> MarkingCodes { get; } = [];

    /// <summary>
    /// Регистрационный номер ККТ (РНМ) для фискализации.
    /// </summary>
    public string FiscalizationRnNumber { get; set; } = "";

    /// <summary>
    /// Системы налогообложения, поддерживаемые ККТ
    /// </summary>
    public TaxSystem[] FiscalizationTaxationSystems { get; set; } = [];

    /// <summary>
    /// ИНН организации.
    /// </summary>
    public string FiscalizationVatin { get; set; } = "";

    /// <summary>
    /// Наименование организации.
    /// </summary>
    public string FiscalizationCompanyName { get; set; } = "";

    /// <summary>
    /// Номер фискального накопителя (ФН).
    /// </summary>
    public string FiscalizationFn { get; set; } = "";

    /// <summary>
    /// Версия ФФД ККТ.
    /// </summary>
    public string FiscalizationFfdVersionKkt { get; set; } = "";

    /// <summary>
    /// Версия ФФД ФН.
    /// </summary>
    public string FiscalizationFfdVersionFn { get; set; } = "";

    /// <summary>
    /// Коды причин перерегистрации (метки).
    /// </summary>
    public string FiscalizationRegistrationLabelCodes { get; set; } = "";

    /// <summary>
    /// Адрес ОФД.
    /// </summary>
    public string FiscalizationOfdAddress { get; set; } = "";

    /// <summary>
    /// Порт ОФД.
    /// </summary>
    public int FiscalizationOfdPort { get; set; }

    /// <summary>
    /// Номер автомата (для автоматического режима).
    /// </summary>
    public string FiscalizationAutomaticNumber { get; set; } = "";

    /// <summary>
    /// Адрес электронной почты отправителя.
    /// </summary>
    public string FiscalizationSenderEmail { get; set; } = "";

    /// <summary>
    /// Код причины перерегистрации.
    /// </summary>
    public FiscalizationReasonCode FiscalizationReasonCode { get; set; }

    /// <summary>
    /// Хост ИСМ (информационная система маркировки).
    /// </summary>
    public string FiscalizationIsmHost { get; set; } = "";

    /// <summary>
    /// Порт ИСМ.
    /// </summary>
    public string FiscalizationIsmPort { get; set; } = "";

    /// <summary>
    /// Адрес сайта ФНС.
    /// </summary>
    public string FiscalizationFnsUrl { get; set; } = "";

    /// <summary>
    /// ИНН ОФД.
    /// </summary>
    public string FiscalizationOfdVatin { get; set; } = "";

    /// <summary>
    /// Наименование ОФД.
    /// </summary>
    public string FiscalizationOfdName { get; set; } = "";

    /// <summary>
    /// Признаки агента, поддерживаемые ККТ.
    /// </summary>
    public AgentType[] FiscalizationAgentTypes { get; set; } = [];

    /// <summary>
    /// Признак БСО.
    /// </summary>
    public bool FiscalizationIsBsoSign { get; set; }

    /// <summary>
    /// Работа с маркировкой.
    /// </summary>
    public bool FiscalizationIsMarking { get; set; }

    /// <summary>
    /// Ломбард.
    /// </summary>
    public bool FiscalizationIsPawnshop { get; set; }

    /// <summary>
    /// Страхование.
    /// </summary>
    public bool FiscalizationIsAssurance { get; set; }

    /// <summary>
    /// Автоматический режим.
    /// </summary>
    public bool FiscalizationIsAutomatic { get; set; }

    /// <summary>
    /// Торговый автомат (вендинг).
    /// </summary>
    public bool FiscalizationIsVending { get; set; }

    /// <summary>
    /// Автоматический принтер.
    /// </summary>
    public bool FiscalizationIsAutomaticPrinter { get; set; }

    /// <summary>
    /// Расчёты только в интернете.
    /// </summary>
    public bool FiscalizationIsOnline { get; set; }

    /// <summary>
    /// Проведение лотерей.
    /// </summary>
    public bool FiscalizationIsLottery { get; set; }

    /// <summary>
    /// Проведение азартных игр.
    /// </summary>
    public bool FiscalizationIsGambling { get; set; }

    /// <summary>
    /// Продажа подакцизных товаров.
    /// </summary>
    public bool FiscalizationIsExcisable { get; set; }

    /// <summary>
    /// Применение в сфере услуг.
    /// </summary>
    public bool FiscalizationIsService { get; set; }

    /// <summary>
    /// Шифрование данных.
    /// </summary>
    public bool FiscalizationIsEncrypted { get; set; }

    /// <summary>
    /// Автономный режим.
    /// </summary>
    public bool FiscalizationIsOffline { get; set; }

    /// <summary>
    /// Услуги общественного питания.
    /// </summary>
    public bool FiscalizationIsCateringServices { get; set; }

    /// <summary>
    /// Оптовая торговля.
    /// </summary>
    public bool FiscalizationIsWholesaleTrade { get; set; }
}
