using CT.Examples.OffloadToHangfire.Api.Models;
using CT.Examples.OffloadToHangfire.Shared.Jobs;
using CT.Examples.OffloadToHangfire.Shared.Models;
using Hangfire;

namespace CT.Examples.OffloadToHangfire.Api.Services
{
    public interface IApiExampleService
    {
        string ScheduleCall(ScheduleSimpleCallModel model);
        string ScheduleRecurringCall(ScheduleRecurringCallModel model);
    }

    public class ApiExampleService : IApiExampleService
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public ApiExampleService(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        public string ScheduleCall(ScheduleSimpleCallModel model)
        {
            var jobModel = new CallApiExampleData
            {
                Url = "http://localhost:5122/example/run-something",
                SomeOtherImportantData = model.SomeImportantData
            };

            var jobId = _backgroundJobClient.Schedule<ICallApiExampleJob>(x => x.Execute(jobModel), TimeSpan.FromSeconds(2));

            return jobId;
        }

        public string ScheduleRecurringCall(ScheduleRecurringCallModel model)
        {
            var jobModel = new CallApiExampleData
            {
                Url = "http://localhost:5122/example/run-something",
                SomeOtherImportantData = model.SomeImportantData
            };

            var recurrenceId = Guid.NewGuid().ToString();
            _recurringJobManager.AddOrUpdate<ICallApiExampleJob>(recurrenceId, x => x.Execute(jobModel), model.CronExpression);

            return recurrenceId;
        }
    }
}
