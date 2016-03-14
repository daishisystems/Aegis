using System;
using System.Configuration;
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

            bool aegisIsEnabled;

            var settingsAreValid =
                bool.TryParse(
                    ConfigurationManager.AppSettings["AegisIsEnabled"],
                    out aegisIsEnabled);

            if (settingsAreValid && aegisIsEnabled)
            {
                var eventHubName =
                    ConfigurationManager.AppSettings["AegisEventHubName"];
                var eventHubConnectionString =
                    ConfigurationManager.AppSettings[
                        "AegisEventHubConnectionString"];

                try
                {
                    // Establish connection to the Azure Event Hub
                    AegisEventPublisher.Instance.Initialise(eventHubName,
                        eventHubConnectionString);

                    // Start the recurring monitor task
                    TaskManager.Initialize(new AegisRegistry());
                }
                catch (Exception)
                {
                    // Fail silently and ignore errors for POC
                }
            }

            #endregion
        }
    }
}