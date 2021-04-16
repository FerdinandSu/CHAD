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
    public class ClassController : ChadController
    {
        public ClassController(ChadManager chadManager)
        {
            ChadManager = chadManager;
        }

        private ChadManager ChadManager { get; }

        [HttpGet("")]
        public async Task<ActionResult<ElementSummary[]>> Get()
        {
            return await ChadManager.GetClassesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> Get([FromRoute] long id)
        {
            var c = await ChadManager.GetClassAsync(id);
            if (c is null) return NotFound();
            return c;
        }

        [HttpPost("")]
        public async Task<ActionResult<ElementSummary>> Post([FromBody] ElementSummary summary)
        {
            var es = await ChadManager.AddClassAsync(summary);
            if (es is null) return NotFound();
            return es;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            await ChadManager.DeleteClassAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/student")]
        public async Task<ActionResult<ElementSummary>> AddStudent(
            [FromRoute] long id, [FromBody] UserSummary summary)
        {
            return await ChadManager.InsertStudentToClassAsync(id, summary) ? NoContent() : NotFound();
        }

        [HttpDelete("{id}/student/{stuId}")]
        public async Task<ActionResult> RemoveStudent([FromRoute] long id, [FromRoute] string stuId)
        {
            return await ChadManager.DeleteStudentFromClassAsync(stuId, id) ? NoContent() : NotFound();
        }
    }
}