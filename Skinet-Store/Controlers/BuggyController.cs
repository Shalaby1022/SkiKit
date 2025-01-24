 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skinet_Store.Core.Entities;

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

	}
}
