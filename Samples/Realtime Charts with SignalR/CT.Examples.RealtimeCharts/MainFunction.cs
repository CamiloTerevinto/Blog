// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Azure;
using Azure.Messaging.EventGrid;
using CT.Examples.RealtimeCharts.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CT.Examples.RealtimeCharts.ProgressReporter
{
    public class MainFunction : ServerlessHub
    {
        private class Headers
        {
            public const string Authorization = "Authorization";
            public const string SignalRUserId = "x-ms-signalr-user-id";
        }

        private const string NewMessageTarget = "newMessage";
        private static readonly SigningCredentials _signingCredentials = new(new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(MainFunctionConfiguration.SigningKey)), SecurityAlgorithms.HmacSha256);

        [FunctionName("index")]
        public IActionResult GetHomePage(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest httpRequest,
            ExecutionContext context)
        {
            var path = Path.Combine(context.FunctionAppDirectory, "Content", "index.html");
            var index = File.ReadAllText(path);

            var handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(new JwtSecurityToken(
                issuer: httpRequest.Host.Value,
                audience: httpRequest.Host.Value,
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.Email, "test@example.com")
                },
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: _signingCredentials));

            index = index.Replace("@@ACCESS_TOKEN@@", token);

            return new ContentResult
            {
                Content = index,
                ContentType = "text/html",
            };
        }

        [FunctionName("trigger-events")]
        public async Task<IActionResult> TriggerEvents([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest httpRequest)
        {
            var bearerToken = httpRequest.Headers[Headers.Authorization].ToString();

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return new UnauthorizedResult();
            }

            // We could, for example, get the user that triggered the action and send the messages only to that user.
            // Since the email is hard-coded, though, it would add little value to this sample code.
            var email = GetClaims(bearerToken).FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            await RandomDataGenerator.GenerateDataAsync();

            return new NoContentResult();
        }

        [FunctionName("negotiate")]
        public IActionResult Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest httpRequest)
        {
            var bearerToken = httpRequest.Headers[Headers.Authorization].ToString();

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return new UnauthorizedResult();
            }

            var userId = httpRequest.Headers[Headers.SignalRUserId].ToString();

            return new OkObjectResult(Negotiate(userId, GetClaims(bearerToken)));
        }

        [FunctionName(nameof(ReportProgress))]
        public async Task ReportProgress([EventGridTrigger] EventGridEvent eventGridEvent)
        {
            var data = eventGridEvent.Data.ToObjectFromJson<DataPoint[]>();

            await Clients.All.SendAsync(NewMessageTarget, data);
        }
    }
}
