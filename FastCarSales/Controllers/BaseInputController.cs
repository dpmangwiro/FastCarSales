using AutoMapper;
using FastCarSales.ComponentModels.Cars.ViewModel;
using FastCarSales.ComponentModels.Posts;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Web.ViewModels.Cars;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FastCarSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseInputController : ControllerBase
    {

        private IPostsService PostsService { get; set; } = null!;
        private ICarsService CarsService { get; set; } = null!;
        private IHostEnvironment Environment { get; set; } = null!;
        private IMapper Mapper { get; set; } = null!;

        public BaseInputController(IPostsService postsService, ICarsService carsService, IMapper mapper, IHostEnvironment environment)
        {
            PostsService = postsService;
            CarsService = carsService;
            Mapper = mapper;
            Environment = environment;
        }


        [HttpGet]
        public ActionResult<BaseCarPropertyListsViewModel> GetPostingInpuModel()
        {
            try
            {
				var createCarServiceModel = CarsService.FillInputCarBaseProperties();

				BaseCarPropertyListsViewModel carPropModel = Mapper.Map<BaseCarPropertyListsViewModel>(createCarServiceModel);

				return Ok(carPropModel);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from database: More details: " + ex);
			}
			
        }

    }
}
