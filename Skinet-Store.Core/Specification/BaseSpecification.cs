﻿using Skinet_Store.Core.Interfaces;
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
		protected BaseSpecification() : this(null)
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

		public bool isDistinct { get; private set; }

		protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
		{
			OrderBy = orderByExpression;
		}
		protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
		{
			OrderByDescending = orderByDescExpression;
		}

		protected void AddDistinct()
		{
			isDistinct = true;
		}
	}

	public class BaseSpecification<T, TResult> : BaseSpecification<T>, ISpecification<T, TResult>
	{
		public BaseSpecification(Expression<Func<T, TResult>>? select) : base(criteria: null)
		{
			select = select ?? throw new ArgumentNullException(nameof(select));
		}

		public Expression<Func<T, TResult>>? Select { get; private set;}

		protected void AddSelect(Expression<Func<T, TResult>>? selectExpression)
		{
			Select = selectExpression;
		}
	}
}
