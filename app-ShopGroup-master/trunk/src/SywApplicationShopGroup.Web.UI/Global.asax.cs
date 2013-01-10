using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Platform.Client;
using SywApplicationShopGroup.Web.UI.Filters;
using SywApplicationShopGroup.Web.UI.Plumbing;

namespace SywApplicationShopGroup.Web.UI
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
        private static IWindsorContainer Container;

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new TokenExtractingFilter());
            filters.Add(new GroupIdExtractorFilter());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			// Post login is special. In an commercial application you'll need to use attributes and reflections to avoid this. For this example, this will be fine for now 
			routes.MapRoute(
				"PostLogin",
				"post-login",
				new { controller = "PostLogin", action = "Index" }
			);
            routes.MapRoute(
                "JoinGroup",
                "JoinGroup",
                new { controller = "JoinSg", action = "JoinGroup" }
            );

            routes.MapRoute(
                "CreateShopGroupAction",
                "CreateShopGroupAction",
                new { controller = "Product", action = "CreateShopGroupAction" }
            );

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}
       

		protected void Application_Start()
		{
    	    AreaRegistration.RegisterAllAreas();
			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);
            BootstrapContainer();
   
		}

        private static void BootstrapContainer()
        {
            Container = new WindsorContainer()
                .Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(Container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
        protected void Application_End()
        {
            Container.Dispose();
        }
    }

}