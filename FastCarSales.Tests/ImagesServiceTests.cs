namespace FastCarSales.Services.Data.Tests
{
	using System;
	using FastCarSales.Data;
	using FastCarSales.Data.Dtos;
	using FastCarSales.Services.Images;
	using Microsoft.EntityFrameworkCore;
	using Xunit;
	using System.IO;
	using System.Threading.Tasks;	
	using FastCarSales.Data.Models;
	

	public class ImagesServiceTests
	{
		private readonly ImagesService _service;
		private readonly FastCarSalesDbContext _dbContext;

		public ImagesServiceTests()
		{
			var options = new DbContextOptionsBuilder<FastCarSalesDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			_dbContext = new FastCarSalesDbContext(options);

			_service = new ImagesService(_dbContext);
		}
				

		[Fact]
		public async Task UploadImageAsync_ShouldUploadImageCorrectly()
		{
			// Arrange
			var imageFile = new ImageFile
			{
				FileName = "test.jpg",
				Image = new byte[] { 1, 2, 3, 4 }
			};
			var userId = "testUserId";
			var imagePath = "testPath";

			Directory.CreateDirectory($"{imagePath}/cars/");
			
			// Act
			var result = await _service.UploadImageAsync(imageFile, userId, imagePath);
			var expectedFilePath = $"{imagePath}/cars/{result.Id}.jpg";

			// Assert
			Assert.NotNull(result); Assert.Equal(userId, result.CreatorId);
			Assert.Equal("jpg", result.Extension);

			var fileExists = File.Exists(expectedFilePath);
			Assert.True(fileExists);

			// Cleanup
			if (fileExists)
			{
				File.Delete(expectedFilePath);
			}
		}

		[Fact]
		public async Task UploadImageAsync_ShouldThrowExceptionForInvalidExtension()
		{
			// Arrange
			var imageFile = new ImageFile
			{
				FileName = "test.bmp",
				Image = new byte[] { 1, 2, 3, 4 }
			};
			var userId = "testUserId";
			var imagePath = "testPath";

			// Act & Assert
			var exception = await Assert.ThrowsAsync<Exception>(() =>
				_service.UploadImageAsync(imageFile, userId, imagePath));
			Assert.Equal("Invalid image extension! The allowed extensions are jpg, png, gif.", exception.Message);
		}

		[Fact]
		public async Task UploadImageAsync_ShouldThrowExceptionForLargeFileSize()
		{
			// Arrange
			var imageFile = new ImageFile
			{
				FileName = "test.jpg",
				Image = new byte[6 * 1024 * 1024] // 6 MB
			};
			var userId = "testUserId";
			var imagePath = "testPath";

			// Act & Assert
			var exception = await Assert.ThrowsAsync<Exception>(() =>
				_service.UploadImageAsync(imageFile, userId, imagePath));
			Assert.Equal("Invalid file size. The maximum allowed file size is 5Mb.", exception.Message);
		}

		[Fact]
		public async Task SetCoverImagePropertyAsync_ShouldSetIsCoverImageToTrue()
		{
			// Arrange
			var imageId = "testImageId1";
			var image = new Image
			{
				Id = imageId,
				IsCoverImage = false,
				CreatorId = "testUserId",
				Extension = "jpg",
				CarId = 1
			};

			_dbContext.Images.Add(image);
			await _dbContext.SaveChangesAsync();

			// Act
			await _service.SetCoverImagePropertyAsync(imageId);
			var updatedImage = await _dbContext.Images.FirstOrDefaultAsync(img => img.Id == imageId);

			// Assert
			Assert.NotNull(updatedImage);
			Assert.True(updatedImage.IsCoverImage);
		}

		[Fact]
		public async Task SetCoverImagePropertyAsync_ShouldNotThrowIfImageNotFound()
		{
			// Arrange
			var imageId = "nonExistentImageId";

			// Act
			await _service.SetCoverImagePropertyAsync(imageId);

			// Assert
			// No exception should be thrown and no changes should be made to the database
		}
	
		[Fact]
		public async Task RemoveCoverImagePropertyAsync_ShouldSetIsCoverImageToFalse()
		{
			// Arrange
			var imageId = "testImageId2";
			var image = new Image
			{
				Id = imageId,
				IsCoverImage = true,
				CreatorId = "testUserId",
				Extension = "jpg",
				CarId = 1
			};

			_dbContext.Images.Add(image);
			await _dbContext.SaveChangesAsync();

			// Act
			await _service.RemoveCoverImagePropertyAsync(imageId);
			var updatedImage = await _dbContext.Images.FirstOrDefaultAsync(img => img.Id == imageId);

			// Assert
			Assert.NotNull(updatedImage);
			Assert.False(updatedImage.IsCoverImage);
		}

		[Fact]
		public async Task RemoveCoverImagePropertyAsync_ShouldNotThrowIfImageNotFound()
		{
			// Arrange
			var imageId = "nonExistentImageId";

			// Act
			await _service.RemoveCoverImagePropertyAsync(imageId);

			// Assert
			// No exception should be thrown and no changes should be made to the database
		}

		[Fact]
		public void GetDefaultCarImagesPath_ShouldReturnCorrectPath()
		{
			// Arrange
			var imageId = "12345";
			var imageExtension = "jpg";
			var expectedPath = $"/images/cars/{imageId}.{imageExtension}";

			// Act
			var result = _service.GetDefaultCarImagesPath(imageId, imageExtension);

			// Assert
			Assert.Equal(expectedPath, result);
		}

		[Fact]
		public void GetCoverImagePath_ShouldReturnCoverImagePath_WhenCoverImageExists()
		{
			// Arrange
			var carImages = new List<Image>
		{
			new Image { Id = "1", Extension = "jpg", IsCoverImage = true },
			new Image { Id = "2", Extension = "png", IsCoverImage = false }
		};
			var expectedPath = $"/images/cars/1.jpg";

			// Act
			var result = _service.GetCoverImagePath(carImages);

			// Assert
			Assert.Equal(expectedPath, result);
		}

		[Fact]
		public void GetCoverImagePath_ShouldReturnFirstImagePath_WhenCoverImageDoesNotExist()
		{
			// Arrange
			var carImages = new List<Image>
		{
			new Image { Id = "1", Extension = "jpg", IsCoverImage = false },
			new Image { Id = "2", Extension = "png", IsCoverImage = false }
		};
			var expectedPath = $"/images/cars/1.jpg";

			// Act
			var result = _service.GetCoverImagePath(carImages);

			// Assert
			Assert.Equal(expectedPath, result);
		}

		[Fact]
		public void GetCoverImagePath_ShouldReturnFirstImagePath_WhenOnlyOneImageExists()
		{
			// Arrange
			var carImages = new List<Image>
		{
			new Image { Id = "1", Extension = "jpg", IsCoverImage = false }
		};
			var expectedPath = $"/images/cars/1.jpg";

			// Act
			var result = _service.GetCoverImagePath(carImages);

			// Assert
			Assert.Equal(expectedPath, result);
		}
	}



}
