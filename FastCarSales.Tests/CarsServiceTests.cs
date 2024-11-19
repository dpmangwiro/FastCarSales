using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using AutoMapper;
using FastCarSales.Data.Dtos;
using FastCarSales.Data;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Images;
using FastCarSales.Data.Models;
using FastCarSales.MapperConfigurations.Profiles;

namespace CarServiceTests
{

	public class CarsServiceTests
	{
		//private readonly CarsService _service;
		
		private readonly Mock<IImagesService> _mockImagesService;
		private readonly IMapper _mapper;

		public CarsServiceTests()
		{
			_mockImagesService = new Mock<IImagesService>();

			var config = new MapperConfiguration(cfg =>
			{				
					cfg.AddProfile<CarsProfile>();				
			});

			_mapper = config.CreateMapper();					
			
		}

		public FastCarSalesDbContext GetDbContext()
		{
			var options = new DbContextOptionsBuilder<FastCarSalesDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var _dbContext = new FastCarSalesDbContext(options);

			return _dbContext;
		}

		public CarsService GetCarsService()
		{
			var dbContext = GetDbContext();
			
			var _service = new CarsService(dbContext, _mockImagesService.Object, _mapper);

			return _service;
		}

		[Fact]
		public async Task GetCarFromInputModelAsync_ShouldCreateCarWithCorrectDetails()
		{
			var _service = GetCarsService();
			// Arrange
			var inputCar = new CarFormInputModelDTO
			{
				MakeId = 1,
				CarModelId = 2,
				Description = "Test description",
				BodyTypeId = 3,
				FuelTypeId = 4,
				TransmissionTypeId = 5,
				Year = 2020,
				Kilometers = 15000,
				EngineCapacity = 2.0m,
				Price = 20000,
				LocationCity = "Test City",
				LocationTown = "Test Town",
				Images = new HashSet<ImageFile>
			{
				new ImageFile { FileName = "test.jpg", Image = new byte[] { 1, 2, 3, 4 } }
			}
			};

			var selectedExtrasIds = new List<int> { 1, 2 };
			var userId = "testUserId";
			var imagePath = "testPath";

			var uploadedImage = new Image
			{
				CreatorId = userId,
				Extension = "jpg",
				IsCoverImage = false,
			};

			_mockImagesService.Setup(s => s.UploadImageAsync(It.IsAny<ImageFile>(), userId, imagePath))
				.ReturnsAsync(uploadedImage);

			// Act
			var car = await _service.GetCarFromInputModelAsync(inputCar, selectedExtrasIds, userId, imagePath);

			// Assert
			Assert.NotNull(car);
			Assert.Equal(inputCar.MakeId, car.MakeId);
			Assert.Equal(inputCar.CarModelId, car.CarModelId);
			Assert.Equal(inputCar.Description, car.Description);
			Assert.Equal(inputCar.BodyTypeId, car.BodyTypeId);
			Assert.Equal(inputCar.FuelTypeId, car.FuelTypeId);
			Assert.Equal(inputCar.TransmissionTypeId, car.TransmissionTypeId);
			Assert.Equal(inputCar.Year, car.Year);
			Assert.Equal(inputCar.Kilometers, car.Kilometers);
			Assert.Equal(inputCar.EngineCapacity, car.EngineCapacity);
			Assert.Equal(inputCar.Price, car.Price);
			Assert.Equal(inputCar.LocationCity, car.LocationCity);
			Assert.Equal(inputCar.LocationTown, car.LocationTown);
			Assert.Single(car.Images);
		}

		[Fact]
		public async Task GetCarFromInputModelAsync_ShouldThrowException_WhenNoImages()
		{
			var _service = GetCarsService();
			// Arrange
			var inputCar = new CarFormInputModelDTO
			{
				Images = new HashSet<ImageFile>()
			};
			var selectedExtrasIds = new List<int>();
			var userId = "testUserId";
			var imagePath = "testPath";

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(async () =>
				await _service.GetCarFromInputModelAsync(inputCar, selectedExtrasIds, userId, imagePath));
		}

		[Fact]
		public async Task GetCarFromInputModelAsync_ShouldThrowException_WhenImagesExceedLimit()
		{
			var _service = GetCarsService();
			// Arrange
			var inputCar = new CarFormInputModelDTO
			{
				Images = Enumerable.Range(1, 11).Select(i => new ImageFile { FileName = $"test{i}.jpg", Image = new byte[] { 1, 2, 3, 4 } }).ToHashSet()
			};
			var selectedExtrasIds = new List<int>();
			var userId = "testUserId";
			var imagePath = "testPath";

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(async () =>
				await _service.GetCarFromInputModelAsync(inputCar, selectedExtrasIds, userId, imagePath));
		}

		[Fact]
		public async Task UpdateCarDataFromInputModelAsync_ShouldUpdateCarWithCorrectDetails()
		{
			var optionsBuilder = new DbContextOptionsBuilder<FastCarSalesDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new FastCarSalesDbContext(optionsBuilder.Options);
			var imagesServiceMock = new Mock<IImagesService>();

			

			// Arrange
			var carId = 17;

			while (dbContext.Cars.ToList().Exists(car => car.Id == carId))
			{
				carId++;
			}
			
			var imageFileToSave = new ImageFile
			{
				FileName = "test.jpg",
				Image = new byte[] { 1, 2, 3, 4 }
			};

			var inputCar = new CarFormInputModelDTO
			{				
				MakeId = 1,
				CarModelId = 2,
				Description = "Test description",
				BodyTypeId = 3,
				FuelTypeId = 4,
				TransmissionTypeId = 5,
				Year = 2020,
				Kilometers = 15000,
				EngineCapacity = 2.0m,
				Price = 20000,
				LocationCity = "Test City",
				LocationTown = "Test Town",
				Images = new HashSet<ImageFile>
			{
				imageFileToSave
			}
			};
			var selectedExtrasIds = new List<int> { 1, 2 };
			var deletedImagesIds = new List<string>();
			var userId = "testUserId";
			var imagePath = "testPath";
			var coverImageId = "123";

			var existingCar = new Car
			{
				Id = carId,
				MakeId = 1,
				CarModelId = 1,
				Description = "Old description",
				BodyTypeId = 1,
				FuelTypeId = 1,
				TransmissionTypeId = 1,
				Year = 2019,
				Kilometers = 10000,
				EngineCapacity = 1.5m,
				Price = 15000,
				LocationCity = "Old City",
				LocationTown = "Old Town"
			};

			dbContext.Cars.Add(existingCar);
			dbContext.SaveChanges();

			var uploadedImage = new Image
			{		
				Id = "Test Id",
				CreatorId = userId,
				Extension = "jpg",
				IsCoverImage = false,
				CarId = carId
			};

			//_mockImagesService.Setup(s => s.UploadImageAsync(imageFileToSave, userId, imagePath))
			//	.ReturnsAsync(uploadedImage);
			//_mockImagesService.Setup(s => s.SetCoverImagePropertyAsync(coverImageId))
			//	.Returns(Task.CompletedTask);
			//_mockImagesService.Setup(s => s.RemoveCoverImagePropertyAsync(It.IsAny<string>()))
			//	.Returns(Task.CompletedTask);
			
			imagesServiceMock.Setup(ism => ism.UploadImageAsync(imageFileToSave, userId, imagePath)).ReturnsAsync(uploadedImage);
			var carsService = new CarsService(dbContext, imagesServiceMock.Object, this._mapper);

			// Act
			await carsService.UpdateCarDataFromInputModelAsync(carId, inputCar, selectedExtrasIds, deletedImagesIds, userId, imagePath, coverImageId);

			// Assert
			var car = dbContext.Cars.First(x => x.Id == carId);
			Assert.NotNull(car);
			Assert.Equal(inputCar.MakeId, car.MakeId);
			Assert.Equal(inputCar.CarModelId, car.CarModelId);
			Assert.Equal(inputCar.Description, car.Description);
			Assert.Equal(inputCar.BodyTypeId, car.BodyTypeId);
			Assert.Equal(inputCar.FuelTypeId, car.FuelTypeId);
			Assert.Equal(inputCar.TransmissionTypeId, car.TransmissionTypeId);
			Assert.Equal(inputCar.Year, car.Year);
			Assert.Equal(inputCar.Kilometers, car.Kilometers);
			Assert.Equal(inputCar.EngineCapacity, car.EngineCapacity);
			Assert.Equal(inputCar.Price, car.Price);
			Assert.Equal(inputCar.LocationCity, car.LocationCity);
			Assert.Equal(inputCar.LocationTown, car.LocationTown);
			Assert.Single(car.Images);

			dbContext.Cars.Remove(existingCar);
		}

		[Fact]
		public async Task UpdateCarDataFromInputModelAsync_ShouldThrowException_WhenCarDoesNotExist()
		{

			var _service = GetCarsService();
			var _dbContext = GetDbContext();

			// Arrange
			var carId = 1;
			var inputCar = new CarFormInputModelDTO();
			var selectedExtrasIds = new List<int>();
			var deletedImagesIds = new List<string>();
			var userId = "testUserId";
			var imagePath = "testPath";
			var coverImageId = "123";

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(async () =>
				await _service.UpdateCarDataFromInputModelAsync(carId, inputCar, selectedExtrasIds, deletedImagesIds, userId, imagePath, coverImageId));
		}

		[Fact]
		public async Task UpdateCarDataFromInputModelAsync_ShouldThrowException_WhenAllImagesDeleted()
		{

			var _service = GetCarsService();
			var _dbContext = GetDbContext();

			// Arrange
			var carId = 3;
			var inputCar = new CarFormInputModelDTO
			{
				Images = new HashSet<ImageFile>()
			};
			var selectedExtrasIds = new List<int>();
			var deletedImagesIds = new List<string> { "123" };
			var userId = "testUserId";
			var imagePath = "testPath";
			var coverImageId = "123";

			var existingCar = new Car
			{
				Id = carId,
				MakeId = 1,
				CarModelId = 1,
				Description = "Old description",
				BodyTypeId = 1,
				FuelTypeId = 1,
				TransmissionTypeId = 1,
				Year = 2019,
				Kilometers = 10000,
				EngineCapacity = 1.5m,
				Price = 15000,
				LocationCity = "Old City",
				LocationTown = "Old Town",
				Images = new List<Image>
			{
				new Image { Id = "123", CreatorId = userId, Extension = "jpg" }
			}
			};
			_dbContext.Cars.Add(existingCar);
			_dbContext.SaveChanges();

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(async () =>
				await _service.UpdateCarDataFromInputModelAsync(carId, inputCar, selectedExtrasIds, deletedImagesIds, userId, imagePath, coverImageId));

			_dbContext.Cars.Remove(existingCar);
		}

		[Fact]
		public async Task UpdateCarDataFromInputModelAsync_ShouldThrowException_WhenImagesExceedLimit()
		{
			var _service = GetCarsService();
			var _dbContext = GetDbContext();

			// Arrange
			
			var inputCar = new CarFormInputModelDTO
			{
				Images = Enumerable.Range(1, 10).Select(i => new ImageFile { FileName = $"test{i}.jpg", Image = new byte[] { 1, 2, 3, 4 } }).ToHashSet()
			};
			var selectedExtrasIds = new List<int>();
			var deletedImagesIds = new List<string>();
			var userId = "testUserId";
			var imagePath = "testPath";
			var coverImageId = "123";

			var carId = 19;

			while (_dbContext.Cars.ToList().Exists(car => car.Id == carId))
			{
				carId++;
			}

			var existingCar = new Car
			{
				Id = carId,
				MakeId = 1,
				CarModelId = 1,
				Description = "Old description",
				BodyTypeId = 1,
				FuelTypeId = 1,
				TransmissionTypeId = 1,
				Year = 2019,
				Kilometers = 10000,
				EngineCapacity = 1.5m,
				Price = 15000,
				LocationCity = "Old City",
				LocationTown = "Old Town",
				Images = new List<Image>
			{
				new Image { Id = "123", CreatorId = userId, Extension = "jpg" }
			}
			};
						
			_dbContext.Cars.Add(existingCar);
			_dbContext.SaveChanges();

			// Act & Assert
			await Assert.ThrowsAsync<Exception>(async () =>
				await _service.UpdateCarDataFromInputModelAsync(carId, inputCar, selectedExtrasIds, deletedImagesIds, userId, imagePath, coverImageId));

			_dbContext.Cars.Remove(existingCar);
		}

		[Fact]
		public async Task DeleteCarByIdAsync_ShouldMarkCarAsDeleted()
		{
			var optionsBuilder = new DbContextOptionsBuilder<FastCarSalesDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new FastCarSalesDbContext(optionsBuilder.Options);
			var imagesServiceMock = new Mock<IImagesService>();

			var carsService = new CarsService(dbContext, imagesServiceMock.Object, this._mapper);

			// Arrange
			var carId = 7;
			var car = new Car
			{
				Id = carId,
				MakeId = 1,
				CarModelId = 1,
				Description = "Test car",
				BodyTypeId = 1,
				FuelTypeId = 1,
				TransmissionTypeId = 1,
				Year = 2020,
				Kilometers = 10000,
				EngineCapacity = 2.0m,
				Price = 20000,
				LocationCity = "Test City",
				LocationTown = "Test Town",
				IsDeleted = false
			};
			dbContext.Cars.Add(car);
			dbContext.SaveChanges();

			// Act
			await carsService.DeleteCarByIdAsync(carId);

			// Assert
			var deletedCar = dbContext.Cars.First(c => c.Id == carId);
			Assert.True(deletedCar.IsDeleted);
			Assert.NotNull(deletedCar.DeletedOn);
			Assert.True(deletedCar.DeletedOn <= DateTime.UtcNow);
		}


		[Fact]
		public async Task DeleteCarByIdShouldMarkTheCarAsDeletedWhenFound()
		{
			//Arrange
			var testCar = new Car
			{
				Id = 56,
				IsDeleted = false,
				Description ="wow",
				LocationCity = "Harare",
				LocationTown ="TestTown"
			};

			var optionsBuilder = new DbContextOptionsBuilder<FastCarSalesDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new FastCarSalesDbContext(optionsBuilder.Options);
			var imagesServiceMock = new Mock<IImagesService>();

			dbContext.Cars.Add(testCar);
			dbContext.SaveChanges();

			var carsService = new CarsService(dbContext, imagesServiceMock.Object, this._mapper);

			//Act
			//carsService.DeleteCarByIdAsync(testCar.Id).GetAwaiter().GetResult();
			await carsService.DeleteCarByIdAsync(testCar.Id);

			var resultCar = dbContext.Cars.FirstOrDefault(x => x.Id == 56);
			//Assert
			Assert.True(testCar.IsDeleted);
			 Assert.True(resultCar.IsDeleted);
		}


		[Fact]
		public void GetAllMakes_ShouldReturnAllMakes()
		{
			var optionsBuilder = new DbContextOptionsBuilder<FastCarSalesDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
			var dbContext = new FastCarSalesDbContext(optionsBuilder.Options);
			var imagesServiceMock = new Mock<IImagesService>();

			var carsService = new CarsService(dbContext, imagesServiceMock.Object, this._mapper);

			// Arrange
			var makes = new List<Make>
		{
			new Make { Id = 1, Name = "Make1" },
			new Make { Id = 2, Name = "Make2" }
		};

			dbContext.Makes.AddRange(makes);
			dbContext.SaveChanges();

			// Act
			var result = carsService.GetAllMakes();

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
			Assert.Equal("Make1", result.First().Name);
			Assert.Equal("Make2", result.Last().Name);

			dbContext.Makes.RemoveRange(makes);
		}
	}




}
