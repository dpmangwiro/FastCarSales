namespace FastCarSales.Services.Images
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Data.Models;
    using FastCarSales.Data.Dtos;
	using Microsoft.EntityFrameworkCore.Storage;

    public interface IImagesService
    {
        Task<Image> UploadImageAsync(ImageFile image, string userId, string imageRootDirectoryPath);

        Task SetCoverImagePropertyAsync(string imageId);

        Task RemoveCoverImagePropertyAsync(string imageId);

        string GetDefaultCarImagesPath(string imageId, string imageExtension);

        string GetCoverImagePath(ICollection<Image> carImages);
		bool DeleteImageFromPhysicalFile(string fileName, string imageRootDirectoryPath, IDbContextTransaction transaction);

		bool DeleteImages(List<Image> images, string imageRootDirectoryPath, IDbContextTransaction transaction);

	}
}