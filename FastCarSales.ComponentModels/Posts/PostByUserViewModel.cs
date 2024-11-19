namespace FastCarSales.Web.ViewModels.Posts
{
    using Cars;

    public class PostByUserViewModel
    {
        public CarByUserViewModel Car { get; init; } = new CarByUserViewModel();
		public int PostID { get; init; }
		public string PublishedOn { get; init; }
    }
}