using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public class ProductSpecification : BaseSpecification<Product>
	{
		public ProductSpecification(ProductSpecificationParameters specificationParameters) : base(x =>
			(specificationParameters.Brands.Count == 0 || specificationParameters.Brands.Contains(x.Brand)) &&
			(specificationParameters.Types.Count == 0 || specificationParameters.Types.Contains(x.Type)))
		{

			// paging 
			ApplyPaging(specificationParameters.PageSize * (specificationParameters.PageIndex - 1), 
				specificationParameters.PageSize);


			switch (specificationParameters.Sort)
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
