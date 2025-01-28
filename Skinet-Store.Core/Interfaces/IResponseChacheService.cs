using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Interfaces
{
	public interface IResponseChacheService
	{
		// 
		Task CacheResponseAsync(string cacheKey, object response , TimeSpan timeToLive);

		// 
		Task<string?>  GetCachedReponseAsync(string cacheKey);

		Task RemoveCachedByPattern(string pattern);

	}
}
