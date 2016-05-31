using System;
using System.IO;
using System.Net;
using System.Threading;
using Aegis.Monitor.Core;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Nest;
using Newtonsoft.Json;

namespace Aegis.Monitor.Logger.Bots
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

            if (!namespaceManager.TopicExists("botq"))
            {
                namespaceManager.CreateTopic("botq");
            }

            if (!namespaceManager.SubscriptionExists("botq", "AllMessages"))
            {
                namespaceManager.CreateSubscription("botq", "AllMessages");
            }

            var Client =
                SubscriptionClient.CreateFromConnectionString
                    (connectionString, "botq", "AllMessages",
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

                    try
                    {
                        var request = WebRequest.Create(
                            "http://ip-api.com/json/" + aegisResult.IPAddress);

                        var response = request.GetResponse();
                        var dataStream = response.GetResponseStream();

                        var reader = new StreamReader(dataStream);
                        var responseFromServer = reader.ReadToEnd();

                        var location =
                            JsonConvert.DeserializeObject<Location>(
                                responseFromServer);

                        reader.Close();
                        response.Close();

                        if (location.CountryName.ToLowerInvariant() ==
                            "united kingdom")
                        {
                            location.CountryName = "UK";
                        }

                        if (location.CountryName.ToLowerInvariant() ==
                            "united states")
                        {
                            location.CountryName = "USA";
                        }

                        aegisResult.City = location.City;
                        aegisResult.Country = location.CountryName;


                        Thread.Sleep(1000);
                    }
                    catch (Exception)
                    {
                        // Do nothing
                    }

                    // Does event exist?
                    var getR = client.Get<AegisResult>(aegisResult.IPAddress,
                        g => g
                            .Index("aegis")
                            .Type("bad"));

                    if (!getR.Found)
                    {
                        // Add if not found
                        client.Index(aegisResult, i => i
                            .Index("aegis")
                            .Type("bad")
                            .Id(aegisResult.IPAddress)
                            .Refresh()
                            );

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("A scraper connected at {0}",
                            DateTime.UtcNow.ToString("T"));
                    }

                    // Was IP previously flagged as human?
                    getR = client.Get<AegisResult>(aegisResult.IPAddress,
                        g => g
                            .Index("aegis")
                            .Type("good"));

                    if (getR.Found)
                    {
                        // Delete if found
                        client.Delete<AegisResult>(
                            aegisResult.IPAddress, d => d
                                .Index("aegis")
                                .Type("good")
                            );
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