using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.ComponentModels.Cars.InputModel
{
	public class BaseCarSearchModel
	{
		[Display(Name = "Make:")]
		public int MakeId { get; set; }

		[Display(Name = "CarModel:")]
		public int CarModelId { get; set; }

		[Display(Name = "BodyType:")]
		public int BodyTypeId { get; set; }

		[Display(Name = "Fuel type:")]
		public int FuelTypeId { get; set; }

		[Display(Name = "Transmission type:")]
		public int TransmissionTypeId { get; set; }

		[Display(Name = "Extras:")]
		public int CarExtraId { get; init; }
	}
}
