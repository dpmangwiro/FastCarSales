using AutoMapper;
using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace FastCarSales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SearchController : ControllerBase
	{
		private const int PostsPerPage = 12;
		private IPostsService PostsService { get; set; } = null!;
		private ICarsService CarsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;
		private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

		public SearchController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
		{
			this.PostsService = postsService;
			CarsService = carsService;
			this.Mapper = mapper;
			Environment = environment;
			this.mapperConfiguration = mapper.ConfigurationProvider;
		}
		
		/// <summary>
		/// get action turned into post so that we can have complex parameters passed
		/// </summary>
		/// <param name="searchPostInputModel"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult<PostsListViewModel> All([FromBody] SearchCarInputModel searchPostInputModel, int pageId = 1, int sorting = 0)
		{
			try
			{				
				if (pageId <= 0)
				{
					return this.NotFound();
				}

				//changing all this to SearchCarInputModel
				var searchPostDTO = this.Mapper.Map<SearchCarInputModelDTO>(searchPostInputModel);
				var matchingPosts = this.PostsService.GetMatchingPosts(searchPostDTO, sorting).ToList();
				var postsByPageDTOs = this.PostsService.GetPostsByPage(matchingPosts, pageId, PostsPerPage);
				var postsByPageAsViewModels = this.Mapper.Map<IEnumerable<PostInListDTO>, IEnumerable<PostInListViewModel>>(postsByPageDTOs);

				var postsListViewModel = new PostsListViewModel()
				{
					PageNumber = pageId,
					PostsPerPage = PostsPerPage,
					PostsCount = matchingPosts.Count,
					Posts = postsByPageAsViewModels,
				};

				if (pageId > postsListViewModel.PagesCount)
				{
					return this.NotFound();
				}

				return postsListViewModel;

			}
			catch (Exception ex)
			{				
				return StatusCode(StatusCodes.Status500InternalServerError, "Error searching car: " + ex.Message);
			}
		}



	}
}
