using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Owin;
using OwinApp.Middleware;

namespace OwinApp
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
            app.Use(typeof(Logger));
            app.Use(typeof(StaticFiles), "Content");
            app.UseCors(CorsOptions.AllowAll);
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "dnbApi",
                routeTemplate: "api/dnb/{resource}",
                defaults: new { controller = "dnb", resource = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(config); 
		}
	}
}
