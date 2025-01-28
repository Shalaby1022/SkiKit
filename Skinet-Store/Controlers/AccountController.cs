using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Core.Entities;
using Skinet_Store.DTOs;
using System.Security.Claims;
using Skinet_Store.Extensions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Skinet_Store.Controlers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(SignInManager<ApplicationUser> signInManager , UserManager<ApplicationUser> userManager)
		{
			_signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
			_userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterDto registerDto)
		{
			var user = new ApplicationUser
			{
				UserName = registerDto.Email,
				Email = registerDto.Email,
				FirstName = registerDto.FirstName,
				LastName = registerDto.LastName
			};

			var result = await _signInManager.UserManager.CreateAsync(user, registerDto.Password);

			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}

				return ValidationProblem();
			}

			return Ok();

		}

		[Authorize]
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return NoContent();
		}

		
		[HttpGet("user-info")]
		public async Task<IActionResult> GetUserInfo()
		{
			// checking if user is authenticated

			if (!User.Identity.IsAuthenticated)
			{
				return NoContent();
			}
			var user = await _userManager.GetUserByEmailWithAddress(User);

			// returning an anonymous object.
			return Ok(new
			{
				user.Email,
				user.FirstName,
				user.LastName,	
				user.Address,
				Roles = User.FindFirstValue(ClaimTypes.Role)
			});
		}

		[HttpGet]
		public ActionResult GetAuthState()
		{
			return Ok(User.Identity.IsAuthenticated);

		}

		[Authorize]
		[HttpPost("address")]
		public async Task<IActionResult> UpdateUserAddress(AddressDto addressDto)
		{
			var user = await _userManager.GetUserByEmailWithAddress(User);

			if (user.Address == null)
			{
				user.Address = addressDto.MapAddressDtoToAddress();
			}
			else
			{
				user.Address.UpdateFromDto(addressDto);
			}

			

			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded) return Ok(user.Address.MapAddressToAddressDto());

			return BadRequest("Problem updating the user");
		}
	}
}
