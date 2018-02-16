using System.Web.Http;
using System.Net.Http.Formatting;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Unity;
using Unity.Lifetime;

namespace Kawaii.NetworkDocumentation.AppDataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //configure dependency injection
            var container = new UnityContainer();
            container.RegisterType<IPersonManager, PersonManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompanyManager, CompanyManager>(new HierarchicalLifetimeManager());
            //container.RegisterType<System.Data.IDbConnection, System.Data.sql>
            config.DependencyResolver = new UnityDependencyResolver(container);

            // enforce json response through content negotiation
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(config.Formatters.JsonFormatter));

            // default route mapping
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

