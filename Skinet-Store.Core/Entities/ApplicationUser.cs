using Microsoft.AspNetCore.Identity;


namespace Skinet_Store.Core.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string FirstName { get; set; }= string.Empty;
		public string LastName { get; set; } = string.Empty;

		 
		public Address? Address { get; set; }


	}
}
