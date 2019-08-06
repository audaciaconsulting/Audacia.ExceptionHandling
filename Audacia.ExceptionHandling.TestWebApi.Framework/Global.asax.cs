using System.Web.Http;
using Audacia.ExceptionHandling.AspNetFramework;
using Audacia.ExceptionHandling.Json;

namespace Audacia.ExceptionHandling.TestWebApi.Framework
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.ConfigureExceptions(e =>
            {
                e.Handle.JsonReaderException();
                e.Handle.KeyNotFoundException();
                //e.Handle.ValidationException();
            });
        }
    }
}
