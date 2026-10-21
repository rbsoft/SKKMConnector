namespace RBSoftSkkm;

// Вход: подключение к серверу ККМ и общие параметры запроса (касса, кассир, пул).
public sealed partial class SkkmConnector
{
    // Подключение

    /// <summary>
    /// Адрес сервера ККМ в виде хост:порт (например, "localhost:4398").
    /// Порт можно опустить — тогда используется 4398. Можно менять между запросами.
    /// </summary>
    public string Address { get; set; } = "localhost:4398";

    /// <summary>
    /// Таймаут запроса к серверу ККМ. По умолчанию 60 секунд.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

    /// <summary>
    /// Токен авторизации (заголовок api_key). Можно менять между запросами, пока программа запущена.
    /// </summary>
    public string Token { get; set; } = "";

    /// <summary>
    /// Идентификатор терминала
    /// </summary>
    public string TerminalId { get; set; } = "";

    /// <summary>
    /// Имя устройства.
    /// </summary>
    public string DeviceName { get; set; } = "";

    /// <summary>
    /// Имя (ФИО) кассира.
    /// </summary>
    public string CashierName { get; private set; } = "";

    /// <summary>
    /// ИНН кассира (при наличии).
    /// </summary>
    public string CashierVatin { get; private set; } = "";

    /// <summary>
    /// Задать кассира на смену
    /// </summary>
    public void SetCashier(string cashierName)
    {
        CashierName = cashierName;
        CashierVatin = "";
    }

    /// <summary>
    /// Задать кассира на смену
    /// </summary>
    public void SetCashier(string cashierName, string cashierVatin)
    {
        CashierName = cashierName;
        CashierVatin = cashierVatin;
    }

    /// <summary>
    /// Имя пула устройств.
    /// </summary>
    public string PoolName { get; set; } = "";
}
