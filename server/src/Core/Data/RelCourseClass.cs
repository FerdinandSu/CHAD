using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    [Index(nameof(ClassId), nameof(CourseId))]
    public class RelCourseClass
    {
        [Required] public DbCourse Course { get; set; }

        [Required] public DbClass Class { get; set; }

        [Required] public long CourseId { get; set; }

        [Required] public long ClassId { get; set; }
    }
}