using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Infrastructure.Data;
using Skinet_Store.Infrastructure.EmailSenderUtility;
using Skinet_Store.Infrastructure.Repository;
using Skinet_Store.Infrastructure.Services;
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
			
			services.AddScoped<IProductRepository, ProductRepository>();

			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			//services.AddAuthentication()
			//		.(IdentityConstants.BearerScheme);


			services.AddSingleton<ICartService, CartService>();

			services.AddScoped<IPaymentService, PaymentService>();



		}
	}
}
