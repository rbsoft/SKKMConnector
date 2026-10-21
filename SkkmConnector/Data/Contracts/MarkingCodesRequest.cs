using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal;

internal sealed class MarkingCodesRequest
{
    [JsonPropertyName("DeviceName")]
    public string? DeviceName { get; set; }

    [JsonPropertyName("Codes")]
    public List<string> Codes { get; set; } = [];
}
