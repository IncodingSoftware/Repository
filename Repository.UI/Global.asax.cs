﻿namespace Repository.UI
{
    #region << Using >>

    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Incoding.Block.Logging;
    using Repository.Domain;
    using Repository.UI.Controllers;

    #endregion

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            Bootstrapper.Start();
            new DispatcherController(); // init routes 

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            LoggingFactory.Instance.LogException(LogType.Debug,ex);
        }
        
    }
}