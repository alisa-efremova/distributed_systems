using System;
using System.Configuration;
using System.Collections.Generic;
using StackExchange.Redis;
using Polly;
using Polly.Retry;

namespace DataAccess
{
    class ValueNotFoundException : SystemException { }

    public class DataAccessLayer
    {
        private static readonly ConnectionMultiplexer[] _connections = new ConnectionMultiplexer[]
        {
            ConnectionMultiplexer.Connect("localhost"),
            ConnectionMultiplexer.Connect("localhost:7777"),
            ConnectionMultiplexer.Connect("localhost:7770")
        };

        private static readonly SortedDictionary<string, int> _userConnectionMap = new SortedDictionary<string, int>
        {
            { "user1", 0 },
            { "user2", 1 },
            { "user3", 1 },
            { "user4", 2 },
        };

        private static SortedDictionary<string, string> _corrIdUserMap = new SortedDictionary<string, string>();

        const int _retryCount = 5;
        const int _initialRetryTimeoutSec = 2;

        const int _maxFailureCountBeforeBreaking = 3;
        static readonly TimeSpan _breakerTimeout = TimeSpan.FromSeconds(10);

        public void Save(string userId, string id, string value)
        {
            int connectionId = GetConnectionIdByUserId(userId);
            SaveUserCorrIdConnection(userId, id);
          
            GetRetryPolicy().Execute(() =>
            {
                Breaker.Execute(() =>
                {
                    Console.WriteLine("Try save to db");
                    _connections[connectionId].GetDatabase().StringSet(id, value);
                });
            });
        }

        public string Get(string id)
        {
            return GetRetryPolicy().Execute(() =>
            {
                return Breaker.Execute(() =>
                {
                    Console.WriteLine("Request poem");
                    int connectionId = GetConnectionIdByCorrId(id);
                    var value = _connections[connectionId].GetDatabase().StringGet(id);
                    if (value.IsNull)
                    {
                        Console.WriteLine("Throw exception");
                        throw new ValueNotFoundException();
                    }
                    return value;
                });
            });
        }

        private int GetConnectionIdByUserId(string userId)
        {
            return _userConnectionMap[userId];
        }

        private int GetConnectionIdByCorrId(string corrId) 
        {
            return GetConnectionIdByUserId(_corrIdUserMap[corrId]);
        }

        private void SaveUserCorrIdConnection(string userId, string CorrId)
        {
            _corrIdUserMap[CorrId] = userId;
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