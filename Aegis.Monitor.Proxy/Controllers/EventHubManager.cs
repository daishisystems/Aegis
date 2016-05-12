using System;
using Microsoft.ServiceBus.Messaging;

namespace Aegis.Monitor.Proxy.Controllers
{
    /// <summary>
    ///     EventHubManager is a Singleton that ensures that only 1 instance of
    ///     <see cref="EventHubClient" /> is leveraged throughout the application,
    ///     across all sessions. This in turn ensures that the same underlying TCP and
    ///     AMQP components are referenced across all running threads. The point is to
    ///     minimise the number of open connections, and to reuse any existing
    ///     connections insofar as possible.
    /// </summary>
    internal sealed class EventHubManager
    {
        private static readonly Lazy<EventHubManager> Lazy =
            new Lazy<EventHubManager>(() => new EventHubManager());

        public EventHubClient EventHubClient;

        private EventHubManager()
        {

        }

        public static EventHubManager Instance => Lazy.Value;
    }
}