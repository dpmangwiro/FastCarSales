using FastCarSales.Web.ViewModels.Cars;
using System.ComponentModel.DataAnnotations;
using static FastCarSales.Data.Constants.DataConstants;


namespace FastCarSales.ComponentModels.Posts
{     

    public class PostFormInputModel
    {
        [Required]
        public CarFormInputModel Car { get; set; } = new CarFormInputModel();

		public int PostID { get; init; }
		public IEnumerable<int> SelectedExtrasIds { get; set; } = new HashSet<int>();
        
        [Required(ErrorMessage = "The seller name field is required.")]
        [StringLength(
            PostSellerNameMaxLength,
            MinimumLength = PostSellerNameMinLength,
            ErrorMessage = "The seller name must be between {2} and {1} characters long.")]
        [Display(Name = "Seller name:")]
        public string SellerName { get; set; }

        [Required(ErrorMessage = "The seller phone number field is required.")]
        [StringLength(
            PostSellerPhoneNumberMaxLength,
            MinimumLength = PostSellerPhoneNumberMinLength,
            ErrorMessage = "The seller phone number must be between {2} and {1} digits long.")]
        [Display(Name = "Seller phone number:")]
        public string SellerPhoneNumber { get; set; }

        public string SellerEmail { get; set; }
               
    }
}
