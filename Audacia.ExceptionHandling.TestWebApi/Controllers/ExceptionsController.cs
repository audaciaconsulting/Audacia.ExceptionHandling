using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Audacia.ExceptionHandling.TestWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExceptionsController : ControllerBase
	{
		// GET api/values
		[HttpGet, Route(nameof(JsonReaderException))]
		public object JsonReaderException()
		{
			return JsonConvert.DeserializeObject("<json>this is definitely not JSON.</json>");
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value) { }

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value) { }

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id) { }
	}
}