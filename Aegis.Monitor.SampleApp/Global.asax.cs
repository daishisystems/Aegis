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

            const string eventHubName = "samplepumper";
            const string eventHubConnectionString =
                "Endpoint=sb://ryanair-samplepumper.servicebus.windows.net/;SharedAccessKeyName=ALL;SharedAccessKey=ptn2jAbyjKdKEdNBoJELBzEMR84+qFo60YhQ4LvIr2I=";

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

            #endregion
        }
    }
}