using FileUploadWithResume.Web.Models;
using FileUploadWithResume.Web.Options;
using FileUploadWithResume.Web.Repositories;
using Microsoft.Extensions.Options;

namespace FileUploadWithResume.Web.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileUploadRepository _fileUploadRepository;
        private readonly FileUploadOption _options;

        public FileUploadService(IFileUploadRepository fileUploadRepository, IOptions<FileUploadOption> options)
        {
            _fileUploadRepository = fileUploadRepository;
            _options = options.Value;
        }

        /// <summary> 建立UploadTask並回傳uploadId </summary>
        public string CreateUploadTask()
        {
            string uploadId = Guid.NewGuid().ToString();
            string uploadPath = Path.Combine(_options.SavePath, Path.GetRandomFileName());
            _fileUploadRepository.Insert(new UploadTaskEntity { UploadId = uploadId, UploadPath = uploadPath });
            return uploadId;
        }

        /// <summary> 由uploadId取得目前檔案大小 </summary>
        public long GetFileLength(string uploadId)
        {
            string? uploadPath = _fileUploadRepository.Get(uploadId)?.UploadPath;
            if (string.IsNullOrEmpty(uploadPath))
                return 0;
            var fi = new FileInfo(uploadPath);
            return fi.Exists ? fi.Length : 0;
        }

        /// <summary> 儲存檔案 </summary>
        public async Task<UploadResultDto> SaveUploadFile(UploadRequestDto model, long start, CancellationToken ct)
        {
            // 可再加入一些存取檢查
            if (string.IsNullOrEmpty(model.UploadId))
                return new UploadResultDto { Error = "UploadId為必須" };
            if (model.File == null)
                return new UploadResultDto { Error = "File為必須" };
            string? uploadPath = _fileUploadRepository.Get(model.UploadId)?.UploadPath;
            if (string.IsNullOrEmpty(uploadPath))
                return new UploadResultDto { Error = "查無UploadTask" };

            (Stream? saveStream, string? error) = OpenOrCreateSaveStream(uploadPath, start);
            if (saveStream == null)
                return new UploadResultDto { Error = error };
            using (saveStream)
                await model.File.OpenReadStream().CopyToAsync(saveStream, ct);
            return new UploadResultDto{ Success = true };
        }

        /// <summary> 開啟或建立儲存上傳檔案的串流 </summary>
        private static (Stream?, string? error) OpenOrCreateSaveStream(string uploadPath, long start)
        {
            string? dirName = Path.GetDirectoryName(uploadPath);
            if (!string.IsNullOrWhiteSpace(dirName))
                Directory.CreateDirectory(dirName);
            FileStream saveStream = new FileStream(uploadPath, FileMode.OpenOrCreate, FileAccess.Write);
            if (start > saveStream.Length)
            {
                saveStream.Dispose();
                return (null, "請求的起始位置超過檔案長度");
            }
            if (start > 0)
                saveStream.Seek(start, SeekOrigin.Begin);
            return (saveStream, null);
        }
    }
}
