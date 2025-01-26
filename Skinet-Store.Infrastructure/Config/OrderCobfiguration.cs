using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet_Store.Core.Entities.OrderAggregates;

namespace Skinet_Store.Infrastructure.Config
{
	public class OrderCobfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(x => x.ShipToAddress, o => o.WithOwner());
			builder.OwnsOne(x => x.paymentSummary, o => o.WithOwner());
			builder.HasMany(x => x.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
			builder.Property(p => p.SubTotal).HasColumnType("decimal(18,2)");

			builder.Property(x => x.OrderStatus).HasConversion(
				o => o.ToString(),
				o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
				);


			builder.Property(x => x.OrderDate).HasConversion(
					D => D.ToUniversalTime(),
					D => DateTime.SpecifyKind(D, DateTimeKind.Utc));
		}

	}


}
