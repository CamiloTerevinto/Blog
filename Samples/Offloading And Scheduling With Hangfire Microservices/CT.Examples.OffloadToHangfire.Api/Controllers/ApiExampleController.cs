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

        [HttpPost("schedule-simple")]
        public IActionResult ScheduleSimpleCall([FromBody]ScheduleSimpleCallModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var jobId = _apiExampleService.ScheduleCall(model);

            return Ok(new { JobId = jobId });
        }

        [HttpPost("schedule-recurring")]
        public IActionResult ScheduleRecurringCall([FromBody] ScheduleRecurringCallModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var recurrenceId = _apiExampleService.ScheduleRecurringCall(model);

            return Ok(new { RecurrenceId = recurrenceId });
        }

        [HttpPost("run-something")]
        public IActionResult ExampleCall([FromBody]object data)
        {
            // Some very important stuff here
            return Ok(data);
        }
    }
}
