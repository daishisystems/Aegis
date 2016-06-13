using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Monitor.Clients.Tests
{
    /// <summary>
    ///     <see cref="HTTPRequestMetadataTests" /> ensures that logic pertaining to
    ///     <see cref="HTTPRequestMetadata" /> instances is executed correctly.
    /// </summary>
    [TestClass]
    public class HTTPRequestMetadataTests
    {
        /// <summary>
        ///     <see cref="HTTPRequestMetadataValidatorFailsOnInvalidURI" /> ensures that
        ///     <see cref="HTTPRequestMetadata" /> instances instantiated with invalid
        ///     <see cref="HTTPRequestMetadata.URI" /> properties fail validation.
        /// </summary>
        [TestMethod]
        public void HTTPRequestMetadataValidatorFailsOnInvalidURI()
        {
            var httpRequestMetadata = new HTTPRequestMetadata();

            HTTPRequestMetadataException httpRequestMetadataException;

            var httpRequestMetadataIsValid =
                HTTPRequestMetadataValidator.TryValidate(httpRequestMetadata,
                    out httpRequestMetadataException);

            Assert.IsFalse(httpRequestMetadataIsValid);
        }
    }
}