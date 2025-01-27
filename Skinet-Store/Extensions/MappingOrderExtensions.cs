using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Entities.OrderAggregates;
using Skinet_Store.DTOs;

namespace Skinet_Store.Extensions
{
	public static class MappingOrderExtensions
	{
		public static OrderDto ToDto(this Order order)
		{
			return new OrderDto
			{
				OrderId = order.Id,
				OrderDate = order.OrderDate,
				BuyerEmail = order.BuyerEmail,
				ShipToAddress = order.ShipToAddress,
				DelieveryMehod = order.DelieveryMehod.ShortName,
				paymentSummary = order.paymentSummary,
				OrderItems = order.OrderItems.Select(x => new OrderItemDto
				{
					ProductId = x.ProductItemOrdered.ProductId,
					ProductName = x.ProductItemOrdered.ProductName,
					PictureUrl = x.ProductItemOrdered.PictureUrl,
					Price = x.Price,
					Quantity = x.Quantity
				}).ToList(),
				SubTotal = order.SubTotal,
				Total = order.GetTotal(),
				OrderStatus = order.OrderStatus.ToString(),
				PaymentIntentId = order.PaymentIntentId
			};
		}

		public static Order ToEntity(this OrderDto orderDto)
		{
			return new Order
			{
				Id = orderDto.OrderId,
				OrderDate = orderDto.OrderDate,
				BuyerEmail = orderDto.BuyerEmail,
				ShipToAddress = orderDto.ShipToAddress,
				DelieveryMehod = new DelieveryMehod
				{
					DelieveryTime = orderDto.DelieveryMehod,
					Description = orderDto.DelieveryMehod,
					ShortName = orderDto.DelieveryMehod
				},
				paymentSummary = orderDto.paymentSummary,
				OrderItems = orderDto.OrderItems.Select(x => new OrderItem
				{
					ProductItemOrdered = new ProductItemOrdered
					{
						ProductId = x.ProductId,
						ProductName = x.ProductName,
						PictureUrl = x.PictureUrl
					},
					Price = x.Price,
					Quantity = x.Quantity
				}).ToList(),
				SubTotal = orderDto.SubTotal,
				OrderStatus = Enum.Parse<OrderStatus>(orderDto.OrderStatus),
				PaymentIntentId = orderDto.PaymentIntentId
			};
		}

		public static OrderItemDto ToDto(this OrderItem orderItem)
		{
			return new OrderItemDto
			{
				ProductId = orderItem.ProductItemOrdered.ProductId,
				ProductName = orderItem.ProductItemOrdered.ProductName,
				PictureUrl = orderItem.ProductItemOrdered.PictureUrl,
				Price = orderItem.Price,
				Quantity = orderItem.Quantity
			};
		}
	}
}
