using System.ComponentModel.DataAnnotations;

namespace Chad.Data
{
    public class DbConfig
    {
        [Key] [MaxLength(32)] public string Type { get; set; } = "";

        public string? Value { get; set; }
    }
}