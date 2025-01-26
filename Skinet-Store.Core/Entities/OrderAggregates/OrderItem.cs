using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities.OrderAggregates
{
	public class OrderItem : BaseEntity
	{
		public ProductItemOrdered ProductItemOrdered { get; set; } = null!;
 		public decimal Price { get; set; }
		public int Quantity { get; set; }

	}
}
