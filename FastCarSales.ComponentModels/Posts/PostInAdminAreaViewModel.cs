namespace FastCarSales.Web.ViewModels.Posts
{
    using Cars;

    public class PostInAdminAreaViewModel
    {
        public BaseCarViewModel Car { get; init; }
		public int PostID { get; init; }
		public string PublishedOn { get; init; }

        public bool IsPublic { get; init; }

		public bool IsFeatured { get; init; }
		public bool IsDeleted { get; init; }
		public bool IsExpired { get; init; }
		public bool CanEmptyRecycleBin { get; init; }
		public string DeletedOn { get; init; }
	}
}
