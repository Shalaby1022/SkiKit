using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Interfaces
{
	public interface ICartService
	{
		Task<ShoppingCart> GetCartAsync(string cartId);
		Task<ShoppingCart> CreateCartAsync(ShoppingCart cart);
		Task<bool> DeleteCartAsync(string cartId);
	}
}
