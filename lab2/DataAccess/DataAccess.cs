using System;
using System.Configuration;
using StackExchange.Redis;
using Polly;
using Polly.Retry;

namespace DataAccess
{
    class ValueNotFoundException : SystemException { }

    public class DataAccessLayer
    {
        private static ConnectionMultiplexer[] _connections = new ConnectionMultiplexer[]
        {
            ConnectionMultiplexer.Connect("localhost")
        };

        const int _retryCount = 5;
        const int _initialRetryTimeoutSec = 2;

        const int _maxFailureCountBeforeBreaking = 3;
        static readonly TimeSpan _breakerTimeout = TimeSpan.FromSeconds(10);

        public void Save(string id, string value)
        {
            GetRetryPolicy().Execute(() =>
            {
                Breaker.Execute(() =>
                {
                    Console.WriteLine("Try save to db");
                    _connections[0].GetDatabase().StringSet(id, value);
                });
            });
        }

        public string Get(string id)
        {
            return GetFromDB(id);
        }

        private string GetFromDB(string id)
        {
            return GetRetryPolicy().Execute(() =>
            {
                return Breaker.Execute(() =>
                {
                    Console.WriteLine("Request poem");
                    var value = _connections[0].GetDatabase().StringGet(id);
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