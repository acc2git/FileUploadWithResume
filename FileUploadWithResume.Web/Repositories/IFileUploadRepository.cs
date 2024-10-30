using FileUploadWithResume.Web.Models;

namespace FileUploadWithResume.Web.Repositories
{
    public interface IFileUploadRepository
    {
        /// <summary> 新增 </summary>
        void Insert(UploadTaskEntity entity);

        /// <summary> 取得單筆 </summary>
        UploadTaskEntity? Get(string uploadId);
    }
}