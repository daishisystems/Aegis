using System;

namespace Aegis.Monitor.Clients
{
    /// <summary>
    ///     <see cref="HTTPRequestMetadataException" /> is thrown when
    ///     <see cref="HTTPRequestMetadata" /> is malformed, invalid, partially, or
    ///     entirely omitted.
    /// </summary>
    public class HTTPRequestMetadataException : Exception
    {
        public HTTPRequestMetadataException()
        {

        }

        public HTTPRequestMetadataException(string message) : base(message)
        {

        }

        public HTTPRequestMetadataException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}