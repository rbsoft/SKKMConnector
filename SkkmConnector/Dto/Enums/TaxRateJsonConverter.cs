using System.Text.Json;
using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Сериализует <see cref="TaxRate"/> в строку ставки НДС, которую ждёт сервер ККМ
/// (none, 0, 5, 7, 10, 18, 20, 22, 5/105, 7/107, 10/110, 18/118, 20/120, 22/122).
/// </summary>
public sealed class TaxRateJsonConverter : JsonConverter<TaxRate>
{
    private static readonly (TaxRate Rate, string Text)[] Rates =
    [
        (TaxRate.None, "none"),
        (TaxRate.Vat0, "0"),
        (TaxRate.Vat5, "5"),
        (TaxRate.Vat7, "7"),
        (TaxRate.Vat10, "10"),
        (TaxRate.Vat18, "18"),
        (TaxRate.Vat20, "20"),
        (TaxRate.Vat22, "22"),
        (TaxRate.Vat5_105, "5/105"),
        (TaxRate.Vat7_107, "7/107"),
        (TaxRate.Vat10_110, "10/110"),
        (TaxRate.Vat18_118, "18/118"),
        (TaxRate.Vat20_120, "20/120"),
        (TaxRate.Vat22_122, "22/122"),
    ];

    /// <summary>Неизвестная строка — без НДС.</summary>
    public override TaxRate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var text = reader.GetString();
        foreach (var (rate, rateText) in Rates)
            if (rateText == text)
                return rate;
        return TaxRate.None;
    }

    /// <summary>Неизвестная ставка — "none".</summary>
    public override void Write(Utf8JsonWriter writer, TaxRate value, JsonSerializerOptions options)
    {
        foreach (var (rate, rateText) in Rates)
            if (rate == value)
            {
                writer.WriteStringValue(rateText);
                return;
            }
        writer.WriteStringValue("none");
    }
}
