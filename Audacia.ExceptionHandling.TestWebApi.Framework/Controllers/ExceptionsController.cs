using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.TestWebApi.Framework.Controllers
{
	public class Person
	{
		public int Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Age { get; set; }

		public DayOfWeek FavouriteDay { get; set; }
	}

	public class TestDbContext : DbContext
	{
		static TestDbContext() => Effort.Provider.EffortProviderConfiguration.RegisterProvider();

		public DbSet<Person> People { get; set; }

		//private static DbConnection GetConnection() => Effort.DbConnectionFactory.CreateTransient();

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Person>().HasIndex(person => person.FavouriteDay).IsUnique();

			base.OnModelCreating(modelBuilder);
		}

		//public TestDbContext() : base(GetConnection(), true) { }
	}

	[RoutePrefix("api/Exceptions")]
	public class ExceptionsController : ApiController
	{
		// GET api/values
		[HttpGet, Route(nameof(JsonReaderException))]
		public object JsonReaderException()
		{
			return JsonConvert.DeserializeObject("<json>this is definitely not JSON.</json>");
		}

		// GET api/values/5
		[HttpGet, Route(nameof(DbUpdateException))]
		public void DbUpdateException()
		{
			var context = new TestDbContext();
			var person = new Person();
			context.People.Add(person);
		}

		// GET api/values/5
		[HttpGet, Route(nameof(DbUpdateException2))]
		public void DbUpdateException2()
		{
			var context = new TestDbContext();
			var person = new Person {FavouriteDay = DayOfWeek.Monday};
			var person2 = new Person {FavouriteDay = DayOfWeek.Monday};
			
//			var cols = ((IObjectContextAdapter) context).ObjectContext
//				.MetadataWorkspace
//				.GetItems(DataSpace.CSpace)
//				//.Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
//				.SelectMany(meta => ((EntityType) meta).DeclaredMembers.Where(x => x.MetadataProperties.First()),//.Where(p => p.DeclaringType.Name == "EntityName"),
//					(meta, p) => new
//					{
//						PropertyName = p.Name,
//						TypeUsageName = p.TypeUsage.EdmType.Name, //type name
//						Documentation = p.Documentation?.LongDescription //if primary key
//					});


			context.People.Add(person);
			context.SaveChanges();

			context.People.Add(person2);
			context.SaveChanges();
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value) { }

		// PUT api/values/5
		[HttpPut]
		public void Put(int id, [FromBody] string value) { }

		// DELETE api/values/5
		[HttpDelete]
		public void Delete(int id) { }
	}
}