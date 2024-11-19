using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.Web.ViewModels.Posts;

namespace FastCarSales.Client.Events
{
    public class BeginSearchArgs
    {        
        public SearchCarInputModel SearchInput { get; }
        public int PageNumber { get; } = 1;
        public int Sorting { get; }   = 0;
        public BeginSearchArgs(SearchCarInputModel searchInput, int pageNumber, int sorting)
        {
            SearchInput = searchInput;
            PageNumber = pageNumber;
            Sorting = sorting;
        }
    }
}
