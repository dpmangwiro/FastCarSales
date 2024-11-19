namespace FastCarSales.Web.ViewModels.Posts
{
    using Cars;

    public class SinglePostViewModel
    {
        public SingleCarViewModel Car { get; init; } = new SingleCarViewModel();
		public int PostID { get; init; }
		public string PublishedOn { get; init; }

        public string SellerName { get; set; }

        public string SellerPhoneNumber { get; set; }
        public string SellerEmail { get; set; }
    }
}