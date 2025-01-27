using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities.OrderAggregates
{
	public class Order : BaseEntity
	{
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public required string BuyerEmail { get; set; }
		public ShippingAdress ShipToAddress { get; set; } = null!;
		public DelieveryMehod DelieveryMehod { get; set; } = null!;
		public PaymentSummary paymentSummary { get; set; } = null!;
		public List<OrderItem> OrderItems { get; set; } = [];
		public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
		public decimal SubTotal { get; set; }
		public required string PaymentIntentId { get; set; }


		// May Be it's Broken The cohesivity But I think it's better to have this method here
		public decimal GetTotal()
		{
			return SubTotal + DelieveryMehod.Price;
		}

	}
}
