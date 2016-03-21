using System;
using Microsoft.ServiceBus.Messaging;

namespace Aegis.Monitor.Logger.Bots
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var eventHubConnectionString =
                "Endpoint=sb://ryanair-bots.servicebus.windows.net/;SharedAccessKeyName=ALL;SharedAccessKey=dfSi0NS7OS3+5RRg6H2jUFAcKe2PugUCLNMWKLAlkYg=";
            var eventHubName = "bots";
            var storageAccountName = "ryanairbots";
            var storageAccountKey =
                "s44unwU9YKC4VcMPu8unmSmQViIkXRJA9kx+hADLHrnBKnZZcmvZDWlYIRjO4Q7MesbZ4Ky20j5kOa5rqcgRaQ==";

            var storageConnectionString =
                string.Format(
                    "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}",
                    storageAccountName, storageAccountKey);

            var eventProcessorHostName = Guid.NewGuid().ToString();
            var eventProcessorHost =
                new EventProcessorHost(eventProcessorHostName, eventHubName,
                    EventHubConsumerGroup.DefaultGroupName,
                    eventHubConnectionString, storageConnectionString);
            Console.WriteLine("Registering EventProcessor...");
            eventProcessorHost
                .RegisterEventProcessorAsync<LoggingEventProcessor>
                ().Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}