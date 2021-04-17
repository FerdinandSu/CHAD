using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
