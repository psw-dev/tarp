using System;
using System.Text.Json;
using StackExchange.Redis;

namespace PSW.ITMS.Common
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _database = _redisConnection.GetDatabase();
        }

        public dynamic Get<T>(string key)
        {
            var value = _database.StringGet(key);
            if (value.HasValue)
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return null;
        }

        public bool KeyExists(string key)
        {
            var value = _database.KeyExists(key);
            return value;
        }

        public bool Set<T>(string key, T value, TimeSpan expiry)
        {
            _database.StringSet(key, JsonSerializer.Serialize(value), expiry);

            return true;
        }
    }
}