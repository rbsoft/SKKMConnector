namespace RBSoftSkkm;

/// <summary>
/// Коннектор Сервера ККМ. Один экземпляр — одна сессия.
/// </summary>
public sealed partial class SkkmConnector : IDisposable
{
    private readonly object _callLock = new();
    private CancellationTokenSource? _callCts;
    private bool _disposed;

    /// <summary>
    /// Адрес сервера ККМ в виде хост:порт (например, "localhost:4398").
    /// Порт можно опустить — тогда используется 4398. Можно менять между запросами.
    /// </summary>
    public string Address { get; set; } = "localhost:4398";

    /// <summary>
    /// Токен авторизации (заголовок api_key). Можно менять между запросами.
    /// </summary>
    public string Token { get; set; } = "";

    /// <summary>
    /// Идентификатор терминала.
    /// </summary>
    public string TerminalId { get; set; } = "";

    /// <summary>
    /// Имя устройства.
    /// </summary>
    public string DeviceName { get; set; } = "";

    /// <summary>
    /// Имя пула устройств.
    /// </summary>
    public string Pool { get; set; } = "";

    /// <summary>
    /// Имя (ФИО) кассира. Сохраняется между запросами: Clear и NewCheck его не сбрасывают.
    /// </summary>
    public string CashierName { get; set; } = "";

    /// <summary>
    /// ИНН кассира (при наличии). Сохраняется между запросами: Clear и NewCheck его не сбрасывают.
    /// </summary>
    public string CashierVatin { get; set; } = "";

    /// <summary>
    /// Отменяет текущий HTTP-запрос к серверу ККМ.
    /// </summary>
    public void Cancel()
    {
        lock (_callLock)
            _callCts?.Cancel();
    }

    /// <summary>
    /// Освобождает HTTP-соединение с сервером ККМ. После этого экземпляр использовать нельзя.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        Cancel();
        _http.Dispose();
    }
}
