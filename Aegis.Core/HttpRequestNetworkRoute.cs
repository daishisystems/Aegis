using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Aegis.Core
{
    /// <summary>
    ///     <see cref="NetworkRoute" /> retains the full network route; that is, the
    ///     list of <see cref="IPAddress" /> instances, through which a connecting
    ///     client accessed a HTTP resource, including the original client
    ///     <see cref="IPAddress" />, and subsequent proxy <see cref="IPAddress" />
    ///     instances, if applicable, and where disclosed.
    /// </summary>
    public abstract class NetworkRoute
    {
        protected readonly List<string> UnparsableIPAddressMetadata = new List<string>();
        protected IEnumerable<string> IPAddressMetadata;

        public List<IPAddress> IPAddresses { get; } = new List<IPAddress>();

        public abstract void GetIPAddressMetadata();

        public abstract void ParseIPAddresses();
    }

    public class HttpRequestNetworkRoute : NetworkRoute
    {
        private readonly string _httpRequestHeaderName;
        private readonly HttpRequestHeaders _httpRequestHeaders;

        public HttpRequestNetworkRoute(string httpRequestHeaderName, HttpRequestHeaders httpRequestHeaders)
        {
            _httpRequestHeaderName = httpRequestHeaderName;
            _httpRequestHeaders = httpRequestHeaders;

        }

        public override void GetIPAddressMetadata()
        {
            if (string.IsNullOrEmpty(_httpRequestHeaderName) || _httpRequestHeaders == null)
            {
                throw new ArgumentException("HTTP metadata not specified.");
            }

            var canGetHttpRequestHeaderValues = IPAddressHttpParser
                .TryGetHttpRequestHeaderValues(_httpRequestHeaderName, _httpRequestHeaders,
                    out IPAddressMetadata);

            if (!canGetHttpRequestHeaderValues)
            {
                throw new Exception("Unable to retrieve any '" + _httpRequestHeaderName + "' HTTP headers");
            }
        }

        public override void ParseIPAddresses()
        {
            if (IPAddressMetadata == null || !IPAddressMetadata.Any())
            {
                throw new ArgumentException("No IP address metadata to parse.");
            }

            foreach (var metadata in IPAddressMetadata)
            {
                IEnumerable<IPAddress> ipAddresses;

                var ipAddressMetadataContainsIPAddresses =
                    IPAddressHttpParser.HttpRequestHeaderValueContainsIPAddress(metadata, out ipAddresses);

                if (!ipAddressMetadataContainsIPAddresses)
                {
                    UnparsableIPAddressMetadata.Add(metadata);
                }
                else
                {
                    IPAddresses.AddRange(ipAddresses);
                }
            }
        }
    }
}