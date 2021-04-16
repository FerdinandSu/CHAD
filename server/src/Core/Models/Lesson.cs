using Chad.Models.Common;

namespace Chad.Models
{
    public record Lesson : DescribedElementSummary
    {
        public ElementSummary Course { get; init; }
        public Resource[] Resources { get; init; }
    }
}