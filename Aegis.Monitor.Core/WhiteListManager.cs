using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;

namespace Aegis.Monitor.Core
{
    /// <summary>
    ///     <see cref="WhiteListManager" /> contains methods to load, manage, and
    ///     interrogate <see cref="WhiteListItem" /> metadata.
    /// </summary>
    public static class WhiteListManager
    {
        /// <summary>
        ///     <see cref="LoadWhiteListItemsFromAzure" /> returns a collection of
        ///     <see cref="WhiteListItem" /> instances.
        /// </summary>
        /// <param name="connectionString">The SQL Azure connection-string.</param>
        /// <returns>A collection of <see cref="WhiteListItem" /> instances.</returns>
        public static List<WhiteListItem> LoadWhiteListItemsFromAzure(
            string connectionString)
        {
            const string commandText = @"
                SELECT LowerIPAddress, UpperIPAddress
                FROM dbo.WhiteList;";

            using (
                var connection =
                    new SqlConnection(connectionString))
            {
                var command = new SqlCommand(commandText,
                    connection);

                connection.Open();

                var reader = command.ExecuteReader();

                var whiteListItems = new List<WhiteListItem>();

                while (reader.Read())
                {
                    whiteListItems.Add(new WhiteListItem
                    {
                        LowerIPAddress = IPAddress.Parse(reader.GetString(0)),
                        UpperIPAddress = IPAddress.Parse(reader.GetString(1))
                    });
                }

                return whiteListItems;
            }
        }

        /// <summary></summary>
        /// <param name="singleIPAddresses"></param>
        /// <param name="ipAddressRanges"></param>
        /// <param name="getWhiteListedItems"></param>
        public static void SegmentIPAddressesByType(
            out HashSet<string> singleIPAddresses,
            out List<WhiteListItem> ipAddressRanges,
            Func<List<WhiteListItem>> getWhiteListedItems)
        {
            singleIPAddresses = new HashSet<string>();
            ipAddressRanges = new List<WhiteListItem>();

            var whiteListedIPAddresses = getWhiteListedItems();

            foreach (var whiteListedIPAddress in whiteListedIPAddresses)
            {
                if (whiteListedIPAddress.IsRange)
                {
                    ipAddressRanges.Add(whiteListedIPAddress);
                }
                else
                {
                    singleIPAddresses.Add(
                        whiteListedIPAddress.LowerIPAddress.ToString());
                }
            }
        }

        /// <summary>
        ///     <see cref="IPAddressIsWhiteListed" /> determines whether or not
        ///     <see cref="ipAddress" /> is whitelisted.
        /// </summary>
        /// <param name="ipAddress">The <see cref="IPAddress" /> to validate.</param>
        /// <param name="whiteListedIPAddresses">
        ///     A collection of single IP addresses that
        ///     are whitelisted.
        /// </param>
        /// <param name="whiteListedIPAddressRanges">
        ///     A collection of
        ///     <see cref="IPAddress" /> ranges that are whitelisted.
        /// </param>
        /// <returns><c>True</c>, if <see cref="ipAddress" /> is whitelisted.</returns>
        /// <remarks>
        ///     <see cref="IPAddressIsWhiteListed" /> compares <see cref="ipAddress" /> to
        ///     an underelying collection of single IP addresses, as well as a range of
        ///     <see cref="IPAddress" /> instances. If <see cref="ipAddress" /> is
        ///     contained within the range of single addresses, or falls within one of the
        ///     IP address ranges, it is considered whitelisted.
        /// </remarks>
        public static bool IPAddressIsWhiteListed(IPAddress ipAddress,
            HashSet<string> whiteListedIPAddresses,
            List<WhiteListItem> whiteListedIPAddressRanges)
        {
            var isWhiteListed =
                whiteListedIPAddresses.Contains(ipAddress.ToString());

            if (isWhiteListed || whiteListedIPAddressRanges.Count == 0)
                return isWhiteListed;

            var counter = 0;

            do
            {
                isWhiteListed =
                    ipAddress.IsInRange(
                        whiteListedIPAddressRanges[counter].LowerIPAddress,
                        whiteListedIPAddressRanges[counter].UpperIPAddress);

                counter++;
            } while (!isWhiteListed &&
                     counter < whiteListedIPAddressRanges.Count);

            return isWhiteListed;
        }
    }
}