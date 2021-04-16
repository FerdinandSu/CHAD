using System.IO;
using System.Threading.Tasks;
using Chad.Models;
using Chad.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChadApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResController : ChadController
    {
        public ResController(ChadManager chadManager)
        {
            ChadManager = chadManager;
        }

        private ChadManager ChadManager { get; }

        [HttpGet("")]
        public async Task<ActionResult<Resource[]>> Get()
        {
            return await ChadManager.GetResourcesAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var file = await ChadManager.GetResourceAsync(id);
            if (file is null) return NotFound();
            return File(file.Content, file.ContentType, file.FileName, true);
        }

        [HttpPost("")]
        public async Task<ActionResult<Resource>> Post([FromForm] IFormFile file)
        {
            if (file.Length == 0) return BadRequest();
            await using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var res = await ChadManager.AddResourceAsync(
                file.FileName, file.ContentType, ms.ToArray());

            if (res is null) return Unauthorized();
            return res;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            await ChadManager.DeleteResourceAsync(id);
            return NoContent();
        }
    }
}