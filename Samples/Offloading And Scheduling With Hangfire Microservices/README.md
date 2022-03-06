# CT.Examples.OffloadToHangfire

This sample ASP.NET Core 6 application demonstrates how to use Hangfire in separate applications to 
offload long-running tasks and to schedule tasks, as well as other features supported by Hangfire.

A SQL Server LocalDb (SQL Server 2019) .mdf file is provided to have a runnable sample, 
but this would obviously be discouraged in production scenarios.

Ideally, especially for bigger teams, the `Shared` project would be a NuGet package consumed by the 
API and the Processor, which would normally be in different repositories.