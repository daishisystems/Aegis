using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Http;
using Aegis.Monitor.Core;
using Newtonsoft.Json;
using D = System.Data; // System.Data.dll  
using C = System.Data.SqlClient; // System.Data.dll  

namespace Aegis.Monitor.Filter.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public List<IPAddressGeoLocation> Get()
        {
            using (var connection = new C.SqlConnection(
                "Server=tcp:w5d1j5qels.database.windows.net,1433;Database=AegisTrafficDB;User ID=mooney@w5d1j5qels;Password=M3c54n1c4L;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;"
                ))
            {
                connection.Open();

                return SelectRows(connection);
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public List<IPAddressGeoLocation> SelectRows(C.SqlConnection connection)
        {
            using (var command = new C.SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = D.CommandType.Text;
                command.CommandText = @"  
                
                    SELECT * FROM	(

	                    SELECT 
		                    IPAddress, 
		                    COUNT(IPAddress) AS HyperActivity, 
		                    SUM(Total) AS TotalNumHits, 
		                    SUM(Total)/COUNT(IPAddress) AS AVGNumHits
	                    FROM dbo.StreamingAnalyticsOutput
	                    WHERE ServerDateTime >= GETDATE() - 1
	                    GROUP BY IPAddress

				                    ) AS BlackList

                    WHERE HyperActivity > 1
                    AND AVGNumHits >= 60
                    ORDER BY TotalNumHits DESC;                    

                ";

                var reader = command.ExecuteReader();

                var ipAddressGeoLocations = new List<IPAddressGeoLocation>();

                while (reader.Read())
                {
                    var ipAddress = reader.GetString(0);

                    try
                    {
                        var ipAddressIsPrivate =
                            IPAddress.Parse(ipAddress).IsPrivate();

                        if (ipAddressIsPrivate)
                        {
                            continue;
                        }

                        var request = WebRequest.Create(
                            "http://freegeoip.net/json/" + ipAddress);

                        var response = request.GetResponse();
                        var dataStream = response.GetResponseStream();

                        var r = new StreamReader(dataStream);
                        var responseFromServer = r.ReadToEnd();

                        var location =
                            JsonConvert.DeserializeObject<IPAddressGeoLocation>(
                                responseFromServer);

                        r.Close();
                        response.Close();

                        var countryIdentifier =
                            location.CountryName.ToLowerInvariant().Trim();

                        if (countryIdentifier.Equals("ireland") ||
                            countryIdentifier.Equals("ie") ||
                            countryIdentifier.Equals("ire"))
                        {
                            ipAddressGeoLocations.Add(location);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }

                return ipAddressGeoLocations;
            }
        }
    }
}