using System;
using Aegis.Monitor.Core;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Nest;
using Newtonsoft.Json;

namespace Aegis.Monitor.Logger.Humans
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create the topic if it does not exist already.
            var connectionString =
                CloudConfigurationManager.GetSetting(
                    "Microsoft.ServiceBus.ConnectionString");

            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.TopicExists("humanq"))
            {
                namespaceManager.CreateTopic("humanq");
            }

            if (!namespaceManager.SubscriptionExists("humanq", "AllMessages"))
            {
                namespaceManager.CreateSubscription("humanq", "AllMessages");
            }

            var Client =
                SubscriptionClient.CreateFromConnectionString
                    (connectionString, "humanq", "AllMessages",
                        ReceiveMode.ReceiveAndDelete);

            // Configure the callback options.
            var options = new OnMessageOptions();
            options.AutoComplete = false;
            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);

            var client = new ElasticClient(settings);

            Client.OnMessage(message =>
            {
                try
                {
                    var parsed = message.GetBody<string>();
                    var aegisResult =
                        JsonConvert.DeserializeObject<AegisResult>(parsed);

                    // Does event exist?
                    var getR = client.Get<AegisResult>(aegisResult.IPAddress,
                        g => g
                            .Index("aegis")
                            .Type("good"));

                    if (!getR.Found)
                    {
                        // Was IP previously flagged as bot?
                        getR = client.Get<AegisResult>(aegisResult.IPAddress,
                            g => g
                                .Index("aegis")
                                .Type("bad"));

                        if (!getR.Found)
                        {
                            // Add if not found
                            client.Index(aegisResult, i => i
                                .Index("aegis")
                                .Type("good")
                                .Id(aegisResult.IPAddress)
                                .Refresh()
                                );

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("A person connected at {0}",
                                DateTime.UtcNow.ToString("T"));
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }, options);

            Console.ReadLine();
        }
    }
}