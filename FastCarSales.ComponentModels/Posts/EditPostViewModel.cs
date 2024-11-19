namespace FastCarSales.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using FastCarSales.ComponentModels.Posts;
    using Images;

    public class EditPostViewModel : PostFormInputModel
    {
        public HashSet<ImageInfoViewModel> CurrentImages { get; set; } = new HashSet<ImageInfoViewModel>();

        public string SelectedCoverImageId { get; set; }

        public HashSet<string> DeletedImagesIds { get; set; } = new HashSet<string>();

        public string CreatorId { get; set; }

        public int CarId { get; set; }
    }
}