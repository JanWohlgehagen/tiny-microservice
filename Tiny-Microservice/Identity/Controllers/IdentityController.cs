using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : ControllerBase
    {
        [HttpGet("Authenticate")]
        public IActionResult Authenticate()
        {
            Console.WriteLine("Authorize");
            return Ok(true);
        }
    }
}
