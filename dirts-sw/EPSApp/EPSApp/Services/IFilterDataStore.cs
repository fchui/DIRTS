using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPSApp.Services
{
    public interface IFilterDataStore<T>
    {
        Task<bool> AddFilterAsync(T item);
        Task<IEnumerable<T>> GetFiltersAsync(bool forceRefresh = false);

        Task<bool> ClearFilters(bool forceRefresh = false);
    }
}

