using System;
using System.Configuration;
using System.Runtime.Caching;
using StackExchange.Redis;
using Polly;
using Polly.Retry;
using DataAccess;

namespace GoodPoem
{
    class ValueNotFoundException : SystemException { }

    class Storage
    {
        static readonly DataAccessLayer _dataAccess = new DataAccessLayer();

        CacheItemPolicy _cachePolicy;
        static readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);

        const int _retryCount = 5;
        const int _initialRetryTimeoutSec = 2;

        const int _maxFailureCountBeforeBreaking = 3;
        static readonly TimeSpan _breakerTimeout = TimeSpan.FromSeconds(10);

        public Storage()
        {
            _cachePolicy = new CacheItemPolicy();
            _cachePolicy.SlidingExpiration = _cacheSlidingExpiration;
        }

        public void Save(string userId, string corrId, string value)
        {
            Console.WriteLine("Save to cache");
            MemoryCache.Default.AddOrGetExisting(corrId, value, _cachePolicy);

            GetRetryPolicy().Execute(() =>
            {
                Breaker.Execute(() =>
                {
                    Console.WriteLine("Try save to db");
                    _dataAccess.GetConnection(userId, corrId).GetDatabase().StringSet(corrId, value);
                });
            });
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
                return GetFromDB(corrId);
            }
        }

        private string GetFromDB(string id)
        {
            return GetRetryPolicy().Execute(() =>
            {
                return Breaker.Execute(() =>
                {
                    Console.WriteLine("Request poem");
                    var value = _dataAccess.GetConnection(id).GetDatabase().StringGet(id);
                    if (value.IsNull)
                    {
                        Console.WriteLine("Throw exception");
                        throw new ValueNotFoundException();
                    }
                    return value;
                });
            });
        }

        private static RetryPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(_initialRetryTimeoutSec, retryAttempt)));
        }

        // Circuit Breaker lazy initialization
        private static Lazy<CircuitBreaker> lazyBreaker = new Lazy<CircuitBreaker>(() =>
        {
            return new CircuitBreaker(_maxFailureCountBeforeBreaking, _breakerTimeout);
        });

        private static CircuitBreaker Breaker
        {
            get
            {
                return lazyBreaker.Value;
            }
        }
    }
}