﻿using Skinet_Store.Core.Entities;
using Skinet_Store.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.Data
{
	public class SpecificationEvaluator<T> where T : BaseEntity
	{
		public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
		{
			if (spec.Criteria != null)
			{
				query = query.Where(spec.Criteria);
			}
			if(spec.OrderBy != null)
			{
				query = query.OrderBy(spec.OrderBy);
			}
			if (spec.OrderByDescending != null)
			{
				query = query.OrderByDescending(spec.OrderByDescending);
			}

			return query; 

		}

		public static IQueryable<TResult> GetQuery<TSpec,TResult>(IQueryable<T> query, ISpecification<T , TResult> spec)
		{
			if (spec.Criteria != null)
			{
				query = query.Where(spec.Criteria);
			}
			if (spec.OrderBy != null)
			{
				query = query.OrderBy(spec.OrderBy);
			}
			if (spec.OrderByDescending != null)
			{
				query = query.OrderByDescending(spec.OrderByDescending);
			}
			var selectedQuery = query as IQueryable<TResult>;

			if (spec.Select != null)
			{
				selectedQuery = query.Select(spec.Select);
			}	

			if(spec.isDistinct)
			{
				selectedQuery = selectedQuery?.Distinct();
			}

			return selectedQuery ?? query.Cast<TResult>();

		}
	}
}
