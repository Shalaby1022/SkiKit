
using Skinet_Store.Core.Entities.OrderAggregates;

namespace Skinet_Store.Core.Specification.Order
{

	public class OrderSpecification : BaseSpecification<Skinet_Store.Core.Entities.OrderAggregates.Order>
	{
		public OrderSpecification(OrderSpecificationParameters parameters)
	: base(x => (string.IsNullOrEmpty(parameters.Email) || x.BuyerEmail == parameters.Email) &&
				(!parameters.OrderId.HasValue || x.Id == parameters.OrderId) &&
				(string.IsNullOrEmpty(parameters.Status) || x.OrderStatus == ParseStatus(parameters.Status)))
		{
			// Add necessary includes
			AddInclude(x => x.OrderItems);
			AddInclude(x => x.DelieveryMehod);

			// Apply ordering
			AddOrderByDescending(x => x.OrderDate);

			// Apply paging
			ApplyPaging(parameters.PageSize * (parameters.PageIndex - 1), parameters.PageSize);
		}




		private static OrderStatus? ParseStatus(string status)
		{
			if (Enum.TryParse<OrderStatus>(status, true, out var statusValue)) return statusValue;
			return null;
		}
	}
}