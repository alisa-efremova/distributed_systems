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

        public void Save(string id, string value)
        {
            GetRetryPolicy().Execute(() =>
            {
                Console.WriteLine("Try save to db");
                Connection.GetDatabase().StringSet(id, value); 
            });
        }

        public string Get(string id)
        {
            return GetRetryPolicy().Execute(() => {
                return Connection.GetDatabase().StringGet(id);
            });
        }

        private static RetryPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<RedisException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => 
        {
            return GetRetryPolicy().Execute(() =>
            {
                Console.WriteLine("Try to connect to db");
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisConnectionString"]);
            });
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}