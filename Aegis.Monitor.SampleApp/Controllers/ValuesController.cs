using System;
using System.Configuration;
using System.Net;
using System.Web.Http;
using Aegis.Monitor.Core;

namespace Aegis.Monitor.SampleApp.Controllers
{
    [RoutePrefix("api/samples")]
    public class ValuesController : ApiController
    {
        [Route("sample/{ipaddress}")]
        [AcceptVerbs("GET")]
        public string Get(string ipAddress)
        {
            bool aegisIsEnabled;

            var settingsAreValid =
                bool.TryParse(
                    ConfigurationManager.AppSettings["AegisIsEnabled"],
                    out aegisIsEnabled);

            IPAddress validator;

            var ipAddressIsValid =
                IPAddress.TryParse(ipAddress.Replace('-', '.'), out validator);

            if (settingsAreValid && aegisIsEnabled && ipAddressIsValid)
            {
                AegisEventCache.Add(new AegisEvent
                {
                    IPAddress = validator.ToString(),
                    Path = Request.RequestUri.AbsolutePath,
                    Time = DateTime.UtcNow.ToString("O")
                });
            }

            return validator.ToString();
        }
    }
}