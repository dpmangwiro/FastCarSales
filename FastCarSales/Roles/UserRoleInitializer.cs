using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public class UserRoleInitializer
{
	public static async Task InitializeAsync(IServiceProvider serviceProvider)
	{
		var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

		var user = await userManager.FindByEmailAsync("user@example.com");
		if (user != null)
		{
			await userManager.AddToRoleAsync(user, "Admin");
		}
	}
}
