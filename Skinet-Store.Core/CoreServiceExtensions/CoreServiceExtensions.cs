using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skinet_Store.Core.Specification;


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
