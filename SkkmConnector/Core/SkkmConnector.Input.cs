namespace RBSoftSkkm;

public sealed partial class SkkmConnector
{

    /// <summary>
    /// Начало периода отбора отчётов, чеков и операций
    /// </summary>
    public System.DateTime FromDate { get; set; } = System.DateTime.Today.AddDays(-7);

    /// <summary>
    /// Конец периода отбора отчётов, чеков и операций
    /// </summary>
    public System.DateTime ToDate { get; set; } = System.DateTime.Today;

    // Наличные

    /// <summary>
    /// Сумма внесения/выемки. Вход и выход: после CashIn, CashOut, GetCashIn, GetCashOut —
    /// фактическая сумма операции из ответа (поле Sum), после GetOverAll — наличные в денежном ящике.
    /// </summary>
    public decimal CashAmount { get; set; }

    // Картинки

    // Слип

    /// <summary>
    /// Текст нефискального документа.
    /// </summary>
    public string Text { get; set; } = "";

    // Шаблоны

    // Чек

    /// <summary>
    /// Тип чека / задания. Сохраняется между чеками: Clear и NewCheck его не сбрасывают.
    /// </summary>
    public CheckType PaymentType { get; set; } = CheckType.Sale;

    /// <summary>
    /// Система налогообложения. Сохраняется между чеками: Clear и NewCheck его не сбрасывают.
    /// </summary>
    public TaxSystem TaxVariant { get; set; } = TaxSystem.Osn;

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
    public string IndustryAttributeIdentifierFoiv { get; set; } = "";

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
    public string IndustryAttributeAttributeValue { get; set; } = "";

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
    public int? OperationalAttributeOperationID { get; set; }

    /// <summary>
    /// Данные операции операционного реквизита чека.
    /// </summary>
    public string OperationalAttributeOperationData { get; set; } = "";

    // Сведения о безналичной оплате, добавленные AddElectronicPaymentInfo.
    private readonly List<(decimal Amount, ElectronicPaymentMethod? Method, string Identifiers, string Information)> _electronicPayments = [];

    /// <summary>
    /// Добавляет сведения о безналичной оплате в чек; для нескольких оплат вызывается несколько раз.
    /// </summary>
    /// <param name="amount">Сумма безналичной оплаты по данной транзакции.</param>
    /// <param name="paymentMethod">Признак способа безналичной оплаты. Если не указан — в запрос не входит.</param>
    /// <param name="identifiers">Идентификаторы оплаты (например, RRN). Пустые в запрос не входят.</param>
    /// <param name="additionalInformation">Дополнительные сведения об оплате. Пустые в запрос не входят.</param>
    public void AddElectronicPaymentInfo(decimal amount, ElectronicPaymentMethod? paymentMethod = null, string identifiers = "", string additionalInformation = "")
        => _electronicPayments.Add((amount, paymentMethod, identifiers, additionalInformation));

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
    public string VendorDataName { get; set; } = "";

    /// <summary>
    /// Телефон(ы) поставщика.
    /// </summary>
    public string[]? VendorDataPhones { get; set; }

    /// <summary>
    /// ИНН поставщика.
    /// </summary>
    public string VendorDataVatin { get; set; } = "";

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
    /// Строки документа. Фискальную строку добавляет <see cref="AddPosition"/>,
    /// текст — <see cref="AddText"/>, картинку — <see cref="AddPicture"/>,
    /// штрихкод — <see cref="AddBarcode"/>, линию — <see cref="AddSeparatorLine"/>.
    /// </summary>
    public PositionTable Positions { get; } = new();

    /// <summary>
    /// Добавление фискальной строки (товара, услуги) в чек.
    /// Обязательны наименование, количество, цена со скидкой, сумма со скидкой, ставка НДС,
    /// признак предмета расчёта и признак способа расчёта. Мера количества необязательна.
    /// </summary>
    /// <param name="name">Наименование товара или услуги.</param>
    /// <param name="quantity">Количество.</param>
    /// <param name="price">Цена за единицу с учётом скидок и наценок.</param>
    /// <param name="sum">Сумма позиции с учётом скидок и наценок.</param>
    /// <param name="tax">Ставка НДС.</param>
    /// <param name="signCalculationObject">Признак предмета расчёта.</param>
    /// <param name="signMethodCalculation">Признак способа расчёта.</param>
    /// <param name="measureOfQuantity">Мера количества предмета расчёта (ФФД 1.2).</param>
    public Position AddPosition(
        string name,
        decimal quantity,
        decimal price,
        decimal sum,
        TaxRate tax,
        SignCalculationObject signCalculationObject,
        SignMethodCalculation signMethodCalculation,
        MeasureOfQuantity? measureOfQuantity = null)
    {
        var row = Positions.Add();
        row.FiscalString = true;
        row.Name = name;
        row.Quantity = quantity;
        row.PriceWithDiscount = price;
        row.SumWithDiscount = sum;
        row.Tax = tax;
        row.SignCalculationObject = signCalculationObject;
        row.SignMethodCalculation = signMethodCalculation;
        row.MeasureOfQuantity = measureOfQuantity;
        return row;
    }

    /// <summary>
    /// Добавление текстовой строки в документ.
    /// </summary>
    public Position AddText(
        string text,
        PrintFont font = PrintFont.Normal,
        PrintAlignment alignment = PrintAlignment.Left)
    {
        var row = Positions.Add();
        row.Text = text;
        row.Font = font;
        row.Alignment = alignment;
        return row;
    }

    /// <summary>
    /// Добавление картинки в документ.
    /// </summary>
    /// <param name="value">Картинка в Base64 (BMP или PNG).</param>
    /// <param name="alignment">Выравнивание. Если не указано — по центру (2), как на Сервере ККМ.</param>
    public Position AddPicture(string value, PictureAlignment alignment = PictureAlignment.Center)
    {
        var row = Positions.Add();
        row.PictureValue = value;
        row.PictureAlignment = alignment;
        return row;
    }

    /// <summary>
    /// Добавление разделительной линии в документ.
    /// </summary>
    /// <param name="lineStyle">Стиль линии. Если не указан — сплошная.</param>
    public Position AddSeparatorLine(LineStyle lineStyle = LineStyle.Solid)
    {
        var row = Positions.Add();
        row.lineStyle = lineStyle;
        return row;
    }

    /// <summary>
    /// Добавление штрихкода в документ. Значение — текстом или в Base64: Base64 коннектор определяет сам.
    /// </summary>
    /// <param name="type">Тип штрихкода.</param>
    /// <param name="value">Значение штрихкода.</param>
    /// <param name="alignment">Выравнивание.</param>
    public Position AddBarcode(BarcodeType type, string value = "", PrintAlignment alignment = PrintAlignment.Center)
    {
        var row = Positions.Add();
        row.BarcodeType = type;
        row.BarcodeValue = value;
        row.BarcodeAlignment = alignment;
        return row;
    }

    // Коррекция

    /// <summary>
    /// Тип коррекции. Не задан — не передаётся.
    /// </summary>
    public CorrectionTypes? CorrectionDataType { get; set; }

    /// <summary>
    /// Описание коррекции.
    /// </summary>
    public string CorrectionDataDescription { get; set; } = "";

    /// <summary>
    /// Дата совершения корректируемого расчёта. Не задана — не передаётся.
    /// </summary>
    public System.DateTime? CorrectionDataDate { get; set; }

    /// <summary>
    /// Номер предписания налогового органа (для типа коррекции «По предписанию»).
    /// </summary>
    public string CorrectionDataNumber { get; set; } = "";

    // Суммы НДС по ставкам для чека коррекции ФФД 1.05. Не заданные (null) не передаются.

    /// <summary>
    /// Сумма расчёта без НДС (коррекция 1.05).
    /// </summary>
    public decimal? SumTaxNone { get; set; }

    /// <summary>
    /// Сумма НДС 0% (коррекция 1.05).
    /// </summary>
    public decimal? SumTax0 { get; set; }

    /// <summary>
    /// Сумма НДС 5% (коррекция 1.05).
    /// </summary>
    public decimal? SumTax5 { get; set; }

    /// <summary>
    /// Сумма НДС 7% (коррекция 1.05).
    /// </summary>
    public decimal? SumTax7 { get; set; }

    /// <summary>
    /// Сумма НДС 10% (коррекция 1.05).
    /// </summary>
    public decimal? SumTax10 { get; set; }

    /// <summary>
    /// Сумма НДС 18% (коррекция 1.05).
    /// </summary>
    public decimal? SumTax18 { get; set; }

    /// <summary>
    /// Сумма НДС 20% (коррекция 1.05).
    /// </summary>
    public decimal? SumTax20 { get; set; }

    /// <summary>
    /// Сумма НДС 22% (коррекция 1.05).
    /// </summary>
    public decimal? SumTax22 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 10/110 (коррекция 1.05).
    /// </summary>
    public decimal? SumTax110 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 18/118 (коррекция 1.05).
    /// </summary>
    public decimal? SumTax118 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 20/120 (коррекция 1.05).
    /// </summary>
    public decimal? SumTax120 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 22/122 (коррекция 1.05).
    /// </summary>
    public decimal? SumTax122 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 5/105 (коррекция 1.05).
    /// </summary>
    public decimal? SumTax105 { get; set; }

    /// <summary>
    /// Сумма НДС по расчётной ставке 7/107 (коррекция 1.05).
    /// </summary>
    public decimal? SumTax107 { get; set; }

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
    public decimal Quantity { get; set; } = 1;

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
    /// Не отправлять результат проверки на сервер ОИСМ.
    /// </summary>
    public bool NotSendToServer { get; set; }

    /// <summary>
    /// Признак ожидания ответа ОИСМ.
    /// </summary>
    public bool WaitForResult { get; set; }

    /// <summary>
    /// Уникальный код запроса КМ
    /// </summary>
    public string Guid { get; set; } = "";

    /// <summary>
    /// Признак подтверждения кода маркировки.
    /// </summary>
    public KmConfirmationType ConfirmationType { get; set; }

    // Параметры отдельных операций: отчёты, очередь, картинки, маркировка, фискализация.

    /// <summary>
    /// Коды маркировки для проверки — одной строкой через запятую:
    /// <c>Codes = "код1,код2"</c>.
    /// </summary>
    public string Codes { get; set; } = "";

    /// <summary>
    /// Коды из <see cref="Codes"/> массивом: делим по запятой (и по переносу строки), пустые пропускаем.
    /// </summary>
    private string[] CodeList() =>
        Codes.Split([',', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    // ── Подготовка входа ──

    /// <summary>
    /// Очищает данные чека.
    /// Тип чека, СНО, кассира, данные маркировки и прочие результаты прошлого вызова не сбрасывает.
    /// </summary>
    public void Clear()
    {
        CustomerInfo = "";
        CustomerVatin = "";
        CustomerEmail = "";
        CustomerPhone = "";
        CustomerDateOfBirth = "";
        CustomerCitizenship = "";
        CustomerDocumentTypeCode = "";
        CustomerDocumentData = "";
        CustomerAddress = "";

        SenderEmail = "";
        SaleAddress = "";
        SaleLocation = "";
        TimeZone = null;

        UserAttributeName = "";
        UserAttributeValue = "";

        TextBefore = "";
        TextAfter = "";

        Positions.Clear();
        Counter = [];
        _electronicPayments.Clear();

        Cash = 0;
        ElectronicPayment = 0;
        Credit = 0;
        AdvancePayment = 0;
        CashProvision = 0;

        CashAmount = 0;

        CorrectionDataType = null;
        CorrectionDataDescription = "";
        CorrectionDataDate = null;
        CorrectionDataNumber = "";
        AdditionalAttribute = "";

        IsElectronically = false;
        IsOperationOnline = false;
        AgentSign = null;
        PayingAgentOperation = "";
        PayingAgentPhone = null;
        ReceivePaymentsOperatorPhone = null;
        MoneyTransferOperatorPhone = null;
        MoneyTransferOperatorName = "";
        MoneyTransferOperatorAddress = "";
        MoneyTransferOperatorVatin = "";
        VendorDataName = "";
        VendorDataPhones = null;
        VendorDataVatin = "";

        OperationalAttributeDateTime = "";
        OperationalAttributeOperationData = "";
        OperationalAttributeOperationID = null;

        IndustryAttributeAttributeValue = "";
        IndustryAttributeDocumentDate = "";
        IndustryAttributeDocumentNumber = "";
        IndustryAttributeIdentifierFoiv = "";

        ShiftNumber = 0;
        CheckNumber = 0;
        FiscalDatetime = default;
        ErrorCode = 0;
        ErrorDescription = "";

        SumTax0 = SumTax5 = SumTax7 = SumTax10 = SumTax18 = SumTax20 = SumTax22 = SumTaxNone = null;
        SumTax105 = SumTax107 = SumTax110 = SumTax118 = SumTax120 = SumTax122 = null;
    }

    /// <summary>
    /// Очищает предыдущий чек и задаёт тип операции, систему налогообложения и, если переданы, кассира.
    /// </summary>
    /// <param name="paymentType">Тип операции.</param>
    /// <param name="taxVariant">Система налогообложения.</param>
    /// <param name="cashierName">ФИО кассира. Пустое — остаётся заданный ранее.</param>
    /// <param name="cashierVatin">ИНН кассира. Пустой — остаётся заданный ранее.</param>
    public void NewCheck(CheckType paymentType, TaxSystem taxVariant, string cashierName = "", string cashierVatin = "")
    {
        Clear();
        PaymentType = paymentType;
        TaxVariant = taxVariant;
        if (cashierName.Length > 0)
            CashierName = cashierName;
        if (cashierVatin.Length > 0)
            CashierVatin = cashierVatin;
    }

}
