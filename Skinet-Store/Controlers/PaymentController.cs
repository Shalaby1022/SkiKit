using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;

namespace Skinet_Store.Controlers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICartService _cartService;
		private readonly IConfiguration _config;

		public PaymentController(IPaymentService paymentService,
								IUnitOfWork unitOfWork,
								ICartService cartService, 
								IGenericRepository<DelieveryMehod> delieveryGenericRepo)
		{
			_paymentService = paymentService ?? throw new System.ArgumentNullException(nameof(paymentService));
			_cartService = cartService ?? throw new System.ArgumentNullException(nameof(cartService));
			_unitOfWork = unitOfWork ?? throw new System.ArgumentNullException(nameof(unitOfWork));
		}

		[Authorize]
		[HttpPost("{cartId}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
		{
			var cart = await _cartService.GetCartAsync(cartId);
			if (cart == null) return BadRequest("Cart not found");

			var service = await _paymentService.CreateOrUpdatePaymentIntent(cartId);
			if (service == null) return BadRequest("Problem with your cart");


			return Ok(service);

		}
		[HttpGet("delivery-methods")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]

		public async Task<ActionResult<IReadOnlyList<DelieveryMehod>>> GetDeliveryMethods()
		{
			var deliveryMethods = await _unitOfWork.Repository<DelieveryMehod>().GetAllAsync();
			return Ok(deliveryMethods);
		}

	}
}
