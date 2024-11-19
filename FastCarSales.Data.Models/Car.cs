using System.ComponentModel.DataAnnotations;

namespace FastCarSales.Data.Models
{
	public class Car
	{
		public int Id { get; init; }

		public int MakeId { get; set; }
		public Make Make { get; set; }
				
		public int CarModelId { get; set; }
        public CarModel CarModel { get; set; }

        [Required]
		public string Description { get; set; }

		public int Year { get; set; }

		public decimal Price { get; set; }

		public int Kilometers { get; set; }

		public decimal EngineCapacity { get; set; }

		[Required]
		[MaxLength(50)]
		public string LocationCity { get; set; }

		[Required]
		[MaxLength(50)]
		public string LocationTown { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }

		public int BodyTypeId { get; set; }

		public BodyType BodyType { get; set; }

		public int FuelTypeId { get; set; }

		public FuelType FuelType { get; set; }

		public int TransmissionTypeId { get; set; }

		public TransmissionType TransmissionType { get; set; }

		public Post Post { get; set; }

		public ICollection<CarExtra> CarExtras { get; set; } = new HashSet<CarExtra>();

		public ICollection<Image> Images { get; set; } = new HashSet<Image>();
	}
}
