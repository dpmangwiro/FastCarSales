namespace FastCarSales.Services.Data.Tests.Data
{
	using FastCarSales.Data.Models;
	using FastCarSales.Services.Posts.Models;
	using static TestDataConstants;
    using static Cars;
    public class Posts
    {
        public static Post ValidTestPublicPost => new()
        {
            Id = TestIdNumber,
            Car = ValidTestCar,
            CreatorId = TestUserId,
            IsPublic = true,
            SellerName = TestPostSellerName,
            SellerPhoneNumber = TestPostSellerPhoneNumber,
            SellerEmail = "abcde@gmail.com"
        };

        public static PostFormInputModelDTO ValidTestPostFormInputModelDTO => new()
        {
            SellerName = TestPostSellerName,
            SellerPhoneNumber = TestPostSellerPhoneNumber,
            SellerEmail = "adbce@gmail.com"
        };

        public static EditPostDTO ValidEditPostDTO => new()
        {
            Car = ValidUpdatedCatTestModel,
            SellerName = UpdatedTestPostSellerName,
            SellerPhoneNumber = UpdatedTestPostSellerPhoneNumber,
			SellerEmail = "adbce@gmail.com"
		};

        public static SearchPostDTO ValidSearchPostDTO => new()
        {
            Car = ValidSearchCarInputModelDTO,
        };
    }
}