using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Skinet_Store.Core.Entities;
using System.Security.Authentication;
using System.Security.Claims;

namespace Skinet_Store.Extensions
{
	public static class ClaimsPrincipleExtensions
	{
		public static async Task<ApplicationUser> GetUserByEmail(this UserManager<ApplicationUser> userManager
																	  ,ClaimsPrincipal user)
		{
			var email = await user.GetEmail();

			var userToReturn = await userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

			if (userToReturn == null) throw new AuthenticationException("User Not Found");

			return userToReturn;

		}

		public static async Task<ApplicationUser> GetUserByEmailWithAddress(this UserManager<ApplicationUser> userManager
																	  , ClaimsPrincipal user)
		{
			var email = await user.GetEmail();

			var userToReturn = await userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email);

			if (userToReturn == null) throw new AuthenticationException("User Not Found");

			return userToReturn;

		}

		public static async Task<string> GetEmail(this ClaimsPrincipal principal)

		{
			var email = principal.FindFirstValue(ClaimTypes.Email);

			if(email == null) throw new AuthenticationException("Email Calims Not Found");

			return email;

		}

	
	}
}
