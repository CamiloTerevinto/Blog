using CT.Examples.OffloadToHangfire.Api.Models;
using CT.Examples.OffloadToHangfire.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CT.Examples.OffloadToHangfire.Api.Controllers
{
    [Route("example")]
    public class ApiExampleController : Controller
    {
        private readonly IApiExampleService _apiExampleService;

        public ApiExampleController(IApiExampleService apiExampleService)
        {
            _apiExampleService = apiExampleService;
        }

        [HttpPost("schedule")]
        public IActionResult ScheduleCall([FromBody]ApiExampleModel apiExampleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var jobId = _apiExampleService.ScheduleCall(apiExampleModel);

            return Ok(new { JobId = jobId });
        }

        [HttpPost("run-something")]
        public IActionResult ExampleCall([FromBody]object data)
        {
            // Some very important stuff here
            return Ok(data);
        }
    }
}
