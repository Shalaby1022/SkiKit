using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Controller;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Entities.OrderAggregates;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Core.Specification;
using Skinet_Store.Core.Specification.Order;
using Skinet_Store.DTOs;
using Skinet_Store.Extensions;

namespace Skinet_Store.Controlers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json", "application/xml")]
	[Consumes("application/json", "application/xml")]
	[Authorize]

	public class OrderController : ControllerBase
	{
		//private readonly IProductRepository _productRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<ProductController> _logger;
		private readonly ICartService _cartService;

		public OrderController(ICartService cartService,
								 IUnitOfWork unitOfWork,
								 ILogger<ProductController> logger)
		{
			_cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}


		[HttpPost(Name = nameof(CreateOrder))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
		{
			try
			{
				// coz it's authorized

				var email = await User.GetEmail();

				// get the cart
				var cart = await _cartService.GetCartAsync(orderDto.CartId);

				if (cart == null || cart.Items.Count == 0)
				{
					return BadRequest("Cart Is Empty");
				}


				// check if the payment intent id is there

				if (cart.PaymentIntentId == null)
				{
					return BadRequest("Payment Intent Id Is Missing");
				}

				// create an items list
				var items = new List<OrderItem>();
				foreach (var item in cart.Items)
				{
					var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
					if (productItem == null)
					{
						return BadRequest("Product Not Found");
					}

					var itemOrdered = new ProductItemOrdered
					{
						ProductId = item.Id,
						ProductName = item.ProductName,
						PictureUrl = item.PictureUrl
					};

					var orderItem = new OrderItem
					{
						ProductItemOrdered = itemOrdered,
						Price = productItem.Price,
						Quantity = item.Quantity
					};

					items.Add(orderItem);
				}

				var deliveryMethod = await _unitOfWork.Repository<DelieveryMehod>().GetByIdAsync(orderDto.DelieveryMehodId);
				if (deliveryMethod == null)
				{
					return BadRequest("Delivery Method Not Found");
				}

				var order = new Order
				{
					BuyerEmail = email,
					OrderDate = DateTime.UtcNow,
					ShipToAddress = orderDto.ShippingAdress,
					DelieveryMehod = deliveryMethod,
					OrderItems = items,
					SubTotal = items.Sum(item => item.Price * item.Quantity) + deliveryMethod.Price,
					PaymentIntentId = cart.PaymentIntentId,
					paymentSummary = orderDto.PaymentSummary
				};

				var createdOrder = await _unitOfWork.Repository<Order>().CreateAsync(order);

				var mappedOrder = createdOrder.ToDto();	


				return CreatedAtRoute(nameof(GetOrderForUserById), new { orderId = mappedOrder.OrderId }, mappedOrder);


			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"An Error Occurred While Creating The Order {nameof(CreateOrder)}");
				return StatusCode(500);
			}
		}
		

		[HttpGet(Name = "GetOrdersForUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<OrderDto>> GetOrdersForUser()
		{
			var email = await User.GetEmail();

			var specificationParameters = new OrderSpecificationParameters
			{
				Email = email
			};

			var spec = new OrderSpecification(specificationParameters);

			var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
			var mappedOrders = orders.Select(o => o.ToDto()).ToList();	


			return Ok(mappedOrders);
		}

		[HttpGet("{id}", Name = "GetOrderForUserById")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<OrderDto>> GetOrderForUserById(int id)
		{
			// Retrieve the user's email
			var email = await User.GetEmail();

			// Create the specification parameters with the user's email and the provided id
			var specificationParameters = new OrderSpecificationParameters
			{
				Email = email,
				OrderId = id
			};

			// Instantiate the specification using the parameters
			var spec = new OrderSpecification(specificationParameters);

			// Fetch the specific order using the specification
			var order = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

			var mappedOrder = order.Select(o => o.ToDto()).ToList();

			if (order == null)
			{
				return NotFound();
			}

			// Return the result
			return Ok(mappedOrder);
		}



	}

}
