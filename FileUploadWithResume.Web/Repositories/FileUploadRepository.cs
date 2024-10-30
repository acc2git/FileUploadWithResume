using FileUploadWithResume.Web.Models;

namespace FileUploadWithResume.Web.Repositories
{
    public class FileUploadRepository : IFileUploadRepository
    {
        // 使用Dictionary模擬儲存uploadTasks資料的DB
        private Dictionary<string, UploadTaskEntity> _uploadTasks;

        public FileUploadRepository()
        {
            _uploadTasks = [];
        }

        /// <summary> 新增 </summary>
        public void Insert(UploadTaskEntity entity)
        {
            _uploadTasks.Add(entity.UploadId, entity);
        }

        /// <summary> 取得單筆 </summary>
        public UploadTaskEntity? Get(string uploadId)
        {
            _uploadTasks.TryGetValue(uploadId, out UploadTaskEntity? entity);
            return entity;
        }
    }
}
