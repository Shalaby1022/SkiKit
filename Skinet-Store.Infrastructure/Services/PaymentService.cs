using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using Stripe;
using Stripe.Issuing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Product = Skinet_Store.Core.Entities.Product;

namespace Skinet_Store.Infrastructure.Services
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration _config;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICartService _cartService;
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IGenericRepository<DelieveryMehod> _delieveryMethod;
		private readonly ILogger<PaymentService> _logger;

		public PaymentService(
			IConfiguration config,
			IUnitOfWork unitOfWork,
			ICartService cartService,
			IGenericRepository<Product> productRepo,
			IGenericRepository<DelieveryMehod> delieveryMethod,
			ILogger<PaymentService> logger)
		{
			_config = config ?? throw new ArgumentNullException(nameof(config));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
			_productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
			_delieveryMethod = delieveryMethod ?? throw new ArgumentNullException(nameof(delieveryMethod));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
		{
			StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

			var cart = await _cartService.GetCartAsync(cartId);

			if (cart == null)
			{
				_logger.LogError($"Cart with ID {cartId} not found.");
				return null;
			}

			// Log the cart object for debugging
			_logger.LogInformation($"Cart: {System.Text.Json.JsonSerializer.Serialize(cart)}");

			var shippingPrice = 0m;

			// Check if DeleieryMethodId is provided
			if (cart.DeleieryMethodId.HasValue)
			{
				var deliveryMethod = await _unitOfWork.Repository<DelieveryMehod>().GetByIdAsync((int)cart.DeleieryMethodId);

				if (deliveryMethod == null)
				{
					_logger.LogError($"Delivery method with ID {cart.DeleieryMethodId} not found.");
				}
				else
				{
					shippingPrice = deliveryMethod.Price;
				}
			}
			else
			{
				_logger.LogInformation("No delivery method selected. Shipping price will be 0.");
			}

			// Validate cart items and update prices if necessary
			foreach (var item in cart.Items)
			{
				var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

				if (productItem == null)
				{
					_logger.LogError($"Product with ID {item.Id} not found.");
					return null; // Exit if a product is not found
				}

				if (productItem.Price != item.Price)
				{
					_logger.LogInformation($"Updating price for product {item.Id} from {item.Price} to {productItem.Price}.");
					item.Price = productItem.Price;
				}
			}

			var service = new PaymentIntentService();
			PaymentIntent? intent = null;

			try
			{
				// Check if PaymentIntentId is missing or invalid
				if (string.IsNullOrEmpty(cart.PaymentIntentId) || !cart.PaymentIntentId.StartsWith("pi_"))
				{
					// Create a new PaymentIntent
					var options = new PaymentIntentCreateOptions
					{
						Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
						Currency = "usd",
						PaymentMethodTypes = new List<string> { "card" }
					};

					intent = await service.CreateAsync(options);
					cart.PaymentIntentId = intent.Id; // Set the PaymentIntentId
					cart.ClientSecret = intent.ClientSecret;

					_logger.LogInformation($"Created new PaymentIntent with ID {intent.Id}.");
				}
				else
				{
					// Update the existing PaymentIntent
					var options = new PaymentIntentUpdateOptions
					{
						Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
					};

					await service.UpdateAsync(cart.PaymentIntentId, options);

					_logger.LogInformation($"Updated PaymentIntent with ID {cart.PaymentIntentId}.");
				}
			}
			catch (StripeException ex)
			{
				_logger.LogError($"Stripe error: {ex.Message}");
				return null; // Exit if there's a Stripe error
			}

			await _cartService.CreateCartAsync(cart);

			return cart;
		}

		public async Task<string> RefundPayment(string paymentIntentId)
		{
			StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

			var options = new RefundCreateOptions
			{
				PaymentIntent = paymentIntentId,
			};

			var refundServices = new RefundService();

			var result = await refundServices.CreateAsync(options);

			return result.Status;
		}
	}
}