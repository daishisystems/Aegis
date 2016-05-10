using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Aegis.Monitor.Core;
using FluentScheduler;

namespace Aegis.Monitor.SampleApp
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region Aegis

            var aegisIsEnabled =
                AegisHelper.IsEnabledInConfigFile("AegisIsEnabled");

            if (!aegisIsEnabled) return;

            try
            {
                // Start the recurring monitor task
                TaskManager.Initialize(new AegisRegistry());
            }
            catch (Exception)
            {
                // Fail silently and ignore errors for POC
            }

            #endregion
        }
    }
}