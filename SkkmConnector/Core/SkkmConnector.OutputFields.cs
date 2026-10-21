namespace RBSoftSkkm;

// Плоские свойства-выводы для полей ответа.
public sealed partial class SkkmConnector
{
    // Очередь непереданных в ОФД документов (структура Backlog)

    /// <summary>
    /// Количество непереданных в ОФД документов.
    /// </summary>
    public long BacklogDocumentsCounter => _backlog?.DocumentsCounter ?? 0;

    /// <summary>
    /// Номер первого непереданного документа.
    /// </summary>
    public long BacklogDocumentFirstNumber => _backlog?.DocumentFirstNumber ?? 0;

    /// <summary>
    /// Дата и время первого непереданного документа.
    /// </summary>
    public DateTime? BacklogDocumentFirstDateTime => _backlog?.DocumentFirstDateTime;

    // Состояние обмена с ОФД (структура OfdStatus) — открытие смены, X/Z-отчёт.

    /// <summary>
    /// Обмен с ОФД завершён.
    /// </summary>
    public bool OfdStatusIsCompleted => Check?.OfdStatus?.IsCompleted ?? false;

    /// <summary>
    /// Соединение с ОФД установлено.
    /// </summary>
    public bool OfdStatusIsConnectedOFD => Check?.OfdStatus?.IsConnectedOFD ?? false;

    /// <summary>
    /// Есть документы для отправки в ОФД.
    /// </summary>
    public bool OfdStatusIsExistDocsToSend => Check?.OfdStatus?.IsExistDocsToSend ?? false;

    /// <summary>
    /// Количество непереданных в ОФД документов (статус обмена).
    /// </summary>
    public int OfdStatusDocumentsCount => Check?.OfdStatus?.DocumentsCount ?? 0;

    /// <summary>
    /// Номер первого непереданного документа (статус обмена).
    /// </summary>
    public long OfdStatusFirstDocumentNumber => Check?.OfdStatus?.FirstDocumentNumber ?? 0;

    /// <summary>
    /// Дата первого непереданного документа (статус обмена).
    /// </summary>
    public DateTime? OfdStatusFirstDocumentDate => Check?.OfdStatus?.FirstDocumentDate;

    /// <summary>
    /// Сообщение ОФД прочитано.
    /// </summary>
    public bool OfdStatusIsOfdMessageRead => Check?.OfdStatus?.OfdMessageRead ?? false;

    // Список устройств (структура Devices) — GetDeviceList / GetDeviceListByPool.

    /// <summary>
    /// Имена устройств из списка (<see cref="Devices"/>).
    /// </summary>
    public string[] DeviceNames => Devices.Select(d => d.DeviceName ?? "").ToArray();

    /// <summary>
    /// Типы драйверов устройств из списка (<see cref="Devices"/>).
    /// </summary>
    public DeviceType[] DeviceDrivers => Devices.Select(d => d.Driver).ToArray();

    /// <summary>
    /// Пулы устройств из списка (<see cref="Devices"/>).
    /// </summary>
    public string[] DevicePools => Devices.Select(d => d.Pool ?? "").ToArray();

    /// <summary>
    /// Описания статусов устройств из списка (<see cref="Devices"/>).
    /// </summary>
    public string[] DeviceStatusDescriptions => Devices.Select(d => d.DeviceStatusDescription ?? "").ToArray();

    // Статус ККТ (структура Status / KktStatus)

    /// <summary>
    /// Присутствует ли фискальный накопитель.
    /// </summary>
    public bool StatusIsFnPresent => Status?.IsFnPresent ?? false;

    /// <summary>
    /// ФН в состоянии ошибки.
    /// </summary>
    public bool StatusIsFnError => Status?.IsFnError ?? false;

    /// <summary>
    /// Информационная система маркировки недоступна.
    /// </summary>
    public bool StatusIsIsmDisconnected => Status?.IsIsmDisconnected ?? false;

    /// <summary>
    /// Оператор фискальных данных недоступен.
    /// </summary>
    public bool StatusIsOfdDisconnected => Status?.IsOfdDisconnected ?? false;

    /// <summary>
    /// Фискальный режим.
    /// </summary>
    public bool StatusIsFiscal => Status?.IsFiscal ?? false;

    /// <summary>
    /// Смена открыта.
    /// </summary>
    public bool StatusIsShiftOpened => Status?.IsShiftOpened ?? false;

    /// <summary>
    /// Смена истекла.
    /// </summary>
    public bool StatusIsShiftExpired => Status?.IsShiftExpired ?? false;

    /// <summary>
    /// Денежный ящик открыт.
    /// </summary>
    public bool StatusIsDrawerOpened => Status?.IsDrawerOpened ?? false;

    /// <summary>
    /// Наличие чековой ленты.
    /// </summary>
    public bool StatusIsCheckPaperPresent => Status?.IsCheckPaperPresent ?? false;

    /// <summary>
    /// Крышка открыта.
    /// </summary>
    public bool StatusIsCoverOpened => Status?.IsCoverOpened ?? false;

    /// <summary>
    /// Аккумулятор разряжен.
    /// </summary>
    public bool StatusIsBatteryLow => Status?.IsBatteryLow ?? false;

    /// <summary>
    /// Есть открытый документ.
    /// </summary>
    public bool StatusIsOpenDocument => Status?.IsOpenDocument ?? false;

    /// <summary>
    /// Предупреждения ФН загружены.
    /// </summary>
    public bool StatusIsWarningsLoaded => Status?.WarningsLoaded ?? false;

    /// <summary>
    /// Номер смены (из статуса).
    /// </summary>
    public int StatusShiftNumber => Status?.ShiftNumber ?? 0;

    /// <summary>
    /// Номер фискального документа (из статуса).
    /// </summary>
    public int StatusDocNumber => Status?.DocNumber ?? 0;

    /// <summary>
    /// Ширина чековой ленты в символах (из статуса).
    /// </summary>
    public int StatusLineLength => Status?.LineLength ?? 0;

    /// <summary>
    /// Время ПК (из статуса).
    /// </summary>
    public DateTime? StatusComputerTime => Status?.ComputerTime;

    /// <summary>
    /// Время в часах ККТ (из статуса).
    /// </summary>
    public DateTime? StatusDeviceTime => Status?.DeviceTime;

    /// <summary>
    /// Номер ФД за смену (из статуса).
    /// </summary>
    public int StatusDocNumberInShift => Status?.DocNumberInShift ?? 0;

    /// <summary>
    /// Наличные в денежном ящике (из статуса).
    /// </summary>
    public decimal StatusCashSum => Status?.CashSum ?? 0;

    /// <summary>
    /// Сумма за смену (из статуса).
    /// </summary>
    public decimal StatusTotalSum => Status?.TotalSum ?? 0;

    /// <summary>
    /// Время открытия смены.
    /// </summary>
    public DateTime? StatusOpenShiftTime => Status?.OpenShiftTime;

    /// <summary>
    /// Наличие контрольной ленты.
    /// </summary>
    public bool StatusIsControlPaperPresent => Status?.IsControlPaperPresent ?? false;

    /// <summary>
    /// Ожидание команды продолжения печати.
    /// </summary>
    public bool StatusIsWaitContinuePrint => Status?.IsWaitContinuePrint ?? false;

    /// <summary>
    /// Касса занята выполнением задания.
    /// </summary>
    public bool StatusIsBusy => Status?.IsBusy ?? false;

    /// <summary>
    /// Идентификатор выполняемого задания.
    /// </summary>
    public string? StatusTaskId => Status?.TaskId;

    /// <summary>
    /// Признак ошибки устройства.
    /// </summary>
    public int StatusError => Status?.Error ?? 0;

    /// <summary>
    /// Код ошибки устройства.
    /// </summary>
    public int StatusErrorCode => Status?.ErrorCode ?? 0;

    /// <summary>
    /// Описание ошибки устройства.
    /// </summary>
    public string? StatusErrorCodeDescription => Status?.ErrorCodeDescription;

    /// <summary>
    /// Режим драйвера.
    /// </summary>
    public int StatusDriverMode => Status?.DriverMode ?? 0;

    /// <summary>
    /// Описание режима драйвера.
    /// </summary>
    public string? StatusDriverModeDescription => Status?.DriverModeDescription;

    /// <summary>
    /// Подрежим драйвера.
    /// </summary>
    public int StatusDriverAdvancedMode => Status?.DriverAdvancedMode ?? 0;

    /// <summary>
    /// Описание подрежима драйвера.
    /// </summary>
    public string? StatusDriverAdvancedModeDescription => Status?.DriverAdvancedModeDescription;

    // Лицензия сервера (структура Status.License / ServerLicense)

    /// <summary>
    /// Статус лицензии.
    /// </summary>
    public int LicenseStatus => Status?.LicenseStatus ?? 0;

    /// <summary>
    /// Дата обновления сведений о лицензии.
    /// </summary>
    public DateTime? LicenseUpdated => Status?.LicenseUpdated;

    /// <summary>
    /// Код лицензии.
    /// </summary>
    public int LicenseCode => Status?.License?.Code ?? 0;

    /// <summary>
    /// Лицензия конечного пользователя.
    /// </summary>
    public bool LicenseIsEndUser => Status?.License?.IsEndUser ?? false;

    /// <summary>
    /// Лицензия активирована.
    /// </summary>
    public bool LicenseIsActivated => Status?.License?.IsActivated ?? false;

    /// <summary>
    /// Лицензия заблокирована.
    /// </summary>
    public bool LicenseIsBlocked => Status?.License?.IsBlocked ?? false;

    /// <summary>
    /// Дата блокировки лицензии.
    /// </summary>
    public DateTime? LicenseBlockDate => Status?.License?.BlockDate;

    /// <summary>
    /// Дата выдачи лицензии.
    /// </summary>
    public DateTime? LicenseDate => Status?.License?.Date;

    /// <summary>
    /// Лицензия действует до.
    /// </summary>
    public DateTime? LicenseExpired => Status?.License?.Expired;

    /// <summary>
    /// Обновления доступны до.
    /// </summary>
    public DateTime? LicenseUpdateExpired => Status?.License?.UpdateExpired;

    /// <summary>
    /// Лимит установок.
    /// </summary>
    public int LicenseLimitInstalls => Status?.License?.LimitInstalls ?? 0;

    /// <summary>
    /// Требуется активация объекта.
    /// </summary>
    public bool LicenseNeedObjectActivation => Status?.License?.NeedObjectActivation ?? false;

    /// <summary>
    /// Лимит объектов.
    /// </summary>
    public int LicenseLimitObjects => Status?.License?.LimitObjects ?? 0;

    // Обмен с ИСМ (структура Status.Ism / ExchangeStatusIsm)

    /// <summary>
    /// Адрес сервера ИСМ.
    /// </summary>
    public string? IsmAddress => Status?.Ism?.Address;

    /// <summary>
    /// Порт сервера ИСМ.
    /// </summary>
    public int IsmPort => Status?.Ism?.Port ?? 0;

    /// <summary>
    /// Количество непереданных в ИСМ уведомлений.
    /// </summary>
    public long IsmBacklogDocumentsCounter => Status?.Ism?.Backlog?.DocumentsCounter ?? 0;

    /// <summary>
    /// Номер первого непереданного в ИСМ уведомления.
    /// </summary>
    public long IsmBacklogDocumentFirstNumber => Status?.Ism?.Backlog?.DocumentFirstNumber ?? 0;

    /// <summary>
    /// Дата первого непереданного в ИСМ уведомления.
    /// </summary>
    public DateTime? IsmBacklogDocumentFirstDateTime => Status?.Ism?.Backlog?.DocumentFirstDateTime;

    /// <summary>
    /// Время последнего успешного подключения к ИСМ.
    /// </summary>
    public DateTime? IsmLastSuccessConnectionDateTime => Status?.Ism?.Errors?.LastSuccessConnectionDateTime;

    /// <summary>
    /// Код команды ФН при ошибке обмена с ИСМ.
    /// </summary>
    public int IsmErrorFnCommandCode => Status?.Ism?.Errors?.FnCommandCode ?? 0;

    /// <summary>
    /// Номер документа при ошибке обмена с ИСМ.
    /// </summary>
    public int IsmErrorDocumentNumber => Status?.Ism?.Errors?.DocumentNumber ?? 0;

    /// <summary>
    /// Код ошибки ФН при обмене с ИСМ.
    /// </summary>
    public int IsmErrorFnCode => Status?.Ism?.Errors?.Fn?.Code ?? 0;

    /// <summary>
    /// Описание ошибки ФН при обмене с ИСМ.
    /// </summary>
    public string? IsmErrorFnDescription => Status?.Ism?.Errors?.Fn?.Description;

    /// <summary>
    /// Код сетевой ошибки обмена с ИСМ.
    /// </summary>
    public int IsmErrorNetworkCode => Status?.Ism?.Errors?.Network?.Code ?? 0;

    /// <summary>
    /// Описание сетевой ошибки обмена с ИСМ.
    /// </summary>
    public string? IsmErrorNetworkDescription => Status?.Ism?.Errors?.Network?.Description;

    /// <summary>
    /// Код ошибки ИСМ.
    /// </summary>
    public int IsmErrorIsmCode => Status?.Ism?.Errors?.Ism?.Code ?? 0;

    /// <summary>
    /// Описание ошибки ИСМ.
    /// </summary>
    public string? IsmErrorIsmDescription => Status?.Ism?.Errors?.Ism?.Description;

    // Режимы ФН (структура Kkt.Fn.Modes / FnModes)

    /// <summary>
    /// Автоматический принтер.
    /// </summary>
    public bool FnModesIsPrinterAutomatic => Kkt?.Fn?.Modes?.PrinterAutomatic ?? false;

    /// <summary>
    /// Автономный режим.
    /// </summary>
    public bool FnModesIsOfflineMode => Kkt?.Fn?.Modes?.OfflineMode ?? false;

    /// <summary>
    /// Признак применения в сфере услуг.
    /// </summary>
    public bool FnModesIsServiceSign => Kkt?.Fn?.Modes?.ServiceSign ?? false;

    /// <summary>
    /// Признак БСО.
    /// </summary>
    public bool FnModesIsBsoSign => Kkt?.Fn?.Modes?.BsoSign ?? false;

    /// <summary>
    /// Признак расчётов только в интернете.
    /// </summary>
    public bool FnModesIsCalcOnlineSign => Kkt?.Fn?.Modes?.CalcOnlineSign ?? false;

    /// <summary>
    /// Шифрование данных.
    /// </summary>
    public bool FnModesIsDataEncryption => Kkt?.Fn?.Modes?.DataEncryption ?? false;

    /// <summary>
    /// Продажа подакцизных товаров.
    /// </summary>
    public bool FnModesIsSaleExcisableGoods => Kkt?.Fn?.Modes?.SaleExcisableGoods ?? false;

    /// <summary>
    /// Признак проведения азартных игр.
    /// </summary>
    public bool FnModesIsSignOfGambling => Kkt?.Fn?.Modes?.SignOfGambling ?? false;

    /// <summary>
    /// Признак проведения лотерей.
    /// </summary>
    public bool FnModesIsSignOfLottery => Kkt?.Fn?.Modes?.SignOfLottery ?? false;

    /// <summary>
    /// Ломбард.
    /// </summary>
    public bool FnModesIsPawnshop => Kkt?.Fn?.Modes?.Pawnshop ?? false;

    /// <summary>
    /// Страхование.
    /// </summary>
    public bool FnModesIsAssurance => Kkt?.Fn?.Modes?.Assurance ?? false;

    /// <summary>
    /// Работа с маркировкой.
    /// </summary>
    public bool FnModesIsMarking => Kkt?.Fn?.Modes?.Marking ?? false;

    /// <summary>
    /// Торговый автомат.
    /// </summary>
    public bool FnModesIsVendingMachine => Kkt?.Fn?.Modes?.VendingMachine ?? false;

    /// <summary>
    /// Услуги общественного питания.
    /// </summary>
    public bool FnModesIsCateringServices => Kkt?.Fn?.Modes?.CateringServices ?? false;

    /// <summary>
    /// Оптовая торговля.
    /// </summary>
    public bool FnModesIsWholesaleTrade => Kkt?.Fn?.Modes?.WholesaleTrade ?? false;

    /// <summary>
    /// Автоматический режим.
    /// </summary>
    public bool FnModesIsAutomaticMode => Kkt?.Fn?.Modes?.AutomaticMode ?? false;

    // Устройство и драйвер 

    /// <summary>
    /// Наименование модели ККТ.
    /// </summary>
    public string? DeviceModelName => Kkt?.Device?.ModelName;

    /// <summary>
    /// Лицензии ККТ.
    /// </summary>
    public IReadOnlyList<KktLicense> DeviceLicenses => Kkt?.Device?.KktLicenses ?? [];

    /// <summary>
    /// Тип драйвера.
    /// </summary>
    public string? DriverType => Kkt?.Driver?.Type;

    /// <summary>
    /// Версия драйвера.
    /// </summary>
    public string? DriverVersion => Kkt?.Driver?.Version;

    /// <summary>
    /// Производитель драйвера.
    /// </summary>
    public string? DriverVendor => Kkt?.Driver?.Vendor;

    // Фискальный накопитель

    /// <summary>
    /// Версия ФН.
    /// </summary>
    public string? FnVersion => Kkt?.Fn?.Version;

    /// <summary>
    /// Исполнение ФН.
    /// </summary>
    public string? FnExecution => Kkt?.Fn?.Execution;

    /// <summary>
    /// Фаза жизни ФН.
    /// </summary>
    public string? FnLivePhase => Kkt?.Fn?.LivePhase;

    /// <summary>
    /// Адрес расчётов, зарегистрированный в ФН.
    /// </summary>
    public string? FnSaleAddress => Kkt?.Fn?.SaleAddress;

    /// <summary>
    /// Место расчётов, зарегистрированное в ФН.
    /// </summary>
    public string? FnSaleLocation => Kkt?.Fn?.SaleLocation;

    /// <summary>
    /// Системы налогообложения, зарегистрированные в ФН (код).
    /// </summary>
    public int FnTaxVariant => Kkt?.Fn?.TaxVariant ?? 0;

    /// <summary>
    /// Признак агента, зарегистрированный в ФН (код).
    /// </summary>
    public int FnSignOfAgent => Kkt?.Fn?.SignOfAgent ?? 0;

    /// <summary>
    /// Номер автомата.
    /// </summary>
    public string? FnAutomaticNumber => Kkt?.Fn?.AutomaticNumber;

    /// <summary>
    /// Эл. почта отправителя чека, зарегистрированная в ФН.
    /// </summary>
    public string? FnSenderEmail => Kkt?.Fn?.SenderEmail;

    /// <summary>
    /// В ФН задан адрес сервера обновления ключей.
    /// </summary>
    public bool FnContainsKeysUpdaterServerUri => Kkt?.Fn?.FnContainsKeysUpdaterServerUri ?? false;

    /// <summary>
    /// Количество выполненных фискализаций.
    /// </summary>
    public int FnFiscalizationsCount => Kkt?.Fn?.FiscalizationsCount ?? 0;

    /// <summary>
    /// Количество оставшихся фискализаций.
    /// </summary>
    public int FnFiscalizationsFree => Kkt?.Fn?.FiscalizationsFree ?? 0;

    /// <summary>
    /// Дата последней фискализации.
    /// </summary>
    public DateTime? FnFiscalizationDateTime => Kkt?.Fn?.FiscalizationDateTime;

    /// <summary>
    /// Номер ФД последней фискализации.
    /// </summary>
    public string? FnFiscalizationDocumentNumber => Kkt?.Fn?.FiscalizationDocumentNumber;

    /// <summary>
    /// Код причины перерегистрации.
    /// </summary>
    public int FnReasonCode => Kkt?.Fn?.ReasonCode ?? 0;

    // Итоги смены (структура ShiftTotals / ResShiftTotal) — заполняются X- и Z-отчётом.

    /// <summary>
    /// Счётчики итогов смены прочитаны из ФН.
    /// </summary>
    public bool ShiftTotalIsCountersReaded => ShiftTotals?.IsCountersReaded ?? false;

    /// <summary>
    /// Номер смены (итоги).
    /// </summary>
    public double ShiftTotalShiftNumber => ShiftTotals?.ShiftNumber ?? 0;

    /// <summary>
    /// Сумма наличных в денежном ящике (итоги).
    /// </summary>
    public decimal ShiftTotalCashDrawerSum => ShiftTotals?.CashDrawer?.Sum ?? 0;

    /// <summary>
    /// Количество операций с денежным ящиком (итоги).
    /// </summary>
    public int ShiftTotalCashDrawerCount => ShiftTotals?.CashDrawer?.Count ?? 0;

    /// <summary>
    /// Сумма внесений за смену.
    /// </summary>
    public decimal ShiftTotalIncomeSum => ShiftTotals?.ShiftIncome?.Sum ?? 0;

    /// <summary>
    /// Количество внесений за смену.
    /// </summary>
    public int ShiftTotalIncomeCount => ShiftTotals?.ShiftIncome?.Count ?? 0;

    /// <summary>
    /// Сумма выплат за смену.
    /// </summary>
    public decimal ShiftTotalOutcomeSum => ShiftTotals?.ShiftOutcome?.Sum ?? 0;

    /// <summary>
    /// Количество выплат за смену.
    /// </summary>
    public int ShiftTotalOutcomeCount => ShiftTotals?.ShiftOutcome?.Count ?? 0;

    /// <summary>
    /// Сумма коррекций за смену.
    /// </summary>
    public decimal ShiftTotalCorrectionSum => ShiftTotals?.Counters?.SumCorrection ?? 0;

    /// <summary>
    /// Количество коррекций за смену.
    /// </summary>
    public int ShiftTotalCorrectionsNumber => ShiftTotals?.Counters?.NumberCorrections ?? 0;

    /// <summary>
    /// Приход за смену: количество документов.
    /// </summary>
    public int ShiftTotalSalesCount => ShiftTotals?.Counters?.Sales?.Count ?? 0;

    /// <summary>
    /// Приход за смену: сумма.
    /// </summary>
    public decimal ShiftTotalSalesSum => ShiftTotals?.Counters?.Sales?.Sum ?? 0;

    /// <summary>
    /// Приход за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalSalesPaymentsSum => ShiftTotals?.Counters?.Sales?.Payments?.Sum ?? 0;

    /// <summary>
    /// Приход за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalSalesPaymentsCash => ShiftTotals?.Counters?.Sales?.Payments?.Cash ?? 0;

    /// <summary>
    /// Приход за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalSalesPaymentsElectronically => ShiftTotals?.Counters?.Sales?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Приход за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalSalesPaymentsPrepaid => ShiftTotals?.Counters?.Sales?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Приход за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalSalesPaymentsCredit => ShiftTotals?.Counters?.Sales?.Payments?.Credit ?? 0;

    /// <summary>
    /// Приход за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalSalesPaymentsBarter => ShiftTotals?.Counters?.Sales?.Payments?.Barter ?? 0;

    /// <summary>
    /// Приход за смену: количество скидок.
    /// </summary>
    public int ShiftTotalSalesDiscountCount => ShiftTotals?.Counters?.Sales?.Discount?.Count ?? 0;

    /// <summary>
    /// Приход за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalSalesDiscountSum => ShiftTotals?.Counters?.Sales?.Discount?.Sum ?? 0;

    /// <summary>
    /// Приход за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalSalesAddingCount => ShiftTotals?.Counters?.Sales?.Adding?.Count ?? 0;

    /// <summary>
    /// Приход за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalSalesAddingSum => ShiftTotals?.Counters?.Sales?.Adding?.Sum ?? 0;

    /// <summary>
    /// Приход за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat0 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_0");

    /// <summary>
    /// Приход за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalSalesTaxVatNo => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_NO");

    /// <summary>
    /// Приход за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat5 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_5");

    /// <summary>
    /// Приход за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat105 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_105");

    /// <summary>
    /// Приход за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat7 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_7");

    /// <summary>
    /// Приход за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat107 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_107");

    /// <summary>
    /// Приход за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat10 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_10");

    /// <summary>
    /// Приход за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat110 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_110");

    /// <summary>
    /// Приход за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat20 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_20");

    /// <summary>
    /// Приход за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat120 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_120");

    /// <summary>
    /// Приход за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat22 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_22");

    /// <summary>
    /// Приход за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalSalesTaxVat122 => CounterTax(ShiftTotals?.Counters?.Sales, "TaxVat_122");

    /// <summary>
    /// Возврат прихода за смену: количество документов.
    /// </summary>
    public int ShiftTotalSalesReturnCount => ShiftTotals?.Counters?.SalesReturn?.Count ?? 0;

    /// <summary>
    /// Возврат прихода за смену: сумма.
    /// </summary>
    public decimal ShiftTotalSalesReturnSum => ShiftTotals?.Counters?.SalesReturn?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalSalesReturnPaymentsSum => ShiftTotals?.Counters?.SalesReturn?.Payments?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalSalesReturnPaymentsCash => ShiftTotals?.Counters?.SalesReturn?.Payments?.Cash ?? 0;

    /// <summary>
    /// Возврат прихода за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalSalesReturnPaymentsElectronically => ShiftTotals?.Counters?.SalesReturn?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Возврат прихода за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalSalesReturnPaymentsPrepaid => ShiftTotals?.Counters?.SalesReturn?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Возврат прихода за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalSalesReturnPaymentsCredit => ShiftTotals?.Counters?.SalesReturn?.Payments?.Credit ?? 0;

    /// <summary>
    /// Возврат прихода за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalSalesReturnPaymentsBarter => ShiftTotals?.Counters?.SalesReturn?.Payments?.Barter ?? 0;

    /// <summary>
    /// Возврат прихода за смену: количество скидок.
    /// </summary>
    public int ShiftTotalSalesReturnDiscountCount => ShiftTotals?.Counters?.SalesReturn?.Discount?.Count ?? 0;

    /// <summary>
    /// Возврат прихода за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalSalesReturnDiscountSum => ShiftTotals?.Counters?.SalesReturn?.Discount?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalSalesReturnAddingCount => ShiftTotals?.Counters?.SalesReturn?.Adding?.Count ?? 0;

    /// <summary>
    /// Возврат прихода за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalSalesReturnAddingSum => ShiftTotals?.Counters?.SalesReturn?.Adding?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat0 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_0");

    /// <summary>
    /// Возврат прихода за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVatNo => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_NO");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat5 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_5");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat105 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_105");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat7 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_7");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat107 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_107");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat10 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_10");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat110 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_110");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat20 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_20");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat120 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_120");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat22 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_22");

    /// <summary>
    /// Возврат прихода за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalSalesReturnTaxVat122 => CounterTax(ShiftTotals?.Counters?.SalesReturn, "TaxVat_122");

    /// <summary>
    /// Коррекция прихода за смену: количество документов.
    /// </summary>
    public int ShiftTotalSalesCorrectionCount => ShiftTotals?.Counters?.SalesCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: сумма.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionSum => ShiftTotals?.Counters?.SalesCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionPaymentsSum => ShiftTotals?.Counters?.SalesCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionPaymentsCash => ShiftTotals?.Counters?.SalesCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionPaymentsElectronically => ShiftTotals?.Counters?.SalesCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalSalesCorrectionPaymentsPrepaid => ShiftTotals?.Counters?.SalesCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalSalesCorrectionPaymentsCredit => ShiftTotals?.Counters?.SalesCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionPaymentsBarter => ShiftTotals?.Counters?.SalesCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: количество скидок.
    /// </summary>
    public int ShiftTotalSalesCorrectionDiscountCount => ShiftTotals?.Counters?.SalesCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionDiscountSum => ShiftTotals?.Counters?.SalesCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalSalesCorrectionAddingCount => ShiftTotals?.Counters?.SalesCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionAddingSum => ShiftTotals?.Counters?.SalesCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat0 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVatNo => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat5 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat105 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat7 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat107 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat10 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat110 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat20 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat120 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat22 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция прихода за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalSalesCorrectionTaxVat122 => CounterTax(ShiftTotals?.Counters?.SalesCorrection, "TaxVat_122");

    /// <summary>
    /// Коррекция возврата прихода за смену: количество документов.
    /// </summary>
    public int ShiftTotalSalesReturnCorrectionCount => ShiftTotals?.Counters?.SalesReturnCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionSum => ShiftTotals?.Counters?.SalesReturnCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionPaymentsSum => ShiftTotals?.Counters?.SalesReturnCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionPaymentsCash => ShiftTotals?.Counters?.SalesReturnCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionPaymentsElectronically => ShiftTotals?.Counters?.SalesReturnCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionPaymentsPrepaid => ShiftTotals?.Counters?.SalesReturnCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionPaymentsCredit => ShiftTotals?.Counters?.SalesReturnCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionPaymentsBarter => ShiftTotals?.Counters?.SalesReturnCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: количество скидок.
    /// </summary>
    public int ShiftTotalSalesReturnCorrectionDiscountCount => ShiftTotals?.Counters?.SalesReturnCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionDiscountSum => ShiftTotals?.Counters?.SalesReturnCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalSalesReturnCorrectionAddingCount => ShiftTotals?.Counters?.SalesReturnCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionAddingSum => ShiftTotals?.Counters?.SalesReturnCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat0 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVatNo => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat5 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat105 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat7 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat107 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat10 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat110 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat20 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat120 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat22 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция возврата прихода за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalSalesReturnCorrectionTaxVat122 => CounterTax(ShiftTotals?.Counters?.SalesReturnCorrection, "TaxVat_122");

    /// <summary>
    /// Расход за смену: количество документов.
    /// </summary>
    public int ShiftTotalPurchasesCount => ShiftTotals?.Counters?.Purchases?.Count ?? 0;

    /// <summary>
    /// Расход за смену: сумма.
    /// </summary>
    public decimal ShiftTotalPurchasesSum => ShiftTotals?.Counters?.Purchases?.Sum ?? 0;

    /// <summary>
    /// Расход за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalPurchasesPaymentsSum => ShiftTotals?.Counters?.Purchases?.Payments?.Sum ?? 0;

    /// <summary>
    /// Расход за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalPurchasesPaymentsCash => ShiftTotals?.Counters?.Purchases?.Payments?.Cash ?? 0;

    /// <summary>
    /// Расход за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalPurchasesPaymentsElectronically => ShiftTotals?.Counters?.Purchases?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Расход за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesPaymentsPrepaid => ShiftTotals?.Counters?.Purchases?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Расход за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesPaymentsCredit => ShiftTotals?.Counters?.Purchases?.Payments?.Credit ?? 0;

    /// <summary>
    /// Расход за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalPurchasesPaymentsBarter => ShiftTotals?.Counters?.Purchases?.Payments?.Barter ?? 0;

    /// <summary>
    /// Расход за смену: количество скидок.
    /// </summary>
    public int ShiftTotalPurchasesDiscountCount => ShiftTotals?.Counters?.Purchases?.Discount?.Count ?? 0;

    /// <summary>
    /// Расход за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalPurchasesDiscountSum => ShiftTotals?.Counters?.Purchases?.Discount?.Sum ?? 0;

    /// <summary>
    /// Расход за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalPurchasesAddingCount => ShiftTotals?.Counters?.Purchases?.Adding?.Count ?? 0;

    /// <summary>
    /// Расход за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalPurchasesAddingSum => ShiftTotals?.Counters?.Purchases?.Adding?.Sum ?? 0;

    /// <summary>
    /// Расход за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat0 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_0");

    /// <summary>
    /// Расход за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVatNo => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_NO");

    /// <summary>
    /// Расход за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat5 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_5");

    /// <summary>
    /// Расход за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat105 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_105");

    /// <summary>
    /// Расход за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat7 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_7");

    /// <summary>
    /// Расход за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat107 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_107");

    /// <summary>
    /// Расход за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat10 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_10");

    /// <summary>
    /// Расход за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat110 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_110");

    /// <summary>
    /// Расход за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat20 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_20");

    /// <summary>
    /// Расход за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat120 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_120");

    /// <summary>
    /// Расход за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat22 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_22");

    /// <summary>
    /// Расход за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalPurchasesTaxVat122 => CounterTax(ShiftTotals?.Counters?.Purchases, "TaxVat_122");

    /// <summary>
    /// Возврат расхода за смену: количество документов.
    /// </summary>
    public int ShiftTotalPurchasesReturnCount => ShiftTotals?.Counters?.PurchasesReturn?.Count ?? 0;

    /// <summary>
    /// Возврат расхода за смену: сумма.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnSum => ShiftTotals?.Counters?.PurchasesReturn?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnPaymentsSum => ShiftTotals?.Counters?.PurchasesReturn?.Payments?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnPaymentsCash => ShiftTotals?.Counters?.PurchasesReturn?.Payments?.Cash ?? 0;

    /// <summary>
    /// Возврат расхода за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnPaymentsElectronically => ShiftTotals?.Counters?.PurchasesReturn?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Возврат расхода за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesReturnPaymentsPrepaid => ShiftTotals?.Counters?.PurchasesReturn?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Возврат расхода за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesReturnPaymentsCredit => ShiftTotals?.Counters?.PurchasesReturn?.Payments?.Credit ?? 0;

    /// <summary>
    /// Возврат расхода за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnPaymentsBarter => ShiftTotals?.Counters?.PurchasesReturn?.Payments?.Barter ?? 0;

    /// <summary>
    /// Возврат расхода за смену: количество скидок.
    /// </summary>
    public int ShiftTotalPurchasesReturnDiscountCount => ShiftTotals?.Counters?.PurchasesReturn?.Discount?.Count ?? 0;

    /// <summary>
    /// Возврат расхода за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnDiscountSum => ShiftTotals?.Counters?.PurchasesReturn?.Discount?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalPurchasesReturnAddingCount => ShiftTotals?.Counters?.PurchasesReturn?.Adding?.Count ?? 0;

    /// <summary>
    /// Возврат расхода за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnAddingSum => ShiftTotals?.Counters?.PurchasesReturn?.Adding?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat0 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_0");

    /// <summary>
    /// Возврат расхода за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVatNo => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_NO");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat5 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_5");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat105 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_105");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat7 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_7");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat107 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_107");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat10 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_10");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat110 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_110");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat20 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_20");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat120 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_120");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat22 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_22");

    /// <summary>
    /// Возврат расхода за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnTaxVat122 => CounterTax(ShiftTotals?.Counters?.PurchasesReturn, "TaxVat_122");

    /// <summary>
    /// Коррекция расхода за смену: количество документов.
    /// </summary>
    public int ShiftTotalPurchasesCorrectionCount => ShiftTotals?.Counters?.PurchasesCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: сумма.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionSum => ShiftTotals?.Counters?.PurchasesCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionPaymentsSum => ShiftTotals?.Counters?.PurchasesCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionPaymentsCash => ShiftTotals?.Counters?.PurchasesCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionPaymentsElectronically => ShiftTotals?.Counters?.PurchasesCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionPaymentsPrepaid => ShiftTotals?.Counters?.PurchasesCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionPaymentsCredit => ShiftTotals?.Counters?.PurchasesCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionPaymentsBarter => ShiftTotals?.Counters?.PurchasesCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: количество скидок.
    /// </summary>
    public int ShiftTotalPurchasesCorrectionDiscountCount => ShiftTotals?.Counters?.PurchasesCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionDiscountSum => ShiftTotals?.Counters?.PurchasesCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalPurchasesCorrectionAddingCount => ShiftTotals?.Counters?.PurchasesCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionAddingSum => ShiftTotals?.Counters?.PurchasesCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat0 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVatNo => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat5 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat105 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat7 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat107 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat10 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat110 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat20 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat120 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat22 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция расхода за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalPurchasesCorrectionTaxVat122 => CounterTax(ShiftTotals?.Counters?.PurchasesCorrection, "TaxVat_122");

    /// <summary>
    /// Коррекция возврата расхода за смену: количество документов.
    /// </summary>
    public int ShiftTotalPurchasesReturnCorrectionCount => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionSum => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: общая сумма оплат.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionPaymentsSum => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: оплата наличными.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionPaymentsCash => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: оплата безналичными.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionPaymentsElectronically => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: оплата авансом (предоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionPaymentsPrepaid => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: оплата в кредит (постоплата).
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionPaymentsCredit => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: встречное предоставление.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionPaymentsBarter => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: количество скидок.
    /// </summary>
    public int ShiftTotalPurchasesReturnCorrectionDiscountCount => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма скидок.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionDiscountSum => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: количество надбавок.
    /// </summary>
    public int ShiftTotalPurchasesReturnCorrectionAddingCount => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма надбавок.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionAddingSum => ShiftTotals?.Counters?.PurchasesReturnCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 0%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat0 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога без НДС.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVatNo => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 5%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat5 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 5/105.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat105 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 7%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat7 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 7/107.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat107 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 10%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat10 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 10/110.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat110 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 20%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat20 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 20/120.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat120 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 22%.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat22 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция возврата расхода за смену: сумма налога НДС 22/122.
    /// </summary>
    public decimal ShiftTotalPurchasesReturnCorrectionTaxVat122 => CounterTax(ShiftTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_122");

    // Необнуляемые итоги за всё время работы (структура OverallTotals) — заполняются X- и Z-отчётом и GetOverAll.

    /// <summary>
    /// Необнуляемые итоги загружены из ФН.
    /// </summary>
    public bool OverallIsDataLoaded => OverallTotals?.DataLoaded ?? false;

    /// <summary>
    /// Необнуляемая общая сумма.
    /// </summary>
    public decimal OverallSum => OverallTotals?.Sum ?? 0;

    /// <summary>
    /// Необнуляемое общее количество документов.
    /// </summary>
    public int OverallCount => OverallTotals?.Count ?? 0;

    /// <summary>
    /// Наличность в денежном ящике (необнуляемые итоги).
    /// </summary>
    public decimal OverallCashDrawerSum => OverallTotals?.CashDrawer?.Sum ?? 0;

    /// <summary>
    /// Количество операций с ящиком (необнуляемые итоги).
    /// </summary>
    public int OverallCashDrawerCount => OverallTotals?.CashDrawer?.Count ?? 0;

    /// <summary>
    /// Общая сумма коррекций за всё время.
    /// </summary>
    public decimal OverallCorrectionSum => OverallTotals?.Counters?.SumCorrection ?? 0;

    /// <summary>
    /// Общее количество коррекций за всё время.
    /// </summary>
    public int OverallCorrectionsNumber => OverallTotals?.Counters?.NumberCorrections ?? 0;

    /// <summary>
    /// Приход за всё время: количество документов.
    /// </summary>
    public int OverallSalesCount => OverallTotals?.Counters?.Sales?.Count ?? 0;

    /// <summary>
    /// Приход за всё время: сумма.
    /// </summary>
    public decimal OverallSalesSum => OverallTotals?.Counters?.Sales?.Sum ?? 0;

    /// <summary>
    /// Приход за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallSalesPaymentsSum => OverallTotals?.Counters?.Sales?.Payments?.Sum ?? 0;

    /// <summary>
    /// Приход за всё время: оплата наличными.
    /// </summary>
    public decimal OverallSalesPaymentsCash => OverallTotals?.Counters?.Sales?.Payments?.Cash ?? 0;

    /// <summary>
    /// Приход за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallSalesPaymentsElectronically => OverallTotals?.Counters?.Sales?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Приход за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallSalesPaymentsPrepaid => OverallTotals?.Counters?.Sales?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Приход за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallSalesPaymentsCredit => OverallTotals?.Counters?.Sales?.Payments?.Credit ?? 0;

    /// <summary>
    /// Приход за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallSalesPaymentsBarter => OverallTotals?.Counters?.Sales?.Payments?.Barter ?? 0;

    /// <summary>
    /// Приход за всё время: количество скидок.
    /// </summary>
    public int OverallSalesDiscountCount => OverallTotals?.Counters?.Sales?.Discount?.Count ?? 0;

    /// <summary>
    /// Приход за всё время: сумма скидок.
    /// </summary>
    public decimal OverallSalesDiscountSum => OverallTotals?.Counters?.Sales?.Discount?.Sum ?? 0;

    /// <summary>
    /// Приход за всё время: количество надбавок.
    /// </summary>
    public int OverallSalesAddingCount => OverallTotals?.Counters?.Sales?.Adding?.Count ?? 0;

    /// <summary>
    /// Приход за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallSalesAddingSum => OverallTotals?.Counters?.Sales?.Adding?.Sum ?? 0;

    /// <summary>
    /// Приход за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallSalesTaxVat0 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_0");

    /// <summary>
    /// Приход за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallSalesTaxVatNo => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_NO");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallSalesTaxVat5 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_5");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallSalesTaxVat105 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_105");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallSalesTaxVat7 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_7");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallSalesTaxVat107 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_107");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallSalesTaxVat10 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_10");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallSalesTaxVat110 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_110");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallSalesTaxVat20 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_20");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallSalesTaxVat120 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_120");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallSalesTaxVat22 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_22");

    /// <summary>
    /// Приход за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallSalesTaxVat122 => CounterTax(OverallTotals?.Counters?.Sales, "TaxVat_122");

    /// <summary>
    /// Возврат прихода за всё время: количество документов.
    /// </summary>
    public int OverallSalesReturnCount => OverallTotals?.Counters?.SalesReturn?.Count ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: сумма.
    /// </summary>
    public decimal OverallSalesReturnSum => OverallTotals?.Counters?.SalesReturn?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallSalesReturnPaymentsSum => OverallTotals?.Counters?.SalesReturn?.Payments?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: оплата наличными.
    /// </summary>
    public decimal OverallSalesReturnPaymentsCash => OverallTotals?.Counters?.SalesReturn?.Payments?.Cash ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallSalesReturnPaymentsElectronically => OverallTotals?.Counters?.SalesReturn?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallSalesReturnPaymentsPrepaid => OverallTotals?.Counters?.SalesReturn?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallSalesReturnPaymentsCredit => OverallTotals?.Counters?.SalesReturn?.Payments?.Credit ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallSalesReturnPaymentsBarter => OverallTotals?.Counters?.SalesReturn?.Payments?.Barter ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: количество скидок.
    /// </summary>
    public int OverallSalesReturnDiscountCount => OverallTotals?.Counters?.SalesReturn?.Discount?.Count ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: сумма скидок.
    /// </summary>
    public decimal OverallSalesReturnDiscountSum => OverallTotals?.Counters?.SalesReturn?.Discount?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: количество надбавок.
    /// </summary>
    public int OverallSalesReturnAddingCount => OverallTotals?.Counters?.SalesReturn?.Adding?.Count ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallSalesReturnAddingSum => OverallTotals?.Counters?.SalesReturn?.Adding?.Sum ?? 0;

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallSalesReturnTaxVat0 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_0");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallSalesReturnTaxVatNo => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_NO");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallSalesReturnTaxVat5 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_5");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallSalesReturnTaxVat105 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_105");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallSalesReturnTaxVat7 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_7");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallSalesReturnTaxVat107 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_107");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallSalesReturnTaxVat10 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_10");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallSalesReturnTaxVat110 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_110");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallSalesReturnTaxVat20 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_20");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallSalesReturnTaxVat120 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_120");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallSalesReturnTaxVat22 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_22");

    /// <summary>
    /// Возврат прихода за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallSalesReturnTaxVat122 => CounterTax(OverallTotals?.Counters?.SalesReturn, "TaxVat_122");

    /// <summary>
    /// Коррекция прихода за всё время: количество документов.
    /// </summary>
    public int OverallSalesCorrectionCount => OverallTotals?.Counters?.SalesCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: сумма.
    /// </summary>
    public decimal OverallSalesCorrectionSum => OverallTotals?.Counters?.SalesCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallSalesCorrectionPaymentsSum => OverallTotals?.Counters?.SalesCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: оплата наличными.
    /// </summary>
    public decimal OverallSalesCorrectionPaymentsCash => OverallTotals?.Counters?.SalesCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallSalesCorrectionPaymentsElectronically => OverallTotals?.Counters?.SalesCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallSalesCorrectionPaymentsPrepaid => OverallTotals?.Counters?.SalesCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallSalesCorrectionPaymentsCredit => OverallTotals?.Counters?.SalesCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallSalesCorrectionPaymentsBarter => OverallTotals?.Counters?.SalesCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: количество скидок.
    /// </summary>
    public int OverallSalesCorrectionDiscountCount => OverallTotals?.Counters?.SalesCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: сумма скидок.
    /// </summary>
    public decimal OverallSalesCorrectionDiscountSum => OverallTotals?.Counters?.SalesCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: количество надбавок.
    /// </summary>
    public int OverallSalesCorrectionAddingCount => OverallTotals?.Counters?.SalesCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallSalesCorrectionAddingSum => OverallTotals?.Counters?.SalesCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat0 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVatNo => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat5 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat105 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat7 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat107 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat10 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat110 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat20 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat120 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat22 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция прихода за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallSalesCorrectionTaxVat122 => CounterTax(OverallTotals?.Counters?.SalesCorrection, "TaxVat_122");

    /// <summary>
    /// Коррекция возврата прихода за всё время: количество документов.
    /// </summary>
    public int OverallSalesReturnCorrectionCount => OverallTotals?.Counters?.SalesReturnCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма.
    /// </summary>
    public decimal OverallSalesReturnCorrectionSum => OverallTotals?.Counters?.SalesReturnCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallSalesReturnCorrectionPaymentsSum => OverallTotals?.Counters?.SalesReturnCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: оплата наличными.
    /// </summary>
    public decimal OverallSalesReturnCorrectionPaymentsCash => OverallTotals?.Counters?.SalesReturnCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallSalesReturnCorrectionPaymentsElectronically => OverallTotals?.Counters?.SalesReturnCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallSalesReturnCorrectionPaymentsPrepaid => OverallTotals?.Counters?.SalesReturnCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallSalesReturnCorrectionPaymentsCredit => OverallTotals?.Counters?.SalesReturnCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallSalesReturnCorrectionPaymentsBarter => OverallTotals?.Counters?.SalesReturnCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: количество скидок.
    /// </summary>
    public int OverallSalesReturnCorrectionDiscountCount => OverallTotals?.Counters?.SalesReturnCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма скидок.
    /// </summary>
    public decimal OverallSalesReturnCorrectionDiscountSum => OverallTotals?.Counters?.SalesReturnCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: количество надбавок.
    /// </summary>
    public int OverallSalesReturnCorrectionAddingCount => OverallTotals?.Counters?.SalesReturnCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallSalesReturnCorrectionAddingSum => OverallTotals?.Counters?.SalesReturnCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat0 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVatNo => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat5 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat105 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat7 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat107 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat10 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat110 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat20 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat120 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat22 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция возврата прихода за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallSalesReturnCorrectionTaxVat122 => CounterTax(OverallTotals?.Counters?.SalesReturnCorrection, "TaxVat_122");

    /// <summary>
    /// Расход за всё время: количество документов.
    /// </summary>
    public int OverallPurchasesCount => OverallTotals?.Counters?.Purchases?.Count ?? 0;

    /// <summary>
    /// Расход за всё время: сумма.
    /// </summary>
    public decimal OverallPurchasesSum => OverallTotals?.Counters?.Purchases?.Sum ?? 0;

    /// <summary>
    /// Расход за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallPurchasesPaymentsSum => OverallTotals?.Counters?.Purchases?.Payments?.Sum ?? 0;

    /// <summary>
    /// Расход за всё время: оплата наличными.
    /// </summary>
    public decimal OverallPurchasesPaymentsCash => OverallTotals?.Counters?.Purchases?.Payments?.Cash ?? 0;

    /// <summary>
    /// Расход за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallPurchasesPaymentsElectronically => OverallTotals?.Counters?.Purchases?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Расход за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallPurchasesPaymentsPrepaid => OverallTotals?.Counters?.Purchases?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Расход за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallPurchasesPaymentsCredit => OverallTotals?.Counters?.Purchases?.Payments?.Credit ?? 0;

    /// <summary>
    /// Расход за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallPurchasesPaymentsBarter => OverallTotals?.Counters?.Purchases?.Payments?.Barter ?? 0;

    /// <summary>
    /// Расход за всё время: количество скидок.
    /// </summary>
    public int OverallPurchasesDiscountCount => OverallTotals?.Counters?.Purchases?.Discount?.Count ?? 0;

    /// <summary>
    /// Расход за всё время: сумма скидок.
    /// </summary>
    public decimal OverallPurchasesDiscountSum => OverallTotals?.Counters?.Purchases?.Discount?.Sum ?? 0;

    /// <summary>
    /// Расход за всё время: количество надбавок.
    /// </summary>
    public int OverallPurchasesAddingCount => OverallTotals?.Counters?.Purchases?.Adding?.Count ?? 0;

    /// <summary>
    /// Расход за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallPurchasesAddingSum => OverallTotals?.Counters?.Purchases?.Adding?.Sum ?? 0;

    /// <summary>
    /// Расход за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallPurchasesTaxVat0 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_0");

    /// <summary>
    /// Расход за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallPurchasesTaxVatNo => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_NO");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallPurchasesTaxVat5 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_5");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallPurchasesTaxVat105 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_105");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallPurchasesTaxVat7 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_7");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallPurchasesTaxVat107 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_107");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallPurchasesTaxVat10 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_10");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallPurchasesTaxVat110 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_110");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallPurchasesTaxVat20 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_20");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallPurchasesTaxVat120 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_120");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallPurchasesTaxVat22 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_22");

    /// <summary>
    /// Расход за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallPurchasesTaxVat122 => CounterTax(OverallTotals?.Counters?.Purchases, "TaxVat_122");

    /// <summary>
    /// Возврат расхода за всё время: количество документов.
    /// </summary>
    public int OverallPurchasesReturnCount => OverallTotals?.Counters?.PurchasesReturn?.Count ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: сумма.
    /// </summary>
    public decimal OverallPurchasesReturnSum => OverallTotals?.Counters?.PurchasesReturn?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallPurchasesReturnPaymentsSum => OverallTotals?.Counters?.PurchasesReturn?.Payments?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: оплата наличными.
    /// </summary>
    public decimal OverallPurchasesReturnPaymentsCash => OverallTotals?.Counters?.PurchasesReturn?.Payments?.Cash ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallPurchasesReturnPaymentsElectronically => OverallTotals?.Counters?.PurchasesReturn?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallPurchasesReturnPaymentsPrepaid => OverallTotals?.Counters?.PurchasesReturn?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallPurchasesReturnPaymentsCredit => OverallTotals?.Counters?.PurchasesReturn?.Payments?.Credit ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallPurchasesReturnPaymentsBarter => OverallTotals?.Counters?.PurchasesReturn?.Payments?.Barter ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: количество скидок.
    /// </summary>
    public int OverallPurchasesReturnDiscountCount => OverallTotals?.Counters?.PurchasesReturn?.Discount?.Count ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: сумма скидок.
    /// </summary>
    public decimal OverallPurchasesReturnDiscountSum => OverallTotals?.Counters?.PurchasesReturn?.Discount?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: количество надбавок.
    /// </summary>
    public int OverallPurchasesReturnAddingCount => OverallTotals?.Counters?.PurchasesReturn?.Adding?.Count ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallPurchasesReturnAddingSum => OverallTotals?.Counters?.PurchasesReturn?.Adding?.Sum ?? 0;

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat0 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_0");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVatNo => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_NO");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat5 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_5");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat105 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_105");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat7 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_7");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat107 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_107");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat10 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_10");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat110 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_110");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat20 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_20");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat120 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_120");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat22 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_22");

    /// <summary>
    /// Возврат расхода за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallPurchasesReturnTaxVat122 => CounterTax(OverallTotals?.Counters?.PurchasesReturn, "TaxVat_122");

    /// <summary>
    /// Коррекция расхода за всё время: количество документов.
    /// </summary>
    public int OverallPurchasesCorrectionCount => OverallTotals?.Counters?.PurchasesCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: сумма.
    /// </summary>
    public decimal OverallPurchasesCorrectionSum => OverallTotals?.Counters?.PurchasesCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallPurchasesCorrectionPaymentsSum => OverallTotals?.Counters?.PurchasesCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: оплата наличными.
    /// </summary>
    public decimal OverallPurchasesCorrectionPaymentsCash => OverallTotals?.Counters?.PurchasesCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallPurchasesCorrectionPaymentsElectronically => OverallTotals?.Counters?.PurchasesCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallPurchasesCorrectionPaymentsPrepaid => OverallTotals?.Counters?.PurchasesCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallPurchasesCorrectionPaymentsCredit => OverallTotals?.Counters?.PurchasesCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallPurchasesCorrectionPaymentsBarter => OverallTotals?.Counters?.PurchasesCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: количество скидок.
    /// </summary>
    public int OverallPurchasesCorrectionDiscountCount => OverallTotals?.Counters?.PurchasesCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: сумма скидок.
    /// </summary>
    public decimal OverallPurchasesCorrectionDiscountSum => OverallTotals?.Counters?.PurchasesCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: количество надбавок.
    /// </summary>
    public int OverallPurchasesCorrectionAddingCount => OverallTotals?.Counters?.PurchasesCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallPurchasesCorrectionAddingSum => OverallTotals?.Counters?.PurchasesCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat0 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVatNo => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat5 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat105 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat7 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat107 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat10 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat110 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat20 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat120 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat22 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция расхода за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallPurchasesCorrectionTaxVat122 => CounterTax(OverallTotals?.Counters?.PurchasesCorrection, "TaxVat_122");

    /// <summary>
    /// Коррекция возврата расхода за всё время: количество документов.
    /// </summary>
    public int OverallPurchasesReturnCorrectionCount => OverallTotals?.Counters?.PurchasesReturnCorrection?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionSum => OverallTotals?.Counters?.PurchasesReturnCorrection?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: общая сумма оплат.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionPaymentsSum => OverallTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: оплата наличными.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionPaymentsCash => OverallTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Cash ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: оплата безналичными.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionPaymentsElectronically => OverallTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Electronically ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: оплата авансом (предоплата).
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionPaymentsPrepaid => OverallTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Prepaid ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: оплата в кредит (постоплата).
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionPaymentsCredit => OverallTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Credit ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: встречное предоставление.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionPaymentsBarter => OverallTotals?.Counters?.PurchasesReturnCorrection?.Payments?.Barter ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: количество скидок.
    /// </summary>
    public int OverallPurchasesReturnCorrectionDiscountCount => OverallTotals?.Counters?.PurchasesReturnCorrection?.Discount?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма скидок.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionDiscountSum => OverallTotals?.Counters?.PurchasesReturnCorrection?.Discount?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: количество надбавок.
    /// </summary>
    public int OverallPurchasesReturnCorrectionAddingCount => OverallTotals?.Counters?.PurchasesReturnCorrection?.Adding?.Count ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма надбавок.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionAddingSum => OverallTotals?.Counters?.PurchasesReturnCorrection?.Adding?.Sum ?? 0;

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 0%.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat0 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_0");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога без НДС.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVatNo => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_NO");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 5%.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat5 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_5");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 5/105.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat105 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_105");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 7%.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat7 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_7");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 7/107.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat107 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_107");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 10%.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat10 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_10");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 10/110.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat110 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_110");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 20%.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat20 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_20");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 20/120.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat120 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_120");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 22%.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat22 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_22");

    /// <summary>
    /// Коррекция возврата расхода за всё время: сумма налога НДС 22/122.
    /// </summary>
    public decimal OverallPurchasesReturnCorrectionTaxVat122 => CounterTax(OverallTotals?.Counters?.PurchasesReturnCorrection, "TaxVat_122");

    private static decimal CounterTax(DocData? counter, string rate)
        => counter?.Tax != null && counter.Tax.TryGetValue(rate, out var sum) ? sum : 0;

    // Документ (структура Check / CheckDocument)

    /// <summary>
    /// Сумма чека.
    /// </summary>
    public decimal CheckSum => Check?.Sum ?? 0;

    /// <summary>
    /// Сдача.
    /// </summary>
    public decimal CheckChange => Check?.Change ?? 0;

    /// <summary>
    /// Остаток наличных в ящике (внесение/выемка).
    /// </summary>
    public decimal? CheckCashSum => Check?.CashSum;

    /// <summary>
    /// Номер денежного ящика.
    /// </summary>
    public int CheckDrawerNumber => Check?.DrawerNumber ?? 0;

    /// <summary>
    /// Подтверждён в ФН.
    /// </summary>
    public bool CheckIsTrustedInFn => Check?.TrustedInFn ?? false;

    /// <summary>
    /// Фискальный документ.
    /// </summary>
    public bool CheckIsFiscal => Check?.IsFiscal ?? false;

    /// <summary>
    /// Расчёт в интернете.
    /// </summary>
    public bool CheckIsOperationOnline => Check?.OperationOnline ?? false;

    /// <summary>
    /// Без печати на ленте.
    /// </summary>
    public bool CheckIsElectronically => Check?.Electronically ?? false;

    /// <summary>
    /// Обработан успешно.
    /// </summary>
    public bool CheckIsProcessed => Check?.Processed ?? false;

    /// <summary>
    /// Признак замены ставки НДС.
    /// </summary>
    public bool CheckIsReplaceTax => Check?.IsReplaceTax ?? false;

    /// <summary>
    /// Телефон/почта клиента.
    /// </summary>
    public string? CheckClientContact => Check?.ClientContact;

    /// <summary>
    /// Система налогообложения (СНО).
    /// </summary>
    public TaxSystem CheckTaxType => (TaxSystem)(Check?.TaxType ?? 0);

    /// <summary>
    /// Часовая зона.
    /// </summary>
    public int CheckTimeZone => Check?.TimeZone ?? 0;

    /// <summary>
    /// Дополнительный реквизит чека.
    /// </summary>
    public string? CheckAdditionalAttribute => Check?.AdditionalAttribute;

    /// <summary>
    /// Номер смены.
    /// </summary>
    public int CheckShiftNumber => Check?.ShiftNumber ?? 0;

    /// <summary>
    /// Номер фискального документа.
    /// </summary>
    public int CheckDocNumber => Check?.DocNumber ?? 0;

    /// <summary>
    /// Номер ФД за смену.
    /// </summary>
    public int CheckDocNumberInShift => Check?.DocNumberInShift ?? 0;

    /// <summary>
    /// Фискальный признак документа.
    /// </summary>
    public string? CheckFiscalSign => Check?.FiscalSign;

    /// <summary>
    /// Серийный номер ФН.
    /// </summary>
    public string? CheckFn => Check?.Fn;

    /// <summary>
    /// Время регистрации по часам ККМ.
    /// </summary>
    public DateTime? CheckFiscalDate => Check?.FiscalDate;

    /// <summary>
    /// Имя кассира.
    /// </summary>
    public string? CheckCashierName => Check?.CashierName;

    /// <summary>
    /// ИНН кассира.
    /// </summary>
    public string? CheckCashierVatin => Check?.CashierVatin;

    /// <summary>
    /// Адрес расчётов.
    /// </summary>
    public string? CheckSaleAddress => Check?.SaleAddress;

    /// <summary>
    /// Место расчётов.
    /// </summary>
    public string? CheckSaleLocation => Check?.SaleLocation;

    /// <summary>
    /// Версия ФФД.
    /// </summary>
    public string? CheckFfdVersion => Check?.FfdVersion;

    /// <summary>
    /// Структура тегов документа.
    /// </summary>
    public string? CheckTlv => Check?.Tlv;

    /// <summary>
    /// Тип документа.
    /// </summary>
    public CheckType CheckTaskType => Check?.TaskType ?? default;

    /// <summary>
    /// Идентификатор документа.
    /// </summary>
    public string? CheckDocId => Check?.DocId;

    /// <summary>
    /// Идентификатор исходного документа (у копий и связанных операций).
    /// </summary>
    public string? CheckBaseDocId => Check?.BaseDocId;

    /// <summary>
    /// Дата создания документа.
    /// </summary>
    public DateTime? CheckDate => Check?.Date;

    /// <summary>
    /// Идентификатор терминала.
    /// </summary>
    public string? CheckTerminalId => Check?.TerminalId;

    /// <summary>
    /// Имя устройства.
    /// </summary>
    public string? CheckDeviceName => Check?.DeviceName;

    /// <summary>
    /// Пул документа.
    /// </summary>
    public string? CheckPoolId => Check?.PoolId;

    /// <summary>
    /// Код результата обработки.
    /// </summary>
    public int CheckResultCode => Check?.ResultCode ?? 0;

    /// <summary>
    /// Описание результата.
    /// </summary>
    public string? CheckResultDescription => Check?.ResultDescription;

    /// <summary>
    /// Версия сервера ККМ.
    /// </summary>
    public string? CheckServerVersion => Check?.ServerVersion;

    /// <summary>
    /// Количество аннулирований (X/Z).
    /// </summary>
    public int CheckAnullatesCount => Check?.AnullatesCount ?? 0;

    // Данные коррекции — чек коррекции ФФД 1.05/1.2.

    /// <summary>
    /// Тип коррекции (самостоятельно / по предписанию).
    /// </summary>
    public CorrectionTypes CorrectionDataType => Check?.CorrectionData?.Type ?? CorrectionTypes.Самостоятельно;

    /// <summary>
    /// Описание (основание) коррекции.
    /// </summary>
    public string? CorrectionDataDescription => Check?.CorrectionData?.Description;

    /// <summary>
    /// Дата совершения корректируемого расчёта.
    /// </summary>
    public DateTime? CorrectionDataDate => Check?.CorrectionData?.Date;

    /// <summary>
    /// Номер предписания налогового органа (для коррекции по предписанию).
    /// </summary>
    public string? CorrectionDataNumber => Check?.CorrectionData?.Number;

    // Организация и ОФД

    /// <summary>
    /// Название организации.
    /// </summary>
    public string? OrganizationInfo => Check?.DocumentHeader?.OrganizationInfo ?? Kkt?.Fn?.OrganizationName;

    /// <summary>
    /// ИНН организации.
    /// </summary>
    public string? OrganizationVatin => Check?.DocumentHeader?.Vatin ?? Kkt?.Fn?.Vatin;

    /// <summary>
    /// Наименование ОФД.
    /// </summary>
    public string? OfdName => Check?.DocumentHeader?.OfdOrganizationName ?? Kkt?.Fn?.Ofd?.Name;

    /// <summary>
    /// ИНН ОФД.
    /// </summary>
    public string? OfdVatin => Check?.DocumentHeader?.OfdVatin ?? Kkt?.Fn?.Ofd?.Vatin;

    /// <summary>
    /// Адрес сервера ОФД (GetKktInfo).
    /// </summary>
    public string? OfdHost => Kkt?.Fn?.Ofd?.Host;

    /// <summary>
    /// Порт сервера ОФД (GetKktInfo).
    /// </summary>
    public int OfdPort => Kkt?.Fn?.Ofd?.Port ?? 0;


    // Покупатель (структура CustomerDetail / CheckCustomer)

    /// <summary>
    /// Наименование/ФИО покупателя.
    /// </summary>
    public string? CustomerDetailInfo => Check?.CustomerDetail?.Info;

    /// <summary>
    /// ИНН покупателя.
    /// </summary>
    public string? CustomerDetailInn => Check?.CustomerDetail?.Inn;

    /// <summary>
    /// Эл. почта покупателя.
    /// </summary>
    public string? CustomerDetailEmail => Check?.CustomerDetail?.Email;

    /// <summary>
    /// Телефон покупателя.
    /// </summary>
    public string? CustomerDetailPhone => Check?.CustomerDetail?.Phone;

    /// <summary>
    /// Дата рождения.
    /// </summary>
    public string? CustomerDetailDateOfBirth => Check?.CustomerDetail?.DateOfBirth;

    /// <summary>
    /// Гражданство (код).
    /// </summary>
    public string? CustomerDetailCitizenship => Check?.CustomerDetail?.Citizenship;

    /// <summary>
    /// Код вида документа.
    /// </summary>
    public int? CustomerDetailDocumentTypeCode => Check?.CustomerDetail?.DocumentTypeCode;

    /// <summary>
    /// Данные документа.
    /// </summary>
    public string? CustomerDetailDocumentData => Check?.CustomerDetail?.DocumentData;

    /// <summary>
    /// Адрес покупателя.
    /// </summary>
    public string? CustomerDetailAddress => Check?.CustomerDetail?.Address;

    // Данные QR-кода (структура QrData / QrCheckData)

    /// <summary>
    /// Дата/время (QR).
    /// </summary>
    public DateTime? QrDataDate => Check?.QrData?.Date;

    /// <summary>
    /// Сумма (QR).
    /// </summary>
    public decimal QrDataAmount => Check?.QrData?.Amount ?? 0;

    /// <summary>
    /// Номер ФН (QR).
    /// </summary>
    public string? QrDataFn => Check?.QrData?.Fn;

    /// <summary>
    /// Номер ФД (QR).
    /// </summary>
    public int QrDataFd => Check?.QrData?.Fd ?? 0;

    /// <summary>
    /// Фискальный признак (QR).
    /// </summary>
    public string? QrDataFp => Check?.QrData?.Fp;

    /// <summary>
    /// Порядковый номер (QR).
    /// </summary>
    public int QrDataN => Check?.QrData?.N ?? 0;

    // Оплаты документа (структура Payments / CheckPayments)

    /// <summary>
    /// Оплата наличными (ответ).
    /// </summary>
    public decimal PaymentsCash => Check?.Payments?.Cash ?? 0;

    /// <summary>
    /// Оплата безналичными (ответ).
    /// </summary>
    public decimal PaymentsElectronic => Check?.Payments?.Electronic ?? 0;

    /// <summary>
    /// Предоплата, зачёт аванса (ответ).
    /// </summary>
    public decimal PaymentsPrePaid => Check?.Payments?.PrePaid ?? 0;

    /// <summary>
    /// Постоплата, кредит (ответ).
    /// </summary>
    public decimal PaymentsCredit => Check?.Payments?.Credit ?? 0;

    /// <summary>
    /// Встречное предоставление (ответ).
    /// </summary>
    public decimal PaymentsBarter => Check?.Payments?.Barter ?? 0;

    /// <summary>
    /// Детализация безналичных оплат (ответ).
    /// </summary>
    public IReadOnlyList<ElectronicPayment> CheckElectronicPayments => Check?.ElectronicPayments ?? [];

    // Позиции чека (массив CheckItems)

    /// <summary>
    /// Количество позиций в чеке.
    /// </summary>
    public int CheckItemsCount => Check?.CheckItems?.Length ?? 0;

    /// <summary>
    /// Позиции чека ответа (перебираются по индексу).
    /// </summary>
    public IReadOnlyList<CheckItem> CheckItems => Check?.CheckItems ?? [];

    // Операция (структура Operation / DeviceTaskInfo) — GetOperation / GetOperationLast

    /// <summary>
    /// Идентификатор документа операции.
    /// </summary>
    public string? OperationDocId => Operation?.DocId;

    /// <summary>
    /// Тип задания операции.
    /// </summary>
    public CheckType OperationTaskType => Operation?.TaskType ?? default;

    /// <summary>
    /// Дата операции.
    /// </summary>
    public DateTime? OperationDate => Operation?.Date;

    /// <summary>
    /// Имя кассы операции.
    /// </summary>
    public string? OperationDeviceName => Operation?.DeviceName;

    /// <summary>
    /// Операция обработана успешно.
    /// </summary>
    public bool OperationIsProcessed => Operation?.Processed ?? false;

    /// <summary>
    /// Код результата операции.
    /// </summary>
    public int OperationResultCode => Operation?.ResultCode ?? 0;

    /// <summary>
    /// Описание результата операции.
    /// </summary>
    public string? OperationResultDescription => Operation?.ResultDescription;

    /// <summary>
    /// Версия сервера (операция).
    /// </summary>
    public string? OperationServerVersion => Operation?.ServerVersion;

    /// <summary>
    /// Идентификатор базового документа операции.
    /// </summary>
    public string? OperationBaseDocId => Operation?.BaseDocId;

    /// <summary>
    /// Идентификатор запроса операции.
    /// </summary>
    public string? OperationRequestId => Operation?.RequestId;

    /// <summary>
    /// Идентификатор терминала операции.
    /// </summary>
    public string? OperationTerminalId => Operation?.TerminalId;

    /// <summary>
    /// Идентификатор пула операции.
    /// </summary>
    public string? OperationPoolId => Operation?.PoolId;

    /// <summary>
    /// Версия клиента (операция).
    /// </summary>
    public string? OperationClientVersion => Operation?.ClientVersion;

    /// <summary>
    /// Название приложения-отправителя.
    /// </summary>
    public string? OperationSenderAppName => Operation?.SenderInfo?.AppName;

    /// <summary>
    /// Версия приложения-отправителя.
    /// </summary>
    public string? OperationSenderAppVersion => Operation?.SenderInfo?.AppVersion;

    /// <summary>
    /// Модель устройства операции.
    /// </summary>
    public string? OperationDeviceModel => Operation?.DeviceInfo?.Model;

    /// <summary>
    /// Заводской номер устройства операции.
    /// </summary>
    public string? OperationDeviceSerialNumber => Operation?.DeviceInfo?.SerialNumber;

    /// <summary>
    /// Версия прошивки устройства операции.
    /// </summary>
    public string? OperationDeviceFirmwareVersion => Operation?.DeviceInfo?.FirmwareVersion;

    // Статус задания (структура TaskStatus / ResponseTaskStatus) — GetTaskStatus

    /// <summary>
    /// Идентификатор документа задания.
    /// </summary>
    public string? TaskStatusDocId => TaskStatus?.DocId;

    /// <summary>
    /// Имя кассы задания.
    /// </summary>
    public string? TaskStatusDeviceName => TaskStatus?.DeviceName;

    /// <summary>
    /// Статус отправки (0 — в очереди, 1 — отправлена, 2 — обработана, −1 — ошибка).
    /// </summary>
    public QueueTaskStatus TaskStatusSentToPrint => TaskStatus?.SentToPrint ?? default;

    /// <summary>
    /// Позиция задания в очереди.
    /// </summary>
    public int TaskStatusNumberInQueue => TaskStatus?.NumberInQueue ?? 0;

    /// <summary>
    /// Размер очереди.
    /// </summary>
    public int TaskStatusQueueSize => TaskStatus?.QueueSize ?? 0;

    /// <summary>
    /// Номер смены задания.
    /// </summary>
    public int TaskStatusShiftNumber => TaskStatus?.ShiftNumber ?? 0;

    /// <summary>
    /// Номер ФД задания.
    /// </summary>
    public int TaskStatusDocNumber => TaskStatus?.DocNumber ?? 0;

    /// <summary>
    /// Тип задания.
    /// </summary>
    public CheckType TaskStatusTaskType => TaskStatus?.TaskType ?? default;

    /// <summary>
    /// Фискальный признак документа задания.
    /// </summary>
    public string? TaskStatusFiscalSign => TaskStatus?.FiscalSign;

    /// <summary>
    /// Код результата задания.
    /// </summary>
    public int TaskStatusResultCode => TaskStatus?.ResultCode ?? 0;

    /// <summary>
    /// Описание результата задания.
    /// </summary>
    public string? TaskStatusResultDescription => TaskStatus?.ResultDescription;

    /// <summary>
    /// Дата постановки задания.
    /// </summary>
    public DateTime? TaskStatusDate => TaskStatus?.Date;

    // Задание очереди (структура QueueTask / QueueTaskState) — GetQueueTask

    /// <summary>
    /// Идентификатор документа задания очереди.
    /// </summary>
    public string? QueueTaskDocId => QueueTask?.DocId;

    /// <summary>
    /// Имя кассы задания очереди.
    /// </summary>
    public string? QueueTaskDeviceName => QueueTask?.DeviceName;

    /// <summary>
    /// Состояние документа в очереди печати. NotFound, если задание не получено.
    /// </summary>
    public DocumentPrintState QueueTaskDocState => QueueTask?.DocState ?? DocumentPrintState.NotFound;

    /// <summary>
    /// Состояние очереди печати.
    /// </summary>
    public QueueState QueueTaskQueueState => QueueTask?.QueueState ?? default;

    /// <summary>
    /// Позиция задания в очереди.
    /// </summary>
    public int QueueTaskNumberInQueue => QueueTask?.NumberInQueue ?? 0;

    /// <summary>
    /// Описание текущего этапа обработки.
    /// </summary>
    public string? QueueTaskPrintStatusDescription => QueueTask?.PrintStatusDescription;

    /// <summary>
    /// Фискальный признак документа (очередь).
    /// </summary>
    public string? QueueTaskFiscalSign => QueueTask?.FiscalSign;

    /// <summary>
    /// Код результата задания очереди.
    /// </summary>
    public int QueueTaskResultCode => QueueTask?.ResultCode ?? 0;

    /// <summary>
    /// Описание результата задания очереди.
    /// </summary>
    public string? QueueTaskResultDescription => QueueTask?.ResultDescription;

    /// <summary>
    /// Дата изменения статуса задания очереди.
    /// </summary>
    public DateTime? QueueTaskDate => QueueTask?.Date;

    // Результат проверки КМ в ОИСМ (структура MarkingProcessing / ProcessingKmResult) — GetProcessingKMResult

    /// <summary>
    /// Идентификатор запроса КМ (Guid).
    /// </summary>
    public string? MarkingProcessingGuid => MarkingProcessing?.Guid;

    /// <summary>
    /// Итог проверки кода маркировки в ОИСМ.
    /// </summary>
    public bool MarkingProcessingResult => MarkingProcessing?.Result ?? false;

    /// <summary>
    /// Код результата проверки (тег 2106).
    /// </summary>
    public int MarkingProcessingResultCode => MarkingProcessing?.ResultCode ?? 0;

    /// <summary>
    /// Статус информации о коде маркировки (тег 2109).
    /// </summary>
    public int? MarkingProcessingStatusInfo => MarkingProcessing?.StatusInfo;

    /// <summary>
    /// Код обработки запроса (тег 2105).
    /// </summary>
    public int MarkingProcessingHandleCode => MarkingProcessing?.HandleCode ?? 0;

    /// <summary>
    /// Статус получения результата от ОИСМ (0 — получен, 1 — ещё нет, 2 — не может быть получен).
    /// </summary>
    public int MarkingProcessingRequestStatus => MarkingProcessing?.RequestStatus ?? 0;

    // Результат фискализации (структура FiscalizationDocument) — GetFiscalization
    // Общие поля (DocumentId/FiscalSign/RnNumber/ShiftNumber/CheckNumber/IsFiscal) заполняются плоскими в GetFiscalization.

    /// <summary>
    /// Тип выполненной операции фискализации.
    /// </summary>
    public FiscalizationOperationType FiscalizationResultOperationType => FiscalizationDocument?.OperationType ?? default;

    /// <summary>
    /// Наименование организации (результат фискализации).
    /// </summary>
    public string? FiscalizationResultCompanyName => FiscalizationDocument?.CompanyName;

    /// <summary>
    /// ИНН организации (результат фискализации).
    /// </summary>
    public string? FiscalizationResultVatin => FiscalizationDocument?.Vatin;

    /// <summary>
    /// Коды систем налогообложения (результат фискализации).
    /// </summary>
    public string? FiscalizationResultTaxationSystems => FiscalizationDocument?.TaxationSystems;

    /// <summary>
    /// Версия ФФД ККТ (результат фискализации).
    /// </summary>
    public string? FiscalizationResultFfdVersionKkt => FiscalizationDocument?.FfdVersionKkt;

    /// <summary>
    /// Версия ФФД ФН (результат фискализации).
    /// </summary>
    public string? FiscalizationResultFfdVersionFn => FiscalizationDocument?.FfdVersionFn;

    // Локальная проверка КМ (структура MarkingCheck / RequestKmResult) — RequestKM

    /// <summary>
    /// Наличие связи с ОИСМ на момент запроса.
    /// </summary>
    public bool MarkingCheckIsmConnected => MarkingCheck?.IsmConnected ?? false;

    /// <summary>
    /// Проверка формата КМ прошла успешно.
    /// </summary>
    public bool MarkingCheckFormatChecking => MarkingCheck?.FormatChecking ?? false;

    /// <summary>
    /// Проверка КМ поставлена в обработку.
    /// </summary>
    public bool MarkingCheckChecking => MarkingCheck?.Checking ?? false;

    /// <summary>
    /// Результат проверки КМ (если уже доступен).
    /// </summary>
    public bool MarkingCheckCheckingResult => MarkingCheck?.CheckingResult ?? false;

    /// <summary>
    /// Штрихкод после приведения (со спецсимволами GS).
    /// </summary>
    public string? MarkingCheckBarcode => MarkingCheck?.Barcode;

    // Проверка КМ в ОИСМ (структура MarkingVerify / MarkingVerifyResult) — VerifyMarking*.
    // Префикс Marking (без Verify); Code → MarkingResultCode, чтобы не конфликтовать с входным MarkingCode.

    /// <summary>
    /// Код результата проверки маркировки.
    /// </summary>
    public int MarkingResultCode => MarkingVerify?.Code ?? 0;

    /// <summary>
    /// Описание результата проверки маркировки.
    /// </summary>
    public string? MarkingResultDescription => MarkingVerify?.Description;

    /// <summary>
    /// Проверенные коды маркировки (перебирайте для сведений по каждому КМ).
    /// </summary>
    public IReadOnlyList<CodeMarkInfo> MarkingCodesInfo => MarkingVerify?.Codes ?? [];

    /// <summary>
    /// Количество проверенных кодов маркировки.
    /// </summary>
    public int MarkingCodesCount => MarkingVerify?.Codes?.Count ?? 0;

    /// <summary>
    /// Идентификатор операции проверки.
    /// </summary>
    public string? MarkingReqId => MarkingVerify?.ReqId;

    /// <summary>
    /// Временная метка операции проверки.
    /// </summary>
    public long MarkingReqTimestamp => MarkingVerify?.ReqTimestamp ?? 0;

    /// <summary>
    /// Признак офлайн-проверки.
    /// </summary>
    public bool MarkingIsCheckedOffline => MarkingVerify?.IsCheckedOffline ?? false;

    /// <summary>
    /// Статус локального модуля проверки (enum).
    /// </summary>
    public LocalModuleStatus MarkingStatus => MarkingVerify?.Status ?? LocalModuleStatus.Unknown;

    /// <summary>
    /// Требуется загрузка данных из ГИС МТ.
    /// </summary>
    public bool MarkingRequiresDownload => MarkingVerify?.RequiresDownload ?? false;

    /// <summary>
    /// Версия локального модуля проверки.
    /// </summary>
    public string? MarkingVersion => MarkingVerify?.Version;

    /// <summary>
    /// Адрес сервиса проверки.
    /// </summary>
    public string? MarkingServiceUrl => MarkingVerify?.ServiceUrl;

    /// <summary>
    /// Режим работы модуля.
    /// </summary>
    public string? MarkingOperationMode => MarkingVerify?.OperationMode;

    /// <summary>
    /// Имя модуля.
    /// </summary>
    public string? MarkingName => MarkingVerify?.Name;

    /// <summary>
    /// Экземпляр модуля.
    /// </summary>
    public string? MarkingInst => MarkingVerify?.Inst;

    /// <summary>
    /// ИНН (локальный модуль).
    /// </summary>
    public string? MarkingInn => MarkingVerify?.Inn;

    /// <summary>
    /// Версия базы данных модуля.
    /// </summary>
    public string? MarkingDbVersion => MarkingVerify?.DbVersion;

    /// <summary>
    /// Метка времени последнего обновления данных.
    /// </summary>
    public long MarkingLastUpdate => MarkingVerify?.LastUpdate ?? 0;

    /// <summary>
    /// Метка времени последней синхронизации с ГИС МТ.
    /// </summary>
    public long MarkingLastSync => MarkingVerify?.LastSync ?? 0;

    /// <summary>
    /// Дата последнего обновления (строкой).
    /// </summary>
    public string? MarkingLastUpdateDate => MarkingVerify?.LastUpdateDate;

    /// <summary>
    /// Дата последней синхронизации (строкой).
    /// </summary>
    public string? MarkingLastSyncDate => MarkingVerify?.LastSyncDate;

    /// <summary>
    /// Статус репликации по товарным группам (ключ — товарная группа).
    /// </summary>
    public IReadOnlyDictionary<string, ReplicationItem> MarkingReplicationStatus
        => MarkingVerify?.ReplicationStatus ?? new Dictionary<string, ReplicationItem>();

    /// <summary>
    /// Результаты проверки локальным модулем «Честного знака».
    /// </summary>
    public IReadOnlyList<LocalCheckResult> MarkingLocalCheckResults =>
        (MarkingVerify?.LocalCheckResults ?? []).Select(x => new LocalCheckResult(x)).ToList();

    // Текущий статус смены (структура ShiftStatus) — GetShiftStatus.

    /// <summary>
    /// Номер смены (из статуса смены).
    /// </summary>
    public int ShiftStatusShiftNumber => ShiftStatus?.ShiftNumber ?? 0;

    /// <summary>
    /// Номер последнего ФД (из статуса смены).
    /// </summary>
    public int ShiftStatusCheckNumber => ShiftStatus?.CheckNumber ?? 0;

    /// <summary>
    /// Состояние смены (из статуса смены).
    /// </summary>
    public ShiftState ShiftStatusShiftState => ShiftStatus?.ShiftState ?? default;

    /// <summary>
    /// Количество непереданных в ОФД документов (из статуса смены).
    /// </summary>
    public long ShiftStatusBacklogDocumentsCounter => ShiftStatus?.Backlog?.DocumentsCounter ?? 0;

    /// <summary>
    /// Номер первого непереданного документа (из статуса смены).
    /// </summary>
    public long ShiftStatusBacklogDocumentFirstNumber => ShiftStatus?.Backlog?.DocumentFirstNumber ?? 0;

    /// <summary>
    /// Дата и время первого непереданного документа (из статуса смены).
    /// </summary>
    public DateTime? ShiftStatusBacklogDocumentFirstDateTime => ShiftStatus?.Backlog?.DocumentFirstDateTime;

    // Печатный шаблон (структура PrintTemplate) — GetTemplate.

    /// <summary>
    /// Имя печатного шаблона.
    /// </summary>
    public string? PrintTemplateName => PrintTemplate?.Name;

    /// <summary>
    /// Тип печатного шаблона.
    /// </summary>
    public PrintTemplateType PrintTemplateType => PrintTemplate?.Type ?? default;

    /// <summary>
    /// Количество строк печатного шаблона.
    /// </summary>
    public int PrintTemplateLineCount => PrintTemplate?.LineCount ?? 0;

    // Шаблон чека (структуры CheckTemplate / CheckTemplateDocument) — GetCheckTemplate.

    /// <summary>
    /// Имя шаблона чека.
    /// </summary>
    public string? CheckTemplateName => CheckTemplate?.Name;

    /// <summary>
    /// Тип чека шаблона.
    /// </summary>
    public CheckType CheckTemplatePaymentType => CheckTemplateDocument?.PaymentType ?? default;

    /// <summary>
    /// Система налогообложения шаблона.
    /// </summary>
    public TaxSystem CheckTemplateTaxVariant => CheckTemplateDocument?.TaxVariant ?? default;

    /// <summary>
    /// Часовая зона шаблона.
    /// </summary>
    public CheckTimeZone CheckTemplateTimeZone => CheckTemplateDocument?.TimeZone ?? default;

    /// <summary>
    /// Признак расчёта в интернете (шаблон чека).
    /// </summary>
    public bool CheckTemplateOperationOnline => CheckTemplateDocument?.OperationOnline ?? false;

    /// <summary>
    /// Признак замены ставки налога (шаблон чека).
    /// </summary>
    public bool CheckTemplateIsReplaceTax => CheckTemplateDocument?.IsReplaceTax ?? false;

    /// <summary>
    /// Чек только в электронном виде (шаблон чека).
    /// </summary>
    public bool CheckTemplateElectronically => CheckTemplateDocument?.Electronically ?? false;

    /// <summary>
    /// Фискальный режим (шаблон чека).
    /// </summary>
    public bool CheckTemplateIsFiscal => CheckTemplateDocument?.IsFiscal ?? false;

    /// <summary>
    /// Признак доверенности в ФН (шаблон чека).
    /// </summary>
    public bool CheckTemplateTrustedInFn => CheckTemplateDocument?.TrustedInFn ?? false;

    /// <summary>
    /// Сумма чека шаблона.
    /// </summary>
    public decimal CheckTemplateSum => CheckTemplateDocument?.Sum ?? 0;

    /// <summary>
    /// Сдача (шаблон чека).
    /// </summary>
    public decimal CheckTemplateChange => CheckTemplateDocument?.Change ?? 0;

    /// <summary>
    /// Номер автомата (шаблон чека).
    /// </summary>
    public int CheckTemplateMtNumber => CheckTemplateDocument?.MtNumber ?? 0;

    /// <summary>
    /// Признак ошибки печати (шаблон чека).
    /// </summary>
    public bool CheckTemplatePrintError => CheckTemplateDocument?.PrintError ?? false;

    /// <summary>
    /// Дополнительный реквизит чека (шаблон чека).
    /// </summary>
    public string? CheckTemplateAdditionalAttribute => CheckTemplateDocument?.AdditionalAttribute;

    /// <summary> Оплата наличными (шаблон чека). </summary>
    public decimal CheckTemplatePaymentsCash => CheckTemplateDocument?.Payments?.Cash ?? 0;

    /// <summary> Оплата безналичными (шаблон чека). </summary>
    public decimal CheckTemplatePaymentsElectronic => CheckTemplateDocument?.Payments?.Electronic ?? 0;

    /// <summary> Оплата авансом/предоплатой (шаблон чека). </summary>
    public decimal CheckTemplatePaymentsPrePaid => CheckTemplateDocument?.Payments?.PrePaid ?? 0;

    /// <summary> Оплата в кредит/постоплатой (шаблон чека). </summary>
    public decimal CheckTemplatePaymentsCredit => CheckTemplateDocument?.Payments?.Credit ?? 0;

    /// <summary> Встречное предоставление (шаблон чека). </summary>
    public decimal CheckTemplatePaymentsBarter => CheckTemplateDocument?.Payments?.Barter ?? 0;

    /// <summary> Покупатель: наименование/ФИО (шаблон чека). </summary>
    public string? CheckTemplateCustomerInfo => CheckTemplateDocument?.CustomerDetail?.Info;

    /// <summary> Покупатель: ИНН (шаблон чека). </summary>
    public string? CheckTemplateCustomerInn => CheckTemplateDocument?.CustomerDetail?.Inn;

    /// <summary> Покупатель: почта (шаблон чека). </summary>
    public string? CheckTemplateCustomerEmail => CheckTemplateDocument?.CustomerDetail?.Email;

    /// <summary> Покупатель: телефон (шаблон чека). </summary>
    public string? CheckTemplateCustomerPhone => CheckTemplateDocument?.CustomerDetail?.Phone;

    /// <summary> Покупатель: дата рождения (шаблон чека). </summary>
    public string? CheckTemplateCustomerDateOfBirth => CheckTemplateDocument?.CustomerDetail?.DateOfBirth;

    /// <summary> Покупатель: гражданство (код страны, шаблон чека). </summary>
    public string? CheckTemplateCustomerCitizenship => CheckTemplateDocument?.CustomerDetail?.Citizenship;

    /// <summary> Покупатель: код вида документа (шаблон чека). </summary>
    public int? CheckTemplateCustomerDocumentTypeCode => CheckTemplateDocument?.CustomerDetail?.DocumentTypeCode;

    /// <summary> Покупатель: данные документа (шаблон чека). </summary>
    public string? CheckTemplateCustomerDocumentData => CheckTemplateDocument?.CustomerDetail?.DocumentData;

    /// <summary> Покупатель: адрес (шаблон чека). </summary>
    public string? CheckTemplateCustomerAddress => CheckTemplateDocument?.CustomerDetail?.Address;

    /// <summary> Коррекция: тип (шаблон чека). </summary>
    public CorrectionTypes CheckTemplateCorrectionType => CheckTemplateDocument?.CorrectionData?.Type ?? default;

    /// <summary> Коррекция: описание (шаблон чека). </summary>
    public string? CheckTemplateCorrectionDescription => CheckTemplateDocument?.CorrectionData?.Description;

    /// <summary> Коррекция: дата (шаблон чека). </summary>
    public DateTime? CheckTemplateCorrectionDate => CheckTemplateDocument?.CorrectionData?.Date;

    /// <summary> Коррекция: номер документа-основания (шаблон чека). </summary>
    public string? CheckTemplateCorrectionNumber => CheckTemplateDocument?.CorrectionData?.Number;

    /// <summary> Позиции шаблона чека (перебираются по индексу). </summary>
    public IReadOnlyList<CheckItem> CheckTemplateItems => CheckTemplateDocument?.CheckItems ?? [];

}
