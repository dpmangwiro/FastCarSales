using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastCarSales.Data.Models;
using FastCarSales.Data;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Images;
using FastCarSales.Data.Dtos;
using Microsoft.EntityFrameworkCore.Storage;

namespace FastCarSales.Services.Cars
{
	public class CarsService : ICarsService
	{
		private readonly FastCarSalesDbContext data;
		private readonly IImagesService imagesService;
		private readonly IConfigurationProvider mapperConfiguration;

		public CarsService(FastCarSalesDbContext data, IImagesService imagesService, IMapper mapper)
		{
			this.data = data;
			this.imagesService = imagesService;
			this.mapperConfiguration = mapper.ConfigurationProvider;
		}

		public async Task<Car> GetCarFromInputModelAsync(CarFormInputModelDTO inputCar, List<int> selectedExtrasIds, string userId, string imageRootDirectoryPath)
		{
			try
			{

				var car = new Car()
				{
					MakeId = inputCar.MakeId,
					CarModelId = inputCar.CarModelId,
					Description = inputCar.Description,
					BodyTypeId = inputCar.BodyTypeId,
					FuelTypeId = inputCar.FuelTypeId,
					TransmissionTypeId = inputCar.TransmissionTypeId,
					Year = inputCar.Year ?? 0,
					Kilometers = inputCar.Kilometers ?? 0,
					EngineCapacity = inputCar.EngineCapacity ?? 0m,
					Price = inputCar.Price ?? 0,
					LocationCity = inputCar.LocationCity,
					LocationTown = inputCar.LocationTown,
				};

				if (selectedExtrasIds.Any())
				{
					foreach (var extraId in selectedExtrasIds)
					{
						var extra = this.data.Extras.FirstOrDefault(e => e.Id == extraId);

						if (extra != null)
						{
							car.CarExtras.Add(new CarExtra
							{
								Extra = extra,
								Car = car,
							});
						}
					}
				}

				if (!inputCar.Images.Any())
				{
					throw new Exception($"At least one car image is required.");
				}

				if (inputCar.Images.Count() > 10)
				{
					throw new Exception($"The maximum allowed number of photos is 10.");
				}

				foreach (var image in inputCar.Images)
				{
					var dbImage = await this.imagesService.UploadImageAsync(image, userId, imageRootDirectoryPath);
					car.Images.Add(dbImage);

					if (image.IsCoverImage)
					{
						await imagesService.SetCoverImagePropertyAsync(image.Id);
					}
				}

				return car;
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task UpdateCarDataFromInputModelAsync(int carId, CarFormInputModelDTO inputCar, List<int> selectedExtrasIds,
				List<string> deletedImagesIds, string userId, string imagePath, string selectedCoverImageId)
		{
			using (var transaction = await this.data.Database.BeginTransactionAsync())
			{
				try
				{

					var car = this.GetDbCarById(carId);

					if (car == null)
					{
						throw new Exception($"Unfortunately, such car in our system doesn't exist!");
					}

					car.MakeId = inputCar.MakeId;
					car.CarModelId = inputCar.CarModelId;
					car.Description = inputCar.Description;
					car.BodyTypeId = inputCar.BodyTypeId;
					car.FuelTypeId = inputCar.FuelTypeId;
					car.TransmissionTypeId = inputCar.TransmissionTypeId;
					car.Year = inputCar.Year ?? 0;
					car.Kilometers = inputCar.Kilometers ?? 0;
					car.EngineCapacity = inputCar.EngineCapacity ?? 0;
					car.Price = inputCar.Price ?? 0;
					car.LocationCity = inputCar.LocationCity;
					car.LocationTown = inputCar.LocationTown;

					if (selectedExtrasIds.Any())
					{
						var currentExtrasIds = this.data.CarExtras.Where(ce => ce.CarId == carId).Select(ce => ce.ExtraId).ToList();

						foreach (var extraId in selectedExtrasIds)
						{
							var extra = this.data.Extras.FirstOrDefault(e => e.Id == extraId);

							if (extra != null && !currentExtrasIds.Contains(extraId))
							{
								car.CarExtras.Add(new CarExtra
								{
									Extra = extra,
									Car = car,
								});
							}
						}

						if (selectedExtrasIds.Count() < currentExtrasIds.Count())
						{
							var deletedExtrasIds = currentExtrasIds.Where(extraId => !selectedExtrasIds.Contains(extraId)).ToList();

							foreach (var deletedExtraId in deletedExtrasIds)
							{
								var deletedCarExtra = this.data.CarExtras.First(ce => ce.CarId == carId && ce.ExtraId == deletedExtraId);

								this.data.CarExtras.Remove(deletedCarExtra);
							}
						}
					}

					var inCarNewImages = inputCar.Images;

					var dbExistingImages = this.data.Images.Where(img => img.CarId == carId).ToList();

					if (deletedImagesIds.Count() >= dbExistingImages.Count() && !inputCar.Images.Any())
					{
						throw new Exception($"You cannot delete all car images. At least one car image is required for each post.");
					}

					if (deletedImagesIds.Any())
					{
						foreach (var deletedImageId in deletedImagesIds)
						{
							if (dbExistingImages.Any(img => img.Id == deletedImageId))
							{
								var imageToRemove = this.data.Images.First(img => img.Id == deletedImageId);
								this.data.Images.Remove(imageToRemove);
								imagesService.DeleteImageFromPhysicalFile(imageToRemove.Id + "." + imageToRemove.Extension, imageRootDirectoryPath: imagePath, transaction: transaction);
							}
						}
					}

					if (inCarNewImages != null)
					{
						if (inCarNewImages.Count() + dbExistingImages.Count() > 10)
						{
							throw new Exception($"The maximum allowed number of car images is 10.");
						}

						foreach (var image in inCarNewImages)
						{
							var dbImage = await this.imagesService.UploadImageAsync(image, userId, imagePath);
							car.Images.Add(dbImage);
						}
					}

					var oldCoverImageId = dbExistingImages.FirstOrDefault(x => x.IsCoverImage)?.Id;

					if (string.IsNullOrEmpty(oldCoverImageId) || oldCoverImageId != selectedCoverImageId)
					{
						await this.imagesService.SetCoverImagePropertyAsync(selectedCoverImageId!);

						if (!string.IsNullOrEmpty(oldCoverImageId))
						{
							await this.imagesService.RemoveCoverImagePropertyAsync(oldCoverImageId);
						}
					}

					await this.data.SaveChangesAsync();

					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw;
				}
			}
		}

		public async Task DeleteCarByIdAsync(int carId, IDbContextTransaction transaction)
		{
			var car = this.GetDbCarById(carId);

			if (car is null)
			{
				throw new Exception("Car does not exist in this post");
			}

			car.IsDeleted = true;
			car.DeletedOn = DateTime.UtcNow;

			await this.data.SaveChangesAsync();
		}

		public async Task EmptyRecycleBinAsync(int carId, string imageRootDirectoryPath ,IDbContextTransaction transaction)
		{
			var carExists = this.data.Cars.ToList().Exists(c => c.Id == carId);

			if (carExists == false)
			{
				throw new Exception("Car does not exist in this post");
			}

			var images = this.data.Cars.First(x => x.Id == carId).Images.ToList();

			imagesService.DeleteImages(images, imageRootDirectoryPath, transaction);

			this.data.Cars.Remove(this.data.Cars.First(x => x.Id == carId));

			await this.data.SaveChangesAsync();
		}

		public async Task RestoreDeletedCar(int carId, IDbContextTransaction transaction)
		{
			var car = this.data.Cars.FirstOrDefault(c => c.Id == carId);

			if (car is null)
			{
				throw new Exception("Car does not exist in this post");
			}

			car.IsDeleted = false;
			car.DeletedOn = null;

			await this.data.SaveChangesAsync();
		}


		public IEnumerable<BaseCarSpecificationServiceModel> GetAllMakes()
		{
			return this.data
				.Makes
				.ProjectTo<BaseCarSpecificationServiceModel>(this.mapperConfiguration)
				.ToList();
		}

		public IEnumerable<BaseCarSpecificationServiceModel> GetAllCarModels()
		{
			return this.data
				.CarModels
				.ProjectTo<BaseCarSpecificationServiceModel>(this.mapperConfiguration)
				.ToList();
		}

		public IEnumerable<CarModel> GetCarModels(int makeID)
		{
			return this.data
				.CarModels
				.Where(x => x.MakeId == makeID)
				.ToList();
		}

		public IEnumerable<CarModel> GetCarModels()
		{
			return this.data
				.CarModels
				.ToList();
		}

		public IEnumerable<BaseCarSpecificationServiceModel> GetAllBodyTypes()
		{
			return this.data
				.BodyTypes
				.ProjectTo<BaseCarSpecificationServiceModel>(this.mapperConfiguration)
				.ToList();
		}


		private CarModel? GetCarModel(int ModelID)
		{
			return this.data
				.CarModels
				.FirstOrDefault(x => x.Id == ModelID);

		}

		public IEnumerable<BaseCarSpecificationServiceModel> GetBodyTypes(int ModelID)
		{
			return this.data
				.CarModels
				.Where(x => x.Id == ModelID)
				.Select(x => x.BodyType)
				 .ProjectTo<BaseCarSpecificationServiceModel>(this.mapperConfiguration)
				.ToList();

		}

		public BaseCarSpecificationServiceModel? GetBodyType(int ModelID)
		{
			return this.data
				.CarModels
				.Where(x => x.Id == ModelID)
				.Select(x => x.BodyType)
				.ProjectTo<BaseCarSpecificationServiceModel>(this.mapperConfiguration)
				.FirstOrDefault();

		}

		public IEnumerable<BaseCarSpecificationServiceModel> GetAllFuelTypes()
		{
			return this.data
				.FuelTypes
				.ProjectTo<BaseCarSpecificationServiceModel>(this.mapperConfiguration)
				.ToList();
		}

		public IEnumerable<BaseCarSpecificationServiceModel> GetAllTransmissionTypes()
		{
			return this.data
				.TransmissionTypes
				.ProjectTo<BaseCarSpecificationServiceModel>(this.mapperConfiguration)
				.ToList();
		}

		public IEnumerable<CarExtrasServiceModel> GetAllCarExtras()
		{
			return this.data
				.Extras
				.OrderBy(e => e.TypeId)
				.ProjectTo<CarExtrasServiceModel>(this.mapperConfiguration)
				.ToList();
		}

		public BaseCarPropertyListsDTO FillInputCarBaseProperties()
		{
			BaseCarPropertyListsDTO inputCar = new BaseCarPropertyListsDTO();

			inputCar.BodyTypes = this.GetAllBodyTypes();
			inputCar.FuelTypes = this.GetAllFuelTypes();
			inputCar.TransmissionTypes = this.GetAllTransmissionTypes();
			inputCar.CarExtras = this.GetAllCarExtras();
			inputCar.Makes = this.GetAllMakes();
			inputCar.CarModels = this.GetAllCarModels();

			return inputCar;
		}

		private Car? GetDbCarById(int carId)
		{
			return this.data.Cars.FirstOrDefault(c => c.Id == carId && !c.IsDeleted);
		}
	}
}