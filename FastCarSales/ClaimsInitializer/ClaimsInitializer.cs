using FastCarSales.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


//public class ClaimsInitializer
//{
//	public static async Task InitializeAsync(IServiceProvider serviceProvider)
//	{
//		using (var scope = serviceProvider.CreateScope())
//		{
//			var scopedServices = scope.ServiceProvider;
//			var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();

//			var adminEmails = new[] { "dpmangwiro@gmail.com", "fastcarsalesadmin@gmail.com" };

//			
//		}
//	}
//}

public class ClaimsInitializer
{
	public static async Task InitializeAsync(IServiceProvider serviceProvider)
	{
		using (var scope = serviceProvider.CreateScope())
		{
			var scopedServices = scope.ServiceProvider;
			var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();

			var adminEmails = new[] { "dpmangwiro@gmail.com", "fastcarsalesadmin@gmail.com" };

			foreach (var email in adminEmails)
			{
				var user = await userManager.FindByEmailAsync(email);
				if (user != null)
				{
					var claims = await userManager.GetClaimsAsync(user);
					if (!claims.Any(c => c.Type == "Permission" && c.Value == "Admin"))
					{
						await userManager.AddClaimAsync(user, new Claim("Permission", "Admin"));
					}
				}
			}

			//foreach (var email in adminEmails)
			//{
			//	var user = await userManager.FindByEmailAsync(email);
			//	if (user != null)
			//	{
			//		var claims = await userManager.GetClaimsAsync(user);
			//		if (!claims.Any(c => c.Type == ClaimTypes.Email && c.Value == email))
			//		{
			//			await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, email));
			//		}
			//	}
			//}



		}
	}
}
