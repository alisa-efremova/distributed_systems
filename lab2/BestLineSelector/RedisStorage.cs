using System;
using System.Configuration;
using StackExchange.Redis;
using Polly;
using Polly.Retry;

namespace BestLineSelector
{
    class RedisStorage
    {
        const int _retryCount = 5;
        const int _initialRetryTimeoutSec = 2;
        const int _maxFailureCountBeforeBreaking = 2;
        static TimeSpan _breakerTimeout = TimeSpan.FromSeconds(15);

        public void Save(string id, string value)
        {
            GetRetryPolicy().Execute(() =>
            {
                Breaker.Execute(() =>
                {
                    Console.WriteLine("Try save to db");
                    Connection.GetDatabase().StringSet(id, value);
                });
            });
        }

        public string Get(string id)
        {
            return GetRetryPolicy().Execute(() => 
            {
                return Breaker.Execute(() =>
                {
                    return Connection.GetDatabase().StringGet(id);
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