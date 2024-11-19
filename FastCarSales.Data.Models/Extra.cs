using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Data.Models
{
	public class Extra
	{
		public int Id { get; init; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public int TypeId { get; set; }

		public ExtraType Type { get; set; }

		public ICollection<CarExtra> CarExtras { get; set; } = new HashSet<CarExtra>();
	}
}
