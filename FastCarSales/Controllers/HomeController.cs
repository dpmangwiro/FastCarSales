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
	public class HomeController : ControllerBase
	{
		private IPostsService PostsService { get; set; } = null!;
		private ICarsService CarsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;
		private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

		public HomeController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
		{
			PostsService = postsService;
			CarsService = carsService;
			Mapper = mapper;
			Environment = environment;
			this.mapperConfiguration = mapper.ConfigurationProvider;
		}

		// GET: api/<PostsController>
		[HttpGet("{Inclfeatured:bool}")]
		public ActionResult<LatestPostsViewModel> GetViewModel(bool Inclfeatured = false)
		{
			try
			{
				var latestPostsDTO = new List<PostInLatestListDTO>();

				if (Inclfeatured)
				{
					latestPostsDTO = (List<PostInLatestListDTO>)this.PostsService.GetLatestInclFeatured(5);
				}
				else
				{
					latestPostsDTO = (List<PostInLatestListDTO>)this.PostsService.GetLatestExclFeatured(5);
				}

				var latestPostsViewModel = this.Mapper.Map<IEnumerable<PostInLatestListDTO>, IEnumerable<PostInLatestListViewModel>>(latestPostsDTO);

				var latestPosts = new LatestPostsViewModel()
				{
					LatestPosts = latestPostsViewModel ?? new List<PostInLatestListViewModel>(),
				};

				return Ok(latestPosts);

			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database: More details: " + ex);
			}

		}





	}
}
