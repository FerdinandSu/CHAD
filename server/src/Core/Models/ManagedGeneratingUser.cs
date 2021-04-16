namespace Chad.Models
{
    /// <summary>
    ///     正在生成的用户
    /// </summary>
    public record ManagedGeneratingUser : ManagedUser
    {
        /// <summary>
        ///     初始密码
        /// </summary>
        public string InitialPassword { get; init; }
    }
}