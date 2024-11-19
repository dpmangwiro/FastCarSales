namespace FastCarSales.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using Contracts;

    public class PostsListViewModel : PagingViewModel, ISortableModel
    {
        public IEnumerable<PostInListViewModel> Posts { get; init; } = new List<PostInListViewModel>();

        public PostsSorting Sorting { get; set; }
    }
}