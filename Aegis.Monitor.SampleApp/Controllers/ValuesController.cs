using System;
using System.Configuration;
using System.Web.Http;
using Aegis.Monitor.Core;

namespace Aegis.Monitor.SampleApp.Controllers
{
    [RoutePrefix("api/samples")]
    public class ValuesController : ApiController
    {
        [Route("sample/{ipaddress}")]
        [AcceptVerbs("GET")]
        public bool Get(string ipAddress)
        {
            bool aegisIsEnabled;

            var settingsAreValid =
                bool.TryParse(
                    ConfigurationManager.AppSettings["AegisIsEnabled"],
                    out aegisIsEnabled);

            if (settingsAreValid && aegisIsEnabled)
            {
                AegisEventCache.Add(new AegisEvent
                {
                    IPAddress = ipAddress.Replace('-', '.'),
                    Path = Request.RequestUri.AbsolutePath,
                    Time = DateTime.UtcNow.ToString("O")
                });
            }

            return aegisIsEnabled;
        }
    }
}