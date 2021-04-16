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
    public class LessonController : ChadController
    {
        public LessonController(ChadManager chadManager)
        {
            ChadManager = chadManager;
        }

        private ChadManager ChadManager { get; }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> Get([FromRoute] long id)
        {
            var les = await ChadManager.GetLessonAsync(id);
            if (les is null) return NotFound();
            return les;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Lesson>> Delete([FromRoute] long id)
        {
            await ChadManager.DeleteLessonAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/res")]
        public async Task<ActionResult> AddResource([FromRoute] long id, [FromBody] ElementSummary summary)
        {
            return await ChadManager.InsertResourceToLessonAsync(id, summary) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}/res/{resId}")]
        public async Task<ActionResult> RemoveResource([FromRoute] long id, [FromRoute] long resId)
        {
            return await ChadManager.DeleteResourceFromLessonAsync(resId, id) ? NoContent() : NotFound();
        }
    }
}