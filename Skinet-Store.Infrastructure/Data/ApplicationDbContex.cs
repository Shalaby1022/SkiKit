using Microsoft.EntityFrameworkCore;
using Skinet_Store.Core.Entities;
using Skinet_Store.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Data
{
	public class ApplicationDbContex : DbContext
	{
		public ApplicationDbContex(DbContextOptions<ApplicationDbContex> options) : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ArgumentNullException.ThrowIfNull(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
		}

	
	}
}
