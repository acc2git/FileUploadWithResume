using FileUploadWithResume.AppClient.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace FileUploadWithResume.AppClient.Api
{
    public class FileApiClient : IFileApiClient
    {
        private readonly HttpClient _client;
        private readonly FileApiClientOption _options;
        private Action<string>? _reportProgress;

        public FileApiClient(IHttpClientFactory httpClientFactory, IOptions<FileApiClientOption> options)
        {
            _options = options.Value;
            _client = httpClientFactory.CreateClient("FileApi");
            _client.BaseAddress = new Uri(_options.ApiHost);
        }

        /// <summary> 分塊上傳 </summary>
        public async Task<string> ChunkedUpload(Stream fileStream, string uploadId, CancellationToken ct)
        {
            long serverFileLength = await GetFileLength(uploadId);
            long start = serverFileLength;
            _reportProgress?.Invoke($"uploadId: {uploadId}, start from {start}");
            while (start < fileStream.Length)
            {
                fileStream.Seek(start, SeekOrigin.Begin);
                long end = Math.Min(start + _options.ChunkSize - 1, fileStream.Length - 1);
                byte[] buffer = new byte[end - start + 1];
                fileStream.Read(buffer, 0, buffer.Length);
                using MemoryStream ms = new(buffer);
                using MultipartFormDataContent fileContent = new()
                {
                    { new StringContent(uploadId), "UploadId" },
                    { new StreamContent(ms), "File", "fileName" }
                };
                fileContent.Headers.ContentRange = new ContentRangeHeaderValue(start, end, fileStream.Length);

                var resp = await _client.PostAsync(_options.Endpoints.Upload, fileContent, ct);
                await EnsureSuccessStatusCodeWithMessage(resp);
                _reportProgress?.Invoke($"uploadId: {uploadId}, {start} - {end}");
                start = end + 1;
            }
            return string.Empty;
        }

        private async Task<long> GetFileLength(string uploadId)
        {
            var resp = await _client.GetAsync($"{_options.Endpoints.GetFileLength}/{uploadId}");
            await EnsureSuccessStatusCodeWithMessage(resp);
            string respStr = await resp.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(respStr);
            return (long?)obj["data"] ?? 0;
        }

        /// <summary> 建立UploadTask </summary>
        public async Task<string> CreateUploadTask()
        {
            var resp = await _client.PostAsync(_options.Endpoints.CreateUploadTask, null);
            await EnsureSuccessStatusCodeWithMessage(resp);
            string respStr = await resp.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(respStr);
            return (string?)obj["data"] ?? string.Empty;
        }

        /// <summary> 設置回報進度委派 </summary>
        public void SetReportProgress(Action<string> reportProgress)
        {
            _reportProgress = reportProgress;
        }

        private async Task EnsureSuccessStatusCodeWithMessage(HttpResponseMessage resp)
        {
            if (resp.IsSuccessStatusCode)
                return;
            string respStr = await resp.Content.ReadAsStringAsync();
            string errorMessage;
            try
            {
                var obj = JObject.Parse(respStr);
                errorMessage = (string?)obj["error"] ?? respStr;
            }
            catch
            {
                errorMessage = respStr;
            }
            throw new HttpRequestException($"HTTP request failed with status code: {(int)resp.StatusCode} ({resp.StatusCode}). Error message: {errorMessage}");
        }
    }
}