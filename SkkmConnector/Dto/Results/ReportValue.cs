namespace RBSoftSkkm
{
    /// <summary>
    /// Строка таблицы 
    /// </summary>
    public sealed class ReportValue
    {
        /// <summary>
        /// Ключ поля в JSON ответа
        /// </summary>
        public string Key { get; init; } = "";

        /// <summary>
        /// Значение поля строкой: логическое — <c>True</c> или <c>False</c>, число — как в ответе,
        /// известное перечисление — именем элемента (<c>ShiftState</c> со значением 2 даёт <c>Opened</c>), пустое (null) — пустая строка.
        /// </summary>
        public string Value { get; init; } = "";

        /// <summary>
        /// Описание поля.
        /// </summary>
        public string Description { get; init; } = "";
    }
}
