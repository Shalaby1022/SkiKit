
using Skinet_Store.Core.Entities.OrderAggregates;
using Skinet_Store.Core.Entities;

namespace Skinet_Store.DTOs
{
	public class OrderDto
	{
		public int OrderId { get; set; }
		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public required string BuyerEmail { get; set; }
		public ShippingAdress ShipToAddress { get; set; } 
		public string DelieveryMehod { get; set; } 
		public PaymentSummary paymentSummary { get; set; } 
		public List<OrderItemDto> OrderItems { get; set; }
		public string OrderStatus { get; set; } 
		public decimal SubTotal { get; set; }
		public decimal Total { get; set; }
		public required string PaymentIntentId { get; set; }


	}
}
