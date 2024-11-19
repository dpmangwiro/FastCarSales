using AutoMapper;
using FastCarSales.ComponentModels.Posts;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Services.Posts;
using FastCarSales.Web.ViewModels.Cars;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FastCarSales.Services.Images.Models;
using FastCarSales.Web.ViewModels.Images;

namespace FastCarSales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EditPostsController : ControllerBase
	{
		private IPostsService PostsService { get; set; } = null!;
		private ICarsService CarsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;
		private readonly AutoMapper.IConfigurationProvider mapperConfiguration;

		public EditPostsController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
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

		[HttpGet("GetUserId")]
		private string? GetCurrentUserId()
		{
			//var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
			var userId = User.Claims.FirstOrDefault()?.Value;

			return userId;
		}
				
		
		[HttpGet("{id:int}")]
		public ActionResult<EditPostViewModel> GetEditView(int id)
		{
			var userId = this.GetCurrentUserId();
			var editPostDTO = this.PostsService.GetPostFormInputModelById(id);

			if (editPostDTO == null)
			{
				return this.NotFound();
			}

			var isAdmin = User.HasClaim(x => x.Value == "Admin");

			if ((editPostDTO.CreatorId != userId) && !isAdmin )
			{
				return this.Unauthorized();
			}
						
			var editPostViewModel = this.Mapper.Map<EditPostViewModel>(editPostDTO);

			return Ok(editPostViewModel);
		}


		[Authorize]
		[HttpPut]
		public async Task<IActionResult> Edit([FromBody] EditPostViewModel editedPost)
		{
			var userId = this.GetCurrentUserId();
			var isAdmin = User.HasClaim(x => x.Value == "Admin");

			if ((editedPost.CreatorId != userId) && !isAdmin )
			{
				return this.Unauthorized();
			}

			var editedPostDTO = this.Mapper.Map<EditPostDTO>(editedPost);
			//var currentImagesDTO = this.PostsService.GetCurrentDbImagesForAPost(editedPost.PostID);
			//var currentImagesViewModel = this.Mapper.Map<IEnumerable<ImageInfoDTO>, IEnumerable<ImageInfoViewModel>>(currentImagesDTO);

			if (!this.ModelState.IsValid)
			{				
				//var editedPostViewModel = this.Mapper.Map<EditPostViewModel>(editedPostDTO);
				//editedPostViewModel.CurrentImages = currentImagesViewModel;

				return BadRequest();
			}

			var selectedExtrasIds = editedPost.SelectedExtrasIds.ToList();
			var deletedImagesIds = editedPost.DeletedImagesIds.ToList();
			var imageRootDirectoryPath = $"{Environment.ContentRootPath}/wwwroot/images";
			imageRootDirectoryPath = imageRootDirectoryPath.Replace("FastCarSales\\FastCarSales", "FastCarSales\\FastCarSales.Client");
			
			try
			{
				await this.CarsService.UpdateCarDataFromInputModelAsync(editedPostDTO.CarId, editedPostDTO.Car, selectedExtrasIds, deletedImagesIds, userId!, imageRootDirectoryPath, editedPost.SelectedCoverImageId);
				await this.PostsService.UpdateAsync(editedPostDTO, isAdmin);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error posting car: " + ex.Message);
			}

			//TempData[WebConstants.SuccessMessageKey] = $"The car post was edited successfully{(isAdmin ? string.Empty : " and is awaiting for approval")}!";

			return Ok();
		}

	}
}
