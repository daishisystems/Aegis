using System.Collections.Generic;
using System.Net;

namespace Aegis.Core
{
    /// <summary>
    ///     <see cref="HttpRequestNetworkRoute" /> retains the full network route; that
    ///     is, the list of <see cref="IPAddress" /> instances, through which a
    ///     connecting client accessed a HTTP resource, including the original client
    ///     <see cref="IPAddress" />, and subsequent proxy <see cref="IPAddress" />
    ///     instances, if applicable, and where disclosed.
    /// </summary>
    public class HttpRequestNetworkRoute
    {
        private List<IPAddress> _networkRoute = new List<IPAddress>();

        public IEnumerable<IPAddress> NetworkRoute => _networkRoute;

    }
}