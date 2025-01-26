using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities.OrderAggregates
{
	public class PaymentSummary
	{
        public int Last4 { get; set; }
		public required string Brand { get; set; }
		public int ExpiryMonth { get; set; }
		public int ExpiryYear { get; set; }

	}
}
