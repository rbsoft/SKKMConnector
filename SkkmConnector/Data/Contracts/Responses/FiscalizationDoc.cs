using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal
{
    /// <summary>
    /// Документ фискализации (сервер: Fiscalization : CheckBase): общие поля документа плюс параметры регистрации.
    /// Эндпоинты: POST fiscalization, GET fiscalization?id=.
    /// </summary>
    internal sealed class FiscalizationDoc : CheckDocument
    {
        [JsonPropertyName("OperationType")]
        public FiscalizationOperationType OperationType { get; set; }

        [JsonPropertyName("RnNumber")]
        public string? RnNumber { get; set; }

        [JsonPropertyName("TaxationSystems")]
        public string? TaxationSystems { get; set; }

        [JsonPropertyName("Vatin")]
        public string? Vatin { get; set; }

        [JsonPropertyName("CompanyName")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("FfdVersionKkt")]
        public string? FfdVersionKkt { get; set; }

        [JsonPropertyName("FfdVersionFn")]
        public string? FfdVersionFn { get; set; }
    }
}
