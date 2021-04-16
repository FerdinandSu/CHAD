namespace Chad.Models
{
    public record ResourceDownload
    {
        public string FileName { get; init; }
        public byte[] Content { get; init; }
        public string ContentType { get; init; }
    }
}