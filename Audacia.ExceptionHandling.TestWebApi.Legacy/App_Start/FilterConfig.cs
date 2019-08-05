using System.Web.Mvc;
using Audacia.ExceptionHandling.AspNetFramework;

namespace Audacia.ExceptionHandling.TestWebApi.Legacy
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new ExceptionFilter
			{
				
			});
		}
	}
}