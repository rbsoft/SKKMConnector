using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal
{

    /// <summary>
    /// Общие параметры результата операции (сервер: CheckbaseParams).
    /// </summary>
    internal class CheckbaseResult
    {
        /// <summary>
        /// Время операции по часам компьютера. На сервере DateTime; здесь строка как есть,
        /// потому что пользователю она отдаётся строкой (ServerDateTime) без потери смещения.
        /// </summary>
        [JsonPropertyName("datetime")]
        public string? DateTime { get; set; }

        /// <summary>
        /// Имя устройства.
        /// </summary>
        [JsonPropertyName("deviceName")]
        public string? DeviceName { get; set; }

        /// <summary>
        /// Идентификатор задания (docId).
        /// </summary>
        [JsonPropertyName("docId")]
        public string? DocId { get; set; }

        /// <summary>
        /// Адрес сайта ФНС.
        /// </summary>
        [JsonPropertyName("fnsUrl")]
        public string? FnsUrl { get; set; }

        /// <summary>
        /// Номер фискального накопителя.
        /// </summary>
        [JsonPropertyName("fnNumber")]
        public string? FnNumber { get; set; }

        /// <summary>
        /// Регистрационный номер ККТ (РНМ).
        /// </summary>
        [JsonPropertyName("rnNumber")]
        public string? RnNumber { get; set; }

        /// <summary>
        /// Дата и время документа по часам ФН.
        /// </summary>
        [JsonPropertyName("fiscalDatetime")]
        public string? FiscalDateTime { get; set; }

        /// <summary>
        /// Номер смены.
        /// </summary>
        [JsonPropertyName("shiftNumber")]
        public int ShiftNumber { get; set; }

        /// <summary>
        /// Данные о ККТ.
        /// </summary>
        [JsonPropertyName("deviceInfo")]
        public Device? DeviceInfo { get; set; }
    }

    /// <summary>
    /// Результат фискального чека и чеков коррекции (сервер: FiscalParams).
    /// Эндпоинты: POST check, correction105, correction120.
    /// </summary>
    internal sealed class FiscalParamsResult : CheckbaseResult
    {
        /// <summary>
        /// Фискальный признак документа.
        /// </summary>
        [JsonPropertyName("fiscalSign")]
        public string? FiscalSign { get; set; }

        /// <summary>
        /// Номер фискального документа (ФД).
        /// </summary>
        [JsonPropertyName("fiscalNumber")]
        public int FiscalNumber { get; set; }
    }

    /// <summary>
    /// Результат открытия смены (сервер: OpenShiftResult).
    /// Эндпоинт: POST shift/open.
    /// </summary>
    internal sealed class OpenShiftResult : CheckbaseResult
    {
        /// <summary>
        /// Выходные параметры операции.
        /// </summary>
        [JsonPropertyName("outputParameters")]
        public FiscalOutputParameters? OutputParameters { get; set; }
    }

    /// <summary>
    /// Результат отчёта (сервер: ReportResult).
    /// Эндпоинты: POST shift/z, shift/x, report/settlement.
    /// </summary>
    internal sealed class ReportResult : CheckbaseResult
    {
        /// <summary>
        /// Выходные параметры операции.
        /// </summary>
        [JsonPropertyName("outputParameters")]
        public FiscalOutputParameters? OutputParameters { get; set; }

        /// <summary>
        /// Итоги смены.
        /// </summary>
        [JsonPropertyName("shiftTotal")]
        public ResShiftTotal? ShiftTotal { get; set; }

        /// <summary>
        /// Необнуляемые итоги.
        /// </summary>
        [JsonPropertyName("overallTotals")]
        public OverallTotals? OverallTotals { get; set; }
    }
}
