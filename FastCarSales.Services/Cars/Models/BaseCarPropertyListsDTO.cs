using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Services.Cars.Models
{
	public class BaseCarPropertyListsDTO
	{
		public IEnumerable<BaseCarSpecificationServiceModel> Makes { get; set; } = new List<BaseCarSpecificationServiceModel>();
		
		public IEnumerable<BaseCarSpecificationServiceModel> CarModels { get; set; } = new List<BaseCarSpecificationServiceModel>();

		public IEnumerable<BaseCarSpecificationServiceModel> BodyTypes { get; set; } = new List<BaseCarSpecificationServiceModel>();

		public IEnumerable<BaseCarSpecificationServiceModel> FuelTypes { get; set; } = new List<BaseCarSpecificationServiceModel>();

		public IEnumerable<BaseCarSpecificationServiceModel> TransmissionTypes { get; set; } = new List<BaseCarSpecificationServiceModel>();

		public IEnumerable<CarExtrasServiceModel> CarExtras { get; set; } = new List<CarExtrasServiceModel>();
	}
}
