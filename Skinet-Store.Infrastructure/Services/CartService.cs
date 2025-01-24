using Microsoft.EntityFrameworkCore.Storage;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Services
{
	public class CartService : ICartService
	{
		private readonly StackExchange.Redis.IDatabase _database;

		public CartService(IConnectionMultiplexer redis)
		{
			_database = redis.GetDatabase(); // Initialize _database
		}

		public async Task<ShoppingCart> CreateCartAsync(ShoppingCart cart)
		{
			if (cart == null)
			{
				throw new ArgumentNullException(nameof(cart));
			}

			// Serialize the cart object to JSON
			var cartJson = JsonSerializer.Serialize(cart);

			// Store the cart in Redis with a 30-day expiration
			var created = await _database.StringSetAsync(cart.Id, cartJson, TimeSpan.FromDays(30));

			if (!created)
			{
				return null; // Return null if the cart could not be created
			}

			// Return the newly created cart
			return await GetCartAsync(cart.Id);
		}

		public async Task<bool> DeleteCartAsync(string cartId)
		{
			if (string.IsNullOrEmpty(cartId))
			{
				throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));
			}

			// Delete the cart from Redis
			return await _database.KeyDeleteAsync(cartId);
		}

		public async Task<ShoppingCart> GetCartAsync(string cartId)
		{
			var keyExists = _database.KeyExists("cart-12345");
			Console.WriteLine($"Key 'cart-12345' exists: {keyExists}");

			if (string.IsNullOrEmpty(cartId))
			{
				throw new ArgumentException("Cart ID cannot be null or empty", nameof(cartId));
			}

			// Retrieve the cart from Redis
			var data = await _database.StringGetAsync(cartId);

			// If the cart does not exist, return null
			if (data.IsNullOrEmpty)
			{
				return null;
			}

			// Deserialize the JSON data to a ShoppingCart object
			return JsonSerializer.Deserialize<ShoppingCart>(data);
		}
	}
}