namespace RBSoftSkkm;

/// <summary>
/// Шаблон чека, сохранённый на сервере.
/// </summary>
public sealed class CheckTemplate
{
    /// <summary>
    /// Имя шаблона чека. Уникальный идентификатор на сервере.
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Идентификатор шаблона на сервере.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Сохранённый документ шаблона.
    /// </summary>
    public CheckTemplateDocument? Document { get; set; }
}
