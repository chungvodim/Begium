using Begium.Dependencies;
using Begium.Installers;
using Begium.Web.App_Start;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Begium.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //GlobalFilters.Filters.Add(new CustomActionFilterAttribute());
            BootstrapContainer();
        }
        protected void Application_End()
        {
            _container.Dispose();
        }

        private static void BootstrapContainer()
        {
            _container = new WindsorContainer()
                .Install(new ControllerInstaller(),
                    new RepositoriesIntaller(),
                    new ServiceInstaller());
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }

        // Temporary
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = Server.GetLastError();
            //Server.ClearError();
            //try
            //{
            //    HttpException httpException = (HttpException)exception;
            //    int httpCode = httpException.GetHttpCode();
            //    switch (httpCode)
            //    {
            //        case 404: Response.Redirect("/Errors/NotFound"); break;
            //        default: Response.Redirect("/Errors/Index"); break;
            //    }
            //}
            //catch
            //{
            //    Response.Redirect("/Errors/Index");
            //}
        }
    }
}
