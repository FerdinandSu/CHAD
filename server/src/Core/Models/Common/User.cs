namespace Chad.Models.Common
{
    public record User : UserBase
    {
        /// <summary>
        ///     用户令牌
        /// </summary>
        public string Token { get; init; }
    }
}