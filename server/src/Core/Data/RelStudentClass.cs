using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    [Index(nameof(ClassId), nameof(StudentId))]
    public class RelStudentClass
    {
        [Required] public DbUser Student { get; set; }

        [Required] public DbClass Class { get; set; }

        [Required] public string StudentId { get; set; }

        [Required] public long ClassId { get; set; }
    }
}