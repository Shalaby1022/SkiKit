using Skinet_Store.Core.Entities.OrderAggregates;
using System.ComponentModel.DataAnnotations;

namespace Skinet_Store.DTOs
{
	public class CreateOrderDto
	{
		[Required]
		public string CartId { get; set; }
		[Required]
		public ShippingAdress ShippingAdress { get; set; }
		[Required]
		public int DelieveryMehodId { get; set; }
		[Required]
		public PaymentSummary PaymentSummary { get; set; }
	}
}
