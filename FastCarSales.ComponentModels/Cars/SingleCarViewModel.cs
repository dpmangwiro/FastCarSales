namespace FastCarSales.Web.ViewModels.Cars
{
    using System.Collections.Generic;

    public class SingleCarViewModel : BaseCarViewModel
    {
        public string Description { get; init; }

        public int Kilometers { get; init; }

        public decimal EngineCapacity { get; set; }

        public string BodyType { get; init; }

        public string FuelType { get; init; }

        public string TransmissionType { get; init; }

        public IList<string> Images { get; init; } = new List<string>();

        public ICollection<string> ComfortExtras { get; init; } = new List<string>();

        public ICollection<string> SafetyExtras { get; init; } = new List<string>();

        public ICollection<string> OtherExtras { get; init; } = new List<string>();

        public string LocationCity { get; init; }

        public string LocationTown { get; init; }
    }
}