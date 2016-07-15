using System;
using Daishi.NewRelic.Insights;
using Jil;

namespace Aegis.Monitor.Filter
{
    public class ExceptionNewRelicEvent : NewRelicInsightsEvent
    {
        private string _eventType;

        /// <summary>
        ///     <see cref="ErrorMessage" /> is the friendly message pertaining to the
        ///     underlying <see cref="Exception" />.
        /// </summary>
        [JilDirective(Name = "errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     <see cref="InnerErrorMessage" /> is the friendly message pertaining to the
        ///     underlying <see cref="Exception.InnerException" />, if applicable.
        /// </summary>
        [JilDirective(Name = "innerErrorMessage")]
        public string InnerErrorMessage { get; set; }

        /// <summary>
        ///     <see cref="ApplicationName" /> is the friendly name of the application in
        ///     which the error occurred.
        /// </summary>
        [JilDirective(Name = "applicationName")]
        public string ApplicationName { get; set; }

        /// <summary>
        ///     <see cref="ComponentName" /> is the friendly name of the component, within
        ///     the application, in which the error occurred.
        /// </summary>
        [JilDirective(Name = "componentName")]
        public string ComponentName { get; set; }

        /// <summary>
        ///     <see cref="UnixTimeStamp" /> is the UTC time at which the error occurred,
        ///     expressed in Unix ticks.
        /// </summary>
        [JilDirective(Name = "unixTimeStamp")]
        public int UnixTimeStamp
            => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        /// <summary>
        ///     <see cref="EventType" /> is the New Relic Insights to which this
        ///     <see cref="AegisErrorNewRelicInsightsEvent" /> will be uploaded.
        /// </summary>
        /// <remarks>Defaults to 'AegisErrors', unless otherwise specified.</remarks>
        [JilDirective(Name = "eventType")]
        public string EventType
        {
            get { return string.IsNullOrEmpty(_eventType) ? "FilterErrors" : _eventType; }
            set { _eventType = value; }
        }
    }
}