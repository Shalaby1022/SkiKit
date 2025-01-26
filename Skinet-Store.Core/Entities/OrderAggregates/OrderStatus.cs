using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Entities.OrderAggregates
{
	public enum OrderStatus
	{
		Pending,
		paymentRecevied,
		PaymentFailed,
		Accepted,
		Shipped,
		Delivered
	}
}
