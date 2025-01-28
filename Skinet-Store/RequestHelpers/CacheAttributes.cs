using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Skinet_Store.Core.Interfaces;
using System.Text;

namespace Skinet_Store.RequestHelpers
{
	[AttributeUsage(AttributeTargets.All)]
	public class CacheAttributes(int timeToLive) : Attribute, IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var cacheService = context.HttpContext.RequestServices.
									  GetRequiredService<IResponseChacheService>();

			var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

			var cachedResponse = await cacheService.GetCachedReponseAsync(cacheKey);

			if (cachedResponse != null)
			{
				var contentResult = new ContentResult
				{
					Content = cachedResponse,
					ContentType = "application/json",
					StatusCode = 200

				};

				context.Result = contentResult;
				return;
			}

			var executedContext = await next();
			if (executedContext.Result is OkObjectResult ok)

			{
				if(ok != null)
				{
					await cacheService.CacheResponseAsync(cacheKey, ok.Value, TimeSpan.FromSeconds(timeToLive));
				}

			}
		}

		private string GenerateCacheKeyFromRequest(HttpRequest request)
		{
			var keyBuilder = new StringBuilder();

			keyBuilder.Append($"{request.Path}");

			foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
			{
				keyBuilder.Append($"{key} - {value}");
			}

			return keyBuilder.ToString();
		}
	}

}
