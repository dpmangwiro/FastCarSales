using FastCarSales.Data.Models;
using Microsoft.AspNetCore.Identity;

public class RoleInitializer
{
	public static async Task InitializeAsync(IServiceProvider serviceProvider)
	{
		var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
		var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

		string[] roleNames = { "Admin", "User" };
		IdentityResult roleResult;

		foreach (var roleName in roleNames)
		{
			var roleExist = await roleManager.RoleExistsAsync(roleName);
			if (!roleExist)
			{
				roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
			}
		}

		var user = await userManager.FindByEmailAsync("dpmangwiro@gmail.com");
		if (user != null)
		{
			await userManager.AddToRoleAsync(user, "Admin");
		}
	}
}
