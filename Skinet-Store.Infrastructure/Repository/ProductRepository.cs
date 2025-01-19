using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Repository
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContex _context;
		private readonly ILogger<ProductRepository> _logger;

		public ProductRepository(ApplicationDbContex dbContext, ILogger<ProductRepository> logger)
		{
			_context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<Product> CreateNewProductAsync(Product product)
		{
			if (product == null) throw new ArgumentNullException(nameof(product));

			await _context.Products.AddAsync(product);
			await _context.SaveChangesAsync();

			return product;
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

			if (product == null) return false;

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<bool> ProductExistsAsync(int id)
		{
			return await _context.Products.AnyAsync(p => p.Id == id);
		}

		public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
		{
			return await _context.Products.ToListAsync();
		}

		public async Task<Product> GetProductByIdAsync(int id)
		{
			if (id <= 0)
			{
				_logger.LogWarning("Invalid Product ID: {Id}", id);
				return null;
			}

			var product = await _context.Products.FirstOrDefaultAsync(r => r.Id == id);

			if (product == null)
			{
				_logger.LogInformation("Product not found with ID: {Id}", id);
				return null;
			}

			_logger.LogInformation("Product found: {ProductName}", product.Name);
			return product;
		}

		public async Task<bool> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> UpdateProductAsync(Product product)
		{
			if (product == null) throw new ArgumentNullException(nameof(product));

			_context.Entry(product).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<IReadOnlyList<string>> GetProductBrandAsync()
		{
			var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
			return brands;
		}

		public async Task<IReadOnlyList<string>> GetProductTypeAsync()
		{
			var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
			return types;
		}
	}
}
