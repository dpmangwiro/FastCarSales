namespace FastCarSales.Web.ViewModels.Posts
{
    using System.Collections.Generic;

    public class LatestPostsViewModel
    {
        public IEnumerable<PostInLatestListViewModel> LatestPosts { get; init; } = new List<PostInLatestListViewModel>();
    }
}