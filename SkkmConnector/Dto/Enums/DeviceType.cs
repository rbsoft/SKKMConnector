namespace RBSoftSkkm
{
    /// <summary>
    /// Тип драйвера устройства:
    /// <para>
    /// Shtrih - Shtrih
    /// </para>
    /// <para>
    /// Native1C - 1С(4.7)
    /// </para>
    /// <para>
    /// AtolFRv10 - Atol
    /// </para>
    /// <para>
    /// RrElectro - RrElectro
    /// </para>
    /// <para>
    /// Native1C5000 - 1С(5.0)
    /// </para>
    /// <para>
    /// KktLab - ККТ-Лаб
    /// </para>
    /// <para>
    /// TorgBalance - Торговый баланс
    /// </para>
    /// <para>
    /// EmulatorFR - Эмулятор
    /// </para>
    /// </summary>
    public enum DeviceType
    {
        /// <summary>
        /// Shtrih.
        /// </summary>
        Shtrih = 1,

        /// <summary>
        /// 1С(4.7).
        /// </summary>
        Native1C = 2,

        /// <summary>
        /// Atol.
        /// </summary>
        AtolFRv10 = 3,

        /// <summary>
        /// RrElectro.
        /// </summary>
        RrElectro = 4,

        /// <summary>
        /// 1С(5.0).
        /// </summary>
        Native1C5000 = 5,

        /// <summary>
        /// ККТ-Лаб.
        /// </summary>
        KktLab = 6,

        /// <summary>
        /// Торговый баланс.
        /// </summary>
        TorgBalance = 7,

        /// <summary>
        /// Эмулятор.
        /// </summary>
        EmulatorFR = 100,
    }
}
