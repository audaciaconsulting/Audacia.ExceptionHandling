using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Audacia.ExceptionHandling.TestWebApi
{
	public class Person
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Age { get; set; }

		public DayOfWeek FavouriteDay { get; set; }

		public int DadId { get; set; }
		
		public Person Dad { get; set; }

		public int MumId { get; set; }
		
		public Person Mum { get; set; } 
	}
	
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}