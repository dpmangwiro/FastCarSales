using FastCarSales.Data.Models;


namespace FastCarSales.Data.Seeding
{
    public class BodyTypeSeeder : ISeeder
    {
        public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.BodyTypes.Any())
            {
                return;
            }

            var bodyTypesToSeed = new List<BodyType>()
            {
                 new BodyType() { Name = "Bus"},
                new BodyType() { Name = "Convertible"},
                new BodyType() { Name = "Coupe" },
                new BodyType() { Name = "Crossover"},
                new BodyType() { Name = "Hatchback"},
                new BodyType() { Name = "Loader"},
                new BodyType() { Name = "Minivan" },
                new BodyType() { Name = "Pickup" },
                new BodyType() { Name = "Roadster" },
                new BodyType() { Name = "Sedan" },
                new BodyType() { Name = "Truck" },
                new BodyType() { Name = "Wagon"},
                new BodyType() { Name = "Sports Car"},
                new BodyType() { Name = "SUV"},
                new BodyType() { Name = "Van"},
                new BodyType() { Name = "Other" }
            };

            await dbContext.BodyTypes.AddRangeAsync(bodyTypesToSeed);
            await dbContext.SaveChangesAsync();
        }
    }
}
