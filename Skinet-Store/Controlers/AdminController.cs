using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Controller;
using Skinet_Store.Core.Entities.OrderAggregates;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Core.Specification;
using Skinet_Store.Core.Specification.Order;
using Skinet_Store.DTOs;
using Skinet_Store.Extensions;
using Skinet_Store.RequestHelpers;

namespace Skinet_Store.Controlers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles ="Admin")]
	public class AdminController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<ProductController> _logger;
		private readonly IPaymentService _paymentService;

		public AdminController(
								IUnitOfWork unitOfWork,
								 ILogger<ProductController> logger ,
								 IPaymentService paymentService
								)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

        [HttpGet("orders")]

		public async Task<ActionResult<OrderDto>> GetAllOrders([FromQuery] OrderSpecificationParameters orderSpecificationParameters)
		{


			try
			{
				var spec = new OrderSpecification(orderSpecificationParameters);

				var orderList = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
				var mappedOrderList = orderList.Select(x => x.ToDto()).ToList().AsReadOnly();
				var count = await _unitOfWork.Repository<Order>().CountAsync(spec);
				var pagination = new Pagination<OrderDto>(orderSpecificationParameters.PageIndex,
														orderSpecificationParameters.PageSize,
														count,
														mappedOrderList);

			


				return Ok(pagination);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An Error Occurred While Retrieving The Products Info {nameof(GetAllOrders)}");
				return StatusCode(500);
			}

		}

		[HttpPost("order/refund/{orderId}")]
		public async Task<ActionResult<OrderDto>> RefundAnOrder(int orderId)
		{
			var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);


			if (order == null)
			{
				return BadRequest("Order can't be found ");
			}
			if (order.OrderStatus == OrderStatus.Pending)
			{
				return BadRequest("No payment DONE TO Be refunded");

			}

			var result = await _paymentService.RefundPayment(order.PaymentIntentId);

			if (result == "succeeded")
			{
				order.OrderStatus = OrderStatus.Refunded;

				return order.ToDto();

			}


			return BadRequest(result);


		}

	}
}
