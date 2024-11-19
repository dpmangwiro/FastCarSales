namespace FastCarSales.Services.Images
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Data.Models;
    using FastCarSales.Data.Dtos;

    public interface IImagesService
    {
        Task<Image> UploadImageAsync(ImageFile image, string userId, string imageRootDirectoryPath);

        Task SetCoverImagePropertyAsync(string imageId);

        Task RemoveCoverImagePropertyAsync(string imageId);

        string GetDefaultCarImagesPath(string imageId, string imageExtension);

        string GetCoverImagePath(ICollection<Image> carImages);
		bool DeleteImage(string fileName, string imageRootDirectoryPath);

	}
}