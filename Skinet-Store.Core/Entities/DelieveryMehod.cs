using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities
{
	public class DelieveryMehod : BaseEntity
	{
		public required string ShortName { get; set; }  
		public required string DelieveryTime { get; set; }
		public required string Description { get; set; }
		public decimal Price { get; set; }

	}
}
