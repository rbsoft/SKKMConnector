using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RBSoftSkkm.Internal
{
    /// <summary>
    /// HTTP-транспорт к серверу ККМ.
    /// </summary>
    internal sealed class KkmTransport : IDisposable
    {
        /// <summary>
        /// Разбор ответа по контрактам: имена полей как на сервере, с учётом регистра.
        /// </summary>
        private static readonly JsonSerializerOptions JsonOptions = new();

        private static readonly JsonSerializerOptions BodyJsonOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly HttpClient _http = CreateHttp();
        private bool _disposed;

        private const string ApiPath = "/PrintService/api/v4/";
        private const string JsonMediaType = "application/json";
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60);

        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 4398;
        public string? Token { get; set; }
        public string? TerminalId { get; set; }
        public TimeSpan Timeout { get; set; } = DefaultTimeout;

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _http.Dispose();
        }

        /// <summary>
        /// Запрос к серверу ККМ: <c>Result</c> ответа разбирается в контракт <typeparamref name="T"/>.
        /// </summary>
        internal async Task<ResponseResult<T>> Send<T>(
            HttpMethod method,
            string relativeUrl,
            object? body = null,
            CancellationToken cancellationToken = default)
        {
            if (_disposed)
                return FailResult<T>(-1, "Коннектор закрыт. Создайте новый SkkmConnector.");
            if (string.IsNullOrWhiteSpace(Host) || Port is < 1 or > 65535)
                return FailResult<T>(-1, "Укажите Host и Port сервера ККМ.");

            using var request = new HttpRequestMessage(method, RequestUri(relativeUrl));
            AddApiKey(request);

            if (body != null && method != HttpMethod.Get && method != HttpMethod.Delete)
            {
                var json = JsonSerializer.Serialize(body, body.GetType(), BodyJsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, JsonMediaType);
            }

            var timeout = Timeout <= TimeSpan.Zero ? DefaultTimeout : Timeout;
            using var timeoutCts = new CancellationTokenSource(timeout);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);

            try
            {
                using var response = await _http.SendAsync(request, linkedCts.Token);
                var responseBody = await response.Content.ReadAsStringAsync(linkedCts.Token);
                var statusCode = (int)response.StatusCode;

                if (string.IsNullOrWhiteSpace(responseBody))
                    return FailResult<T>(statusCode, DescribeHttpError(statusCode, response.StatusCode.ToString()));

                try
                {
                    var parsed = JsonSerializer.Deserialize<ResponseResult<T>>(responseBody, JsonOptions);
                    if (parsed != null)
                        return parsed;
                }
                catch (JsonException)
                {
                    // Result не по контракту — статус сервера отдаём как есть, без Result.
                    try
                    {
                        var status = JsonSerializer.Deserialize<ResponseResult<JsonElement>>(responseBody, JsonOptions);
                        if (status != null)
                            return new ResponseResult<T> { Success = status.Success, Code = status.Code, Description = status.Description };
                    }
                    catch (JsonException)
                    {
                    }
                }

                return FailResult<T>(statusCode, DescribeHttpError(statusCode, "некорректный ответ сервера"));
            }
            catch (HttpRequestException ex)
            {
                return FailResult<T>(-1, $"Ошибка соединения: {ex.Message}");
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return FailResult<T>(-3, "Запрос отменён");
            }
            catch (OperationCanceledException)
            {
                return FailResult<T>(-2, "Превышено время ожидания ответа сервера");
            }
        }

        private Uri RequestUri(string relativeUrl)
        {
            var queryStart = relativeUrl.IndexOf('?');
            var path = queryStart < 0 ? relativeUrl : relativeUrl[..queryStart];
            var query = queryStart < 0 ? "" : relativeUrl[(queryStart + 1)..];

            return new UriBuilder
            {
                Scheme = Uri.UriSchemeHttp,
                Host = Host,
                Port = Port,
                Path = $"{ApiPath.TrimEnd('/')}/{path.TrimStart('/')}",
                Query = query
            }.Uri;
        }

        private static HttpClient CreateHttp()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1)
            };

            return new HttpClient(handler)
            {
                Timeout = System.Threading.Timeout.InfiniteTimeSpan
            };
        }

        private static ResponseResult<T> FailResult<T>(int code, string description)
            => new() { Success = false, Code = code, Description = description };

        private static string DescribeHttpError(int statusCode, string fallback)
            => statusCode switch
            {
                401 => "Ошибка авторизации. Укажите токен или включите анонимный доступ на сервере ККМ.",
                403 => "Доступ запрещён. Проверьте токен API.",
                _ => $"Ошибка HTTP {statusCode}: {fallback}"
            };

        private void AddApiKey(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(Token))
                request.Headers.TryAddWithoutValidation("api_key", Token);

            if (!string.IsNullOrEmpty(TerminalId))
                request.Headers.TryAddWithoutValidation("TerminalId", TerminalId);
        }
    }
}
