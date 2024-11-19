using AutoMapper;
using FastCarSales.ComponentModels.Posts;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Web.ViewModels.Cars;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper.QueryableExtensions;
using FastCarSales.Data.Models;
using FastCarSales.Data;
using FastCarSales.Services.Images;
using Microsoft.AspNetCore.Authorization;


namespace FastCarSales.Controllers
{
	
	[Route("api/[controller]")]
	[ApiController]
	public class PostsController : ControllerBase
	{
		private IPostsService PostsService { get; set; } = null!;
		private ICarsService CarsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;
		private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

		public PostsController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
		{
			PostsService = postsService;
			CarsService = carsService;
			Mapper = mapper;
			Environment = environment;
			this.mapperConfiguration = mapper.ConfigurationProvider;
		}


		[AllowAnonymous]
		[HttpGet("{Inclfeatured:bool}")]
		public ActionResult<LatestPostsViewModel> GetViewModel(bool Inclfeatured = false)
		{
			try
			{
				var latestPostsDTO = new List<PostInLatestListDTO>();

				if (Inclfeatured)
				{
					latestPostsDTO = (List<PostInLatestListDTO>)this.PostsService.GetLatestInclFeatured(8);
				}
				else
				{
					latestPostsDTO = (List<PostInLatestListDTO>)this.PostsService.GetLatestExclFeatured(8);
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

		[HttpGet("GetUserId")]
		private string? GetCurrentUserId()
		{
			//var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
			var userId = User.Claims.FirstOrDefault()?.Value;

			return userId;
		}

		// GET api/<PostsController>/5
		[HttpGet("{id:int}")]
		public ActionResult<SinglePostViewModel> Get(int id)
		{			
			var isAdmin = User.HasClaim(x => x.Value == "Admin");
			var publicOnly = !isAdmin;

			if (!isAdmin)
			{
				var userId = this.GetCurrentUserId();
				var postCreatorId = this.PostsService.GetPostCreatorId(id);

				publicOnly = userId != postCreatorId;
			}

			var singlePostDataDTO = this.PostsService.GetSinglePostViewModelById(id, publicOnly);

			if (singlePostDataDTO == null)
			{
				return this.NotFound();
			}

			var singlePostViewModel = this.Mapper.Map<SinglePostViewModel>(singlePostDataDTO);

			return Ok(singlePostViewModel);
		}

		[Authorize]
		[HttpPost]
		public async Task<ActionResult> Post([FromBody] PostFormInputModel inputModel)
		{
			var isAdmin = User.HasClaim(x => x.Value == "Admin");

			var inputCarDTO = this.Mapper.Map<CarFormInputModelDTO>(inputModel.Car);

			if (!this.ModelState.IsValid)
			{				
				inputModel.Car = this.Mapper.Map<CarFormInputModel>(inputCarDTO);
				return StatusCode(StatusCodes.Status406NotAcceptable);
			}

			var imageRootDirectoryPath = $"{Environment.ContentRootPath}/wwwroot/images";
			imageRootDirectoryPath = imageRootDirectoryPath.Replace("FastCarSales\\FastCarSales", "FastCarSales\\FastCarSales.Client");

			var userId = this.GetCurrentUserId();
			var inputPostDTO = this.Mapper.Map<PostFormInputModelDTO>(inputModel);
			var selectedExtrasIds = inputModel.SelectedExtrasIds.ToList();
			
			try
			{
				var car = await this.CarsService.GetCarFromInputModelAsync(inputCarDTO, selectedExtrasIds, userId, imageRootDirectoryPath);
				var postId = await this.PostsService.CreateAsync(inputPostDTO, car, userId, isAdmin);

				return Ok(postId);
			}
			catch (Exception ex)
			{
				//this.ModelState.AddModelError("CustomError", ex.Message);				
				//inputModel.Car = this.Mapper.Map<CarFormInputModel>(inputCarDTO);
				return StatusCode(StatusCodes.Status500InternalServerError, "Error posting car: " + ex.Message);
			}

		}

		[Authorize]
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		[Authorize]
		// DELETE api/<PostsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}


	}
}
