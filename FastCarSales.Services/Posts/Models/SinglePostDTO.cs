namespace FastCarSales.Services.Posts.Models
{
    using Cars.Models;

    public class SinglePostDTO
    {
        public SingleCarDTO Car { get; init; }

        public int PostID { get; init; }
		public string PublishedOn { get; init; }

		public string SellerName { get; set; }

        public string SellerPhoneNumber { get; set; }

        public string SellerEmail { get; set; }
    }
}