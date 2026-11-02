namespace RBSoftSkkm;

/// <summary>
/// Статус лицензии Сервера ККМ:
/// <para>
/// None — проверка ещё не проводилась
/// </para>
/// <para>
/// Ok — лицензия присутствует
/// </para>
/// <para>
/// Fail — лицензия отсутствует
/// </para>
/// <para>
/// Blocked — лицензия заблокирована
/// </para>
/// </summary>
public enum LicenseStatus
{
    /// <summary>
    /// Проверка ещё не проводилась.
    /// </summary>
    None = 0,

    /// <summary>
    /// Лицензия присутствует.
    /// </summary>
    Ok = 1,

    /// <summary>
    /// Лицензия отсутствует.
    /// </summary>
    Fail = 2,

    /// <summary>
    /// Лицензия заблокирована.
    /// </summary>
    Blocked = 3,
}
