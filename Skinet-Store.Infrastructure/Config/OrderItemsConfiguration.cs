using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skinet_Store.Core.Entities.OrderAggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Config
{
	public class OrderItemsConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.OwnsOne(x => x.ProductItemOrdered, o => o.WithOwner());
			builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

		}
	}
}
