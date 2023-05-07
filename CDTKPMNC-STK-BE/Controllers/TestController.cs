using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // POST api/<TestController>
        [HttpPost]
        public IActionResult Post([FromBody] DateOnly value)
        {
            return Ok (value.ToString());
        }

    }
}
