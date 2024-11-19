using FastCarSales.Web.ViewModels.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.ComponentModels.Cars.ViewModel
{
	public class BaseCarPropertyListsViewModel
	{
		public IEnumerable<BaseCarSpecificationViewModel> Makes { get; set; } = new List<BaseCarSpecificationViewModel>();

		public IEnumerable<BaseCarSpecificationViewModel> CarModels { get; set; } = new List<BaseCarSpecificationViewModel>();

		public IEnumerable<BaseCarSpecificationViewModel> BodyTypes { get; set; } = new List<BaseCarSpecificationViewModel>();

		public IEnumerable<BaseCarSpecificationViewModel> FuelTypes { get; set; } = new List<BaseCarSpecificationViewModel>();

		public IEnumerable<BaseCarSpecificationViewModel> TransmissionTypes { get; set; } = new List<BaseCarSpecificationViewModel>();

		public IEnumerable<BaseCarSpecificationViewModel> CarExtras { get; set; } = new List<BaseCarSpecificationViewModel>();

	}
}
