using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using StackExchange.Redis;

namespace BackendOwin
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
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["redisConnectionString"]);
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
