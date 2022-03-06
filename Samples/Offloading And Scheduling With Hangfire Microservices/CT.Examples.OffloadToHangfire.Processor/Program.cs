using CT.Examples.OffloadToHangfire.Processor.Services;
using CT.Examples.OffloadToHangfire.Shared.Jobs;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
        services
            .AddScoped<ICallApiExampleJob, CallApiExampleJob>(sp => new CallApiExampleJob(new HttpClient()))
            .AddHangfire(configuration =>
            {
                var currentPath = builder.HostingEnvironment.ContentRootPath;
                var projectPath = Path.GetFullPath(Path.Combine(currentPath, "..", "..", ".."));
                var pathToDb = Path.Combine(projectPath, "Hangfire.mdf");

                configuration.UseSqlServerStorage($"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"{pathToDb}\";Integrated Security=True");
            })
            .AddHangfireServer())
    .Build();

await host.RunAsync();