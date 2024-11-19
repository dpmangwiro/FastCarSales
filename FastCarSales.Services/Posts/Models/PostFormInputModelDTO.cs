namespace FastCarSales.Services.Posts.Models
{
    using System.Collections.Generic;
    using Cars.Models;

    public class PostFormInputModelDTO
    {
        public CarFormInputModelDTO Car { get; set; }
		public int PostID { get; init; }
		public IEnumerable<int> SelectedExtrasIds { get; set; } = new HashSet<int>();
        
        public string SellerName { get; set; }
        
        public string SellerPhoneNumber { get; set; }

        public string SellerEmail { get; set; }

	}
}
