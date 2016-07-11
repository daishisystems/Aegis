using System;
using Daishi.NewRelic.Insights;
using Jil;

namespace Aegis.Pumps
{
    public class BlackListUpdatedNewRelicInsightsEvent : NewRelicInsightsEvent
    {

        [JilDirective(Name = "country")]
        public string Country { get; set; }

        [JilDirective(Name = "numScrapers")]
        public int NumScrapers { get; set; }

        [JilDirective(Name = "unixTimeStamp")]
        public int UnixTimeStamp
            => (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        [JilDirective(Name = "eventType")]
        public string EventType { get; set; }
    }
}