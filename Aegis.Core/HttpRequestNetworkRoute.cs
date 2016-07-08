using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;

namespace Aegis.Core
{
    /// <summary>
    ///     <see cref="NetworkRouteMapper" /> retains the full network route; that is,
    ///     the list of <see cref="IPAddress" /> instances, through which a connecting
    ///     client accessed a HTTP resource, including the original client
    ///     <see cref="IPAddress" />, and subsequent proxy <see cref="IPAddress" />
    ///     instances, if applicable, and where disclosed.
    /// </summary>
    public abstract class NetworkRouteMapper
    {
        protected IEnumerable<string> HttpRequestHeaderValues;

        public IEnumerable<IPAddress> ParsedIPAddresses { get; protected set; }

        public IEnumerable<string> UnparsableHttpRequestHeaders { get; protected set; }

        public abstract void GetHttpRequestHeaderValues();

        public abstract void GetIPAddresses();
    }

    public class HttpRequestNetworkRouteMapper : NetworkRouteMapper
    {
        private readonly string _httpRequestHeaderName;
        private readonly HttpRequestHeaders _httpRequestHeaders;

        public HttpRequestNetworkRouteMapper(string httpRequestHeaderName,
            HttpRequestHeaders httpRequestHeaders)
        {
            _httpRequestHeaderName = httpRequestHeaderName;
            _httpRequestHeaders = httpRequestHeaders;
        }

        public override void GetHttpRequestHeaderValues()
        {
            if (string.IsNullOrEmpty(_httpRequestHeaderName) || _httpRequestHeaders == null)
            {
                throw new ArgumentException("HTTP metadata not specified.");
            }

            var canGetHttpRequestHeaderValues = HttpRequestHeaderParser
                .TryGetHttpRequestHeaderValues(_httpRequestHeaderName, _httpRequestHeaders,
                    out HttpRequestHeaderValues);

            if (!canGetHttpRequestHeaderValues)
            {
                throw new NoHttpRequestHeadersFoundException(_httpRequestHeaderName);
            }
        }

        public override void GetIPAddresses()
        {
            if (HttpRequestHeaderValues == null || !HttpRequestHeaderValues.Any())
            {
                throw new NoHttpRequestHeadersFoundException(_httpRequestHeaderName);
            }

            var unparsableHttpRequestHeaders = new List<string>();
            var parsedIpAddresses = new List<IPAddress>();

            foreach (var httpRequestHeaderValue in HttpRequestHeaderValues)
            {
                IEnumerable<IPAddress> ipAddresses;

                var httpRequestHeaderValueContainsIPAddresses =
                    HttpRequestHeaderParser.HttpRequestHeaderValueContainsIPAddress(httpRequestHeaderValue,
                        out ipAddresses);

                if (!httpRequestHeaderValueContainsIPAddresses)
                {
                    unparsableHttpRequestHeaders.Add(httpRequestHeaderValue);
                }
                else
                {
                    parsedIpAddresses.AddRange(ipAddresses);
                }
            }

            UnparsableHttpRequestHeaders = unparsableHttpRequestHeaders;
            ParsedIPAddresses = parsedIpAddresses;
        }
    }

    [Serializable]
    public class NoHttpRequestHeadersFoundException : Exception
    {
        public NoHttpRequestHeadersFoundException(string httpRequestHeaderName)
        {
            HttpRequestHeaderName = httpRequestHeaderName;
        }

        public string HttpRequestHeaderName { get; private set; }
    }

    public class NetworkRoute
    {
        public void Map(NetworkRouteMapper networkRouteMapper)
        {
            networkRouteMapper.GetHttpRequestHeaderValues();
            networkRouteMapper.GetIPAddresses();
        }
    }
}