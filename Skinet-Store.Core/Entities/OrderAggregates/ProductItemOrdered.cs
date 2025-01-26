using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities.OrderAggregates
{
	public class ProductItemOrdered
	{
		public int ProductId { get; set; }
		public required string ProductName { get; set; }
		public required string PictureUrl { get; set; }

	}
}
