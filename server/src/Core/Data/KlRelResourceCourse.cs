using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    [Keyless]
    public class KlRelResourceCourse
    {
        [Required] public long CourseId { get; set; }

        [Required] public long ResourceId { get; set; }
    }
}