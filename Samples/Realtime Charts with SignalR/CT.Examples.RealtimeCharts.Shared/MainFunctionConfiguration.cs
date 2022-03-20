namespace CT.Examples.RealtimeCharts.Shared
{
    public static class MainFunctionConfiguration
    {
        public static string SigningKey { get; } = Environment.GetEnvironmentVariable("RealtimeCharts_SigningKey", EnvironmentVariableTarget.Process) 
            ?? throw new Exception("The environment variable RealtimeCharts_SigningKey is missing");

        public static string EventGridKey { get; } = Environment.GetEnvironmentVariable("RealtimeCharts_EventGridKey", EnvironmentVariableTarget.Process)
            ?? throw new Exception("The environment variable RealtimeCharts_EventGridKey is missing");

        public static string EventGridTopic { get; } = Environment.GetEnvironmentVariable("RealtimeCharts_EventGridTopic", EnvironmentVariableTarget.Process)
            ?? throw new Exception("The environment variable RealtimeCharts_EventGridTopic is missing");
    }
}