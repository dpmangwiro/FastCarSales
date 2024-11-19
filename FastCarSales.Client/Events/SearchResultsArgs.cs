using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.Web.ViewModels.Posts;

namespace FastCarSales.Client.Events
{
	public class SearchResultsArgs
	{
		public PostsListViewModel SearchResults {get; }
		public SearchCarInputModel SearchData { get; }
		public int PageNumber { get; } = 1;
		public int Sorting { get; } =0;
		public SearchResultsArgs(PostsListViewModel resultsModel, SearchCarInputModel searchData, int pageNumber, int sorting)
		{
			SearchResults = resultsModel;
			SearchData = searchData;
			PageNumber = pageNumber;
			Sorting = sorting;
		}
	}
}
