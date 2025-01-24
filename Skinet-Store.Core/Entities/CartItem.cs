using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities
{
	public class CartItem
	{
		[Key]
		public int Id { get; set; }
		public string ProductName { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public int Quantity { get; set; }
		public string PictureUrl { get; set; } = string.Empty;
		public string Brand { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;

	}
}
