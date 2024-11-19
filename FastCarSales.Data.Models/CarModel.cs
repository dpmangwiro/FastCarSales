using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Data.Models
{
    public class CarModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int MakeId { get; set; }
        public Make Make { get; set; } = new Make();

        public int BodyTypeId { get; set; }
        public BodyType BodyType { get; set; }= new BodyType();
        		        
		public int FuelTypeId { get; set; }

		public FuelType FuelType { get; set; } = new FuelType();

		public int TransmissionTypeId { get; set; }

		public decimal EngineCapacity { get; set; }

		public int? Year { get; set; }

		public TransmissionType TransmissionType { get; set; } = new TransmissionType();
		public ICollection<Car> Cars { get; set; } = new HashSet<Car>();

    }
}
