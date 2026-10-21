namespace RBSoftSkkm;

/// <summary>
/// Элемент очереди печати.
/// </summary>
public sealed class QueueItem
{
    /// <summary>
    /// Идентификатор документа задания.
    /// </summary>
    public string DocId { get; set; } = "";

    /// <summary>
    /// Название устройства, которому адресовано задание.
    /// </summary>
    public string DeviceName { get; set; } = "";

    /// <summary>
    /// Идентификатор пула (если задание адресовано пулу, а не конкретному устройству).
    /// </summary>
    public string PoolId { get; set; } = "";

    /// <summary>
    /// Состояние отправки задания на устройство (сервер: int).
    /// </summary>
    public int SentToPrint { get; set; }

    /// <summary>
    /// Время постановки задания в очередь.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Время успешной печати.
    /// </summary>
    public DateTime PrintedTime { get; set; }

    /// <summary>
    /// Признак успешной печати задания.
    /// </summary>
    public bool Printed { get; set; }

    /// <summary>
    /// Сумма документа.
    /// </summary>
    public decimal Sum { get; set; }

    /// <summary>
    /// Описание текущего состояния или ошибки задания.
    /// </summary>
    public string ErrorDescription { get; set; } = "";

    /// <summary>
    /// Номер кассовой смены.
    /// </summary>
    public int Session { get; set; }

    /// <summary>
    /// Номер документа (заполняется после успешной обработки).
    /// </summary>
    public int DocNumber { get; set; }
}
