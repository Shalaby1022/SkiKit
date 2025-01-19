using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skinet_Store.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.CoreServiceExtensions
{
	public static class CoreServiceExtensions
	{
		public static void AddCoreExtensionsToRgisterItInMainProgram(this IServiceCollection services , IConfiguration configuration)
		{

			services.AddScoped<IProductSpecificationFactory, ProductSpecificationFactory>();
		}
	}
}
