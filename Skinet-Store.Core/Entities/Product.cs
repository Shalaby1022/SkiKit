﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities
{
	public class Product : BaseEntity
	{
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; } 
		public string PictureUrl { get; set; } = string.Empty;
		public string Brand { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;
		public int QuantityInStock { get; set; }	

	}
}
