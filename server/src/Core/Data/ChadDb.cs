using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chad.Data
{
    /// <summary>
    ///     数据库
    /// </summary>
    public class ChadDb : IdentityDbContext<DbUser>
    {
        public ChadDb(DbContextOptions<ChadDb> options) : base(options)
        {
        }

        public DbSet<KlRelResourceCourse> RelResourceCourses { get; set; } = null!;
        public DbSet<KlResourceSummary> ResourceSummaries { get; set; } = null!;
        public DbSet<KlRelStudentCourse> RelStudentCourses { get; set; } = null!;
        public DbSet<RelResourceLesson> RelResourceLessons { get; set; } = null!;
        public DbSet<RelCourseClass> RelCourseClasses { get; set; } = null!;
        public DbSet<RelStudentClass> RelStudentClasses { get; set; } = null!;
        public DbSet<DbLesson> Lessons { get; set; } = null!;
        public DbSet<DbClass> Classes { get; set; } = null!;
        public DbSet<DbConfig> Configs { get; set; } = null!;
        public DbSet<DbCourse> Courses { get; set; } = null!;
        public DbSet<DbMessage> Messages { get; set; } = null!;
        public DbSet<DbResource> Resources { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DbMessage>()
                .Property(msg => msg.Time)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            builder.Entity<DbResource>()
                .Property(res => res.UploadTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            builder.Entity<KlResourceSummary>()
                .HasNoKey()
                .ToView(nameof(ResourceSummaries));

            builder.Entity<KlRelStudentCourse>()
                .HasNoKey()
                .ToView(nameof(RelStudentCourses));

            builder.Entity<KlRelResourceCourse>()
                .HasNoKey()
                .ToView(nameof(RelResourceCourses));

            builder.Entity<RelCourseClass>()
                .HasKey(r => new {r.ClassId, r.CourseId});

            builder.Entity<RelResourceLesson>()
                .HasKey(r => new {r.ResourceId, r.LessonId});

            builder.Entity<RelStudentClass>()
                .HasKey(r => new {r.StudentId, r.ClassId});
            base.OnModelCreating(builder);
        }
    }
}