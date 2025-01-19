﻿using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public class ProductSpecification : BaseSpecification<Product>
	{
		public ProductSpecification(string? brand, string? type, string? sort) : base(x =>
			(string.IsNullOrEmpty(brand) || x.Brand == brand) &&
			(string.IsNullOrEmpty(type) || x.Type == type))
		{
			switch(sort)
			{
				case "priceAsc":
					AddOrderBy(x => x.Price);
					break;
				case "priceDesc":
					AddOrderByDescending(x => x.Price);
					break;
				default:
					AddOrderBy(x => x.Name);
					break;
			}
		}
	}
}
