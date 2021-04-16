using Chad.Models.Common;

namespace Chad.Models
{
    public record ManagedUser : UserBase
    {
        /// <summary>
        ///     用户名
        /// </summary>
        public string Username { get; init; }
    }
}