using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Core.Entities;
using System.Security.Claims;

namespace Skinet_Store.Controlers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BuggyController : ControllerBase
	{
		[HttpGet("unauthorized", Name = nameof(GetunAutorized))]
		public IActionResult GetunAutorized()
		{
			return Unauthorized();
		}
		[HttpGet("badrequest", Name = nameof(GetBadRequest))]
		public IActionResult GetBadRequest()
		{
			return BadRequest("Not the typical request to proceed");
		}
		[HttpGet("notfound", Name = nameof(GetNotFound))]
		public IActionResult GetNotFound()
		{
			return NotFound();
		}
		[HttpGet("internalservererror", Name = nameof(GetInternalServerError))]
		public IActionResult GetInternalServerError()
		{
			throw new Exception("This is a test exception");
		}
		[HttpPost("validationerror", Name = nameof(GetValidationError))]
		public IActionResult GetValidationError(Product product)
		{
			return Ok();
		}

		[Authorize]
		[HttpGet("secret" , Name = nameof(GetSecret))]
		public IActionResult GetSecret()
		{
			var name = User.FindFirst(ClaimTypes.Name)?.Value;
			var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


			return Ok("Hello " + name + "  Your Id is " + id);
		}

		[Authorize(Roles=("Admin"))]
		[HttpGet("admin-secret", Name = nameof(GetAdminSecret))]
		public IActionResult GetAdminSecret()
		{
			var name = User.FindFirst(ClaimTypes.Name)?.Value;
			var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var isadmin = User.IsInRole("Admin");
			var roles = User.FindFirstValue(ClaimTypes.Role);

			return Ok("Hello " + name + "  Your Id is " + id +
					  " Am i an admin? " + isadmin + 
					  " so what's my Role here " + roles) ;
		}
	}
}
