using System.ComponentModel.DataAnnotations;

namespace Skinet_Store.DTOs
{
	public class AddressDto
	{
		public string Line1 { get; set; } = string.Empty;
		public string Line2 { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public string State { get; set; } = string.Empty;
		public string PostalCode { get; set; } = string.Empty;
	}
}
