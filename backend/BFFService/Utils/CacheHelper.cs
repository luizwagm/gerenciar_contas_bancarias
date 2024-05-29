using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BFFService.Utils
{
    public static class CacheHelper
    {
        public static async Task<T> GetOrSetCacheAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> factory, TimeSpan? absoluteExpirationRelativeToNow = null)
        {
            var cacheData = await cache.GetStringAsync(key);

            if (!string.IsNullOrEmpty(cacheData))
            {
                var cacheDataJson = JsonSerializer.Deserialize<T>(cacheData);

                return cacheDataJson ?? throw new KeyNotFoundException("Cache not found.");
            }

            var data = await factory();
            await cache.SetStringAsync(key, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow ?? TimeSpan.FromMinutes(5)
            });

            return data;
        }
    }
}
