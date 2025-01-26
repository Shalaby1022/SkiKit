using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Config
{
	public class DelieveryMethodConfiguration : IEntityTypeConfiguration<DelieveryMehod>
	{
		public void Configure(EntityTypeBuilder<DelieveryMehod> builder)
		{
			builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
		}

	}
}
