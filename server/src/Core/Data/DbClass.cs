using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chad.Data
{
    public class DbClass
    {
        [Key] public long Id { get; set; }

        [Required] [MaxLength(32)] public string Name { get; set; }

        [Required] public string DirectorId { get; set; }

        [Required] public DbUser Director { get; set; }

        public List<RelStudentClass> Students { get; set; }
    }
}