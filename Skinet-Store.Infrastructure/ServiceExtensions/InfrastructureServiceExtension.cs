using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Infrastructure.Data;
using Skinet_Store.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.ServiceExtensions
{
	public static class InfrastructureServiceExtension
	{
		public static void AddInfrastructureExtensionsToRgisterItInMainProgram(this IServiceCollection services,
																					IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");

			services.AddDbContext<ApplicationDbContex>(options =>
					options.UseSqlServer(connectionString)
					 .EnableSensitiveDataLogging());

			services.AddScoped<IProductRepository, ProductRepository>();

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

		}
	}
}
