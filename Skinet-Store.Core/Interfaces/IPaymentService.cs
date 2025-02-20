﻿using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Interfaces
{
	public interface IPaymentService
	{
		Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
		Task<string> RefundPayment(string paymentIntentId);
	}
}
