namespace FastCarSales.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using FastCarSales.Data.Models;

    public class ExtraTypesSeeder : ISeeder
    {
        public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ExtraTypes.Any())
            {
                return;
            }
            
            var extraTypesToSeed = new List<ExtraType>()
            {
                new ExtraType() { Name = "Comfort"},
                new ExtraType() { Name = "Safety"},
                new ExtraType() { Name = "Other"},
            };

            await dbContext.ExtraTypes.AddRangeAsync(extraTypesToSeed);
            await dbContext.SaveChangesAsync();
        }
    }
}