using System.Collections.Concurrent;

namespace Aegis.Monitor.Core
{
    public class FilterListByCountry<TFilterListItem>
    {
        private readonly ConcurrentBag<TFilterListItem> _filterListItems;

        public FilterListByCountry()
        {
            _filterListItems = new ConcurrentBag<TFilterListItem>();
        }

        /// <summary>
        ///     <see cref="IsEmpty" /> returns <c>true</c> if this instance does not
        ///     contain any instances of <see cref="TFilterListItem" />.
        /// </summary>
        public bool IsEmpty => _filterListItems.IsEmpty;

        /// <summary>
        ///     <see cref="FilterListByCountry{TFilterListItem}.Add" /> adds
        ///     <see cref="filterListItem" /> to this instance.
        /// </summary>
        /// <param name="filterListItem">
        ///     The <see cref="TFilterListItem" /> to be added to
        ///     this instance.
        /// </param>
        public void Add(TFilterListItem filterListItem)
        {
            _filterListItems.Add(filterListItem);
        }
    }
}