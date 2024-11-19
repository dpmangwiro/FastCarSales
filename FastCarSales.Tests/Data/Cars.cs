using FastCarSales.Data.Models;
using FastCarSales.Services.Cars.Models;
using static FastCarSales.Services.Data.Tests.Data.Images;
using static FastCarSales.Services.Data.Tests.Data.TestDataConstants;

namespace FastCarSales.Services.Data.Tests.Data
{    
    public class Cars
    {
        public static CarFormInputModelDTO ValidTestCarFormInputModelDTO => new()
        {
            MakeId = TestIdNumber,
            CarModelId = TestIdNumber,
            Description = TestDescription,
            BodyTypeId = TestIdNumber,
            FuelTypeId = TestIdNumber,
            TransmissionTypeId = TestIdNumber,
            Year = TestYear,
            Kilometers = TestIntValue,
            EngineCapacity = TestIntValue,
            Price = TestIntValue,
            LocationTown = TestLocationTown,
            LocationCity = TestLocationCity,
        };

        public static Car ValidTestCar => new()
        {
            Id = TestIdNumber,
            MakeId = TestIdNumber,
            CarModelId = TestIdNumber,
            Description = TestDescription,
            BodyTypeId = TestIdNumber,
            FuelTypeId = TestIdNumber,
            TransmissionTypeId = TestIdNumber,
            Year = TestYear,
            Kilometers = TestIntValue,
            EngineCapacity = TestIntValue,
            Price = TestIntValue,
            LocationTown = TestLocationTown,
            LocationCity = TestLocationCity,
        };

        public static CarFormInputModelDTO ValidUpdatedCatTestModel => new()
        {
			MakeId = TestIdNumber,
			CarModelId = TestIdNumber,
			Description = UpdatedTestDescription,
            BodyTypeId = UpdatedTestIdNumber,
            FuelTypeId = UpdatedTestIdNumber,
            TransmissionTypeId = UpdatedTestIdNumber,
            Year = UpdatedTestYear,
            Kilometers = UpdatedTestIntValue,
            EngineCapacity = UpdatedTestIntValue,
            Price = UpdatedTestIntValue,
            LocationTown = UpdatedTestLocationCountry,
            LocationCity = UpdatedTestLocationCity,
        };

        public static SearchCarInputModelDTO ValidSearchCarInputModelDTO => new()
        {
            TextSearchTerm = TestMake,
            FromYear = TestYear,
        };
    }
}