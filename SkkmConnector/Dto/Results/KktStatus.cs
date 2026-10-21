using System.Text.Json.Serialization;

namespace RBSoftSkkm
{
    /// <summary>
    /// Состояние ККТ 
    /// </summary>
    public class KktStatus
    {
        /// <summary>
        /// Присутствует ли фискальный накопитель.
        /// </summary>
        [JsonPropertyName("IsFnPresent")]
        public bool IsFnPresent { get; set; }

        /// <summary>
        /// Находится ли фискальный накопитель в состоянии ошибки.
        /// </summary>
        [JsonPropertyName("IsFnError")]
        public bool IsFnError { get; set; }

        /// <summary>
        /// Доступна ли информационная система маркировки.
        /// </summary>
        [JsonPropertyName("IsIsmDisconnected")]
        public bool IsIsmDisconnected { get; set; }

        /// <summary>
        /// Доступен ли оператор фискальных данных.
        /// </summary>
        [JsonPropertyName("IsOfdDisconnected")]
        public bool IsOfdDisconnected { get; set; }

        /// <summary>
        /// Предупреждения ФН
        /// </summary>
        [JsonPropertyName("Warnings")]
        public Warnings? Warnings { get; set; }

        /// <summary>
        /// Номер смены.
        /// </summary>
        [JsonPropertyName("ShiftNumber")]
        public int ShiftNumber { get; set; }

        /// <summary>
        /// Номер фискального документа.
        /// </summary>
        [JsonPropertyName("DocNumber")]
        public int DocNumber { get; set; }

        /// <summary>
        /// Фискальный режим.
        /// </summary>
        [JsonPropertyName("IsFiscal")]
        public bool IsFiscal { get; set; }

        /// <summary>
        /// Смена открыта.
        /// </summary>
        [JsonPropertyName("IsShiftOpened")]
        public bool IsShiftOpened { get; set; }

        /// <summary>
        /// Смена истекла.
        /// </summary>
        [JsonPropertyName("IsShiftExpired")]
        public bool IsShiftExpired { get; set; }

        /// <summary>
        /// Время получения данных.
        /// </summary>
        [JsonPropertyName("ComputerTime")]
        public DateTime ComputerTime { get; set; }

        /// <summary>
        /// Время в часах устройства.
        /// </summary>
        [JsonPropertyName("DeviceTime")]
        public DateTime DeviceTime { get; set; }

        /// <summary>
        /// Открыт денежный ящик.
        /// </summary>
        [JsonPropertyName("IsDrawerOpened")]
        public bool IsDrawerOpened { get; set; }

        /// <summary>
        /// Наличие чековой ленты.
        /// </summary>
        [JsonPropertyName("IsCheckPaperPresent")]
        public bool IsCheckPaperPresent { get; set; }

        /// <summary>
        /// Открыта ли крышка.
        /// </summary>
        [JsonPropertyName("IsCoverOpened")]
        public bool IsCoverOpened { get; set; }

        /// <summary>
        /// Аккумулятор разряжен.
        /// </summary>
        [JsonPropertyName("IsBatteryLow")]
        public bool IsBatteryLow { get; set; }

        /// <summary>
        /// Открытый документ.
        /// </summary>
        [JsonPropertyName("IsOpenDocument")]
        public bool IsOpenDocument { get; set; }

        /// <summary>
        /// Предупреждения ФН загружены.
        /// </summary>
        [JsonPropertyName("WarningsLoaded")]
        public bool WarningsLoaded { get; set; }

        /// <summary>
        /// Ширина чековой ленты.
        /// </summary>
        [JsonPropertyName("LineLength")]
        public int LineLength { get; set; }

        /// <summary>
        /// Данные о непереданных в ОФД документах.
        /// </summary>
        [JsonPropertyName("Backlog")]
        public Backlog? Backlog { get; set; }

        /// <summary>
        /// Состояние обмена с информационной системой маркировки.
        /// </summary>
        [JsonPropertyName("Ism")]
        public ExchangeStatusIsm? Ism { get; set; }

        /// <summary>
        /// Номер фискального документа за смену.
        /// </summary>
        [JsonPropertyName("DocNumberInShift")]
        public int DocNumberInShift { get; set; }

        /// <summary>
        /// Сумма наличных в денежном ящике.
        /// </summary>
        [JsonPropertyName("CashSum")]
        public decimal CashSum { get; set; }

        /// <summary>
        /// Сумма выручки.
        /// </summary>
        [JsonPropertyName("TotalSum")]
        public decimal TotalSum { get; set; }

        /// <summary>
        /// Время открытия кассовой смены.
        /// </summary>
        [JsonPropertyName("OpenShiftTime")]
        public DateTime OpenShiftTime { get; set; }

        /// <summary>
        /// Наличие контрольной ленты.
        /// </summary>
        [JsonPropertyName("IsControlPaperPresent")]
        public bool IsControlPaperPresent { get; set; }

        /// <summary>
        /// Ожидание продолжения печати.
        /// </summary>
        [JsonPropertyName("IsWaitContinuePrint")]
        public bool IsWaitContinuePrint { get; set; }

        /// <summary>
        /// Ширина чековой ленты в пикселях.
        /// </summary>
        [JsonPropertyName("LineLengthPixels")]
        public int LineLengthPixels { get; set; }

        /// <summary>
        /// Идентификатор текущей задачи.
        /// </summary>
        [JsonPropertyName("TaskId")]
        public string? TaskId { get; set; }

        /// <summary>
        /// Код ошибки.
        /// </summary>
        [JsonPropertyName("Error")]
        public int Error { get; set; }

        /// <summary>
        /// Признак занятости устройства.
        /// </summary>
        [JsonPropertyName("IsBusy")]
        public bool IsBusy { get; set; }

        /// <summary>
        /// Код ошибки устройства по данным драйвера.
        /// </summary>
        [JsonPropertyName("ErrorCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Описание ошибки устройства по данным драйвера.
        /// </summary>
        [JsonPropertyName("ErrorCodeDescription")]
        public string? ErrorCodeDescription { get; set; }

        /// <summary>
        /// Режим по данным драйвера.
        /// </summary>
        [JsonPropertyName("DriverMode")]
        public int DriverMode { get; set; }

        /// <summary>
        /// Описание режима по данным драйвера.
        /// </summary>
        [JsonPropertyName("DriverModeDescription")]
        public string? DriverModeDescription { get; set; }

        /// <summary>
        /// Специальный режим по данным драйвера.
        /// </summary>
        [JsonPropertyName("DriverAdvancedMode")]
        public int DriverAdvancedMode { get; set; }

        /// <summary>
        /// Описание специального режима по данным драйвера.
        /// </summary>
        [JsonPropertyName("DriverAdvancedModeDescription")]
        public string? DriverAdvancedModeDescription { get; set; }

        /// <summary>
        /// Статус состояния лицензии.
        /// </summary>
        [JsonPropertyName("LicenseStatus")]
        public int LicenseStatus { get; set; }

        /// <summary>
        /// Описание лицензии.
        /// </summary>
        [JsonPropertyName("License")]
        public ServerLicense? License { get; set; }

        /// <summary>
        /// Время последней проверки лицензии.
        /// </summary>
        [JsonPropertyName("LicenseUpdated")]
        public DateTime LicenseUpdated { get; set; }

        /// <summary>
        /// Состояние смены по флагам: закрыта / открыта / истекла.
        /// </summary>
        [JsonIgnore]
        public ShiftState ShiftState
            => IsShiftExpired ? ShiftState.Expired : IsShiftOpened ? ShiftState.Opened : ShiftState.Closed;
    }
}
