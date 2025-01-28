using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Entities.OrderAggregates;
using Skinet_Store.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Data
{
	public class ApplicationDbContex : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContex(DbContextOptions<ApplicationDbContex> options) : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<DelieveryMehod> DelieveryMethods { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
		public DbSet<Order> Orders { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ArgumentNullException.ThrowIfNull(modelBuilder);

		
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
		}



	}
}
