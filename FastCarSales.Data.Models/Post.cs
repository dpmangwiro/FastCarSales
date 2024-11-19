using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Data.Models
{
	public class Post
	{
		public int Id { get; init; }

		public DateTime PublishedOn { get; init; }

		public DateTime? ModifiedOn { get; set; }

		public bool IsPublic { get; set; }
		public bool IsFeatured { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }

		[Required]
		public string CreatorId { get; set; }

		public ApplicationUser Creator { get; set; }

		public int CarId { get; set; }

		public Car Car { get; set; }

		[Required]
		[MaxLength(50)]
		public string SellerName { get; set; }

		[Required]
		[MaxLength(50)]
		public string SellerPhoneNumber { get; set; }

		public string SellerEmail { get; set; } = string.Empty;
    }
}
