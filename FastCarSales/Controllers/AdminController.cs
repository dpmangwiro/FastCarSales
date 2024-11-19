using AutoMapper;
using FastCarSales.Services;
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
    public class AdminController : ControllerBase
    {
        #region Variable Declaration

        private const int PostsPerPage = 12;
        private IPostsService PostsService { get; set; } = null!;        
		private IUserService UserService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
        private IMapper Mapper { get; set; } = null!;
        private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

        #endregion

        public AdminController(IPostsService postsService, IUserService userSevice, IMapper mapper, IHostEnvironment environment)
        {
            PostsService = postsService;
            UserService = userSevice;
            Mapper = mapper;
            Environment = environment;
            this.mapperConfiguration = mapper.ConfigurationProvider;
        }

        [HttpGet("{pageNumber:int?}/{sorting:int?}/{filter:int?}")]
        public ActionResult<PostsListAdminAreaViewModel> All(int? pageNumber, int? sorting, int? filter)
        {
            try
            {
				int actualPageNumber = pageNumber ?? 1; int actualSorting = sorting ?? 0; int actualFilter = filter ?? 0;

				if (pageNumber <= 0)
                {
                    return this.NotFound();
                }

                var postsDTO = this.PostsService.GetAllPostsBaseInfo(page: actualPageNumber, postsPerPage: PostsPerPage, sort: actualSorting, filter: actualFilter);
                var postsViewModel = this.Mapper.Map<IEnumerable<BasePostInListDTO>, IEnumerable<PostInAdminAreaViewModel>>(postsDTO);
                var allPosts = this.PostsService.GetAllPostsCount();

                var postsListViewModel = new PostsListAdminAreaViewModel()
                {
                    PageNumber = actualPageNumber,
                    PostsPerPage = PostsPerPage,
                    PostsCount = allPosts,
                    Posts = postsViewModel,
                };

                if (pageNumber > postsListViewModel.PagesCount)
                {
                    return this.NotFound();
                }

                return Ok(postsListViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]              
        public async Task<ActionResult<bool>> ChangeVisibility([FromBody]int id)
        {
            await this.PostsService.ChangeVisibilityAsync(id);

            return true;
        }

		[HttpDelete("{id:int}")]
		public async Task<ActionResult<bool>> RestoreDeletedPost(int id)
		{
			await this.PostsService.RestoreDeletedPost(id);

			return true;
		}

		[HttpPut]
		public async Task<ActionResult<bool>> SetFeatured([FromBody]int id)
		{
			await this.PostsService.SetFeatured(id);

			return true;
		}

		[HttpGet("users/")]
        public async Task<ActionResult<IEnumerable<string>>> GetAdmins()
        {
            try
            {
                var admins = await UserService.GetAdmins();

                return Ok(admins);
            }
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
    }
}
