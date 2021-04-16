namespace Chad.Models
{
    public record ChatMessage
    {
        public string Content { get; init; }
        public bool FromCurrentUser { get; init; }
        public UserSummary Remote { get; init; }
        public string Time { get; init; }
    }
}