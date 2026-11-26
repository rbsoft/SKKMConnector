namespace RBSoftSkkm
{
    /// <summary>
    /// Тип чека:
    /// <para>
    /// Text - Текст
    /// </para>
    /// <para>
    /// Sale - Продажа (приход)
    /// </para>
    /// <para>
    /// SaleReturn - Возврат (возврат прихода)
    /// </para>
    /// <para>
    /// Purchase - Покупка (расход)
    /// </para>
    /// <para>
    /// PurchaseReturn - Возврат покупки (возврат расхода)
    /// </para>
    /// <para>
    /// CorrectionSale - Чек коррекции прихода
    /// </para>
    /// <para>
    /// CorrectionSaleReturn - Чек коррекции возврата прихода
    /// </para>
    /// <para>
    /// CorrectionPurchase - Чек коррекции расхода
    /// </para>
    /// <para>
    /// CorrectionPurchaseReturn - Чек коррекции возврата расхода
    /// </para>
    /// <para>
    /// Slip - Слип, нефискальный документ
    /// </para>
    /// <para>
    /// Fiscalization - Фискализация
    /// </para>
    /// <para>
    /// OpenShift - Открытие смены
    /// </para>
    /// <para>
    /// CloseShift - Z-отчёт
    /// </para>
    /// <para>
    /// ReportX - X-отчёт
    /// </para>
    /// <para>
    /// ReportSettlement - Отчёт о состоянии расчётов
    /// </para>
    /// <para>
    /// CashOut - Выемка
    /// </para>
    /// <para>
    /// CashIn - Внесение
    /// </para>
    /// <para>
    /// OpenCashDrawer - Открытие денежного ящика
    /// </para>
    /// <para>
    /// CopyFromFn - Копия из ФН
    /// </para>
    /// <para>
    /// DocumentCopy - Дубликат документа
    /// </para>
    /// </summary>
    public enum CheckType
    {
        /// <summary>
        /// Текст.
        /// </summary>
        Text = 0,

        /// <summary>
        /// Продажа (приход).
        /// </summary>
        Sale = 1,

        /// <summary>
        /// Возврат (возврат прихода).
        /// </summary>
        SaleReturn = 2,

        /// <summary>
        /// Покупка (расход).
        /// </summary>
        Purchase = 3,

        /// <summary>
        /// Возврат покупки (возврат расхода).
        /// </summary>
        PurchaseReturn = 4,

        /// <summary>
        /// Чек коррекции прихода.
        /// </summary>
        CorrectionSale = 5,

        /// <summary>
        /// Чек коррекции возврата прихода.
        /// </summary>
        CorrectionSaleReturn = 6,

        /// <summary>
        /// Чек коррекции расхода.
        /// </summary>
        CorrectionPurchase = 7,

        /// <summary>
        /// Чек коррекции возврата расхода.
        /// </summary>
        CorrectionPurchaseReturn = 8,

        /// <summary>
        /// Слип.
        /// </summary>
        Slip = 9,

        /// <summary>
        /// Фискализация.
        /// </summary>
        Fiscalization = 10,

        /// <summary>
        /// Открытие смены.
        /// </summary>
        OpenShift = 11,

        /// <summary>
        /// Z-отчёт.
        /// </summary>
        CloseShift = 12,

        /// <summary>
        /// X-отчёт.
        /// </summary>
        ReportX = 13,

        /// <summary>
        /// Отчёт о состоянии расчётов.
        /// </summary>
        ReportSettlement = 14,

        /// <summary>
        /// Выемка.
        /// </summary>
        CashOut = 20,

        /// <summary>
        /// Внесение.
        /// </summary>
        CashIn = 21,

        /// <summary>
        /// Открытие денежного ящика.
        /// </summary>
        OpenCashDrawer = 22,

        /// <summary>
        /// Копия из ФН.
        /// </summary>
        CopyFromFn = 23,

        /// <summary>
        /// Дубликат документа.
        /// </summary>
        DocumentCopy = 24
    }
}
