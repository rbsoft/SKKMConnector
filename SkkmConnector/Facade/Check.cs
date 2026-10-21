namespace RBSoftSkkm;

/// <summary>
/// Плоское представление одного чека из списка (<see cref="SkkmConnector.Checks"/>).
/// Обёртка только для чтения поверх <see cref="CheckDocument"/>: не копирует данные,
/// а разворачивает под-объекты (DocumentHeader/QrData/Payments/…) в плоские свойства.
/// Имена совпадают с одиночным фасадом коннектора (без префикса Check для прямых полей).
/// </summary>
public sealed class Check
{
    private readonly CheckDocument _d;

    public Check(CheckDocument source) => _d = source;

    // ─── Прямые поля документа ───
    /// <summary> Идентификатор документа (docId). </summary>
    public string? DocId => _d.DocId;
    /// <summary> Тип документа/задания. </summary>
    public CheckType TaskType => _d.TaskType;
    /// <summary> Номер фискального документа (ФД). </summary>
    public int DocNumber => _d.DocNumber;
    /// <summary> Номер документа за смену. </summary>
    public int DocNumberInShift => _d.DocNumberInShift;
    /// <summary> Номер смены. </summary>
    public int ShiftNumber => _d.ShiftNumber;
    /// <summary> Фискальный признак документа (ФПД). </summary>
    public string? FiscalSign => _d.FiscalSign;
    /// <summary> Номер фискального накопителя (ФН). </summary>
    public string? Fn => _d.Fn;
    /// <summary> Дата/время документа по часам ФН. </summary>
    public DateTime FiscalDate => _d.FiscalDate;
    /// <summary> Дата/время постановки документа. </summary>
    public DateTime Date => _d.Date;
    /// <summary> Сумма чека. </summary>
    public decimal Sum => _d.Sum;
    /// <summary> Сдача. </summary>
    public decimal Change => _d.Change;
    /// <summary> Остаток наличных в ящике (если есть в документе). </summary>
    public decimal? CashSum => _d.CashSum;
    /// <summary> Номер денежного ящика. </summary>
    public int DrawerNumber => _d.DrawerNumber;
    /// <summary> Система налогообложения. </summary>
    public TaxSystem TaxType => (TaxSystem)_d.TaxType;
    /// <summary> Фискальный режим. </summary>
    public bool IsFiscal => _d.IsFiscal;
    /// <summary> Чек только в электронном виде. </summary>
    public bool IsElectronically => _d.Electronically;
    /// <summary> Расчёт в интернете. </summary>
    public bool IsOperationOnline => _d.OperationOnline;
    /// <summary> Документ обработан. </summary>
    public bool IsProcessed => _d.Processed;
    /// <summary> Замена ставки НДС. </summary>
    public bool IsReplaceTax => _d.IsReplaceTax;
    /// <summary> Подтверждён в ФН. </summary>
    public bool IsTrustedInFn => _d.TrustedInFn;
    /// <summary> Контакт клиента. </summary>
    public string? ClientContact => _d.ClientContact;
    /// <summary> Часовая зона. </summary>
    public int TimeZone => _d.TimeZone;
    /// <summary> Дополнительный реквизит чека (тег 1192). </summary>
    public string? AdditionalAttribute => _d.AdditionalAttribute;
    /// <summary> Имя (ФИО) кассира. </summary>
    public string? CashierName => _d.CashierName;
    /// <summary> ИНН кассира. </summary>
    public string? CashierVatin => _d.CashierVatin;
    /// <summary> Адрес расчётов. </summary>
    public string? SaleAddress => _d.SaleAddress;
    /// <summary> Место расчётов. </summary>
    public string? SaleLocation => _d.SaleLocation;
    /// <summary> Версия ФФД. </summary>
    public string? FfdVersion => _d.FfdVersion;
    /// <summary> TLV-данные документа. </summary>
    public string? Tlv => _d.Tlv;
    /// <summary> Имя кассы. </summary>
    public string? DeviceName => _d.DeviceName;
    /// <summary> Идентификатор терминала. </summary>
    public string? TerminalId => _d.TerminalId;
    /// <summary> Идентификатор пула. </summary>
    public string? PoolId => _d.PoolId;
    /// <summary> Код результата. </summary>
    public int ResultCode => _d.ResultCode;
    /// <summary> Описание результата. </summary>
    public string? ResultDescription => _d.ResultDescription;
    /// <summary> Версия сервера. </summary>
    public string? ServerVersion => _d.ServerVersion;
    /// <summary> Количество аннулирований. </summary>
    public int AnullatesCount => _d.AnullatesCount;
    /// <summary> E-mail отправителя чека. </summary>
    public string? SenderEmail => _d.SenderEmail;

    // ─── Суммы НДС по ставкам ───
    /// <summary> Сумма НДС по ставке 0%. </summary>
    public decimal TaxSum0 => _d.TaxSum0;
    /// <summary> Сумма НДС по ставке 5%. </summary>
    public decimal TaxSum5 => _d.TaxSum5;
    /// <summary> Сумма НДС по ставке 7%. </summary>
    public decimal TaxSum7 => _d.TaxSum7;
    /// <summary> Сумма НДС по ставке 10%. </summary>
    public decimal TaxSum10 => _d.TaxSum10;
    /// <summary> Сумма НДС по ставке 18%. </summary>
    public decimal TaxSum18 => _d.TaxSum18;
    /// <summary> Сумма НДС по ставке 20%. </summary>
    public decimal TaxSum20 => _d.TaxSum20;
    /// <summary> Сумма НДС по ставке 22%. </summary>
    public decimal TaxSum22 => _d.TaxSum22;
    /// <summary> Сумма без НДС. </summary>
    public decimal TaxSumNone => _d.TaxSumNone;
    /// <summary> Сумма НДС по расчётной ставке 10/110. </summary>
    public decimal TaxSum110 => _d.TaxSum110;
    /// <summary> Сумма НДС по расчётной ставке 20/120. </summary>
    public decimal TaxSum120 => _d.TaxSum120;
    /// <summary> Сумма НДС по расчётной ставке 5/105. </summary>
    public decimal TaxSum105 => _d.TaxSum105;
    /// <summary> Сумма НДС по расчётной ставке 7/107. </summary>
    public decimal TaxSum107 => _d.TaxSum107;
    /// <summary> Сумма НДС по расчётной ставке 18/118. </summary>
    public decimal TaxSum118 => _d.TaxSum118;
    /// <summary> Сумма НДС по расчётной ставке 22/122. </summary>
    public decimal TaxSum122 => _d.TaxSum122;

    // ─── DocumentHeader ───
    /// <summary> Наименование организации. </summary>
    public string? OrganizationInfo => _d.DocumentHeader?.OrganizationInfo;
    /// <summary> ИНН организации. </summary>
    public string? OrganizationVatin => _d.DocumentHeader?.Vatin;
    /// <summary> Наименование ОФД. </summary>
    public string? OfdName => _d.DocumentHeader?.OfdOrganizationName;
    /// <summary> ИНН ОФД. </summary>
    public string? OfdVatin => _d.DocumentHeader?.OfdVatin;
    /// <summary> Регистрационный номер ККТ (РНМ). </summary>
    public string? RnNumber => _d.DocumentHeader?.RnNumber;
    /// <summary> Адрес сайта ФНС. </summary>
    public string? FnsUrl => _d.DocumentHeader?.FnsUrl;

    // ─── QrData ───
    /// <summary> QR: дата/время. </summary>
    public DateTime? QrDataDate => _d.QrData?.Date;
    /// <summary> QR: сумма. </summary>
    public decimal QrDataAmount => _d.QrData?.Amount ?? 0;
    /// <summary> QR: номер ФН. </summary>
    public string? QrDataFn => _d.QrData?.Fn;
    /// <summary> QR: номер ФД. </summary>
    public int QrDataFd => _d.QrData?.Fd ?? 0;
    /// <summary> QR: фискальный признак. </summary>
    public string? QrDataFp => _d.QrData?.Fp;
    /// <summary> QR: тип операции (N). </summary>
    public int QrDataN => _d.QrData?.N ?? 0;

    // ─── Payments ───
    /// <summary> Оплата наличными. </summary>
    public decimal PaymentsCash => _d.Payments?.Cash ?? 0;
    /// <summary> Оплата безналичными. </summary>
    public decimal PaymentsElectronic => _d.Payments?.Electronic ?? 0;
    /// <summary> Оплата авансом (предоплата). </summary>
    public decimal PaymentsPrePaid => _d.Payments?.PrePaid ?? 0;
    /// <summary> Оплата в кредит (постоплата). </summary>
    public decimal PaymentsCredit => _d.Payments?.Credit ?? 0;
    /// <summary> Встречное предоставление. </summary>
    public decimal PaymentsBarter => _d.Payments?.Barter ?? 0;

    // ─── CustomerDetail ───
    /// <summary> Покупатель: наименование/ФИО. </summary>
    public string? CustomerDetailInfo => _d.CustomerDetail?.Info;
    /// <summary> Покупатель: ИНН. </summary>
    public string? CustomerDetailInn => _d.CustomerDetail?.Inn;
    /// <summary> Покупатель: почта. </summary>
    public string? CustomerDetailEmail => _d.CustomerDetail?.Email;
    /// <summary> Покупатель: телефон. </summary>
    public string? CustomerDetailPhone => _d.CustomerDetail?.Phone;
    /// <summary> Покупатель: дата рождения. </summary>
    public string? CustomerDetailDateOfBirth => _d.CustomerDetail?.DateOfBirth;
    /// <summary> Покупатель: гражданство. </summary>
    public string? CustomerDetailCitizenship => _d.CustomerDetail?.Citizenship;
    /// <summary> Покупатель: код вида документа. </summary>
    public int? CustomerDetailDocumentTypeCode => _d.CustomerDetail?.DocumentTypeCode;
    /// <summary> Покупатель: данные документа. </summary>
    public string? CustomerDetailDocumentData => _d.CustomerDetail?.DocumentData;
    /// <summary> Покупатель: адрес. </summary>
    public string? CustomerDetailAddress => _d.CustomerDetail?.Address;

    // ─── OfdStatus ───
    /// <summary> Обмен с ОФД завершён. </summary>
    public bool OfdStatusIsCompleted => _d.OfdStatus?.IsCompleted ?? false;
    /// <summary> Соединение с ОФД установлено. </summary>
    public bool OfdStatusIsConnectedOFD => _d.OfdStatus?.IsConnectedOFD ?? false;
    /// <summary> Есть документы для отправки в ОФД. </summary>
    public bool OfdStatusIsExistDocsToSend => _d.OfdStatus?.IsExistDocsToSend ?? false;
    /// <summary> Количество документов обмена с ОФД. </summary>
    public int OfdStatusDocumentsCount => _d.OfdStatus?.DocumentsCount ?? 0;
    /// <summary> Номер первого документа обмена с ОФД. </summary>
    public long OfdStatusFirstDocumentNumber => _d.OfdStatus?.FirstDocumentNumber ?? 0;
    /// <summary> Дата первого документа обмена с ОФД. </summary>
    public DateTime? OfdStatusFirstDocumentDate => _d.OfdStatus?.FirstDocumentDate;
    /// <summary> Сообщение ОФД прочитано. </summary>
    public bool OfdStatusIsOfdMessageRead => _d.OfdStatus?.OfdMessageRead ?? false;
    /// <summary> Ожидается запрос от ОФД. </summary>
    public bool OfdStatusIsWaitRequestFromOFD => _d.OfdStatus?.IsWaitRequestFromOFD ?? false;
    /// <summary> Есть команда от ОФД. </summary>
    public bool OfdStatusIsExistCommandFromOFD => _d.OfdStatus?.IsExistCommandFromOFD ?? false;
    /// <summary> Изменены параметры соединения с ОФД. </summary>
    public bool OfdStatusIsConnectionParametersChanged => _d.OfdStatus?.IsConnectionParametersChanged ?? false;
    /// <summary> Ожидается ответ на команду от ОФД. </summary>
    public bool OfdStatusIsWaitingForResponseToCommandFromOFD => _d.OfdStatus?.WaitingForResponseToCommandFromOFD ?? false;

    // ─── DeviceInfo ───
    /// <summary> Модель ККТ. </summary>
    public string? DeviceModel => _d.DeviceInfo?.Model;
    /// <summary> Наименование модели ККТ. </summary>
    public string? DeviceModelName => _d.DeviceInfo?.ModelName;
    /// <summary> Заводской номер ККТ. </summary>
    public string? DeviceSerialNumber => _d.DeviceInfo?.SerialNumber;
    /// <summary> Версия прошивки ККТ. </summary>
    public string? DeviceFirmwareVersion => _d.DeviceInfo?.FirmwareVersion;
    /// <summary> Версия конфигурации прошивки ККТ. </summary>
    public string? DeviceConfigurationVersion => _d.DeviceInfo?.ConfigurationVersion;
    /// <summary> Версия ФФД ККТ. </summary>
    public string? DeviceFfdVersion => _d.DeviceInfo?.FfdVersion;
    /// <summary> Версия ФФД ФН. </summary>
    public string? DeviceFnFfdVersion => _d.DeviceInfo?.FnFfdVersion;
    /// <summary> Класс устройства. </summary>
    public DeviceClass? DeviceClass => _d.DeviceInfo?.DeviceClass;
    /// <summary> Часовая зона устройства. </summary>
    public int DeviceTimeZone => _d.DeviceInfo?.TimeZone ?? 0;
    /// <summary> Фискальный режим устройства. </summary>
    public bool DeviceIsFiscal => _d.DeviceInfo?.IsFiscal ?? false;

    // ─── CorrectionData ───
    /// <summary> Коррекция: тип. </summary>
    public CorrectionTypes CorrectionDataType => _d.CorrectionData?.Type ?? CorrectionTypes.Самостоятельно;
    /// <summary> Коррекция: основание/описание. </summary>
    public string? CorrectionDataDescription => _d.CorrectionData?.Description;
    /// <summary> Коррекция: дата корректируемого расчёта. </summary>
    public DateTime? CorrectionDataDate => _d.CorrectionData?.Date;
    /// <summary> Коррекция: номер документа-основания. </summary>
    public string? CorrectionDataNumber => _d.CorrectionData?.Number;

    // ─── Позиции ───
    /// <summary> Количество позиций в чеке. </summary>
    public int ItemsCount => _d.CheckItems?.Length ?? 0;
    /// <summary> Позиции чека (перебираются по индексу). </summary>
    public IReadOnlyList<CheckItem> Items => _d.CheckItems ?? [];

    // ─── Печатная форма документа (для слипов/нефискальных) ───
    /// <summary> Количество строк печатной формы документа. </summary>
    public int LinesCount => _d.Lines?.Length ?? 0;
    /// <summary> Строки печатной формы документа (плоские, перебираются по индексу). </summary>
    public IReadOnlyList<PrintLine> Lines => (_d.Lines ?? []).Select(x => new PrintLine(x)).ToList();
}
