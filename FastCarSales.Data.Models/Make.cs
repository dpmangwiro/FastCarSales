using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Data.Models
{
    public class Make
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public ICollection<CarModel> CarModels { get; set; } = new HashSet<CarModel>();
        public ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}
