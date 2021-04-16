using Chad.Models.Common;

namespace Chad.Models
{
    public record Class : ElementSummary
    {
        public UserSummary Director { get; init; }
        public UserSummary[] Students { get; init; }
    }
}