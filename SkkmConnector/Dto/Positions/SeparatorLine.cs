namespace RBSoftSkkm;

/// <summary>
/// Разделительная линия в чеке:
/// <para>
/// LineStyle - Стиль.
/// </para>
/// </summary>
public sealed class SeparatorLine : Position
{
    /// <summary>
    /// Стиль разделительной линии.
    /// </summary>
    public LineStyle LineStyle { get; set; } = LineStyle.Solid;
}
