using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    [Index(nameof(LessonId), nameof(ResourceId))]
    public class RelResourceLesson
    {
        [Required] public DbLesson Lesson { get; set; }

        [Required] public DbResource Resource { get; set; }

        [Required] public long LessonId { get; set; }

        [Required] public long ResourceId { get; set; }

        [Required] public ushort Index { get; set; }
    }
}