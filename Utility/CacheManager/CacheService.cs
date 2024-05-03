using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;

namespace BlazorLoanModule.Utility.CacheManager
{
    public class CacheService:ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;
        public CacheService(IDistributedCache distributedCache, IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        public async Task<T> GetData<T>(string key)
        {
            var value = await _distributedCache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value)!;
            }
            return default!;
        }

        public async Task SetData(string key, object value, int ExpireMinute = 0)
        {
            if (ExpireMinute == 0)
            {
                ExpireMinute = 10;
            }
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(ExpireMinute)).SetSlidingExpiration(TimeSpan.FromMinutes(60));
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
        }
        public async Task RemoveData(string key)
        {
            await _distributedCache.RemoveAsync(key);

        }

        public async Task<List<string>> GetKeylistAsync(string Pattern)
            => await Task.Run(() =>
            {
                List<string> keys = new List<string>();
                ConfigurationOptions options = ConfigurationOptions.Parse(_configuration.GetSection("redis:connectionString").Value!);
                ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
                EndPoint endPoint = connection.GetEndPoints().First();
                var pattern = $"{Pattern}*";
                foreach (var key in connection.GetServer(endPoint).Keys(pattern: pattern))
                {
                    keys.Add(key!);
                }
                return keys;
            });
        public async Task<string?> GetValue(string key) => await _distributedCache.GetStringAsync(key);

    }
}
