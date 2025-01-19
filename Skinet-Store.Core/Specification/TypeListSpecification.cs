using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public class TypeListSpecification : BaseSpecification<Product, string>
	{
		public TypeListSpecification() : base(p => p.Type) // Providing a default criteria
		{
			AddSelect(p => p.Type);
			AddDistinct();
		}
	}
}
