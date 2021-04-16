namespace Chad.Models.Common
{
    public record UserBase : UserSummary
    {
        /// <summary>
        ///     用户的角色
        /// </summary>
        public UserRole Role { get; init; }

    }
}