using System.Threading.Tasks;
using Chad.Models;
using Chad.Models.Common;
using Chad.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChadApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ChadController
    {
        public CourseController(ChadManager chadManager)
        {
            ChadManager = chadManager;
        }

        private ChadManager ChadManager { get; }

        [HttpGet("")]
        public async Task<ActionResult<ElementSummary[]>> Get()
        {
            return await ChadManager.GetCoursesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> Get([FromRoute] long id)
        {
            var c = await ChadManager.GetCourseAsync(id);
            if (c is null) return NotFound();
            return c;
        }

        [HttpPost("")]
        public async Task<ActionResult<ElementSummary>> Post([FromBody] DescribedElementSummary summary)
        {
            var es = await ChadManager.AddCourseAsync(summary);
            if (es is null) return NotFound();
            return es;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            await ChadManager.DeleteCourseAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/lesson/{index}")]
        public async Task<ActionResult<ElementSummary>> AddLesson(
            [FromRoute] long id, [FromRoute] ushort index, [FromBody] DescribedElementSummary summary)
        {
            var es = await ChadManager.AddLessonAsync(id, index, summary);
            if (es is null) return NotFound();
            return es;
        }

        [HttpPost("{id}/class")]
        public async Task<ActionResult> AddClass([FromRoute] long id, [FromBody] ElementSummary summary)
        {
            return await ChadManager.InsertClassToCourseAsync(id, summary) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}/class/{classId}")]
        public async Task<ActionResult> RemoveClass([FromRoute] long id, [FromRoute] long classId)
        {
            return await ChadManager.DeleteClassFromCourseAsync(id, classId) ? NoContent() : NotFound();
        }
    }
}