
using System.ComponentModel.DataAnnotations;


namespace FastCarSales.Data.Models
{
    public class CarModelViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int MakeId { get; set; }
        public string Make {  get; set; }

        public int BodyTypeId { get; set; }
        public string BodyType { get; set; }
        		        
		public int FuelTypeId { get; set; }

		public string FuelType { get; set; }

		public string TransmissionType { get; set; }
		public int TransmissionTypeId { get; set; }

		public decimal EngineCapacity { get; set; }

		public int? Year { get; set; }

		
		//public ICollection<Car> Cars { get; set; } = new HashSet<Car>();

    }
}
