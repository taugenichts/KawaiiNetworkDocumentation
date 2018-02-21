using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http.Formatting;
using System.Web.Http;
using Kawaii.NetworkDocumentation.AppDataService.Managers;
using Kawaii.NetworkDocumentation.AppDataService.DataModel.Database;
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
            container.RegisterInstance<IDatabaseSession>(new DatabaseSession(
                                                                ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 
                                                                con => new SqlConnection(con)));
            
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

