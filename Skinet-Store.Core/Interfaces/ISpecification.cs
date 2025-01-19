using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Interfaces
{
	public interface ISpecification<T>
	{
		Expression<Func<T, bool>>? Criteria { get; }
		Expression<Func<T, object>>? OrderBy { get; }
		Expression<Func<T, object>>? OrderByDescending { get; }

	
	}

	public interface ISpecification<T , TResult> : ISpecification<T>
	{
		bool isDistinct { get; }
		Expression<Func<T , TResult>>? Select { get; }

	
	}
}
