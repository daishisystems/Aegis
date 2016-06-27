using System;
using Daishi.NewRelic.Insights;
using Jil;

namespace Aegis.Pumps
{
    /// <summary>
    ///     <see cref="BlackListClientErrorNewRelicInsightsEvent" /> is a template that
    ///     describes an error event that occurs during a
    ///     <see cref="BlackListClient" /> operation.
    /// </summary>
    public class BlackListClientErrorNewRelicInsightsEvent : NewRelicInsightsEvent
    {
        /// <summary>
        ///     <see cref="ApplicationName" /> is the name of the application from which
        ///     the event arose.
        /// </summary>
        [JilDirective(Name = "applicationName")]
        public string ApplicationName => "Aegis Black-list Client";

        /// <summary>
        ///     <see cref="ComponentName" /> is the name of the component from which the
        ///     event rose.
        /// </summary>
        [JilDirective(Name = "componentName")]
        public string ComponentName { get; set; }

        /// <summary>
        ///     <see cref="ErrorMessage" /> is a short message that describes the error
        ///     that occurred.
        /// </summary>
        [JilDirective(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     <see cref="InnerErrorMessage" /> is a short message that describes any
        ///     underlying error that led to <see cref="ErrorMessage" />.
        /// </summary>
        [JilDirective(Name = "innerErrorMessage")]
        public string InnerErrorMessage { get; set; }

        /// <summary>
        ///     <see cref="UnixTimeStamp" /> is the time at which the event occurred,
        ///     expressed in Unix ticks.
        /// </summary>
        [JilDirective(Name = "unixTimeStamp")]
        public int UnixTimeStamp
            => (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        /// <summary>
        ///     <see cref="MachineName" /> is the name of the underlying server, upon which
        ///     the application is running.
        /// </summary>
        [JilDirective(Name = "machineName")]
        public string MachineName => GetMachineName(() => Environment.MachineName);

        [JilDirective(Name = "eventType")]
        public string EventType { get; set; }

        /// <summary>
        ///     <see cref="GetMachineName" /> returns the name of the underlying server,
        ///     upon which the application is running.
        /// </summary>
        /// <param name="getMachineName">
        ///     A function that returns the name of the underlying
        ///     server, upon which the application is running.
        /// </param>
        /// <returns>
        ///     The name of the underlying server, upon which the application is
        ///     running.
        /// </returns>
        public static string GetMachineName(Func<string> getMachineName)
        {
            try
            {
                return getMachineName();
            }
            catch (Exception)
            {
                return "UNKNOWN";
            }
        }
    }
}