using CT.Examples.ExtendedObservability.Extensions.Client;
using CT.Examples.ExtendedObservability.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace CT.Examples.ExtendedObservability.Order.Command
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CT.Examples.ExtendedObservability.Order.Command", Version = "v1" });
            });

            services.AddMicroserviceClient<ICustomerQueryClient, CustomerQueryClient>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:8083");
            }, "Example.Order.Command");

            services.AddCustomHeadersLoggerMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CT.Examples.ExtendedObservability.Order.Command v1"));
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
