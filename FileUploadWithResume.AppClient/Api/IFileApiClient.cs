namespace FileUploadWithResume.AppClient.Api
{
    public interface IFileApiClient
    {
        /// <summary> 建立UploadTask </summary>
        Task<string> CreateUploadTask();

        /// <summary> 設置回報進度委派 </summary>
        void SetReportProgress(Action<string> reportProgress);

        /// <summary> 分塊上傳 </summary>
        Task<string> ChunkedUpload(Stream fileStream, string uploadId, CancellationToken ct);
    }
}