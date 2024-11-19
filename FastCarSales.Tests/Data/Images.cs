using FastCarSales.Data.Models;
using static FastCarSales.Services.Data.Tests.Data.TestDataConstants;

namespace FastCarSales.Services.Data.Tests.Data
{
    public class Images
    {
        public static Image ValidTestImage => new()
        {
            Id = TestImageId,
            CarId = TestIdNumber,
            CreatorId = TestImageId,
        };
    }
}