
namespace Skinet_Store.Core.Specification.Order
{

	public class OrderSpecification : BaseSpecification<Skinet_Store.Core.Entities.OrderAggregates.Order>
	{
		public OrderSpecification(OrderSpecificationParameters parameters)
			: base(x => (string.IsNullOrEmpty(parameters.Email) || x.BuyerEmail == parameters.Email) &&
						(!parameters.OrderId.HasValue || x.Id == parameters.OrderId))
		{
			AddInclude(x => x.OrderItems);
			AddInclude(x => x.DelieveryMehod);
			AddOrderByDescending(x => x.OrderDate);
		}
	}
}