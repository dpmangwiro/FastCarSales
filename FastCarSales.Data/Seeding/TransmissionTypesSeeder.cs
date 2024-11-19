namespace FastCarSales.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using FastCarSales.Data.Models;

    public class TransmissionTypesSeeder : ISeeder
    {
        public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.TransmissionTypes.Any())
            {
                return;
            }
            
            var transmissionTypesToSeed = new List<TransmissionType>()
            {
                new TransmissionType() { Name = "Manual"},
                new TransmissionType() { Name = "Automatic"},
                new TransmissionType() { Name = "Semi-automatic"},
            };

            await dbContext.TransmissionTypes.AddRangeAsync(transmissionTypesToSeed);
            await dbContext.SaveChangesAsync();
        }
    }
}