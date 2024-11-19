using FastCarSales.Data.Models;

namespace FastCarSales.Services.Cars.Models
{
    public class BaseCarDTO
    {
        public int Id { get; init; }

        public int MakeId { get; set; }
        public string Make { get; set; }

        public int CarModelId { get; set; }
        public CarModel CarModel { get; set; } = new CarModel();

        public int Year { get; init; }

        public decimal Price { get; init; }
    }
}