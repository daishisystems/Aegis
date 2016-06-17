using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Aegis.Monitor.Core;

namespace Aegis.Monitor.Filter.Controllers
{
    /// <summary>
    ///     <see cref="BlackListController" /> provides a collection of cached,
    ///     blacklisted items, calculated by Aegis.
    /// </summary>
    public class BlackListController : ApiController
    {
        /// <summary>
        ///     <see cref="GetBlackListByCountry" /> returns a collection of
        ///     <see cref="BlackListItem" /> instances that pertain to
        ///     <see cref="countryName" />.
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>A collection of <see cref="BlackListItem" /> instances that pertain to
        ///     <see cref="countryName" />.</returns>
        [Route("blacklist/{countryName}")]
        public IEnumerable<BlackListItem> GetBlackListByCountry(string countryName)
        {
            List<BlackListItem> blackListItems;

            var blackListExists =
                BlackList.Instance.BlackListsByCountry.TryGetValue(countryName.ToLowerInvariant(),
                    out blackListItems);

            if (!blackListExists)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return blackListItems;
        }
    }
}