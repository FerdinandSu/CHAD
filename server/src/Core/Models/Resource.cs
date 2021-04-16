using Chad.Models.Common;

namespace Chad.Models
{
    public record Resource : ElementSummary
    {
        public string Uploader { get; init; }
        public int Size { get; init; }
    }
}