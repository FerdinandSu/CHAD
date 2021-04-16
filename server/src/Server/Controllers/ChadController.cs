using Microsoft.AspNetCore.Mvc;

namespace ChadApi.Controllers
{
    public abstract class ChadController : Controller
    {
        protected IActionResult ApplicationError(string expression)
        {
            return StatusCode(500, new {Message = expression});
        }
    }
}