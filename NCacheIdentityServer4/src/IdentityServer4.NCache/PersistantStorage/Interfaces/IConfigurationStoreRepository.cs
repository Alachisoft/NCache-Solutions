using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Stores.Interfaces
{
    public interface IConfigurationStoreRepository<T> 
        where T: IConfigurationStoreEntity
    {
        Task AddAsync(T item);
        Task AddAsync(IEnumerable<T> items);
        Task<T> GetSingleAsync(string key);
        Task<IEnumerable<T>> GetMultipleAsync(IEnumerable<string> keys);
        Task<IEnumerable<T>> GetMultipleByTagsAsync(
            IEnumerable<string> tags);
    }
}
