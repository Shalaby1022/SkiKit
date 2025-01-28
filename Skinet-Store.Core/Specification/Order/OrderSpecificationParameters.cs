using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification.Order
{
	public  class OrderSpecificationParameters : PagingSpecifications
	{
        public OrderSpecificationParameters()
        {
				
        }
        public int? OrderId { get; set; }
		public string? Email { get; set; }

		public string? Status { get; set; }



	}
}
