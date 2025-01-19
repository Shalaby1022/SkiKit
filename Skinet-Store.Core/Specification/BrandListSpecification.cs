using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public class BrandListSpecification : BaseSpecification<Product , string>
	{
        public BrandListSpecification() : base(p => p.Brand)
        {
                AddSelect(p => p.Brand);
                AddDistinct();
		}
    }
}
