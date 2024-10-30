using FileUploadWithResume.Web.Models;

namespace FileUploadWithResume.Web.Services
{
    public interface IFileUploadService
    {
        /// <summary> 建立UploadTask並回傳uploadId </summary>
        string CreateUploadTask();

        /// <summary> 由uploadId取得目前檔案大小 </summary>
        long GetFileLength(string uploadId);

        /// <summary> 儲存檔案 </summary>
        Task<UploadResultDto> SaveUploadFile(UploadRequestDto model, long start, CancellationToken ct);
    }
}