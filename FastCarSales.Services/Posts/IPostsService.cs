namespace FastCarSales.Services.Posts
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Models;
    using Data.Models;
    using Images.Models;
	using FastCarSales.Services.Cars.Models;

	public interface IPostsService
    {
        Task<int> CreateAsync(PostFormInputModelDTO inputPost, Car car, string userId, bool isPublic);

        IEnumerable<PostInListDTO> GetMatchingPosts(SearchPostDTO searchInputModel, int sortingNumber);
		IEnumerable<PostInListDTO> GetMatchingPosts(SearchCarInputModelDTO searchCarInputModel, int sortingNumber);

		IEnumerable<BasePostInListDTO> GetAllPostsBaseInfo(int page, int postsPerPage, int sort, int filter);

		int GetAllPostsCount();

        IEnumerable<T> GetPostsByPage<T>(IEnumerable<T> posts, int page, int postsPerPage);

        IEnumerable<PostByUserDTO> GetPostsByUser(string userId, int sortingNumber);

        SinglePostDTO GetSinglePostViewModelById(int postId, bool publicOnly = true);

        EditPostDTO GetPostFormInputModelById(int postId);

        IEnumerable<PostInLatestListDTO> GetLatestExclFeatured(int count);
		IEnumerable<PostInLatestListDTO> GetLatestInclFeatured(int count);
		IEnumerable<SinglePostDTO> GetFeatured();


		Task UpdateAsync(EditPostDTO input, bool isPublic);

        Task ChangeVisibilityAsync(int postId);

		Task SetFeatured(int postId);

		IEnumerable<ImageInfoDTO> GetCurrentDbImagesForAPost(int postId);

        PostByUserDTO GetBasicPostInformationById(int postId);

        string GetPostCreatorId(int postId);

        Task DeletePostByIdAsync(int postId);
		Task RestoreDeletedPost(int postId);

	}
}
