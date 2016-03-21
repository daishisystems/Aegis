using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Aegis.Monitor.Core;
using Microsoft.ServiceBus.Messaging;
using Nest;
using Newtonsoft.Json;

namespace Aegis.Monitor.Logger.Bots
{
    internal class LoggingEventProcessor : IEventProcessor
    {
        private readonly ConcurrentDictionary<string, string> _blacklist =
            new ConcurrentDictionary<string, string>();

        private readonly ElasticClient _client;

        private Stopwatch checkpointStopWatch;

        public LoggingEventProcessor()
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);

            _client = new ElasticClient(settings);
        }

        async Task IEventProcessor.CloseAsync(PartitionContext context,
            CloseReason reason)
        {
            Console.WriteLine(
                "Processor Shutting Down. Partition '{0}', Reason: '{1}'.",
                context.Lease.PartitionId, reason);
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            Console.WriteLine(
                "SimpleEventProcessor initialized.  Partition: '{0}', Offset: '{1}'",
                context.Lease.PartitionId, context.Lease.Offset);
            checkpointStopWatch = new Stopwatch();
            checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context,
            IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                try
                {
                    Console.Clear();

                    var data = Encoding.UTF8.GetString(eventData.GetBytes());
                    var aegisEvent =
                        JsonConvert.DeserializeObject<AegisEvent>(data);

                    // Does event exist?
                    var getR = _client.Get<AegisEvent>(aegisEvent.IPAddress,
                        g => g
                            .Index("traffic")
                            .Type("bad"));

                    if (!getR.Found)
                    {
                        // Add if not found
                        _client.Index(aegisEvent, i => i
                            .Index("traffic")
                            .Type("bad")
                            .Id(aegisEvent.IPAddress)
                            .Refresh()
                            );

                        Console.WriteLine("Added new event.");
                    }

                    // Was IP previously flagged as human?
                    getR = _client.Get<AegisEvent>(aegisEvent.IPAddress,
                        g => g
                            .Index("traffic")
                            .Type("good"));

                    if (getR.Found)
                    {
                        // Delete if found
                        _client.Delete<AegisEvent>(
                            aegisEvent.IPAddress, d => d
                                .Index("traffic")
                                .Type("good")
                            );
                        
                        Console.WriteLine("Flagged event as bot.");
                    }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                    Console.WriteLine("-----");
                }
            }

            //Call checkpoint every 5 minutes, so that worker can resume processing from 5 minutes back if it restarts.
            if (checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                checkpointStopWatch.Restart();
            }
        }
    }
}