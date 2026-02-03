using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataMinersAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestorLoginAPIController : ControllerBase
    {
        [HttpGet("HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok("RequestorLoginAPI is running.");
        }
    }
}
