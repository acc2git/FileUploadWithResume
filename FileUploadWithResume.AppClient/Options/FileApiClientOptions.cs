namespace FileUploadWithResume.AppClient.Options
{
    public class FileApiClientOption
    {
        public string ApiHost { get; set; } = string.Empty;
        public long ChunkSize { get; set; }
        public EndpointOption Endpoints { get; set; } = new();
    }

    public class EndpointOption
    {
        public string Upload { get; set; } = string.Empty;
        public string GetFileLength { get; set; } = string.Empty;
        public string CreateUploadTask { get; set; } = string.Empty;
    }
}
