using System;
using System.Configuration;
using StackExchange.Redis;
using Polly;
using Polly.Retry;
using System.Runtime.Caching;

namespace GoodPoem
{
    class ValueNotFoundException : SystemException { }

    class Storage
    {
        const int _retryCount = 5;
        const int _initialRetryTimeoutSec = 2;

        const int _maxFailureCountBeforeBreaking = 3;
        static readonly TimeSpan _breakerTimeout = TimeSpan.FromSeconds(10);

        CacheItemPolicy _cachePolicy;
        static readonly TimeSpan _cacheSlidingExpiration = TimeSpan.FromMinutes(5);

        public Storage()
        {
            _cachePolicy = new CacheItemPolicy();
            _cachePolicy.SlidingExpiration = _cacheSlidingExpiration;
        }

        public void Save(string id, string value)
        {
            GetRetryPolicy().Execute(() =>
            {
                Console.WriteLine("Save to cache");
                MemoryCache.Default.AddOrGetExisting(id, value, _cachePolicy);

                Breaker.Execute(() =>
                {
                    Console.WriteLine("Try save to db");
                    Connection.GetDatabase().StringSet(id, value);
                });
            });
        }

        public string Get(string id)
        {
            var value = MemoryCache.Default.Get(id);
            if (value != null)
            {
                Console.WriteLine("Found poem in cache");
                return (string)value;
            }
            else
            {
                return GetFromDB(id);
            }
        }

        private string GetFromDB(string id)
        {
            return GetRetryPolicy().Execute(() =>
            {
                return Breaker.Execute(() =>
                {
                    Console.WriteLine("Request poem");
                    var value = Connection.GetDatabase().StringGet(id);
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

        // ConnectionMultiplexer lazy initialization
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => 
        {
            return GetRetryPolicy().Execute(() =>
            {
                return Breaker.Execute(() => 
                {
                    Console.WriteLine("Try to connect to db");
                    return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisConnectionString"]);
                });
            });
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        // Circuit Breaker lazy initialization
        private static Lazy<CircuitBreaker> lazyBreaker = new Lazy<CircuitBreaker>(() =>
        {
            return new CircuitBreaker(_maxFailureCountBeforeBreaking, _breakerTimeout);
        });

        public static CircuitBreaker Breaker
        {
            get
            {
                return lazyBreaker.Value;
            }
        }
    }
}