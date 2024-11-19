using Microsoft.AspNetCore.Identity;

namespace FastCarSales.Data.Models
{
	public class ApplicationUser : IdentityUser
	{
		public ApplicationUser()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public string FullName { get; set; }

		public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
	}
}
