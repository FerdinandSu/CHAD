namespace Chad.Models.Common
{
    public record DescribedElementSummary : ElementSummary
    {
        public string Description { get; init; }
    }
}