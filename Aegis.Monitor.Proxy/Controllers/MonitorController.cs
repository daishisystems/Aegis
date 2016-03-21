using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Aegis.Monitor.Proxy.Controllers
{
    public class MonitorController : ApiController
    {


        // POST: api/Monitor
        public void Post([FromBody] string value)
        {
            var events =
                JsonConvert.DeserializeObject<IEnumerable<EventData>>(value);

            var eventHubName =
                ConfigurationManager.AppSettings["AegisEventHubName"];
            var connectionString =
                ConfigurationManager.AppSettings["AegisEventHubConnectionString"
                    ];

            var eventHubClient =
                EventHubClient.CreateFromConnectionString(connectionString,
                    eventHubName);

            eventHubClient.SendBatch(events);
            eventHubClient.Close();
        }
    }
}