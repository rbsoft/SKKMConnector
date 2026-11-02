namespace RBSoftSkkm
{
    /// <summary>
    /// Тип коррекции:
    /// <para>
    /// OnOwn - Самостоятельно
    /// </para>
    /// <para>
    /// AsPrescribed - По предписанию налогового органа
    /// </para>
    /// </summary>
    public enum CorrectionTypes
    {
        /// <summary>
        /// Самостоятельно
        /// </summary>
        OnOwn = 0,

        /// <summary>
        /// По предписанию налогового органа. Рекомендуется заполнять номер предписания.
        /// </summary>
        AsPrescribed = 1,
    }
}
