namespace FastCarSales.Data.Seeding
{
    using FastCarSales.Data;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class FastCarSalesDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }                  

        var seeders = new List<ISeeder>
            {
                new BodyTypeSeeder(),
                new MakeSeeder(),
                new ExtraTypesSeeder(),
                new FuelTypesSeeder(),
                new TransmissionTypesSeeder(),
                new CarModelSeeder(),
                new RoleSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
