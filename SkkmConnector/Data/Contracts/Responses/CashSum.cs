using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal
{
    /// <summary>
    /// Остаток наличных в денежном ящике.
    /// </summary>
    internal class CashSum
    {
        /// <summary>
        /// Сумма наличных в денежном ящике
        /// </summary>
        [JsonPropertyName("Sum")]
        public decimal Sum { get; set; }
    }
}
