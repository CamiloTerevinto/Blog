using CT.Example.Extensions.Client;
using CT.Example.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace CT.Example.Gateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CT.Example.Gateway", Version = "v1" });
            });

            services.AddMicroserviceClient<IOrderApiClient, OrderApiClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:8081");
            }, "Example.Gateway");

            services.AddCustomHeadersLoggerMiddleware("Gateway");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CT.Example.Gateway v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseCustomHeadersLoggerMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
