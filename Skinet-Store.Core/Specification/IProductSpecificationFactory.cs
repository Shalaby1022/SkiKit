using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public interface IProductSpecificationFactory
	{
		ISpecification<Product> Create(ProductSpecificationParameters specificationParameters);
	}
}
