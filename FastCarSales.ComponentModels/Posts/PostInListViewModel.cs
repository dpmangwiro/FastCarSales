namespace FastCarSales.Web.ViewModels.Posts
{
    using Cars;

    public class PostInListViewModel
    {
        public CarInListViewModel Car { get; init; } = new CarInListViewModel();
		public int PostID { get; init; }
		public string PublishedOn { get; init; }
    }
}