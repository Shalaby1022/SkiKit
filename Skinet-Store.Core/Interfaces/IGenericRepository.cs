using Skinet_Store.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Interfaces
{
	public interface IGenericRepository<T> where T : BaseEntity
	{
		Task<IReadOnlyList<T>> GetAllAsync();
		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
		Task<IReadOnlyList<T>> GetEntitiesWithSpec(ISpecification<T> spec);
		Task<IReadOnlyList<T>> GetAllWithSpecAsync<TResult>(ISpecification<T , TResult> spec);
		Task<IReadOnlyList<T>> GetEntitiesWithSpec<TResult>(ISpecification<T , TResult> spec);
		Task<T> GetByIdAsync(int id);
		Task<T> CreateAsync(T entity);
		Task CreateRangeAsync(IEnumerable<T> entities);
		Task<bool> UpdateAsync(T entity);
		Task<bool> DeleteAsync(T entity);
		Task<bool> EntityExistsAsync(int id);
		Task<bool> SaveChangesAsync();

	}
}
