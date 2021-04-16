using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chad.Data
{
    public class DbCourse
    {
        [Key] public long Id { get; set; }

        [Required] [MaxLength(32)] public string Name { get; set; }

        [Required] [MaxLength(128)] public string Description { get; set; }

        [Required] public string DirectorId { get; set; }

        [Required] public DbUser Director { get; set; }

        public List<DbLesson> Lessons { get; set; }

        public List<RelCourseClass> Classes { get; set; }
    }
}