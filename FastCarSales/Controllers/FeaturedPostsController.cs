using AutoMapper;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Services.Posts;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastCarSales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FeaturedPostsController : ControllerBase
	{
		private IPostsService PostsService { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;

		public FeaturedPostsController(IPostsService postsService, IMapper mapper)
		{
			PostsService = postsService;
			Mapper = mapper;
		}



		// GET: api/<PostsController>
		[HttpGet]
		public ActionResult<IEnumerable<SinglePostViewModel>> Get()
		{
			try
			{
				var singlePosts = this.PostsService.GetFeatured();

				if (singlePosts == null)
				{
					return this.NotFound();
				}
				
				var singlePostViewModels = this.Mapper.Map<IEnumerable<SinglePostDTO>, IEnumerable<SinglePostViewModel>>(singlePosts);

				return Ok(singlePostViewModels);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database: More details: " + ex);
			}

		}


	}
}
