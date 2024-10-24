namespace CT.Examples.CqrsWithAspire.Tests.Internal;

public sealed class AspireResourcesFactory : IAsyncDisposable
{
    private class Resources(DistributedApplication app, CommandApiWebAppFactory commandFactory, QueryApiWebAppFactory queryFactory) : IAsyncDisposable
    {
        private readonly DistributedApplication _app = app;
        private readonly CommandApiWebAppFactory _commandApiWebAppFactory = commandFactory;
        private readonly QueryApiWebAppFactory _queryApiWebAppFactory = queryFactory;

        public HttpClient CommandApiClient { get; } = commandFactory.CreateClient();
        public HttpClient QueryApiClient { get; } = queryFactory.CreateClient();

        public static async Task<Resources> StartAsync()
        {
            var options = new DistributedApplicationOptions
            {
                AssemblyName = typeof(AspireResourcesFactory).Assembly.FullName,
                DisableDashboard = true
            };

            var builder = DistributedApplication.CreateBuilder(options);

            var postgres = builder.AddPostgres("postgres");
            postgres.AddDatabase("shop");

            var app = builder.Build();

            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();

            await app.StartAsync();

            await resourceNotificationService.WaitForResourceAsync(postgres.Resource.Name, KnownResourceStates.Running);

            // WaitForResourceAsync does not really wait until the database is ready
            await Task.Delay(10_000);

            var postgresConnectionString = await postgres.Resource.GetConnectionStringAsync();

            var commandApiWebAppFactory = await CommandApiWebAppFactory.CreateInitializedAsync($"{postgresConnectionString};Database=shop");
            var queryApiWebAppFactory = await QueryApiWebAppFactory.CreateInitializedAsync($"{postgresConnectionString};Database=shop");

            return new Resources(app, commandApiWebAppFactory, queryApiWebAppFactory);
        }

        public async ValueTask DisposeAsync()
        {
            CommandApiClient.Dispose();
            QueryApiClient.Dispose();

            if (_commandApiWebAppFactory is CommandApiWebAppFactory commandFactory)
            {
                await commandFactory.DisposeAsync();
            }

            if (_queryApiWebAppFactory is QueryApiWebAppFactory queryFactory)
            {
                await queryFactory.DisposeAsync();
            }

            if (_app is DistributedApplication distributedApplication)
            {
                await distributedApplication.StopAsync();
                await distributedApplication.DisposeAsync();
            }
        }
    }

    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private Resources _resources;
    private static AspireResourcesFactory _testBase;

    internal HttpClient CommandApiClient { get => _resources.CommandApiClient; }
    internal HttpClient QueryApiClient { get => _resources.QueryApiClient; }

    public static async Task<AspireResourcesFactory> StartAsync()
    {
        await _semaphore.WaitAsync();

        if (_testBase != null)
        {
            _semaphore.Release();
            return _testBase;
        }

        _testBase = new()
        {
            _resources = await Resources.StartAsync()
        };

        _semaphore.Release();

        return _testBase;
    }

    public async ValueTask DisposeAsync()
    {
        if (_resources != null)
        {
            await _resources.DisposeAsync();
        }
    }
}

