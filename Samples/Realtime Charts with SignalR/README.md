# CT.Examples.RealtimeCharts

This sample ASP.NET Core 6 application demonstrates how to use Azure SignalR Serverless using an Azure Function.
It also shows how to use the SignalR messages received in the front-end to create Plotly graphs. 

This sample also contains a solution diagram, `Solution diagram.drawio`, which can be opened with [Diagrams.net](http://diagrams.net/).

## How to run this sample
To run this sample, you must generate a local.settings.json file under the folder `CT.Examples.RealtimeCharts`, with contents similar to:

```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AzureSignalRConnectionString": "YOUR-SIGNALR-CONNECTION-STRING",
    "RealtimeCharts_SigningKey": "ANY-SIGNING-KEY",
    "RealtimeCharts_EventGridKey": "YOUR-EVENT-GRID-KEY",
    "RealtimeCharts_EventGridTopic": "YOUR-EVENT-GRID-TOPIC-ENDPOINT"
  }
}
```

The data generation can be run from the webpage (which is under /api/index), or by running the `Processor` project.
