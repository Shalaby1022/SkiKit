using System.ComponentModel.DataAnnotations;

namespace Skinet_Store.Core.Entities
{
	public class Address : BaseEntity
	{
		[Required]
		public string Line1 { get; set; } = string.Empty;

		public string Line2 { get; set; } = string.Empty; 

		[Required]
		public string City { get; set; } = string.Empty;

		[Required]
		public string State { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Postal Code")]
		public string PostalCode { get; set; } = string.Empty;
	}
}
