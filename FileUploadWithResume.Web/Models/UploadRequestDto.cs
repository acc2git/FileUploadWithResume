namespace FileUploadWithResume.Web.Models
{
    public class UploadRequestDto
    {
        public IFormFile? File { get; set; }
        public string? UploadId { get; set; }
    }

    public class UploadResultDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }

    public class UploadTaskEntity
    {
        public string UploadId { get; set; } = string.Empty;
        public string UploadPath { get; set; } = string.Empty;
    }
}
