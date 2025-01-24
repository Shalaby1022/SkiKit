

using Microsoft.Extensions.Configuration;
using Skinet_Store.Core.CoreServiceExtensions;
using Skinet_Store.Infrastructure.ServiceExtensions;
using Skinet_Store.Middleware;
using StackExchange.Redis;

namespace Skinet_Store
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			#region Infrastructure Service Registration 

			builder.Services.AddInfrastructureExtensionsToRgisterItInMainProgram(builder.Configuration);

			#endregion

			#region Core Service Registration
			builder.Services.AddCoreExtensionsToRgisterItInMainProgram(builder.Configuration);


			#endregion
			#region Redis Registration 

			// Register Redis ConnectionMultiplexer as a singleton
			var redisConnectionString = builder.Configuration.GetConnectionString("Redis")
		              ?? throw new Exception("Redis connection string is missing");
		

		    var redis = ConnectionMultiplexer.Connect(redisConnectionString);
			builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

			// Register IDatabase as a transient service
			builder.Services.AddTransient<IDatabase>(sp =>
			{
				var redis = sp.GetRequiredService<IConnectionMultiplexer>();
				return redis.GetDatabase();
			});


			#endregion

			builder.Services.AddCors();


			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseMiddleware<ExceptionMiddleware>();

			app.UseExceptionHandler("/error");

			app.UseCors(x => 
						x.AllowAnyHeader()
						.AllowAnyMethod()
						.WithOrigins("https://localhost:4200" , "http://localhost:4200"));

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
