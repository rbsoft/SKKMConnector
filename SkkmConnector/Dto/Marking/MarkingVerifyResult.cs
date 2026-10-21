using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Результат проверки кода маркировки.
/// </summary>
public sealed class MarkingVerifyResult
{
    /// <summary>
    /// Код результата проверки.
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }

    /// <summary>
    /// Описание результата проверки.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    /// <summary>
    /// Данные проверки кодов маркировки.
    /// </summary>
    [JsonPropertyName("codes")]
    public List<CodeMarkInfo> Codes { get; set; } = [];

    /// <summary>
    /// Идентификатор операции проверки.
    /// </summary>
    [JsonPropertyName("reqId")]
    public string ReqId { get; set; } = "";

    /// <summary>
    /// Временная метка операции проверки.
    /// </summary>
    [JsonPropertyName("reqTimestamp")]
    public long ReqTimestamp { get; set; }

    /// <summary>
    /// Признак офлайн-проверки.
    /// </summary>
    [JsonPropertyName("isCheckedOffline")]
    public bool IsCheckedOffline { get; set; }

    /// <summary>
    /// Версия локального модуля проверки.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Статус локального модуля проверки.
    /// </summary>
    [JsonPropertyName("status")]
    public LocalModuleStatus Status { get; set; }

    /// <summary>
    /// Адрес сервиса проверки.
    /// </summary>
    [JsonPropertyName("serviceUrl")]
    public string? ServiceUrl { get; set; }

    /// <summary>
    /// Требуется загрузка данных из ГИС МТ.
    /// </summary>
    [JsonPropertyName("requiresDownload")]
    public bool RequiresDownload { get; set; }

    /// <summary>
    /// Статус репликации по товарным группам (ключ — товарная группа).
    /// </summary>
    [JsonPropertyName("replicationStatus")]
    public Dictionary<string, ReplicationItem>? ReplicationStatus { get; set; }

    /// <summary>
    /// Режим работы модуля.
    /// </summary>
    [JsonPropertyName("operationMode")]
    public string? OperationMode { get; set; }

    /// <summary>
    /// Имя модуля.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Метка времени последнего обновления данных.
    /// </summary>
    [JsonPropertyName("lastUpdate")]
    public long LastUpdate { get; set; }

    /// <summary>
    /// Метка времени последней синхронизации с ГИС МТ.
    /// </summary>
    [JsonPropertyName("lastSync")]
    public long LastSync { get; set; }

    /// <summary>
    /// Экземпляр модуля.
    /// </summary>
    [JsonPropertyName("inst")]
    public string? Inst { get; set; }

    /// <summary>
    /// ИНН.
    /// </summary>
    [JsonPropertyName("inn")]
    public string? Inn { get; set; }

    /// <summary>
    /// Версия базы данных модуля.
    /// </summary>
    [JsonPropertyName("dbVersion")]
    public string? DbVersion { get; set; }

    /// <summary>
    /// Дата последнего обновления (строкой).
    /// </summary>
    [JsonPropertyName("lastUpdateDate")]
    public string? LastUpdateDate { get; set; }

    /// <summary>
    /// Дата последней синхронизации (строкой).
    /// </summary>
    [JsonPropertyName("lastSyncDate")]
    public string? LastSyncDate { get; set; }

    /// <summary>
    /// Результаты проверки локальным модулем «Честного знака».
    /// </summary>
    [JsonPropertyName("localCheckResults")]
    public LocalTrueApiCheckResult[] LocalCheckResults { get; set; } = [];
}
