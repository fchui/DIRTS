using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EPSApp.Models;

namespace EPSApp.Services
{
    public class FilterDataStore : IFilterDataStore<Filters>
    {
        readonly List<Filters> items;
        public FilterDataStore()
        {
            items = new List<Filters>();
        }
        public async Task<bool> AddFilterAsync(Filters item)
        {
            items.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Filters>> GetFiltersAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }

        public Task<bool> ClearFilters(bool forceRefresh = false)
        {
            items.Clear();
            return Task.FromResult(true);
        }
    }
}
