namespace FastCarSales.Web.ViewModels.Posts
{
    using Cars;

    public class PostInLatestListViewModel
    {
        public LatestPostsCarViewModel Car { get; init; } = new LatestPostsCarViewModel();

        public int PostID { get; init; }
        public string PublishedOn { get; init; }

        
    }
}