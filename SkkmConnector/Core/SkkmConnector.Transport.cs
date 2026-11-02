using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RBSoftSkkm;

// HTTP-вызов Сервера ККМ: JSON запроса → сервер → разбор ответа в свойства.
public sealed partial class SkkmConnector
{
    private const string ApiPath = "/PrintService/api/v4/";

    private static readonly JsonSerializerOptions BodyJson = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly HttpClient _http = new(new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1)
    })
    {
        Timeout = System.Threading.Timeout.InfiniteTimeSpan
    };

    private string DeviceQuery => $"device={Uri.EscapeDataString(DeviceName)}";

    private string IdQuery => $"id={Uri.EscapeDataString(DocId)}";

    private static string DateQuery(System.DateTime from, System.DateTime to)
        => $"from={from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}&to={to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";

    /// <summary>
    /// Запрос к серверу.
    /// </summary>
    private async Task<bool> Call(HttpMethod method, string path, JsonObject? body = null, Action<JsonElement>? read = null)
    {
        var cts = new CancellationTokenSource();
        lock (_callLock)
            _callCts = cts;
        try
        {
            var (ok, code, description, result) = await Send(method, path, body, cts.Token);

            // Каждый ответ отражает только сам себя.
            ResetResponse();
            _ok = ok;
            ErrorCode = code;
            ErrorDescription = description;
            ReportValues = ToReportValues(result);
            if (result is { } r)
                read?.Invoke(r);
            // DocId теперь из ответа: в следующий чек коррекции 1.05 он сам не уйдёт.
            _docIdFromResponse = true;
            return _ok;
        }
        finally
        {
            lock (_callLock)
                if (ReferenceEquals(_callCts, cts))
                    _callCts = null;
            cts.Dispose();
        }
    }

    private async Task<(bool Ok, int Code, string Description, JsonElement? Result)> Send(
        HttpMethod method, string path, JsonObject? body, CancellationToken cancellation)
    {
        if (_disposed)
            return (false, -1, "Коннектор закрыт. Создайте новый SkkmConnector.", null);

        var (host, port) = ParseAddress(Address);
        using var request = new HttpRequestMessage(method, new Uri($"http://{host}:{port}{ApiPath}{path}"));
        request.Headers.TryAddWithoutValidation("api_key", Token);
        request.Headers.TryAddWithoutValidation("terminalId", TerminalId);
        if (body != null)
            request.Content = new StringContent(body.ToJsonString(BodyJson), Encoding.UTF8, "application/json");

        try
        {
            using var response = await _http.SendAsync(request, cancellation);
            var text = await response.Content.ReadAsStringAsync(cancellation);
            var status = (int)response.StatusCode;
            if (status != 200)
                return (false, status, ErrorText(text) ?? HttpStatusError(host, port, status), null);
            if (string.IsNullOrWhiteSpace(text))
                return (false, 0, "", null);

            try
            {
                using var doc = JsonDocument.Parse(text);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    var result = root.Get("Result");
                    return (root.Bool("Success"), root.Int("Code"), root.Str("Description"),
                        result.Exists() ? result.Clone() : null);
                }
            }
            catch (JsonException)
            {
            }
            return (false, status, HttpStatusError(host, port, status) + " Некорректный ответ сервера.", null);
        }
        catch (HttpRequestException ex)
        {
            return (false, -1, $"Ошибка соединения: {ex.Message}", null);
        }
        catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
        {
            return (false, -3, "Запрос отменён", null);
        }
    }

    /// <summary>
    /// Текст ошибки из тела: JSON Description или фраза в кавычках из абзаца страницы ошибки.
    /// Абзац — тег p без класса внутри div#content. p.heading1 — заголовок, его пропускаем.
    /// </summary>
    private static string? ErrorText(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return null;

        var trimmed = body.TrimStart('\uFEFF', ' ', '\t', '\r', '\n');
        if (trimmed.StartsWith('{'))
        {
            try
            {
                using var doc = JsonDocument.Parse(trimmed);
                var description = doc.RootElement.ValueKind == JsonValueKind.Object
                    ? doc.RootElement.Str("Description")
                    : "";
                return description.Length > 0 ? description : null;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        const string paragraph = "<p>";
        var from = body.IndexOf(paragraph, StringComparison.OrdinalIgnoreCase);
        if (from < 0)
            return null;
        from += paragraph.Length;
        var to = body.IndexOf("</p>", from, StringComparison.OrdinalIgnoreCase);
        if (to < 0)
            return null;

        var text = System.Net.WebUtility.HtmlDecode(body[from..to]);
        var quote = text.IndexOf('"');
        var end = quote < 0 ? -1 : text.IndexOf('"', quote + 1);
        text = (end < 0 ? text : text[(quote + 1)..end]).Trim();
        return text.Length > 0 ? text : null;
    }

    /// <summary>Текст ошибки HTTP</summary>
    private static string HttpStatusError(string host, int port, int status)
    {
        var text = $"{host}:{port} Код ошибки: {status}.";
        if (status == 401)
            text += " Авторизация не пройдена.";
        else if (status == 404)
            text += " Ресурс не найден на сервере.";
        else if (status == 500)
            text += " Сбой на сервере.";
        return text;
    }

    /// <summary>
    /// Разбирает <see cref="Address"/> на хост и порт (по умолчанию 4398).
    /// </summary>
    private static (string Host, int Port) ParseAddress(string address)
    {
        const int defaultPort = 4398;
        var value = address.Trim();
        var separator = value.LastIndexOf(':');
        if (value.Length == 0)
            return ("localhost", defaultPort);
        if (separator >= 0 && int.TryParse(value[(separator + 1)..], out var port) && port is >= 1 and <= 65535)
            return (separator == 0 ? "localhost" : value[..separator], port);
        return (value, defaultPort);
    }

    /// <summary>
    /// Постановка задания в очередь (POST .../async): в ответе — docId.
    /// </summary>
    private Task<bool> PostTask(string path, JsonObject body)
        => Call(HttpMethod.Post, path, body, r =>
        {
            if (_ok && r.Text().Length > 0)
                DocId = r.Text();
        });

    /// <summary>
    /// GET документа по идентификатору (docId): чек, отчёт, внесение, слип и т. д.
    /// </summary>
    private Task<bool> GetDocument(string path, Action<JsonElement>? read = null)
        => Call(HttpMethod.Get, $"{path}?{IdQuery}", null, read ?? ReadDocument);

    /// <summary>
    /// GET списка отчётов за период <see cref="FromDate"/>..<see cref="ToDate"/>.
    /// </summary>
    private Task<bool> GetReportList(string path)
        => GetDocumentList($"{path}?{DeviceQuery}&{DateQuery(FromDate, ToDate)}");

    /// <summary>
    /// GET списка документов: основные поля каждого документа — в <see cref="ReportValues"/> («[номер] Имя»).
    /// </summary>
    private Task<bool> GetDocumentList(string pathAndQuery)
        => Call(HttpMethod.Get, pathAndQuery, null, ReadDocuments);
}
