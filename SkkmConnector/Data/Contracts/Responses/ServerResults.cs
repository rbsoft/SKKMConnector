using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal
{
    /// <summary>
    /// Ответ на GET ping (сервер: PingResponce).
    /// </summary>
    internal sealed class PingResult
    {
        [JsonPropertyName("product")]
        public string? Product { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }
    }

    /// <summary>
    /// Ответ на GET version (сервер: ResServerVersion).
    /// </summary>
    internal sealed class ServerVersionResult
    {
        [JsonPropertyName("ServerVersion")]
        public string? ServerVersion { get; set; }
    }
}
