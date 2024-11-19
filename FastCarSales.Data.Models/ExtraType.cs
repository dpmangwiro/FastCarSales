using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Data.Models
{
	public class ExtraType
	{
		public int Id { get; init; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public ICollection<Extra> Extras { get; set; } = new HashSet<Extra>();
	}
}
