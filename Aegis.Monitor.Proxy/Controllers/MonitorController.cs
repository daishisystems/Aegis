using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Http;
using Aegis.Monitor.Core;
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
                JsonConvert.DeserializeObject<IEnumerable<AegisEvent>>(value);

            var eventHubName =
                ConfigurationManager.AppSettings["AegisEventHubName"];
            var connectionString =
                ConfigurationManager.AppSettings["AegisEventHubConnectionString"
                    ];

            var batch =
                events.Select(
                    e =>
                        new EventData(
                            Encoding.UTF8.GetBytes(
                                JsonConvert.SerializeObject(e)))).ToList();

            var eventHubClient =
                EventHubClient.CreateFromConnectionString(connectionString,
                    eventHubName);

            eventHubClient.SendBatch(batch);
            eventHubClient.Close();
        }
    }
}