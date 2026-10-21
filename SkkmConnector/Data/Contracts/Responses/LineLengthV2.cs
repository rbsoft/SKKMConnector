using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal
{
    /// <summary>
    /// Ширина строки чека.
    /// </summary>
    internal class LineLengthV2
    {
        /// <summary>
        /// Ширина строки чека в символах.
        /// </summary>
        [JsonPropertyName("LineLength")]
        public int LineLength { get; set; }

        /// <summary>
        /// Ширина печатной области в пикселях.
        /// </summary>
        [JsonPropertyName("LineLengthPixels")]
        public int LineLengthPixels { get; set; }
    }
}
