using AutoMapper;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Services.Posts;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastCarSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeletePostController : ControllerBase
    {
        private const int PostsPerPage = 12;
        private IPostsService PostsService { get; set; } = null!;
        private ICarsService CarsService { get; set; } = null!;
        private IHostEnvironment Environment { get; set; } = null!;
        private IMapper Mapper { get; set; } = null!;
        private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

        public DeletePostController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
        {
            PostsService = postsService;
            CarsService = carsService;
            Mapper = mapper;
            Environment = environment;
            this.mapperConfiguration = mapper.ConfigurationProvider;
        }


        [Authorize]
        [HttpGet("{postId:int}")]
        public ActionResult<PostByUserViewModel> Get(int postId)
        {
            var userId = this.GetCurrentUserId();
            
            var postDTO = this.PostsService.GetBasicPostInformationById(postId);

            if (postDTO == null)
            {
                return this.NotFound();
            }

            var postCreatorId = this.PostsService.GetPostCreatorId(postId);

            if ((userId != postCreatorId) && !UserIsAdmin )
            {
                return this.Unauthorized();
            }

            var postByUserViewModel = this.Mapper.Map<PostByUserViewModel>(postDTO);

            return postByUserViewModel;
        }

		bool UserIsAdmin => User.HasClaim(x => x.Value == "Admin");

		[HttpGet("GetUserId")]
        private string? GetCurrentUserId()
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var userId = User.Claims.FirstOrDefault()?.Value;

            return userId;
        }

		[Authorize]
		[HttpDelete("{id:int}")]			
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var userId = this.GetCurrentUserId();
			var postCreatorId = this.PostsService.GetPostCreatorId(id);
			
			if ((userId != postCreatorId) && !UserIsAdmin)
			{
				return this.Unauthorized();
			}

			try
			{
				await this.PostsService.DeletePostByIdAsync(id);
				return Ok();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database: More details: " + ex);
			}

		}


	}
}
