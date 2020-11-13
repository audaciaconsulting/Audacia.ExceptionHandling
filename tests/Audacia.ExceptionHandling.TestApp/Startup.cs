using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Audacia.ExceptionHandling.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Robotify.AspNetCore;

namespace Audacia.ExceptionHandling.TestApp
{
    public class Startup
    {
        private HttpClient _http = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRobotify();
            services.AddCorsPolicy();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseCors("CorsPolicy");
            app.UseRobotify();
            app.UseRouting();

            var logger = app.ApplicationServices.GetService<ILogger>();

            app.ConfigureExceptions(e =>
            {
                e.Handle((KeyNotFoundException ex) => new
                {
                    Result = ex.Message
                }, HttpStatusCode.NotFound);

                e.Handle((InvalidOperationException ex) => new
                {
                    Result = ex.Message
                });

                e.Handle((ArgumentException ex) => new
                {
                    Result = ex.Message
                }, ex =>
                {
                    Console.Error.Write("Argument exception encountered");
                });

                e.WithDefaultLogging(ex =>
                {
                    logger.LogError(ex, ex.Message);
                    Console.Error.Write(ex);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context => throw new InvalidOperationException("hehehe"));
            });

            Task.Run(async () =>
            {
                var response = await _http.GetAsync("/");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("RESPONSE: " + await response.Content.ReadAsStringAsync());
                Console.ResetColor();
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("localhost:12345");
                    builder.WithHeaders("authorization", "content-type", "connection", "host", "X-Requested-With",
                        "Refer", "Origin");
                    builder.WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete,
                        HttpMethods.Options, HttpMethods.Patch);
                    builder.AllowCredentials();
                });
            });

            return serviceCollection;
        }
    }
}