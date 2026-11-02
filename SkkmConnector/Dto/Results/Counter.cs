namespace RBSoftSkkm
{
    /// <summary>
    /// Строка таблицы счётчиков <see cref="SkkmConnector.Counter"/> — итоги по одному типу операции.
    /// </summary>
    public sealed class Counter
    {
        /// <summary>
        /// Тип операции: «Приход», «Возврат прихода», «Расход», «Возврат расхода».
        /// </summary>
        public string Operation { get; init; } = "";

        /// <summary>
        /// Количество операций.
        /// </summary>
        public int Count { get; init; }

        /// <summary>
        /// Сумма операций.
        /// </summary>
        public decimal Sum { get; init; }

        /// <summary>
        /// Количество коррекций этого типа операции.
        /// </summary>
        public int SalesCorrectionCount { get; init; }

        /// <summary>
        /// Сумма коррекций этого типа операции.
        /// </summary>
        public decimal SalesCorrectionSum { get; init; }

        /// <summary>
        /// Сумма оплат наличными.
        /// </summary>
        public decimal Cash { get; init; }

        /// <summary>
        /// Сумма оплат безналичными.
        /// </summary>
        public decimal Electronically { get; init; }

        /// <summary>
        /// Сумма оплат авансом.
        /// </summary>
        public decimal Prepaid { get; init; }

        /// <summary>
        /// Сумма оплат в кредит.
        /// </summary>
        public decimal Credit { get; init; }

        /// <summary>
        /// Сумма оплат встречным предоставлением.
        /// </summary>
        public decimal Barter { get; init; }

        /// <summary>
        /// Сумма предоставленных скидок.
        /// </summary>
        public decimal DiscountSum { get; init; }

        /// <summary>
        /// Количество предоставленных скидок.
        /// </summary>
        public int DiscountCount { get; init; }

        /// <summary>
        /// Сумма надбавок.
        /// </summary>
        public decimal AddingSum { get; init; }

        /// <summary>
        /// Количество надбавок.
        /// </summary>
        public int AddingCount { get; init; }
    }
}
