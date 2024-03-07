using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Search.Entities;
using Newtonsoft.Json;

namespace Search.Services
{
    public class RedisUserService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisUserService(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task<List<User>> SearchUsers(string searchString)
        {
            // Define the Lua script that searches for keys matching the pattern
            const string script = @"
        local keys = redis.call('keys', '@0*')
        local results = {}
        for i, key in ipairs(keys) do
            local userJson = redis.call('get', key)
            if userJson then
                table.insert(results, userJson)
            end
        end
        return results";

            // Run the Lua script with the search string as an argument
            RedisValue[] values = (await _db.ScriptEvaluateAsync(script, new RedisKey[] { searchString })).ToString().Split(',').Select(v => (RedisValue)v).ToArray();

            // Deserialize the user objects and return them as a list
            List<User> users = values == null ? new List<User>() : values
                .Where(v => v.HasValue)
                .Select(v => v.ToString())
                .Where(v => !string.IsNullOrEmpty(v))
                .Select(v => JsonConvert.DeserializeObject<User>(v))
                .ToList()!;

            return users;
        }

        public async Task SaveUserToRedis(User user)
        {
            // Serialize the user object to JSON
            string userJson = JsonConvert.SerializeObject(user);

            // Set the user object in Redis with an expiration time of 1 hour
            await _db.StringSetAsync(user.email, userJson, TimeSpan.FromHours(1));
        }

    }
}
