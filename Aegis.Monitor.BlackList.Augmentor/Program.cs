using System;
using System.IO;
using System.Net;
using Aegis.Monitor.Core;
using Newtonsoft.Json;
using D = System.Data; // System.Data.dll  
using C = System.Data.SqlClient; // System.Data.dll  

namespace Aegis.Monitor.BlackList.Augmentor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var connection = new C.SqlConnection(
                "Server=tcp:w5d1j5qels.database.windows.net,1433;Database=AegisTrafficDB;User ID=mooney@w5d1j5qels;Password=M3c54n1c4L;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
                ))
            {
                connection.Open();
                Console.WriteLine("Connected successfully.");

                SelectRows(connection);

                Console.WriteLine("Press any key to finish...");
                Console.ReadKey(true);
            }
        }

        public static void SelectRows(C.SqlConnection connection)
        {
            using (var command = new C.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = D.CommandType.Text;
                command.CommandText = @"  
                
                SELECT  
                IPAddress
                FROM  
                dbo.BlackList

                ";

                var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    var ipAddress = reader.GetString(0);

                    try
                    {
                        var request = WebRequest.Create(
                            "http://freegeoip.net/json/" + ipAddress);

                        var response = request.GetResponse();
                        var dataStream = response.GetResponseStream();

                        var r = new StreamReader(dataStream);
                        var responseFromServer = r.ReadToEnd();

                        var location =
                            JsonConvert.DeserializeObject<Location>(
                                responseFromServer);

                        r.Close();
                        response.Close();

                        if (
                            location.CountryName.ToLowerInvariant()
                                .Trim()
                                .Equals("ireland"))
                        {
                            Console.WriteLine("{0} is Irish", ipAddress);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}