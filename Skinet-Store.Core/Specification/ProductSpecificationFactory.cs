﻿using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public class ProductSpecificationFactory : IProductSpecificationFactory
	{
		public ISpecification<Product> Create(string? brand, string? type, string? sort)
		{
			return new ProductSpecification(brand, type , sort);
		}
	}

}
