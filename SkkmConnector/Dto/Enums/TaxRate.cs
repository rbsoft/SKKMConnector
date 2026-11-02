using System.Text.Json.Serialization;

namespace RBSoftSkkm;

/// <summary>
/// Ставка НДС:
/// none — Без НДС;
/// Vat0 — НДС 0%;
/// Vat5 — НДС 5%;
    /// Vat7 — НДС 7%;
/// Vat10 — НДС 10%;
/// Vat20 — НДС 20%;
/// Vat22 — НДС 22%
/// Расчётные ставки: 
/// Vat5_105 — 5/105%;
/// Vat7_107 — 7/107%;
/// Vat10_110 — 10/110%; 
/// Vat20_120 — 20/120%;
/// Vat22_122 — 22/122%
/// </summary>
[JsonConverter(typeof(TaxRateJsonConverter))]
public enum TaxRate
{
    /// <summary>
    /// Без НДС.
    /// </summary>
    None,

    /// <summary>
    /// НДС 0%.
    /// </summary>
    Vat0,

    /// <summary>
    /// НДС 5%.
    /// </summary>
    Vat5,

    /// <summary>
    /// НДС 7%.
    /// </summary>
    Vat7,

    /// <summary>
    /// НДС 10%.
    /// </summary>
    Vat10,

    /// <summary>
    /// НДС 20%.
    /// </summary>
    Vat20,

    /// <summary>
    /// НДС 22%.
    /// </summary>
    Vat22,

    /// <summary>
    /// Расчётная ставка НДС 5/105.
    /// </summary>
    Vat5_105,

    /// <summary>
    /// Расчётная ставка НДС 7/107.
    /// </summary>
    Vat7_107,

    /// <summary>
    /// Расчётная ставка НДС 10/110.
    /// </summary>
    Vat10_110,

    /// <summary>
    /// Расчётная ставка НДС 20/120.
    /// </summary>
    Vat20_120,

    /// <summary>
    /// Расчётная ставка НДС 22/122.
    /// </summary>
    Vat22_122
}
