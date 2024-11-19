namespace FastCarSales.Web.ViewModels.Cars
{
    public class CarInListViewModel : BaseCarViewModel
    {
        public string Description { get; init; }

        public int Kilometers { get; init; }
        
        public string BodyType { get; init; }
        
        public string FuelType { get; init; }

        public string TransmissionType { get; init; }

        public string CoverImage { get; init; }

        public string LocationTown { get; init; }

        public string LocationCity { get; init; }
    }
}
