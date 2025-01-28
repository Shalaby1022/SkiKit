

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skinet_Store.Infrastructure.EmailSenderUtility;
using Skinet_Store.Core.CoreServiceExtensions;
using Skinet_Store.Core.Entities;
using Skinet_Store.Infrastructure.Data;
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

			#region Identity Service Registration

			// Configuration for the database connection
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			// Add DbContext with SQL Server
			builder.Services.AddDbContext<ApplicationDbContex>(options =>
				options.UseSqlServer(connectionString)
					   .EnableSensitiveDataLogging());

			builder.Services.AddAuthorization();

			// Add Id entity services with ApplicationUser and IdentityRole
			builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContex>()
				.AddDefaultTokenProviders()
				.AddSignInManager<SignInManager<ApplicationUser>>();

			#endregion

			builder.Services.AddScoped<Infrastructure.EmailSenderUtility.IEmailSender , EmailSender>();
			builder.Services.AddScoped<Infrastructure.EmailSenderUtility.IEmailSender<ApplicationUser>, EmailSender>();


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
						.WithOrigins("https://localhost:4200", "http://localhost:4200"));

			app.UseHttpsRedirection();


			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapGroup("api")
				 .WithTags("Account")
				.MapIdentityApi<ApplicationUser>();
			app.MapControllers();
			

			app.Run();
		}
	}
}