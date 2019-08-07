using System.Configuration;
using System.Net;
using Audacia.ExceptionHandling.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.TestWebApi
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.ConfigureExceptions(e =>
			{
				// Add the default handler for a KeyNotFoundException
				e.Handle.KeyNotFoundException();
				e.Handle.UnauthorizedAccessException();
				
				// Add a custom handler for an exception 
				e.Handle<ConfigurationErrorsException>(exception => new ErrorResult(HttpStatusCode.ServiceUnavailable, "The app is not configured properly."));
			});
			
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}