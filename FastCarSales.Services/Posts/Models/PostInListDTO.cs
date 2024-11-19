namespace FastCarSales.Services.Posts.Models
{
    using Cars.Models;

    public class PostInListDTO
    {
        public CarInListDTO Car { get; init; }
		public int PostID { get; init; }
		public string PublishedOn { get; init; }
    }
}