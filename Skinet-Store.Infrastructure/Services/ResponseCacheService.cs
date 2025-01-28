using Skinet_Store.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Services
{
	internal class ResponseCacheService : IResponseChacheService
	{
		private readonly StackExchange.Redis.IDatabase _database;

		public ResponseCacheService(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase(1); // Initialize _database
		}

		public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
		{
			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
			
			var serializerObjectResponse = JsonSerializer.Serialize(response, options);

			await _database.StringSetAsync(cacheKey, serializerObjectResponse, timeToLive);

		}

		public async Task<string?> GetCachedReponseAsync(string cacheKey)
		{
			var cachedResponse = await _database.StringGetAsync(cacheKey);
			if(cachedResponse.IsNullOrEmpty)
			{
				return null;
			}

			return cachedResponse;

		}

		public Task RemoveCachedByPattern(string pattern)
		{
			throw new NotImplementedException();
		}
	}
}
