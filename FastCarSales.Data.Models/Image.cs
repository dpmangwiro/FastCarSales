using System.ComponentModel.DataAnnotations;

namespace FastCarSales.Data.Models
{
	public class Image
	{
		public Image()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public string Id { get; set; }

		public string Extension { get; set; }

		public bool IsCoverImage { get; set; }

		public int CarId { get; set; }

		public Car Car { get; set; }

		[Required]
		public string CreatorId { get; set; }

		public ApplicationUser Creator { get; set; }
	}
}
