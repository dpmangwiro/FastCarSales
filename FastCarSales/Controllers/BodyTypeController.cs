using AutoMapper;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastCarSales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BodyTypeController : ControllerBase
	{
		private ICarsService CarsService { get; set; } = null!;
		private IPostsService PostsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;

		public BodyTypeController(ICarsService carsService, IPostsService postsService, IHostEnvironment environment, IMapper mapper)
		{
			CarsService = carsService;
			PostsService = postsService;
			Environment = environment;
			Mapper = mapper;
		}

		// GET api/<ModelsController>/5
		[HttpGet("{Id:int}")]
		public ActionResult<IEnumerable<BaseCarSpecificationServiceModel>> GetBodyTypes(int Id)
		{
			try
			{
				if (Id <= 0)
				{
					return BadRequest();
				}

				var models = CarsService.GetBodyTypes(Id);

				return Ok(models);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database: More details: " + ex);
			}

		}



	}
}
