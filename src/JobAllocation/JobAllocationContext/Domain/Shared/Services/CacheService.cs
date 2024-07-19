using JobAllocation.Shared;
using Microsoft.Extensions.Caching.Memory;

namespace JobAllocation.JobAllocationContext.Domain.Shared.Services;

public class CacheService : IService<CacheService>
{
    private readonly IMemoryCache _memoryCache;

    public CacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<T>> GetCache<T>(string key, Func<Task<IEnumerable<T>>> getValues) where T : class
    {
        if (!_memoryCache.TryGetValue(key, out IEnumerable<T> list))
        {
            list = (await getValues()).ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(GetMidnightExpirationCache());

            _memoryCache.Set(key, list, cacheEntryOptions);
        }

        return list;
    }

    private TimeSpan GetMidnightExpirationCache()
        => DateTime.Today.AddDays(1).Subtract(DateTime.Now);
}