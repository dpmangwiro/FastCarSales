namespace FastCarSales.Services.Posts.Models
{
    using Cars.Models;

    public class PostByUserDTO
    {
        public CarByUserDTO Car { get; init; }
		public int PostID { get; init; }
		public string PublishedOn { get; init; }
    }
}