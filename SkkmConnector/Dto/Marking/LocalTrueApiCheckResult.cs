using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Результат проверки кодов локальным модулем «Честного знака».
/// </summary>
public sealed class LocalTrueApiCheckResult
{
    /// <summary>
    /// Версия локального модуля.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Временная метка запроса.
    /// </summary>
    [JsonPropertyName("reqTimestamp")]
    public long ReqTimestamp { get; set; }

    /// <summary>
    /// Идентификатор запроса.
    /// </summary>
    [JsonPropertyName("reqId")]
    public string? ReqId { get; set; }

    /// <summary>
    /// Экземпляр модуля.
    /// </summary>
    [JsonPropertyName("inst")]
    public string? Inst { get; set; }

    /// <summary>
    /// Описание результата.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Сведения по проверенным кодам.
    /// </summary>
    [JsonPropertyName("codes")]
    public CodeMarkInfo[] Codes { get; set; } = [];

    /// <summary>
    /// Код результата.
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; set; }
}
