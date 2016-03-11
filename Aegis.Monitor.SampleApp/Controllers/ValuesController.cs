using System;
using System.Collections.Generic;
using System.Web.Http;
using Aegis.Monitor.Core;

namespace Aegis.Monitor.SampleApp.Controllers
{
    [RoutePrefix("api/samples")]
    public class ValuesController : ApiController
    {
        [Route("sample/{ipaddress}")]
        [AcceptVerbs("GET")]
        public IEnumerable<string> Get(string ipAddress)
        {
            AegisEventCache.Add(new AegisEvent
            {
                IPAddress = ipAddress.Replace('-', '.'),
                Path = Request.RequestUri.AbsolutePath,
                Time = DateTime.UtcNow.ToString("O")
            });

            return new[] {ipAddress};
        }
    }
}