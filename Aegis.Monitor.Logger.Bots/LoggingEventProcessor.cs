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

        private ElasticClient _client;

        private Stopwatch checkpointStopWatch;

        public LoggingEventProcessor()
        {
            var node = new Uri("http://localhost:9200");
            _client = new ElasticClient();
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
                var data = Encoding.UTF8.GetString(eventData.GetBytes());

                //Console.WriteLine(
                //    "Message received.  Partition: '{0}', Data: '{1}'",
                //    context.Lease.PartitionId, data);

                var metadata = JsonConvert.DeserializeObject<AegisEvent>(data);

                _blacklist.GetOrAdd(metadata.IPAddress,
                    key =>
                    {
                        // todo: pump to ElasticSearch here...
                        return "BOT";
                    });

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Blacklist:");

                foreach (var pair in _blacklist)
                {
                    Console.WriteLine(pair.Key);
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