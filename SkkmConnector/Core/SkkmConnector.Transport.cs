using System.Globalization;
using System.Text.Json;
using RBSoftSkkm.Internal;

namespace RBSoftSkkm;

// Вызов сервера ККМ: адрес, строки запроса, Call/Execute/PostTask и общие запросы документов и списков.
public sealed partial class SkkmConnector
{
    private string DeviceQuery => $"device={Uri.EscapeDataString(DeviceName)}";

    private string IdQuery => $"id={Uri.EscapeDataString(DocumentId)}";

    private string DocIdQuery => $"docId={Uri.EscapeDataString(DocumentId)}";

    private KkmTransport Transport()
    {
        var (host, port) = ParseAddress(Address);
        _http.Host = host;
        _http.Port = port;
        _http.Token = Token;
        _http.TerminalId = TerminalId;
        _http.Timeout = Timeout;
        return _http;
    }

    /// <summary>
    /// Разбирает <see cref="Address"/> на хост и порт.
    /// </summary>
    private static (string Host, int Port) ParseAddress(string address)
    {
        const int defaultPort = 4398;
        if (string.IsNullOrWhiteSpace(address))
            return ("localhost", defaultPort);

        var value = address.Trim();
        var separator = value.LastIndexOf(':');
        if (separator < 0)
            return (value, defaultPort);

        var host = value[..separator];
        var portText = value[(separator + 1)..];
        if (int.TryParse(portText, out var port) && port is >= 1 and <= 65535)
            return (host.Length == 0 ? "localhost" : host, port);

        return (value, defaultPort);
    }

    private string DateQuery(DateTime from, DateTime to)
    {
        var fromText = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var toText = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return $"from={fromText}&to={toText}";
    }

    private CancellationTokenSource BeginCall()
    {
        var cts = new CancellationTokenSource();
        lock (_callLock)
            _callCts = cts;
        return cts;
    }

    private void EndCall(CancellationTokenSource cts)
    {
        lock (_callLock)
        {
            if (ReferenceEquals(_callCts, cts))
                _callCts = null;
        }
        cts.Dispose();
    }

    /// <summary>
    /// Запрос с типизированным ответом: <c>Result</c> разбирается в контракт <typeparamref name="T"/>
    /// (Data/Contracts/Responses), а <paramref name="apply"/> раскладывает его по свойствам.
    /// </summary>
    private async Task<bool> Call<T>(HttpMethod method, string path, object? body, Action<T> apply)
    {
        var cts = BeginCall();
        try
        {
            var response = await Transport().Send<T>(method, path, body, cts.Token);

            // Каждый ответ отражает только сам себя.
            ResetResponse();
            Ok = response.Success;
            ErrorCode = response.Code;
            ErrorDescription = response.Description ?? "";

            if (response.Result != null)
                apply(response.Result);
            return Ok;
        }
        finally
        {
            EndCall(cts);
        }
    }

    /// <summary>
    /// GET списка документов по кассе.
    /// </summary>
    /// <summary>
    /// Запрос, в ответе на который только статус (ResponseResultBase).
    /// </summary>
    private Task<bool> Execute(HttpMethod method, string path, object? body = null)
        => Call<JsonElement>(method, path, body, _ => { });

    /// <summary>
    /// Постановка задания в очередь (POST .../async): в ответе — docId.
    /// </summary>
    private Task<bool> PostTask(string path, object body)
        => Call<string>(HttpMethod.Post, path, body, docId =>
        {
            if (Ok && docId.Length > 0)
                DocumentId = docId;
        });

    /// <summary>
    /// GET документа по <see cref="DocumentId"/>.
    /// </summary>
    private Task<bool> GetDocumentById(string path)
        => Call<CheckDocument>(HttpMethod.Get, $"{path}?{IdQuery}", null, ApplyCheckBaseDoc);

    private Task<bool> GetCheckList(string path)
        => Call<CheckDocument[]>(HttpMethod.Get, $"{path}?{DeviceQuery}", null, ApplyCheckList);

    /// <summary>
    /// GET списка отчётов за период <see cref="ShiftsFrom"/>..<see cref="ShiftsTo"/>.
    /// </summary>
    private async Task<bool> GetReportList(string path, string? extraQuery = null)
    {
        var query = $"{DeviceQuery}&{DateQuery(ShiftsFrom, ShiftsTo)}";
        if (!string.IsNullOrWhiteSpace(extraQuery))
            query += $"&{extraQuery}";
        return await Call<ShiftListItem[]>(HttpMethod.Get, $"{path}?{query}", null, shifts => Shifts = shifts);
    }
}
