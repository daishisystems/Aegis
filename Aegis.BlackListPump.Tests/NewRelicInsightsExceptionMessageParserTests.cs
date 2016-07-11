using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aegis.Monitor.Clients.Tests
{
    /// <summary>
    ///     <see cref="NewRelicInsightsExceptionMessageParserTests" /> ensures that
    ///     logic pertaining to <see cref="NewRelicInsightsExceptionMessageParser" />
    ///     executes correctly.
    /// </summary>
    [TestClass]
    public class NewRelicInsightsExceptionMessageParserTests
    {
        /// <summary>
        ///     <see cref="CustomExceptionMessageIsReturnedWhenSpecified" /> ensures that a
        ///     custom exception-message is returned, when specified.
        /// </summary>
        [TestMethod]
        public void CustomExceptionMessageIsReturnedWhenSpecified()
        {
            var exceptionMessage = NewRelicInsightsExceptionMessageParser.GetExceptionMessage(
                "TEST", new Exception());

            Assert.AreEqual("TEST", exceptionMessage);
        }

        /// <summary>
        ///     <see
        ///         cref="StandardExceptionMessageIsReturnedWhenNoCustomExceptionMessageIsSpecified" />
        ///     ensures that a standard exception-message is returned, when no custom
        ///     exception-message is specified.
        /// </summary>
        [TestMethod]
        public void StandardExceptionMessageIsReturnedWhenNoCustomExceptionMessageIsSpecified()
        {
            var exceptionMessage = NewRelicInsightsExceptionMessageParser.GetExceptionMessage(
                null, new Exception("TEST"));

            Assert.AreEqual("TEST", exceptionMessage);
        }
    }
}