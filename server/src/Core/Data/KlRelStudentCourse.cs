using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    [Keyless]
    public class KlRelStudentCourse
    {
        public string StudentId { get; set; }
        public long CourseId { get; set; }
    }
}