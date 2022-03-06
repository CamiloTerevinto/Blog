using CT.Examples.OffloadToHangfire.Api.Models;
using CT.Examples.OffloadToHangfire.Shared.Jobs;
using CT.Examples.OffloadToHangfire.Shared.Models;
using Hangfire;

namespace CT.Examples.OffloadToHangfire.Api.Services
{
    public interface IApiExampleService
    {
        string ScheduleCall(ApiExampleModel model);
    }

    public class ApiExampleService : IApiExampleService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ApiExampleService(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        public string ScheduleCall(ApiExampleModel model)
        {
            var jobModel = new CallApiExampleData
            {
                Url = "http://localhost:5122/example/run-something",
                SomeOtherImportantData = model.SomeImportantData
            };

            var jobId = _backgroundJobClient.Schedule<ICallApiExampleJob>(x => x.Execute(jobModel), TimeSpan.FromSeconds(2));

            return jobId;
        }
    }
}
