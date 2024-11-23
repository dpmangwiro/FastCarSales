namespace FastCarSales.Services.Images
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Collections.Generic;
	using Data;
	using Data.Models;
	using FastCarSales.Data.Dtos;
	using Microsoft.EntityFrameworkCore.Storage;

	public class ImagesService : IImagesService
	{
		private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };
		private readonly FastCarSalesDbContext data;

		public ImagesService(FastCarSalesDbContext data)
		{
			this.data = data;
		}

		public async Task<Image> UploadImageAsync(ImageFile image, string userId, string imageRootDirectoryPath)
		{
			// /wwwroot/images/cars/ce8f1a9e-6c4a-44a1-adf3-f7c3d54ff859.jpg
			Directory.CreateDirectory($"{imageRootDirectoryPath}/cars/");

			var extension = Path.GetExtension(image.FileName).TrimStart('.');

			if (!this.allowedExtensions.Any(ex => extension.EndsWith(ex)))
			{
				throw new Exception($"Invalid image extension! The allowed extensions are {string.Join(", ", this.allowedExtensions)}.");
			}

			if (image.Image.Length > (5 * 1024 * 1024))
			{
				throw new Exception($"Invalid file size. The maximum allowed file size is 5Mb.");
			}

			var dbImage = new Image
			{
				CreatorId = userId,
				Extension = extension,
				IsCoverImage = image.IsCoverImage,
			};

			var physicalPath = $"{imageRootDirectoryPath}/cars/{dbImage.Id}.{extension}";
			await using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
			// await image.CopyToAsync(fileStream);
			fileStream.Write(image.Image);

			return dbImage;
		}

		public bool DeleteImageFromPhysicalFile(string fileName, string imageRootDirectoryPath, IDbContextTransaction transaction)
		{
			try
			{
				// /wwwroot/images/cars/ce8f1a9e-6c4a-44a1-adf3-f7c3d54ff859.jpg
				Directory.CreateDirectory($"{imageRootDirectoryPath}/cars/");

				var physicalPath = $"{imageRootDirectoryPath}/cars/{fileName}";

				if (File.Exists(physicalPath))
				{
					File.Delete(physicalPath);
				}

				return true;
			}
			catch (Exception)
			{

				throw;
			}
			
		}

		public bool DeleteImages(List<Image> images, string imageRootDirectoryPath, IDbContextTransaction transaction)
		{
			try
			{
				foreach(Image image in images)
				{
					var imageToRemove = this.data.Images.First(img => img.Id == image.Id);
					this.data.Images.Remove(imageToRemove);
					this.DeleteImageFromPhysicalFile(imageToRemove.Id + "." + imageToRemove.Extension, imageRootDirectoryPath: imageRootDirectoryPath, transaction: transaction);
				}

				return true;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task SetCoverImagePropertyAsync(string imageId)
		{
			var image = this.data.Images.FirstOrDefault(img => img.Id == imageId);

			if (image != null)
			{
				image.IsCoverImage = true;
			}

			await this.data.SaveChangesAsync();
		}

		public async Task RemoveCoverImagePropertyAsync(string imageId)
		{
			var image = this.data.Images.FirstOrDefault(img => img.Id == imageId);

			if (image != null)
			{
				image.IsCoverImage = false;
			}

			await this.data.SaveChangesAsync();
		}

		public string GetDefaultCarImagesPath(string imageId, string imageExtension)
		{
			return $"/images/cars/{imageId}.{imageExtension}";
		}

		public string GetCoverImagePath(ICollection<Image> carImages)
		{
			var coverImage = carImages.FirstOrDefault(img => img.IsCoverImage);
			var imageId = coverImage != null ? coverImage.Id : carImages.First().Id;
			var imageExtension = coverImage != null ? coverImage.Extension : carImages.First().Extension;

			return this.GetDefaultCarImagesPath(imageId, imageExtension);
		}


	}
}