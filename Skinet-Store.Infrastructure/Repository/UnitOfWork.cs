using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Infrastructure.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContex _contex;
		private readonly ConcurrentDictionary<string , object> _repositories = new ConcurrentDictionary<string, object>();
		public UnitOfWork(ApplicationDbContex contex)
        {
			_contex = contex;
		}
        public async Task<bool> Complete()
		{
			return await _contex.SaveChangesAsync() > 0;
		}

		public void Dispose()
		{
			_contex.Dispose();
		}

		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{
			var type = typeof(TEntity).Name;

			return (IGenericRepository<TEntity>)_repositories.GetOrAdd(type, t =>
			{
				var repositoryType = typeof(GenericRepository<>);
				return Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _contex);

			});

		}
	}
}
