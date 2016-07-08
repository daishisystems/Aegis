using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Core.Tests
{
    /// <summary>
    ///     <see cref="NetworkRouteMapperTests" /> ensures that logic pertaining to
    ///     <see cref="NetworkRouteMapper" /> instances executes correctly.
    /// </summary>
    [TestClass]
    public class NetworkRouteMapperTests
    {
        /// <summary>
        ///     <see cref="InvalidInputDataThrowsException" /> ensures that an
        ///     <see cref="Exception" /> is thrown if input-data is invalid.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void InvalidInputDataThrowsException()
        {
            var networkRouteMapper = new CitrixNetworkRouteMapper(null, null);
            networkRouteMapper.GetHttpRequestHeaderValues();
        }

        /// <summary>
        ///     <see cref="EmptyHttpRequestHeaderCollectionThrowsException" /> ensures that
        ///     an <see cref="Exception" /> is thrown if no HTTP request headers are found.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (NoHttpRequestHeadersFoundException))]
        public void EmptyHttpRequestHeaderCollectionThrowsException()
        {
            var request = new HttpRequestMessage();

            var networkRouteMapper = new CitrixNetworkRouteMapper("TEST", request.Headers);
            networkRouteMapper.GetHttpRequestHeaderValues();
        }

        // ToDo: Complete 'GetIPAddresses' tests, deploy.
    }
}