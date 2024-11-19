namespace FastCarSales.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using FastCarSales.Data.Models;

    public class FuelTypesSeeder : ISeeder
    {
        public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.FuelTypes.Any())
            {
                return;
            }
            
            var fuelTypesToSeed = new List<FuelType>()
            {
                new FuelType() { Name = "Petrol"},
                new FuelType() { Name = "Diesel"},
                new FuelType() { Name = "LPG"},
                new FuelType() { Name = "Electric" },
                new FuelType() {Name = "Hybrid"},
                new FuelType() { Name = "Hybrid (petrol/electric)" },
                new FuelType() { Name = "Hybrid (diesel/electric)" },
                new FuelType() { Name = "Other" },
            };

            await dbContext.FuelTypes.AddRangeAsync(fuelTypesToSeed);
            await dbContext.SaveChangesAsync();
        }
    }
}