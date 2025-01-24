using Microsoft.AspNetCore.Http;
using Skinet_Store.Errors;
using System.Net;
using System.Text.Json;

namespace Skinet_Store.Middleware
{
	public class ExceptionMiddleware
	{
		private RequestDelegate _next;
		private IHostEnvironment _hostEnvironment;

		public ExceptionMiddleware(IHostEnvironment hostEnvironment , RequestDelegate next)
		{
			_next = next;
			_hostEnvironment = hostEnvironment;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{

				await HandleExceptionAsync(context , ex , _hostEnvironment);

				
			}
		}

		private async Task HandleExceptionAsync(HttpContext context , Exception ex , IHostEnvironment hostEnvironment)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

			var response = _hostEnvironment.IsDevelopment()
				? new ApiErrorResponseHandling(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
				: new ApiErrorResponseHandling(context.Response.StatusCode, ex.Message , "Internal Server Error");

			var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			var json = JsonSerializer.Serialize(response, options);

			await context.Response.WriteAsync(json);
		}
	}
}
