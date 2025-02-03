using System.Runtime.Caching;

namespace Gitpilot.Caches
{
    public abstract class CacheBase<T>
    {
        private readonly object _lockKey;
        private string CacheKey { get; set; }
        private MemoryCache _cache = MemoryCache.Default;

        protected CacheBase()
        {
            CacheKey = GetType().Name;
            _lockKey = new object();
        }

        public bool Contains(string key)
        {
            return _cache.Contains($"{CacheKey}_{key}");
        }

        public async Task<T> GetAsync(string key)
        {
            if (Contains(key))
            {
                return (T)_cache[$"{CacheKey}_{key}"];
            }

            if (_cache.Contains($"{CacheKey}_{key}"))
            {
                return (T)_cache[$"{CacheKey}_{key}"];
            }

            if ((T)_cache[$"{CacheKey}_{key}"] != null)
            {
                return (T)_cache[$"{CacheKey}_{key}"];
            }

            var result = await ReloadFromDb(key);
            _cache.Set($"{CacheKey}_{key}", result, GetItemPolicy());
            return result;
        }

        public void ClearAll()
        {
            lock (_lockKey)
            {
                if (_cache == null)
                {
                    return;
                }

                var cacheKeys = MemoryCache.Default.Select(kvp => kvp.Key).ToList();
                foreach (var key in cacheKeys.Where(x => x.Contains($"{CacheKey}_")))
                {
                    MemoryCache.Default.Remove(key);
                }
            }
        }

        public List<T> GetAll()
        {
            return MemoryCache.Default.Where(kvp => kvp.Key.StartsWith(CacheKey.ToString()))
                .Select(kvp => (T)kvp.Value)
                .ToList();
        }

        public void Remove(string key)
        {
            MemoryCache.Default.Remove($"{CacheKey}_{key}");
        }

        public void AddOrUpdate(string key, T item)
        {
            _cache.Set($"{CacheKey}_{key}", item, GetItemPolicy());
        }

        protected abstract Task<T> ReloadFromDb(string key);

        protected abstract CacheItemPolicy GetItemPolicy();
    }
}