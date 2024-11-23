using FastCarSales.Data.Models;
using Microsoft.AspNetCore.Identity;


namespace FastCarSales.Data.Seeding
{
	public class RoleSeeder : ISeeder
	{
		public async Task SeedAsync(FastCarSalesDbContext dbContext, IServiceProvider serviceProvider)
		{
			if (dbContext.Roles.Any())
			{
				return;
			}

			var rolesToSeed = new List<IdentityRole>()
			{
				 new IdentityRole { Name = "Admin", NormalizedName = "ADMIN", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString()},
				 new IdentityRole { Name = "User", NormalizedName = "USER", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString()}
			};

			await dbContext.Roles.AddRangeAsync(rolesToSeed);
			await dbContext.SaveChangesAsync();
		}
	}
}
