using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Aegis.Core;

namespace Aegis.Outlet.Controllers
{
    /// <summary>
    ///     <see cref="BlackListController" /> provides a collection of cached,
    ///     blacklisted items, calculated by Aegis.
    /// </summary>
    public class BlackListController : ApiController
    {
        /// <summary>
        ///     <see cref="GetBlackList" /> returns a collection of
        ///     <see cref="BlackListItem" /> instances that pertain to each
        ///     <see cref="country" />.
        /// </summary>
        /// <param name="country">
        ///     The country from which each
        ///     <see cref="BlackListItem" /> originates.
        /// </param>
        /// <returns>
        ///     A collection of <see cref="BlackListItem" /> instances that pertain to
        ///     each <see cref="country" />.
        /// </returns>
        [Route("blacklist")]
        public IEnumerable<BlackListItem> GetBlackList([FromUri] string[] country)
        {
            var blackList = new List<BlackListItem>();

            foreach (var c in country)
            {
                List<BlackListItem> blackListItems;

                var blackListExists =
                    BlackList.Instance.BlackListsByCountry.TryGetValue(c.ToLowerInvariant(),
                        out blackListItems);

                if (blackListExists)
                {
                    blackList.AddRange(blackListItems);
                }
            }

            if (blackList.Count.Equals(0))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return blackList;
        }
    }
}