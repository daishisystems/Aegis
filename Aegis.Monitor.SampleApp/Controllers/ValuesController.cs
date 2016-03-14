using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Aegis.Monitor.Core;

namespace Aegis.Monitor.SampleApp.Controllers
{
    [RoutePrefix("api/samples")]
    public class ValuesController : ApiController
    {
        [Route("sample")]
        [AcceptVerbs("GET")]
        public OkResult Get()
        {
            var aegisIsEnabled =
                AegisHelper.IsEnabledInConfigFile("AegisIsEnabled");

            if (!aegisIsEnabled) return Ok();

            IPAddress ipAddress;

            var ipAddressIsValid =
                AegisHelper.TryParseIPAddressFromHeader("NS_CLIENT_IP",
                    Request.Headers, out ipAddress);


            if (ipAddressIsValid)
            {
                AegisEventCache.Add(new AegisEvent
                {
                    IPAddress = ipAddress.ToString(),
                    Path = Request.RequestUri.AbsolutePath,
                    Time = DateTime.UtcNow.ToString("O")
                });
            }

            return Ok();
        }
    }
}