using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Data.Models
{
	public class FuelType
	{
		public int Id { get; init; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
		public ICollection<CarModel> CarModels { get; set; } = new HashSet<CarModel>();
	}
}
