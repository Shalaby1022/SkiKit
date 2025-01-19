using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Interfaces
{
	public interface IProductRepository
	{
		Task<IReadOnlyList<Product>> GetAllProductsAsync();
		Task<Product> GetProductByIdAsync(int id);
		Task<Product> CreateNewProductAsync(Product product);

		Task<IReadOnlyList<string>> GetProductBrandAsync();
		Task<IReadOnlyList<string>> GetProductTypeAsync();
		Task<bool> DeleteProductAsync(int id);
		Task<bool> UpdateProductAsync(Product product);
		Task<bool> ProductExistsAsync(int id);
		Task<bool> SaveChangesAsync();
	}
}
