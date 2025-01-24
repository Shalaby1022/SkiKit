namespace Skinet_Store.RequestHelpers
{
	public class Pagination<T>
	{
		public Pagination(int pageSize , int pageIndex , int count , IReadOnlyList<T> data) 
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			Count = count;
			Data = data;
		}
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int? Count { get; set; }
		public IReadOnlyList<T> Data { get; set; }
	}
}
