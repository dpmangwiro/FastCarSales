using AutoMapper;
using FastCarSales.Data.Models;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Posts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FastCarSales.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ModelsController : ControllerBase
	{
		private ICarsService CarsService { get; set; } = null!;
		private IPostsService PostsService { get; set; } = null!;
		private IHostEnvironment Environment { get; set; } = null!;
		private IMapper Mapper { get; set; } = null!;

		public ModelsController(ICarsService carsService, IPostsService postsService, IHostEnvironment environment, IMapper mapper)
		{
			CarsService = carsService;
			PostsService = postsService;
			Environment = environment;
			Mapper = mapper;
		}



		// GET: api/<ModelsController>
		[HttpGet]
		public ActionResult<IEnumerable<CarModel>> Get()
		{
			try
			{				
				var models = CarsService.GetCarModels();

				return Ok(models);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database: More details: " + ex);
			}
		}

		// GET api/<ModelsController>/5
		[HttpGet("{Id}")]
		public ActionResult<IEnumerable<CarModel>> GetCarModels(int Id)
		{
			try
			{
				if (Id <= 0)
				{
					return BadRequest();
				}

				var models = CarsService.GetCarModels(Id);

				return Ok(models);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database: More details: " + ex);
			}
			
		}

		// POST api/<ModelsController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<ModelsController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<ModelsController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
