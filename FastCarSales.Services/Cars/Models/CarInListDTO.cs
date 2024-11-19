namespace FastCarSales.Services.Cars.Models
{
    public class CarInListDTO : BaseCarDTO
    {
        public string Description { get; init; }

        public int Kilometers { get; init; }
        
        public string BodyType { get; init; }
        
        public string FuelType { get; init; }

        public string TransmissionType { get; init; }

        public string CoverImage { get; init; }

        public string LocationCity { get; init; }

        public string LocationTown { get; init; }
    }
}
