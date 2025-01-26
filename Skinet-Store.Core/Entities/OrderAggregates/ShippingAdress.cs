using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities.OrderAggregates
{
	public class ShippingAdress
	{
		public required string Name { get; set; } 
		public required string Line1 { get; set; }
		public string Line2 { get; set; } = string.Empty;
		public required string City { get; set; } 
		public required string State { get; set; } 
		public required string PostalCode { get; set; } 
	}
}
