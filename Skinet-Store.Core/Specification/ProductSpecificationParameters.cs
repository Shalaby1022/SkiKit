using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Core.Specification
{
	public class ProductSpecificationParameters
	{
		private List<string> _brands = [];
		private List<string> _types = [];
		private int _pageSize = 10;
		private const int MaxPageSize = 50;
		private string? _searchQUery;


		public List<string> Brands
		{
			get => _brands;
			set => _brands = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
		}

		public List<string> Types
		{
			get => _types;
			set => _types = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
		}

		public string? Sort { get; set; }

		// pagination properties
		private int _pageIndex = 1; // Default page index
		public int PageIndex
		{
			get => _pageIndex;
			set => _pageIndex = (value < 1) ? 1 : value;
		}

		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}


		public string SearchQuery
		{
			get => _searchQUery ?? "";
			set => _searchQUery = value;
		}



	}
}
