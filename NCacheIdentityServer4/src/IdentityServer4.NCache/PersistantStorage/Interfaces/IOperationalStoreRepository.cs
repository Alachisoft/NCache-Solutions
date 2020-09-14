using IdentityServer4.NCache.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer4.NCache.Stores.Interfaces
{
    public interface IOperationalStoreRepository<T>
        where T:IOperationalStoreEntity
    {
        Task AddAsync(T item);
        Task AddAsync(IEnumerable<T> items);
        Task DeleteAsync(string key);
        Task DeleteAsync(IEnumerable<string> keys);
        Task DeleteByTagsAsync(
            IEnumerable<string> tags);
        Task<T> GetSingleAsync(string key);
        Task<IEnumerable<T>> GetMultipleAsync(IEnumerable<string> keys);
        Task<IEnumerable<T>> GetMultipleByTagsAsync(
            IEnumerable<string> tags);
    }

    public interface IPersistedGrantStoreRepository:
                        IOperationalStoreRepository<PersistedGrant>
    {
    }

    public interface IDeviceStoreRepository:
                        IOperationalStoreRepository<DeviceFlowCodes>
    {
    }
}
