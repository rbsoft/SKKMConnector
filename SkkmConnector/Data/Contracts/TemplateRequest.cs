using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal;

internal sealed class TemplateRequest
{
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Type")]
    public int Type { get; set; }

    [JsonPropertyName("TemplateItems")]
    public TemplateItem[] TemplateItems { get; set; } = [];
}

internal sealed class CheckTemplateRequest
{
    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Document")]
    public CheckTemplateDocumentRequest? Document { get; set; }
}

internal sealed class CheckTemplateDocumentRequest
{
    [JsonPropertyName("PaymentType")]
    public int PaymentType { get; set; }

    [JsonPropertyName("TaxVariant")]
    public int TaxVariant { get; set; }

    [JsonPropertyName("Customer")]
    public Customer? Customer { get; set; }

    [JsonPropertyName("SenderEmail")]
    public string? SenderEmail { get; set; }

    [JsonPropertyName("SaleAddress")]
    public string? SaleAddress { get; set; }

    [JsonPropertyName("SaleLocation")]
    public string? SaleLocation { get; set; }

    [JsonPropertyName("Positions")]
    public ApiPosition[]? Positions { get; set; }

    [JsonPropertyName("Payments")]
    public Payments? Payments { get; set; }

    [JsonPropertyName("ElectronicPaymentInfo")]
    public List<ElectronicPayment>? ElectronicPaymentInfo { get; set; }

    [JsonPropertyName("Electronically")]
    public bool Electronically { get; set; }

    [JsonPropertyName("OperationalAttribute")]
    public OperationalAttribute? OperationalAttribute { get; set; }

    [JsonPropertyName("IndustryAttribute")]
    public Industry? IndustryAttribute { get; set; }

    [JsonPropertyName("UserAttribute")]
    public UserAttribute? UserAttribute { get; set; }

    [JsonPropertyName("TimeZone")]
    public int? TimeZone { get; set; }

    [JsonPropertyName("OperationOnline")]
    public bool OperationOnline { get; set; }

    [JsonPropertyName("AdditionalAttribute")]
    public string? AdditionalAttribute { get; set; }

    [JsonPropertyName("CorrectionData")]
    public CorrectionData? CorrectionData { get; set; }
}

