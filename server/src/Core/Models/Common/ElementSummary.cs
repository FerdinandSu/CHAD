namespace Chad.Models.Common
{
    /// <summary>
    ///     元素摘要
    /// </summary>
    public record ElementSummary
    {
        /// <summary>
        ///     元素名称
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        ///     ID, 主键。
        /// </summary>
        public long Id { get; init; }
    }
}