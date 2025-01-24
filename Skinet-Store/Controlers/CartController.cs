using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;

namespace Skinet_Store.Controlers
{
	[Route("api/[controller]")]
	[ApiController]
	[Produces("application/json", "application/xml")]
	[Consumes("application/json", "application/xml")]
	public class CartController : ControllerBase
	{
		private readonly ICartService _cartService;

		public CartController(ICartService cartService)
        {
			_cartService = cartService ?? throw new System.ArgumentNullException(nameof(cartService));
		}

		[HttpGet("{cartId}", Name = nameof(GetCart))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<ActionResult<ShoppingCart>> GetCart(string cartId)
		{
			var cart = await _cartService.GetCartAsync(cartId);
			return Ok(cart ?? new ShoppingCart { Id = cartId});
		}

		[HttpPost(Name = nameof(UpdatedCat))]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<ShoppingCart>> UpdatedCat(ShoppingCart cart)
		{
			var createdCart = await _cartService.CreateCartAsync(cart);
			return CreatedAtAction(nameof(GetCart), new { cartId = createdCart.Id }, createdCart);
		}

		[HttpDelete("{cartId}", Name = nameof(DeleteCart))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<ActionResult<bool>> DeleteCart(string cartId)
		{
			var deleted = await _cartService.DeleteCartAsync(cartId);
			if (!deleted) return BadRequest("Cart isn't found!!!!!!!!!!!!!!");
			return NoContent();
		}

	}
}
