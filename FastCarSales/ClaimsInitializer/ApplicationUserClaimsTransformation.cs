using FastCarSales.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FastCarSales.ClaimsInitializer
{
	public class ApplicationUserClaimsTransformation : IClaimsTransformation
	{
		private readonly UserManager<ApplicationUser> _UserManager;

		public ApplicationUserClaimsTransformation(UserManager<ApplicationUser> userManager)
		{
			_UserManager = userManager;
		}

		public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
		{
			var identity = principal.Identities.FirstOrDefault(x => x.IsAuthenticated);
			if (identity == null) {return principal; }

			var user = await _UserManager.GetUserAsync(principal);
			if (user != null) { return principal; }

			//Add or replace idenity.claims

			if (!principal.HasClaim(c => c.Type == "Permission" && c.Value == "Admin"))
			{
				identity.AddClaim(new Claim(type: "Permission", value: "Admin") );

			}

			return new ClaimsPrincipal(identity);

		}

	}
}
