namespace FastCarSales.Services.Posts.Models
{
    using Cars.Models;

    public class BasePostInListDTO
    {
        public BaseCarDTO Car { get; init; }
		public int PostID { get; init; }
		public DateTime PublishedOn { get; init; }

        public bool IsPublic { get; init; }
        public bool IsFeatured { get; init; }
		public bool IsDeleted { get; init; }
		public bool IsExpired { get; init; }
		public bool CanEmptyRecycleBin { get; init; }
		public DateTime? DeletedOn { get; init; }
	}
}