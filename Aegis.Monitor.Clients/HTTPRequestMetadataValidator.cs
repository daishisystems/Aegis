namespace Aegis.Monitor.Clients
{
    /// <summary>
    ///     <see cref="HTTPRequestMetadataValidator" /> validates and instance of
    ///     <see cref="HTTPRequestMetadata" />, ensuring that relevant properties are
    ///     instantiated correctly.
    /// </summary>
    public class HTTPRequestMetadataValidator
    {
        /// <summary>
        ///     <see cref="TryValidate" /> ensures that <see cref="httpRequestMetadata" />
        ///     is instantiated correctly. If any <see cref="HTTPRequestMetadata" />
        ///     properties are not instantiated correctly, the method returns <c>false</c>,
        ///     and outputs a <see cref="HTTPRequestMetadataException" />.
        /// </summary>
        /// <param name="httpRequestMetadata">
        ///     The <see cref="HTTPRequestMetadata" />
        ///     instance to validate.
        /// </param>
        /// <param name="httpRequestMetadataException">
        ///     A
        ///     <see cref="HTTPRequestMetadataException" />, returned if any
        ///     <see cref="HTTPRequestMetadata" /> properties are not instantiated
        ///     correctly
        /// </param>
        /// <returns>
        ///     <c>True</c> if all <see cref="httpRequestMetadata" /> properties are
        ///     instantiated correctly.
        /// </returns>
        public static bool TryValidate(HTTPRequestMetadata httpRequestMetadata,
            out HTTPRequestMetadataException httpRequestMetadataException)
        {
            httpRequestMetadataException = null;

            if (httpRequestMetadata == null)
            {
                httpRequestMetadataException =
                    new HTTPRequestMetadataException("No HTTP request metadata specified.");

                return false;
            }

            if (httpRequestMetadata.URI == null)
            {
                httpRequestMetadataException = new HTTPRequestMetadataException("No URI specified.");

                return false;
            }

            return true;
        }
    }
}