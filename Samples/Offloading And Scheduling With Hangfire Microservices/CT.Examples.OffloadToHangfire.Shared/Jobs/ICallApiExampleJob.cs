using CT.Examples.OffloadToHangfire.Shared.Models;

namespace CT.Examples.OffloadToHangfire.Shared.Jobs
{
    public interface ICallApiExampleJob
    {
        Task Execute(CallApiExampleData data);
    }
}
