using AutoMapper;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastCarSales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AllPostsController : ControllerBase
	{
		private const int PostsPerPage = 12;
		private IPostsService PostsService { get; set; } = null!;
		private ICarsService CarsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;
		private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

		public AllPostsController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
		{
			this.PostsService = postsService;
			CarsService = carsService;
			this.Mapper = mapper;
			Environment = environment;
			this.mapperConfiguration = mapper.ConfigurationProvider;
		}

		[HttpGet]
		public ActionResult<PostsListViewModel> All(int id = 1, int sorting = 0)
		{
			try
			{
				if (id <= 0)
				{
					return this.NotFound();
				}

				SearchPostInputModel searchPostInputModel = new SearchPostInputModel();
				////car must be null if we are searching all items
				//searchPostInputModel.Car = null;

				var searchPostDTO = this.Mapper.Map<SearchPostDTO>(searchPostInputModel);
				var matchingPosts = this.PostsService.GetMatchingPosts(searchPostDTO, sorting).ToList();
				var postsByPageDTOs = this.PostsService.GetPostsByPage(matchingPosts, id, PostsPerPage);
				var postsByPageAsViewModels = this.Mapper.Map<IEnumerable<PostInListDTO>, IEnumerable<PostInListViewModel>>(postsByPageDTOs);

				var postsListViewModel = new PostsListViewModel()
				{
					PageNumber = id,
					PostsPerPage = PostsPerPage,
					PostsCount = matchingPosts.Count,
					Posts = postsByPageAsViewModels,
				};

				if (id > postsListViewModel.PagesCount)
				{
					return this.NotFound();
				}

				return postsListViewModel;
			}
			catch (Exception ex)
			{
				//TempData[WebConstants.ErrorMessageKey] = ex.Message;

				//var searchCarInputModel = searchPostInputModel.Car;
				//var searchCarInputModelDTO = this.Mapper.Map<SearchCarInputModelDTO>(searchCarInputModel);

				//this.CarsService.FillInputCarBaseProperties(searchCarInputModelDTO);

				//searchCarInputModel = this.Mapper.Map<SearchCarInputModel>(searchCarInputModelDTO);
				//searchPostInputModel.Car = searchCarInputModel;

				//return Ok("Search", searchPostInputModel);

				return StatusCode(StatusCodes.Status500InternalServerError, "Error searching car: " + ex.Message);
			}
		}
	}
}
