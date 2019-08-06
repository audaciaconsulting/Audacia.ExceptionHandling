using System.IO;
using System.Reflection;
using Audacia.ExceptionHandling.Annotations;
using Audacia.ExceptionHandling.AspNetCore;
using Audacia.ExceptionHandling.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
				e.Handle.JsonReaderException();
				e.Handle.KeyNotFoundException();
				e.Handle.ValidationException();
			});
			
			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}