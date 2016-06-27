using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Aegis.Core;
using FluentScheduler;

namespace Aegis.Inlet
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
                AegisHelper.IsEnabledInConfigFile(
                    "AegisNewRelicInsightsIsEnabled");

            if (!newRelicInsightsIsEnabled) return;

            try
            {
                // Start the recurring monitor task
                JobManager.Initialize(new NewRelicInsightsRegistry());
            }
            catch (Exception)
            {
                // Fail silently and ignore errors until a fall-back solution exists.
            }

            #endregion
        }
    }
}