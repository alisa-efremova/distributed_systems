using System;
using System.Configuration;
using StackExchange.Redis;

namespace BestLineSelector
{
    class RedisStorage
    {
        public void Save(string id, string value)
        {
            Connection.GetDatabase().StringSet(id, value);
        }

        public string Get(string id)
        {
            return Connection.GetDatabase().StringGet(id);
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() => 
        {
            return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["RedisConnectionString"]);
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