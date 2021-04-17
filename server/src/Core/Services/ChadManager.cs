using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Chad.Data;
using Chad.Models;
using Chad.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Chad.Services
{
    public class ChadManager
    {
        public ChadManager(AccountManager accountManager, ChadDb db, ILogger<ChadManager> logger)
        {
            AccountManager = accountManager;
            Db = db;
            Logger = logger;
        }

        private AccountManager AccountManager { get; }
        private ChadDb Db { get; }
        private ILogger<ChadManager> Logger { get; }

        public async Task DeleteCourseAsync(long id)
        {
            var r = new DbCourse { Id = id };
            Db.Courses.Attach(r);
            Db.Courses.Remove(r);
            await Db.SaveChangesAsync();
        }

        public async Task DeleteClassAsync(long id)
        {
            var r = new DbClass { Id = id };
            Db.Classes.Attach(r);
            Db.Classes.Remove(r);
            await Db.SaveChangesAsync();
        }

        public async Task DeleteResourceAsync(long id)
        {
            var r = new DbResource { Id = id };
            Db.Resources.Attach(r);
            Db.Resources.Remove(r);
            await Db.SaveChangesAsync();
        }

        public async Task<Resource?> AddResourceAsync(string name, string contentType, byte[] content)
        {
            var u = await AccountManager.GetCurrentUserAsync();
            if (u is null) return null;
            var ir = new DbResource
            {
                Content = content,
                Expired = DateTime.MaxValue,
                Name = name,
                ContentType = contentType,
                Uploader = u
            };
            await Db.Resources.AddAsync(ir);
            await Db.SaveChangesAsync();
            return new Resource()
            {
                Uploader = u.FriendlyName,
                Id = ir.Id,
                Name = name,
                Size = content.Length
            };
        }

        public async Task<ResourceDownload?> GetResourceAsync(long id)
        {
            var q =
                from res in Db.Resources
                where res.Id == id
                select new ResourceDownload
                {
                    FileName = res.Name,
                    Content = res.Content,
                    ContentType = res.ContentType
                };
            return await q.SingleOrDefaultAsync();
        }

        private IQueryable<long> GetClassForStudent(string stuName)
        {
            return
                from stuClass in Db.RelStudentClasses
                where stuClass.StudentId == stuName
                select stuClass.ClassId;
        }

        private IQueryable<long> GetCourseForStudent(string stuName)
        {
            return from courseClass in Db.RelCourseClasses
                   join classId in GetClassForStudent(stuName)
                       on courseClass.ClassId equals classId
                   select courseClass.CourseId;
        }

        public async ValueTask<Resource[]> GetResourcesAsync()
        {
            return await GetElementsAsync(cru =>
                    from r in Db.Resources
                    where r.UploaderId == cru.Username
                    orderby r.UploadTime descending
                    select new Resource
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Uploader = cru.Name,
                        Size = r.Content.Length
                    }
                , cru =>
                    from res in Db.ResourceSummaries
                    join resCrs in Db.RelResourceCourses
                        on res.Id equals resCrs.ResourceId
                    join stuCrs in Db.RelStudentCourses
                        on resCrs.CourseId equals stuCrs.CourseId
                    where stuCrs.StudentId == cru.Username
                    select new Resource
                    {
                        Id = res.Id,
                        Name = res.Name,
                        Uploader = res.Uploader,
                        Size = res.Size
                    }

            );
        }

        private async ValueTask<T[]> GetElementsAsync<T>(
            Func<ManagedUser, IQueryable<T>> teacherCase,
            Func<ManagedUser, IQueryable<T>> studentCase
        )
            where T : ElementSummary
        {
            var cru = AccountManager.GetCurrentUser();
            if (cru is null || cru.Role == UserRole.Administrator) return Array.Empty<T>();
            if (cru.Role == UserRole.Teacher) return await teacherCase(cru).ToArrayAsync();
            return await studentCase(cru).ToArrayAsync();
        }

        public async ValueTask<ElementSummary[]> GetCoursesAsync()
        {
            return await GetElementsAsync(
                cru =>
                    from r in Db.Courses
                    where r.DirectorId == cru.Username
                    orderby r.Name
                    select new ElementSummary
                    {
                        Id = r.Id,
                        Name = r.Name
                    },
                cru =>

                #region StudentCourse

                    from r in Db.Courses
                    join id in GetCourseForStudent(cru.Username)
                        on r.Id equals id
                    orderby r.Name
                    select new ElementSummary
                    {
                        Id = r.Id,
                        Name = r.Name
                    }

                    #endregion

            );
        }

        public async ValueTask<Class?> GetClassAsync(long id)
        {
            var qStudent =
                from stu in Db.Users
                join rel in Db.RelStudentClasses
                    on stu.Id equals rel.StudentId
                where rel.ClassId == id
                select new UserSummary
                {
                    Name = stu.FriendlyName,
                    Username = stu.Id
                };
            var students = await qStudent.ToArrayAsync();
            var q =
                from c in Db.Classes
                join director in Db.Users
                    on c.DirectorId equals director.Id
                where c.Id == id
                select new Class
                {
                    Id = id,
                    Director = new()
                    {
                        Username = director.Id,
                        Name = director.FriendlyName
                    },
                    Name = c.Name,
                    Students = students
                };
            return await q.FirstOrDefaultAsync();
        }

        public async ValueTask<Course?> GetCourseAsync(long id)
        {
            var qClass =
                from rel in Db.RelCourseClasses
                join cls in Db.Classes
                    on rel.ClassId equals cls.Id
                where rel.CourseId == id
                select new ElementSummary
                {
                    Id = cls.Id,
                    Name = cls.Name
                };
            var classes = await qClass.ToArrayAsync();
            var qLesson =
                from lesson in Db.Lessons
                where id == lesson.CourseId
                select new ElementSummary
                {
                    Id = lesson.Id,
                    Name = lesson.Name
                };
            var lessons = await qLesson.ToArrayAsync();
            var q =
                from c in Db.Courses
                join director in Db.Users
                    on c.DirectorId equals director.Id
                where c.Id == id
                select new Course
                {
                    Id = id,
                    Description = c.Description,
                    Director = new()
                    {
                        Name = director.FriendlyName,
                        Username = c.DirectorId
                    },
                    Name = c.Name,
                    Classes = classes,
                    Lessons = lessons
                };
            return await q.FirstOrDefaultAsync();
        }

        public async ValueTask<Lesson?> GetLessonAsync(long id)
        {
            var qResource =
                from rel in Db.RelResourceLessons
                join res in Db.ResourceSummaries
                    on rel.ResourceId equals res.Id
                where rel.LessonId == id
                select new Resource
                {
                    Id = res.Id,
                    Name = res.Name,
                    Uploader = res.Uploader,
                    Size = res.Size
                };
            var resources = await qResource.ToArrayAsync();
            var q =
                from l in Db.Lessons
                join c in Db.Courses
                    on l.CourseId equals c.Id
                where l.Id == id
                select new Lesson
                {
                    Name = l.Name,
                    Id = l.Id,
                    Course = new()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }
                    ,
                    Description = l.Description,
                    Resources = resources
                };
            return await q.SingleOrDefaultAsync();
        }

        public async ValueTask DeleteLessonAsync(long id)
        {
            var r = new DbLesson { Id = id };
            Db.Lessons.Attach(r);
            Db.Lessons.Remove(r);
            await Db.SaveChangesAsync();
        }

        public async ValueTask<ElementSummary?> AddLessonAsync(long courseId, ushort i, DescribedElementSummary des)
        {
            try
            {
                var ir = new DbLesson
                {
                    CourseId = courseId,
                    Description = des.Description,
                    Name = des.Name,
                    Index = i
                };
                await Db.Lessons.AddAsync(ir);
                await Db.SaveChangesAsync();
                return new ElementSummary()
                {
                    Id = ir.Id,
                    Name = ir.Name
                };
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return null;
            }
        }

        public async ValueTask<bool> InsertResourceToLessonAsync(long lessonId, ElementSummary des)
        {
            try
            {
                var ir = new RelResourceLesson
                {
                    LessonId = lessonId,
                    ResourceId = des.Id
                };
                await Db.RelResourceLessons.AddAsync(ir);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return false;
            }
        }

        public async ValueTask<ElementSummary[]> GetClassesAsync()
        {
            return await GetElementsAsync(
                u =>
                    from c in Db.Classes
                    where c.DirectorId == u.Username
                    orderby c.Name
                    select new ElementSummary
                    {
                        Id = c.Id,
                        Name = c.Name
                    },
                u =>
                    from c in Db.Classes
                    join cid in GetClassForStudent(u.Username)
                        on c.Id equals cid
                    orderby c.Name
                    select new ElementSummary
                    {
                        Id = c.Id,
                        Name = c.Name
                    }
            );
        }

        public async ValueTask<bool> DeleteClassFromCourseAsync(long courseId, long classId)
        {
            try
            {
                var rel = new RelCourseClass { ClassId = classId, CourseId = courseId };
                Db.RelCourseClasses.Attach(rel);
                Db.RelCourseClasses.Remove(rel);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return false;
            }
        }

        public async ValueTask<bool> DeleteStudentFromClassAsync(string username, long classId)
        {
            try
            {
                var rel = new RelStudentClass { ClassId = classId, StudentId = username };
                Db.RelStudentClasses.Attach(rel);
                Db.RelStudentClasses.Remove(rel);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return false;
            }
        }

        public async ValueTask<bool> DeleteResourceFromLessonAsync(long resId, long lessonId)
        {
            try
            {
                var rel = new RelResourceLesson { ResourceId = resId, LessonId = lessonId };
                Db.Attach(rel);
                /*var rel =
                    Db.RelResourceLessons.FirstOrDefaultAsync(
                        r => r.LessonId == lessonId && r.ResourceId == resId);*/
                //if (rel is null) return;
                Db.RelResourceLessons.Remove(rel);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return false;
            }
        }

        public async ValueTask<bool> InsertStudentToClassAsync(long classId, UserSummary des)
        {
            try
            {
                var ir = new RelStudentClass
                {
                    ClassId = classId,
                    StudentId = des.Username
                };
                await Db.RelStudentClasses.AddAsync(ir);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return false;
            }
        }


        public async ValueTask<ChatMessage[]> GetChatsAsync()
        {
            var u = AccountManager.GetCurrentUser();
            if (u is null) return Array.Empty<ChatMessage>();
            var q =
                from msg in Db.Messages
                join sender in (
                        from user in Db.Users
                        select new { user.Id, Name = user.FriendlyName }
                        )
                    on msg.SenderId equals sender.Id
                join receiver in (
                        from user in Db.Users
                        select new { user.Id, Name = user.FriendlyName }
                    )
                    on msg.ReceiverId equals receiver.Id
                where msg.SenderId == u.Username
                      || msg.ReceiverId == u.Username
                orderby msg.Time descending
                select sender.Id == u.Username
                    ? new ChatMessage
                    {
                        Content = msg.Content,
                        FromCurrentUser = true,
                        Time = msg.Time.ToString(CultureInfo.CurrentCulture),
                        Remote = new()
                        {
                            Name = receiver.Name,
                            Username = receiver.Id
                        }
                    }
                    : new ChatMessage
                    {
                        Content = msg.Content,
                        FromCurrentUser = false,
                        Time = msg.Time.ToString(CultureInfo.CurrentCulture),
                        Remote = new()
                        {
                            Name = sender.Name,
                            Username = sender.Id
                        }
                    };
            return await q.ToArrayAsync();
        }

        public async ValueTask<bool> PostChatAsync(ChatMessage msg)
        {
            try
            {
                var u = AccountManager.GetCurrentUser();
                if (u is null) return false;
                var ir = new DbMessage
                {
                    SenderId = u.Username,
                    ReceiverId = msg.Remote.Username,
                    Content = msg.Content
                };
                await Db.Messages.AddAsync(ir);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return false;
            }
        }

        public async ValueTask<bool> InsertClassToCourseAsync(long courseId, ElementSummary des)
        {
            try
            {
                var ir = new RelCourseClass
                {
                    CourseId = courseId,
                    ClassId = des.Id
                };
                await Db.RelCourseClasses.AddAsync(ir);
                await Db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
                
                return false;
            }
        }

        public async ValueTask<ElementSummary?> AddCourseAsync(DescribedElementSummary des)
        {
            var u = await AccountManager.GetCurrentUserAsync();
            if (u is null) return null;
            var ir = new DbCourse
            {
                Director = u,
                Description = des.Description,
                Name = des.Name
            };
            await Db.Courses.AddAsync(ir);
            await Db.SaveChangesAsync();
            return new ElementSummary()
            {
                Id = ir.Id,
                Name = ir.Name
            };
        }

        public async ValueTask<ElementSummary?> AddClassAsync(ElementSummary summary)
        {
            var u = await AccountManager.GetCurrentUserAsync();
            if (u is null) return null;
            var ir = new DbClass
            {
                Director = u,
                Name = summary.Name
            };
            await Db.Classes.AddAsync(ir);
            await Db.SaveChangesAsync();
            return new ElementSummary()
            {
                Id = ir.Id,
                Name = ir.Name
            };
        }
    }
}