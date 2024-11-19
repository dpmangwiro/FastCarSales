namespace FastCarSales.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using Contracts;

    public class PostsByUserViewModel : PagingViewModel, ISortableModel
    {
        public IEnumerable<PostByUserViewModel> Posts { get; init; } = new List<PostByUserViewModel>();

        public PostsSorting Sorting { get; set; }
    }
}