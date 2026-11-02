namespace RBSoftSkkm;

/// <summary>
/// Статус локального модуля:
/// <para>Unknown — неизвестно</para>
/// <para>Ready — готов</para>
/// <para>Initialization — инициализация</para>
/// <para>Failure — ошибка</para>
/// <para>NotConfigured — не настроен</para>
/// </summary>
public enum LocalModuleStatus
{
    /// <summary>Неизвестно.</summary>
    Unknown = 0,

    /// <summary>Готов.</summary>
    Ready = 1,

    /// <summary>Инициализация.</summary>
    Initialization = 2,

    /// <summary>Ошибка.</summary>
    Failure = 3,

    /// <summary>Не настроен.</summary>
    NotConfigured = 4,
}
