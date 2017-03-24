using System;
using System.Configuration;
using System.Runtime.Caching;
using DataAccess;

namespace GoodPoem
{
    class ValueNotFoundException : SystemException { }

    class Storage
    {
        static readonly DataAccessLayer _dataAccess = new DataAccessLayer();

        CacheItemPolicy _cachePolicy;
        static readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);

        public Storage()
        {
            _cachePolicy = new CacheItemPolicy();
            _cachePolicy.SlidingExpiration = _cacheSlidingExpiration;
        }

        public void Save(string userId, string corrId, string value)
        {
            Console.WriteLine("Save to cache");
            MemoryCache.Default.AddOrGetExisting(corrId, value, _cachePolicy);
            _dataAccess.Save(userId, corrId, value);
        }

        public string Get(string corrId)
        {
            var value = MemoryCache.Default.Get(corrId);
            if (value != null)
            {
                Console.WriteLine("Found poem in cache");
                return (string)value;
            }
            else
            {
                return _dataAccess.Get(corrId);
            }
        }
    }
}