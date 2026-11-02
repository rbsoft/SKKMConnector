namespace RBSoftSkkm;

/// <summary>
/// Признак предмета расчёта (тег 1212 ФФД):
/// <para>
/// NotApplicable - Не применяется
/// </para>
/// <para>
/// Goods - Товар (за исключением подакцизного)
/// </para>
/// <para>
/// ExcisableGoods - Подакцизный товар
/// </para>
/// <para>
/// Work - Работа
/// </para>
/// <para>
/// Service - Услуга
/// </para>
/// <para>
/// GamblingStake - Ставка (азартные игры)
/// </para>
/// <para>
/// GamblingPrize - Выигрыш (азартные игры)
/// </para>
/// <para>
/// LotteryTicket - Лотерейный билет или ставка
/// </para>
/// <para>
/// LotteryPrize - Выигрыш в лотерее
/// </para>
/// <para>
/// IntellectualProperty - Право на использование РИД или средств индивидуализации
/// </para>
/// <para>
/// Advance - Аванс, задаток, предоплата и аналогичные предметы расчёта
/// </para>
/// <para>
/// AgentFee - Агентское вознаграждение
/// </para>
/// <para>
/// Payout - Выплата (взнос, пени, штраф, вознаграждение, бонус и т.п.)
/// </para>
/// <para>
/// Other - Иной предмет расчёта
/// </para>
/// <para>
/// PropertyRight - Имущественное право
/// </para>
/// <para>
/// NonOperatingIncome - Внереализационный доход
/// </para>
/// <para>
/// OtherPayments - Иные платежи и взносы
/// </para>
/// <para>
/// TradeFee - Торговый сбор
/// </para>
/// <para>
/// TouristTax - Туристический налог
/// </para>
/// <para>
/// Deposit - Залог
/// </para>
/// <para>
/// Expense - Расход
/// </para>
/// <para>
/// PensionContributionIp - Взносы на ОПС ИП
/// </para>
/// <para>
/// PensionContribution - Взносы на ОПС
/// </para>
/// <para>
/// MedicalContributionIp - Взносы на ОМС ИП
/// </para>
/// <para>
/// MedicalContribution - Взносы на ОМС
/// </para>
/// <para>
/// SocialContribution - Взносы на ОСС
/// </para>
/// <para>
/// CasinoPayment - Платёж казино
/// </para>
/// <para>
/// CashWithdrawalByAgent - Выдача денежных средств банковским платёжным агентом
/// </para>
/// <para>
/// ExcisableMarkedWithoutCode - Подакцизный товар с маркировкой без кода (АТНМ)
/// </para>
/// <para>
/// ExcisableMarkedWithCode - Подакцизный товар с маркировкой с кодом (АТМ)
/// </para>
/// <para>
/// MarkedWithoutCode - Товар с маркировкой без кода, не подакцизный (ТНМ)
/// </para>
/// <para>
/// MarkedWithCode - Товар с маркировкой с кодом, не подакцизный (ТМ)
/// </para>
/// </summary>
public enum SignCalculationObject
{
    /// <summary>
    /// Не применяется.
    /// </summary>
    NotApplicable = 0,

    /// <summary>
    /// Товар (за исключением подакцизного).
    /// </summary>
    Goods = 1,

    /// <summary>
    /// Подакцизный товар.
    /// </summary>
    ExcisableGoods = 2,

    /// <summary>
    /// Работа.
    /// </summary>
    Work = 3,

    /// <summary>
    /// Услуга.
    /// </summary>
    Service = 4,

    /// <summary>
    /// Ставка (азартные игры).
    /// </summary>
    GamblingStake = 5,

    /// <summary>
    /// Выигрыш (азартные игры).
    /// </summary>
    GamblingPrize = 6,

    /// <summary>
    /// Лотерейный билет или ставка.
    /// </summary>
    LotteryTicket = 7,

    /// <summary>
    /// Выигрыш в лотерее.
    /// </summary>
    LotteryPrize = 8,

    /// <summary>
    /// Право на использование РИД или средств индивидуализации.
    /// </summary>
    IntellectualProperty = 9,

    /// <summary>
    /// Аванс, задаток, предоплата и аналогичные предметы расчёта.
    /// </summary>
    Advance = 10,

    /// <summary>
    /// Агентское вознаграждение.
    /// </summary>
    AgentFee = 11,

    /// <summary>
    /// Выплата (взнос, пени, штраф, вознаграждение, бонус и т.п.).
    /// </summary>
    Payout = 12,

    /// <summary>
    /// Иной предмет расчёта.
    /// </summary>
    Other = 13,

    /// <summary>
    /// Имущественное право.
    /// </summary>
    PropertyRight = 14,

    /// <summary>
    /// Внереализационный доход.
    /// </summary>
    NonOperatingIncome = 15,

    /// <summary>
    /// Иные платежи и взносы.
    /// </summary>
    OtherPayments = 16,

    /// <summary>
    /// Торговый сбор.
    /// </summary>
    TradeFee = 17,

    /// <summary>
    /// Туристический налог.
    /// </summary>
    TouristTax = 18,

    /// <summary>
    /// Залог.
    /// </summary>
    Deposit = 19,

    /// <summary>
    /// Расход.
    /// </summary>
    Expense = 20,

    /// <summary>
    /// Взносы на ОПС ИП.
    /// </summary>
    PensionContributionIp = 21,

    /// <summary>
    /// Взносы на ОПС.
    /// </summary>
    PensionContribution = 22,

    /// <summary>
    /// Взносы на ОМС ИП.
    /// </summary>
    MedicalContributionIp = 23,

    /// <summary>
    /// Взносы на ОМС.
    /// </summary>
    MedicalContribution = 24,

    /// <summary>
    /// Взносы на ОСС.
    /// </summary>
    SocialContribution = 25,

    /// <summary>
    /// Платёж казино.
    /// </summary>
    CasinoPayment = 26,

    /// <summary>
    /// Выдача денежных средств банковским платёжным агентом.
    /// </summary>
    CashWithdrawalByAgent = 27,

    /// <summary>
    /// Подакцизный товар с маркировкой без кода (АТНМ).
    /// </summary>
    ExcisableMarkedWithoutCode = 30,

    /// <summary>
    /// Подакцизный товар с маркировкой с кодом (АТМ).
    /// </summary>
    ExcisableMarkedWithCode = 31,

    /// <summary>
    /// Товар с маркировкой без кода, не подакцизный (ТНМ).
    /// </summary>
    MarkedWithoutCode = 32,

    /// <summary>
    /// Товар с маркировкой с кодом, не подакцизный (ТМ).
    /// </summary>
    MarkedWithCode = 33,
}
