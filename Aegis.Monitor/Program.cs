using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Pumps;
using Daishi.NewRelic.Insights;

namespace Aegis.Monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            NewRelicInsightsClient.Instance.NewRelicInsightsMetadata.APIKey = "bx97bcRZ0nVQSb80O5GcVtyYREbQLNSz";
            NewRelicInsightsClient.Instance.NewRelicInsightsMetadata.AccountID = "646832";
            NewRelicInsightsClient.Instance.NewRelicInsightsMetadata.URI = new Uri("https://insights-collector.newrelic.com/v1/accounts/646832/events");

            BlackListPump.Instance.RecurringTaskInterval = 1200;
            BlackListPump.Instance.Initialise();

            Console.ReadLine();
        }
    }
}
