namespace CT.Examples.SonarqubeDocker.Api.Services;

public interface IDateService 
{
    Task<string> GetCurrentDateAsync();
}

public class DateService : IDateService
{
    public Task<string> GetCurrentDateAsync()
    {
        return Task.FromResult(DateTime.UtcNow.ToString());
    }

    private static void DumbTest()
    {
        Console.WriteLine("I do nothing useful, except show code coverage features :)");
    }
}
