using Skinet_Store.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public class BaseSpecification<T> : ISpecification<T>
	{
		protected BaseSpecification() : this(x => true)
		{
		}
		private readonly Expression<Func<T, bool>>? _criteria;
		public BaseSpecification(Expression<Func<T, bool>>? criteria)
        {
            _criteria = criteria ?? throw new ArgumentNullException(nameof(criteria));
		}

		public Expression<Func<T, bool>>? Criteria => _criteria;

		public Expression<Func<T, object>>? OrderBy { get; private set; }

		public Expression<Func<T, object>>? OrderByDescending { get; private set; }

		public int Take { get; private set; }

		public int Skip { get; private set; }

		public bool isPagingEnabled { get; private set; }

		public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

		public List<string> IncludeStrings { get; } = new List<string>();

		protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
		{
			OrderBy = orderByExpression;
		}
		protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
		{
			OrderByDescending = orderByDescExpression;
		}

		protected void ApplyPaging(int skip, int take)
		{
			Skip = skip;
			Take = take;
			isPagingEnabled = true;
		}

		public IQueryable<T> ApplyCriteria(IQueryable<T> query)
		{
			if(Criteria != null)
			{
				query = query.Where(Criteria);
			}

			return query;
		}

		public void AddInclude(Expression<Func<T, object>> includeExpression)
		{
			if (includeExpression == null)
			{
				throw new ArgumentNullException(nameof(includeExpression), "Include expression cannot be null.");
			}

			Includes.Add(includeExpression);
		}
		protected void AddInclude(string includeString)
		{
			IncludeStrings.Add(includeString);
		}
	}

	public class BaseSpecification<T, TResult> : BaseSpecification<T>, ISpecification<T, TResult>
	{
		protected BaseSpecification() : this(null!)
		{
		}
		public BaseSpecification(Expression<Func<T, TResult>>? select) : base()
		{
			select = select ?? throw new ArgumentNullException(nameof(select));
		}
		public bool isDistinct { get; private set; }

		public Expression<Func<T, TResult>>? Select { get; private set;}

		protected void AddSelect(Expression<Func<T, TResult>>? selectExpression)
		{
			Select = selectExpression;
		}

		protected void AddDistinct()
		{
			isDistinct = true;
		}


	}
}
