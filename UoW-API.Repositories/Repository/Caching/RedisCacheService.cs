using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UoW_API.Repositories.Repository.Caching.Interfaces;

namespace UoW_API.Repositories.Repository.Caching;
public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public T? Get<T>(string key)
    {
       var data = _cache.GetString(key);
       return data is not null ? JsonSerializer.Deserialize<T>(data) : default;
    }

    public void Set<T>(string key, T data)
    {
        var timeToLive = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
        };

        _cache.SetString(key, JsonSerializer.Serialize(data), timeToLive);
    }
}
