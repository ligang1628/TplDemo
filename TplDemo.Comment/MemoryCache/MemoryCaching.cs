using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace TplDemo.Comment.MemoryCache
{
    public class MemoryCaching : ICaching
    {
        private readonly IMemoryCache cache;

        public MemoryCaching(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public object Get(string cacheKey)
        {
            return cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue)
        {
            cache.Set(cacheKey, cacheValue);
        }
    }
}
