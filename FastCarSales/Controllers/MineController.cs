using AutoMapper;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastCarSales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MineController : ControllerBase
	{
		private const int PostsPerPage = 12;
		private IPostsService PostsService { get; set; } = null!;
		private ICarsService CarsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;
		private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

		public MineController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
		{
			PostsService = postsService;
			CarsService = carsService;
			Mapper = mapper;
			Environment = environment;
			this.mapperConfiguration = mapper.ConfigurationProvider;
		}


		[Authorize]
		public ActionResult<PostByUserViewModel> Mine()
		{			
			var userId = this.GetCurrentUserId();
			var postsByUserDTOs = this.PostsService.GetPostsByUser(userId, 0).ToList();
            //var postsByUserDTOsForThisPage = this.PostsService.GetPostsByPage(postsByUserDTOs, pageNo, PostsPerPage);
            //var postsByUserViewModels = this.Mapper.Map<IEnumerable<PostByUserDTO>, IEnumerable<PostByUserViewModel>>(postsByUserDTOsForThisPage);
            var postsByUserViewModels = this.Mapper.Map<IEnumerable<PostByUserDTO>, IEnumerable<PostByUserViewModel>>(postsByUserDTOs);

            var postsByUserViewModel = new PostsByUserViewModel()
			{
				PageNumber = 1,
				PostsPerPage = PostsPerPage,
				PostsCount = postsByUserDTOs.Count,
				Posts = postsByUserViewModels,
			};
						
			return Ok(postsByUserViewModel);
		}

		[HttpGet("GetUserId")]
		private string? GetCurrentUserId()
		{
			//var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
			var userId = User.Claims.FirstOrDefault()?.Value;

			return userId;
		}


	}
}
