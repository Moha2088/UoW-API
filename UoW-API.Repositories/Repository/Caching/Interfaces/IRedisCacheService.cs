using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoW_API.Repositories.Repository.Caching.Interfaces;
public interface IRedisCacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T data);
}
