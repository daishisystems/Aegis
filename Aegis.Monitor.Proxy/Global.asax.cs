using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Aegis.Monitor.Core;
using FluentScheduler;

namespace Aegis.Monitor.Proxy
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

            #region New Relic

            var newRelicInsightsIsEnabled =
                AegisHelper.IsEnabledInConfigFile("NewRelicInsightsIsEnabled");

            if (!newRelicInsightsIsEnabled) return;

            try
            {
                // Start the recurring monitor task
                TaskManager.Initialize(new NewRelicInsightsRegistry());
            }
            catch (Exception)
            {
                // Fail silently and ignore errors until a fallback solution exists.
            }

            #endregion
        }
    }
}