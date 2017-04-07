using System;
using System.Configuration;
using System.Collections.Generic;
using StackExchange.Redis;

namespace DataAccess
{
    public class DataAccessLayer
    {
        private static readonly string[] _connectionStrings = new string[]
        {
            "localhost",
            "localhost:7777",
            "localhost:7770"
        };

        private static readonly SortedDictionary<string, int> _userConnectionMap = new SortedDictionary<string, int>
        {
            { "user1", 0 },
            { "user2", 1 },
            { "user3", 1 },
            { "user4", 2 },
        };

        private static SortedDictionary<int, ConnectionMultiplexer> _connections = new SortedDictionary<int,ConnectionMultiplexer>();
        private static SortedDictionary<string, string> _corrIdUserMap = new SortedDictionary<string, string>();


        public ConnectionMultiplexer GetConnection(string userId, string corrId)
        {
            var connection = GetConnectionByUserId(userId);
            _corrIdUserMap[corrId] = userId;
            return connection;
        }

        public ConnectionMultiplexer GetConnection(string corrId)
        {
            string userId;
            if (!_corrIdUserMap.TryGetValue(corrId, out userId))
            {
                throw new ArgumentException("Invalid correlation id provided.");
            }
            return GetConnectionByUserId(userId);
        }

        private ConnectionMultiplexer GetConnectionByUserId(string userId)
        {
            var connectionId = _userConnectionMap[userId];
            if (!_connections.ContainsKey(connectionId))
            {
                if (connectionId >= _connectionStrings.Length)
                {
                    throw new ArgumentException("Invalid user id provided.");
                }
                string connectionString = _connectionStrings[connectionId];
                return ConnectionMultiplexer.Connect(connectionString);
            }
            else
            {
                return _connections[connectionId];
            }
        }
    }
}