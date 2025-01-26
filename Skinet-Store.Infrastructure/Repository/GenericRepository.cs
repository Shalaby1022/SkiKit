using Microsoft.EntityFrameworkCore;
using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using Skinet_Store.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly ApplicationDbContex _context;
		private readonly DbSet<T> _dbSet;

		public GenericRepository(ApplicationDbContex context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_dbSet = _context.Set<T>();
		}

		public async Task<int> CountAsync(ISpecification<T> spec)
		{
			var query = _context.Set<T>().AsQueryable();
			query = spec.ApplyCriteria(query);

			return await query.CountAsync();

		}

		public async Task<T> CreateAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			await _dbSet.AddAsync(entity);
			return entity;
		}

		public async Task CreateRangeAsync(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);

		}

		public async Task<bool> DeleteAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			_dbSet.Remove(entity);
			return true;
		}

		public async Task<bool> EntityExistsAsync(int id)
		{
			return await _dbSet.AnyAsync(p => p.Id == id);
		}

		public async Task<IReadOnlyList<T>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
		{
			return await ApplySpecification(spec).ToListAsync();
		}

		public async Task<IReadOnlyList<TResult>> GetAllWithSpecAsync<TResult>(ISpecification<T, TResult> spec)
		{
			return await ApplySpecification(spec).ToListAsync();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		public async Task<IReadOnlyList<T>> GetEntitiesWithSpec(ISpecification<T> spec)
		{
			return (IReadOnlyList<T>)await ApplySpecification(spec).FirstOrDefaultAsync();
		}

		public async Task<IReadOnlyList<T>> GetEntitiesWithSpec<TResult>(ISpecification<T, TResult> spec)
		{
			return (IReadOnlyList<T>)await ApplySpecification(spec).FirstOrDefaultAsync();
		}

		public async Task<bool> UpdateAsync(T entity)
		{
			if (entity == null) throw new ArgumentNullException(nameof(entity));

			_dbSet.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
			return true;
		}

		private IQueryable<T> ApplySpecification(ISpecification<T> spec)
		{
			return SpecificationEvaluator<T>.GetQuery(_dbSet, spec);
		}


		private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
		{
			return SpecificationEvaluator<T>.GetQuery<T, TResult>(_dbSet, spec);

		}
	}
}
