using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chad.Data
{
    public class DbLesson
    {
        [Key] public long Id { get; set; }

        [Required] [MaxLength(32)] public string Name { get; set; }

        [Required] public ushort Index { get; set; }

        [Required] [MaxLength(128)] public string Description { get; set; }

        [Required] public long CourseId { get; set; }

        [Required] public DbCourse Course { get; set; }

        public List<RelResourceLesson> Resources { get; set; }
    }
}