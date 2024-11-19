namespace FastCarSales.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider);
    }
}
