using Chad.Models.Common;

namespace Chad.Models
{
    public record Course : DescribedElementSummary
    {
        public UserSummary Director { get; init; }
        public ElementSummary[] Lessons { get; init; }
        public ElementSummary[] Classes { get; init; }
    }
}