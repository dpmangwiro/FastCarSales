namespace FastCarSales.Services.Posts.Models
{
    using System.Collections.Generic;
    using Images.Models;

    public class EditPostDTO : PostFormInputModelDTO
    {
        public HashSet<ImageInfoDTO> CurrentImages { get; set; } = new HashSet<ImageInfoDTO>();

        public string SelectedCoverImageId { get; set; }

        public HashSet<string> DeletedImagesIds { get; set; } = new HashSet<string>();

        public string CreatorId { get; set; }

        public int CarId { get; set; }
    }
}