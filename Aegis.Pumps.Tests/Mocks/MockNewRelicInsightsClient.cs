using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Daishi.NewRelic.Insights;

namespace Aegis.Pumps.Tests.Mocks
{
    public class MockNewRelicInsightsClient : INewRelicInsightsClient
    {
        public readonly List<NewRelicInsightsEvent> UploadNewRelicInsightsEvents = new List<NewRelicInsightsEvent>();

        public MockNewRelicInsightsClient()
        {
            this.NewRelicInsightsEvents = new ConcurrentQueue<NewRelicInsightsEvent>();
        }

        public void AddNewRelicInsightEvent(NewRelicInsightsEvent newRelicInsightsEvent)
        {
            this.NewRelicInsightsEvents.Enqueue(newRelicInsightsEvent);
            this.UploadNewRelicInsightsEvents.Add(newRelicInsightsEvent);
        }

        public void Initialise()
        {
        }

        public void ShutDown()
        {
            NewRelicInsightsEvent item;
            while (this.NewRelicInsightsEvents.TryDequeue(out item))
            {
            }
        }

        public void UploadEvents(IEnumerable<NewRelicInsightsEvent> newRelicInsightsEvents, HttpClientFactory httpClientFactory)
        {
            this.UploadNewRelicInsightsEvents.AddRange(newRelicInsightsEvents);
        }

        public async Task UploadEventsAsync(IEnumerable<NewRelicInsightsEvent> newRelicInsightsEvents, HttpClientFactory httpClientFactory)
        {
            this.UploadNewRelicInsightsEvents.AddRange(newRelicInsightsEvents);
            await Task.Yield();
        }

        public void AddNewRelicInsightsEventsUploadException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public ConcurrentQueue<NewRelicInsightsEvent> NewRelicInsightsEvents { get; private set; }

        public bool HasStarted => true;

        public string RecurringTaskName { get; set; }

        public int RecurringTaskInterval { get; set; }

        public int CacheUploadLimit { get; set; }

        public NewRelicInsightsMetadata NewRelicInsightsMetadata { get; }

        public event NewRelicInsightsClient.NewRelicInsightsEventsUploadEventHandler NewRelicInsightsEventsUploadException;
    }
}
