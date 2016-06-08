using System;
using System.Collections.Generic;
using System.Net;
using Aegis.Monitor.Core;
using D = System.Data; // System.Data.dll  
using C = System.Data.SqlClient; // System.Data.dll  

namespace Aegis.Monitor.BlackList.Augmentor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var connectionString =
                "Server=tcp:w5d1j5qels.database.windows.net,1433;Database=AegisTrafficDB;User ID=mooney@w5d1j5qels;Password=M3c54n1c4L;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;";

            var commandText = @"SELECT * FROM	(

                SELECT
                    IPAddress,
                    COUNT(IPAddress)AS HyperActivity,
                    SUM(Total) AS TotalNumHits,
                    SUM(Total) / COUNT(IPAddress) AS AVGNumHits,
                    MAX(ServerDateTime) AS LatestServerTime

                FROM dbo.BlackList

                WHERE ServerDateTime >= GETDATE() - 1

                GROUP BY IPAddress  

				            ) AS BlackList

            WHERE HyperActivity > @HYPERACTIVITY
            AND AVGNumHits >= @AVGNUMHITS
            ORDER BY TotalNumHits DESC;";

            using (var connection = new C.SqlConnection(connectionString))
            {
                var command = new C.SqlCommand(commandText, connection);
                command.Parameters.Add("@HYPERACTIVITY", D.SqlDbType.Int);
                command.Parameters["@HYPERACTIVITY"].Value = 1;

                command.Parameters.Add("@AVGNUMHITS", D.SqlDbType.Int);
                command.Parameters["@AVGNUMHITS"].Value = 60;

                try
                {
                    connection.Open();

                    var reader = command.ExecuteReader();

                    var blackListItems = new List<BlackListItem>();

                    while (reader.Read())
                    {
                        blackListItems.Add(new BlackListItem
                        {
                            IPAddress = IPAddress.Parse(reader.GetString(0)),
                            HyperActivity = reader.GetInt32(1),
                            TotalNumHits = reader.GetInt32(2),
                            AvgNumHits = reader.GetInt32(3),
                            LatestServerTime = reader.GetDateTime(4)
                        });
                    }

                    Console.WriteLine(
                        $"Returned {blackListItems.Count} BlackList items");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            }
        }
    }
}