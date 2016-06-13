using System;
using System.Net;

namespace Aegis.Monitor.Clients
{
    /// <summary>
    ///     <see cref="HTTPRequestMetadata" /> encapsulates peripheral metadata
    ///     pertaining to a HTTP request. It facilitates a degree of flexibility when
    ///     issuing HTTP requests, such as specifying a web proxy, etc.
    /// </summary>
    public class HTTPRequestMetadata
    {
        /// <summary>
        ///     <see cref="URI" /> is the HTTP <see cref="Uri" /> pertaining to the HTTP
        ///     request.
        /// </summary>
        public Uri URI { get; set; }

        /// <summary>
        ///     <see cref="WebProxy" />, if specified, will incorporate a HTTP proxy when
        ///     issuing HTTP requests.
        /// </summary>
        /// <remarks>
        ///     The feature facilitates HTTP connectivity, even when internet
        ///     connectivity is funnelled through a proxy server: e.g, corporate networks.
        /// </remarks>
        public WebProxy WebProxy { get; set; }

        /// <summary>
        ///     <see cref="TimeOutInMilliseconds" /> allows for a non-default HTTP request
        ///     timeout.
        /// </summary>
        /// <remarks>
        ///     This feature is a crumple-zone, ensuring that failed, or slow internet
        ///     connectivity will not create a bottleneck in consuming systems.
        /// </remarks>
        public int TimeOutInMilliseconds { get; set; }
    }
}