using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities
{
	public class  ShoppingCart
	{
		public required string Id { get; set; }
		public List<CartItem> Items { get; set; } = new List<CartItem>();
		public int? DeleieryMethodId { get; set; } 
		public string? ClientSecret { get; set; }

		public string? PaymentIntentId { get; set; }


	}
}
