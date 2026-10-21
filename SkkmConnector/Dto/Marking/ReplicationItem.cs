using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Статус репликации данных товарной группы из ГИС МТ.
/// </summary>
public sealed class ReplicationItem
{
    /// <summary>
    /// Количество документов.
    /// </summary>
    [JsonPropertyName("docCount")]
    public int DocCount { get; set; }
}
