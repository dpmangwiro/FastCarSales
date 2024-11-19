using FastCarSales.Data;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Images;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Services
{
	public class UserService : IUserService
	{
		private readonly FastCarSalesDbContext data;

		public UserService(FastCarSalesDbContext data)
		{
			this.data = data;
		}
		public async Task<IEnumerable<string>> GetAdmins()
		{
			List<string?> adminUserNames = await data.Users
											.Where(user => data.UserClaims
											.Any(claim => claim.UserId == user.Id && claim.ClaimType == "Permission" && claim.ClaimValue == "Admin"))
											.Select(user => user.UserName)
											.ToListAsync();

			if (adminUserNames is null || adminUserNames.Count == 0)
			{ return new List<string>(); }
			else
			{
				var admins = new List<string>();

				foreach (var x in adminUserNames)
				{
					if (x is not null)
					{
						admins.Add(x);
					}
				}

				return admins;
			}
		}


	}
}
