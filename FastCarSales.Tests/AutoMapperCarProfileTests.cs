using AutoMapper;
using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.ComponentModels.Cars.ViewModel;
using FastCarSales.ComponentModels.Images;
using FastCarSales.Data.Dtos;
using FastCarSales.Data.Models;
using FastCarSales.MapperConfigurations.Profiles;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Web.ViewModels.Cars;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace AutoMapperCarProfileTest
{

    public class AutoMapperCarProfileTests
	{
		private readonly IConfigurationProvider _configuration;
		private readonly IMapper _mapper;

		public AutoMapperCarProfileTests()
		{
			_configuration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<CarsProfile>();
				cfg.AddProfile<ImagesProfile>();
			});
			_mapper = _configuration.CreateMapper();
		}

		[Fact]
		public void AutoMapper_ConfigurationIsValid()
		{
			_configuration.AssertConfigurationIsValid();
		}

		[Fact]
		public void AutoMapper_CarFormInputModelDTO_To_CarFormInputModel()
		{
			//this.CreateMap<CarFormInputModelDTO, CarFormInputModel>().ReverseMap();
			// Arrange
			var inputDto = new CarFormInputModelDTO
			{
				MakeId = 1,
				CarModelId = 1,
				Description = "A nice car",
				BodyTypeId = 1,
				FuelTypeId = 1,
				TransmissionTypeId = 1,
				Year = 2020,
				Kilometers = 10000,
				EngineCapacity = 2.0m,
				Price = 20000,
				LocationCity = "Harare",
				LocationTown = "Town",
				Images = new HashSet<ImageFile> { },
				
			};

			// Act
			var result = _mapper.Map<CarFormInputModel>(inputDto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(inputDto.MakeId, result.MakeId);
			Assert.Equal(inputDto.CarModelId, result.CarModelId);
			Assert.Equal(inputDto.Description, result.Description);
			Assert.Equal(inputDto.BodyTypeId, result.BodyTypeId);
			Assert.Equal(inputDto.FuelTypeId, result.FuelTypeId);
			Assert.Equal(inputDto.TransmissionTypeId, result.TransmissionTypeId);
			Assert.Equal(inputDto.Year, result.Year);
			Assert.Equal(inputDto.Kilometers, result.Kilometers);
			Assert.Equal(inputDto.EngineCapacity, result.EngineCapacity);
			Assert.Equal(inputDto.Price, result.Price);
			Assert.Equal(inputDto.LocationCity, result.LocationCity);
			Assert.Equal(inputDto.LocationTown, result.LocationTown);
		}


		[Fact]
		public void AutoMapper_BaseCarInputModelDTO_To_BaseCarInputModel()
		{
			// Arrange
			var inputDto = new BaseCarInputModelDTO
			{
				MakeId = 1,
				CarModelId = 1,
				BodyTypeId = 1,
				FuelTypeId = 1,
				TransmissionTypeId = 1,
				CarExtraId = 1				
			};

			// Act
			var result = _mapper.Map<BaseCarInputModel>(inputDto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(inputDto.MakeId, result.MakeId);
			Assert.Equal(inputDto.CarModelId, result.CarModelId);
			Assert.Equal(inputDto.BodyTypeId, result.BodyTypeId);
			Assert.Equal(inputDto.FuelTypeId, result.FuelTypeId);
			Assert.Equal(inputDto.TransmissionTypeId, result.TransmissionTypeId);
			Assert.Equal(inputDto.CarExtraId, result.CarExtraId);			;
		}



		[Fact]
		public void AutoMapper_BodyType_To_BaseCarSpecificationServiceModel()
		{
			// Arrange
			var bodyType = new BodyType
			{
				Id = 1,
				Name = "Sedan",
				Cars = new List<Car>(),
				CarModels = new List<CarModel>()
			};

			// Act
			var result = _mapper.Map<BaseCarSpecificationServiceModel>(bodyType);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(bodyType.Id, result.Id);
			Assert.Equal(bodyType.Name, result.Name);
		}

		[Fact]
		public void AutoMapper_FuelType_To_BaseCarSpecificationServiceModel()
		{
			// Arrange
			var bodyType = new FuelType
			{
				Id = 1,
				Name = "Petrol",
				Cars = new List<Car>(),
				CarModels = new List<CarModel>()
			};

			// Act
			var result = _mapper.Map<BaseCarSpecificationServiceModel>(bodyType);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(bodyType.Id, result.Id);
			Assert.Equal(bodyType.Name, result.Name);
		}

		[Fact]
		public void AutoMapper_Make_To_BaseCarSpecificationServiceModel()
		{
			// Arrange
			var bodyType = new Make
			{
				Id = 1,
				Name = "Petrol",
				Cars = new List<Car>(),
				CarModels = new List<CarModel>()
			};

			// Act
			var result = _mapper.Map<BaseCarSpecificationServiceModel>(bodyType);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(bodyType.Id, result.Id);
			Assert.Equal(bodyType.Name, result.Name);
		}

		[Fact]
		public void AutoMapper_TransmissionType_To_BaseCarSpecificationServiceModel()
		{
			// Arrange
			var bodyType = new TransmissionType
			{
				Id = 1,
				Name = "Petrol",
				Cars = new List<Car>(),
				CarModels = new List<CarModel>()
			};

			// Act
			var result = _mapper.Map<BaseCarSpecificationServiceModel>(bodyType);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(bodyType.Id, result.Id);
			Assert.Equal(bodyType.Name, result.Name);
		}


		[Fact]
		public void AutoMapper_Extra_To_CarExtrasServiceModel()
		{
			// Arrange
			var extra = new Extra
			{
				Id = 1,
				Name = "Sunroof",
				TypeId = 2,
				Type = new ExtraType { Id = 2, Name = "Luxury" },
				CarExtras = new List<CarExtra>()
			};

			// Act
			var result = _mapper.Map<CarExtrasServiceModel>(extra);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(extra.Id, result.Id);
			Assert.Equal(extra.Name, result.Name);
			Assert.Equal(extra.TypeId, result.TypeId);
			Assert.Equal(extra.Type.Name, result.TypeName);
		}


		[Fact]
		public void AutoMapper_BaseCarSpecificationServiceModel_To_BaseCarSpecificationViewModel()
		{
			// Arrange
			var serviceModel = new BaseCarSpecificationServiceModel
			{
				Id = 1,
				Name = "Sedan"
			};

			// Act
			var result = _mapper.Map<BaseCarSpecificationViewModel>(serviceModel);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(serviceModel.Id, result.Id);
			Assert.Equal(serviceModel.Name, result.Name);
		}

		[Fact]
		public void AutoMapper_BaseCarSpecificationViewModel_To_BaseCarSpecificationServiceModel()
		{
			// Arrange
			var viewModel = new BaseCarSpecificationViewModel
			{
				Id = 2,
				Name = "SUV"
			};

			// Act
			var result = _mapper.Map<BaseCarSpecificationServiceModel>(viewModel);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(viewModel.Id, result.Id);
			Assert.Equal(viewModel.Name, result.Name);
		}


		[Fact]
		public void AutoMapper_CarExtrasServiceModel_To_CarExtrasViewModel()
		{
			// Arrange
			var serviceModel = new CarExtrasServiceModel
			{
				Id = 1,
				Name = "Leather Seats",
				TypeId = 2,
				TypeName = "Interior"
			};

			// Act
			var result = _mapper.Map<CarExtrasViewModel>(serviceModel);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(serviceModel.Id, result.Id);
			Assert.Equal(serviceModel.Name, result.Name);
			Assert.Equal(serviceModel.TypeId, result.TypeId);
			Assert.Equal(serviceModel.TypeName, result.TypeName);
		}

		[Fact]
		public void AutoMapper_CarExtrasViewModel_To_CarExtrasServiceModel()
		{
			// Arrange
			var viewModel = new CarExtrasViewModel
			{
				Id = 2,
				Name = "Sunroof",
				TypeId = 3,
				TypeName = "Exterior"
			};

			// Act
			var result = _mapper.Map<CarExtrasServiceModel>(viewModel);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(viewModel.Id, result.Id);
			Assert.Equal(viewModel.Name, result.Name);
			Assert.Equal(viewModel.TypeId, result.TypeId);
			Assert.Equal(viewModel.TypeName, result.TypeName);
		}

		[Fact]
		public void AutoMapper_BaseCarDTO_To_BaseCarViewModel()
		{
			// Arrange
			var baseCarDTO = new BaseCarDTO
			{
				Id = 1,
				MakeId = 2,
				Make = "Toyota",
				CarModelId = 3,
				CarModel = new CarModel { Id = 3, Name = "Corolla" },
				Year = 2020,
				Price = 15000
			};

			// Act
			var result = _mapper.Map<BaseCarViewModel>(baseCarDTO);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(baseCarDTO.Id, result.Id);
			Assert.Equal(baseCarDTO.Make, result.Make);
			Assert.Equal(baseCarDTO.CarModelId, result.CarModelId);
			Assert.Equal(baseCarDTO.CarModel.Name, result.CarModel.Name);
			Assert.Equal(baseCarDTO.Year, result.Year);
			Assert.Equal(baseCarDTO.Price, result.Price);
		}

		[Fact]
		public void AutoMapper_BaseCarViewModel_To_BaseCarDTO()
		{
			// Arrange
			var baseCarViewModel = new BaseCarViewModel
			{
				Id = 1,
				MakeId = 2,
				Make = "Toyota",
				CarModelId = 3,
				CarModel = new CarModel { Id = 3, Name = "Corolla" },
				Year = 2020,
				Price = 15000
			};

			//// Act
			var result = _mapper.Map<BaseCarDTO>(baseCarViewModel);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(baseCarViewModel.Id, result.Id);
			Assert.Equal(baseCarViewModel.MakeId, result.MakeId);
			Assert.Equal(baseCarViewModel.Make, result.Make);
			Assert.Equal(baseCarViewModel.CarModelId, result.CarModelId);
			Assert.Equal(baseCarViewModel.CarModel.Name, result.CarModel.Name);
			Assert.Equal(baseCarViewModel.Year, result.Year);
			Assert.Equal(baseCarViewModel.Price, result.Price);
		}


		[Fact]
		public void AutoMapper_SearchCarInputModelDTO_To_SearchCarInputModel()
		{
			// Arrange
			var dto = new SearchCarInputModelDTO
			{
				//MakeId = 1,
				//CarModelId = 1,
				//BodyTypeId = 1,
				//FuelTypeId = 1,
				//TransmissionTypeId = 1,
				//CarExtraId = 1,
				TextSearchTerm = "SUV",
				FromYear = 2015,
				ToYear = 2020,
				MinEngineCapacity = 1.6m,
				MaxEngineCapacity = 2.5m,
				MinPrice = 5000,
				MaxPrice = 30000
			};

			// Act
			var result = _mapper.Map<SearchCarInputModel>(dto);

			// Assert
			Assert.NotNull(result);
			//Assert.Equal(dto.MakeId, result.MakeId);
			//Assert.Equal(dto.CarModelId, result.CarModelId);
			//Assert.Equal(dto.BodyTypeId, result.BodyTypeId);
			//Assert.Equal(dto.FuelTypeId, result.FuelTypeId);
			//Assert.Equal(dto.TransmissionTypeId, result.TransmissionTypeId);
			//Assert.Equal(dto.CarExtraId, result.CarExtraId);
			Assert.Equal(dto.TextSearchTerm, result.TextSearchTerm);
			Assert.Equal(dto.FromYear, result.FromYear);
			Assert.Equal(dto.ToYear, result.ToYear);
			Assert.Equal(dto.MinEngineCapacity, result.MinEngineCapacity);
			Assert.Equal(dto.MaxEngineCapacity, result.MaxEngineCapacity);
			Assert.Equal(dto.MinPrice, result.MinPrice);
			Assert.Equal(dto.MaxPrice, result.MaxPrice);
		}

		[Fact]
		public void AutoMapper_SearchCarInputModel_To_SearchCarInputModelDTO()
		{
			// Arrange
			var model = new SearchCarInputModel
			{
				//MakeId = 1,
				//CarModelId = 1,
				//BodyTypeId = 1,
				//FuelTypeId = 1,
				//TransmissionTypeId = 1,
				//CarExtraId = 1,
				TextSearchTerm = "SUV",
				FromYear = 2015,
				ToYear = 2020,
				MinEngineCapacity = 1.6m,
				MaxEngineCapacity = 2.5m,
				MinPrice = 5000,
				MaxPrice = 30000
			};

			// Act
			var result = _mapper.Map<SearchCarInputModelDTO>(model);

			// Assert
			Assert.NotNull(result);
			//Assert.Equal(model.MakeId, result.MakeId);
			//Assert.Equal(model.CarModelId, result.CarModelId);
			//Assert.Equal(model.BodyTypeId, result.BodyTypeId);
			//Assert.Equal(model.FuelTypeId, result.FuelTypeId);
			//Assert.Equal(model.TransmissionTypeId, result.TransmissionTypeId);
			//Assert.Equal(model.CarExtraId, result.CarExtraId);
			Assert.Equal(model.TextSearchTerm, result.TextSearchTerm);
			Assert.Equal(model.FromYear, result.FromYear);
			Assert.Equal(model.ToYear, result.ToYear);
			Assert.Equal(model.MinEngineCapacity, result.MinEngineCapacity);
			Assert.Equal(model.MaxEngineCapacity, result.MaxEngineCapacity);
			Assert.Equal(model.MinPrice, result.MinPrice);
			Assert.Equal(model.MaxPrice, result.MaxPrice);
		}


		[Fact]
		public void AutoMapper_CarInListDTO_To_CarInListViewModel()
		{
			// Arrange
			var carInListDTO = new CarInListDTO
			{
				Id = 1,
				MakeId = 2,
				Make = "Toyota",
				CarModelId = 3,
				CarModel = new CarModel { Id = 3, Name = "Corolla", Year = 2013 },
				Year = 2020,
				Price = 15000,
				Description = "Well maintained",
				Kilometers = 20000,
				BodyType = "Sedan",
				FuelType = "Petrol",
				TransmissionType = "Automatic",
				CoverImage = "image.jpg",
				LocationCity = "Harare",
				LocationTown = "Town"
			};

			// Act
			var result = _mapper.Map<CarInListViewModel>(carInListDTO);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(carInListDTO.Id, result.Id);
			Assert.Equal(carInListDTO.MakeId, result.MakeId);
			Assert.Equal(carInListDTO.CarModelId, result.CarModelId);
			Assert.Equal(carInListDTO.CarModel.Name, result.CarModel.Name);
			Assert.Equal(carInListDTO.CarModel.Id, result.CarModel.Id);
			Assert.Equal(carInListDTO.CarModel.Year, result.CarModel.Year);
			Assert.Equal(carInListDTO.Year, result.Year);
			Assert.Equal(carInListDTO.Price, result.Price);
			Assert.Equal(carInListDTO.Description, result.Description);
			Assert.Equal(carInListDTO.Kilometers, result.Kilometers);
			Assert.Equal(carInListDTO.BodyType, result.BodyType);
			Assert.Equal(carInListDTO.FuelType, result.FuelType);
			Assert.Equal(carInListDTO.TransmissionType, result.TransmissionType);
			Assert.Equal(carInListDTO.CoverImage, result.CoverImage);
			Assert.Equal(carInListDTO.LocationCity, result.LocationCity);
			Assert.Equal(carInListDTO.LocationTown, result.LocationTown);
		}

		[Fact]
		public void AutoMapper_CarInListViewModel_To_CarInListDTO()
		{
			// Arrange
			var carInListViewModel = new CarInListViewModel
			{
				Id = 1,
				MakeId = 2,
				Make = "Toyota",
				CarModelId = 3,
				CarModel = new CarModel { Id = 3, Name = "Corolla", Year = 2013 },
				Year = 2020,
				Price = 15000,
				Description = "Well maintained",
				Kilometers = 20000,
				BodyType = "Sedan",
				FuelType = "Petrol",
				TransmissionType = "Automatic",
				CoverImage = "image.jpg",
				LocationCity = "Harare",
				LocationTown = "Town"
			};

			var make = new Make { Id = 2, Name = "Toyota" };
			var carModel = new CarModel { Id = 3, Name = "Corolla" };

			// Act
			var result = _mapper.Map<CarInListDTO>(carInListViewModel);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(carInListViewModel.Id, result.Id);
			Assert.Equal(carInListViewModel.MakeId, result.MakeId);
			Assert.Equal(carInListViewModel.CarModelId, result.CarModelId);
			Assert.Equal(carInListViewModel.CarModel.Name, result.CarModel.Name);
			Assert.Equal(carInListViewModel.CarModel.Id, result.CarModel.Id);
			Assert.Equal(carInListViewModel.CarModel.Year, result.CarModel.Year);
			Assert.Equal(carInListViewModel.Year, result.Year);
			Assert.Equal(carInListViewModel.Price, result.Price);
			Assert.Equal(carInListViewModel.Description, result.Description);
			Assert.Equal(carInListViewModel.Kilometers, result.Kilometers);
			Assert.Equal(carInListViewModel.BodyType, result.BodyType);
			Assert.Equal(carInListViewModel.FuelType, result.FuelType);
			Assert.Equal(carInListViewModel.TransmissionType, result.TransmissionType);
			Assert.Equal(carInListViewModel.CoverImage, result.CoverImage);
			Assert.Equal(carInListViewModel.LocationCity, result.LocationCity);
			Assert.Equal(carInListViewModel.LocationTown, result.LocationTown);
		}


		[Fact]
		public void AutoMapper_SingleCarDTO_To_SingleCarViewModel()
		{
			// Arrange
			var dto = new SingleCarDTO
			{
				Id = 1,
				MakeId = 2,
				Make = "Toyota",
				CarModelId = 3,
				CarModel = new CarModel { Id = 3, Name = "Corolla" },
				Year = 2020,
				Price = 15000,
				Description = "A nice car",
				Kilometers = 5000,
				EngineCapacity = 1.8m,
				BodyType = "Sedan",
				FuelType = "Petrol",
				TransmissionType = "Automatic",
				Images = new List<string> { "image1.jpg", "image2.jpg" },
				ComfortExtras = new List<string> { "Air Conditioning", "Leather Seats" },
				SafetyExtras = new List<string> { "Airbags", "ABS" },
				OtherExtras = new List<string> { "Sunroof" },
				LocationCity = "Harare",
				LocationTown = "Epworth"
			};

			// Act
			var result = _mapper.Map<SingleCarViewModel>(dto);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(dto.Id, result.Id);
			Assert.Equal(dto.Make, result.Make);
			Assert.Equal(dto.CarModel.Name, result.CarModel.Name);
			Assert.Equal(dto.Year, result.Year);
			Assert.Equal(dto.Price, result.Price);
			Assert.Equal(dto.Description, result.Description);
			Assert.Equal(dto.Kilometers, result.Kilometers);
			Assert.Equal(dto.EngineCapacity, result.EngineCapacity);
			Assert.Equal(dto.BodyType, result.BodyType);
			Assert.Equal(dto.FuelType, result.FuelType);
			Assert.Equal(dto.TransmissionType, result.TransmissionType);
			Assert.Equal(dto.Images, result.Images);
			Assert.Equal(dto.ComfortExtras, result.ComfortExtras);
			Assert.Equal(dto.SafetyExtras, result.SafetyExtras);
			Assert.Equal(dto.OtherExtras, result.OtherExtras);
			Assert.Equal(dto.LocationCity, result.LocationCity);
			Assert.Equal(dto.LocationTown, result.LocationTown);
		}

		[Fact]
		public void AutoMapper_SingleCarViewModel_To_SingleCarDTO()
		{
			// Arrange
			var viewModel = new SingleCarViewModel
			{
				Id = 1,
				MakeId = 3,
				Make = "Toyota",
				CarModelId = 7,
				CarModel = new CarModel { Name = "ModelT", Id = 7, MakeId = 1, Year = 2012 },
				Year = 2020,
				Price = 15000,
				Description = "A nice car",
				Kilometers = 5000,
				EngineCapacity = 1.8m,
				BodyType = "Sedan",
				FuelType = "Petrol",
				TransmissionType = "Automatic",
				Images = new List<string> { "image1.jpg", "image2.jpg" },
				ComfortExtras = new List<string> { "Air Conditioning", "Leather Seats" },
				SafetyExtras = new List<string> { "Airbags", "ABS" },
				OtherExtras = new List<string> { "Sunroof" },
				LocationCity = "Harare",
				LocationTown = "Epworth"
			};


			// Act
			var result = _mapper.Map<SingleCarDTO>(viewModel);


			// Assert
			Assert.NotNull(result);
			Assert.Equal(viewModel.Id, result.Id);
			Assert.Equal(viewModel.Make, result.Make);
			Assert.Equal(viewModel.CarModelId, result.CarModelId);
			Assert.Equal(viewModel.CarModel.Name, result.CarModel.Name);
			Assert.Equal(viewModel.Year, result.Year);
			Assert.Equal(viewModel.Price, result.Price);
			Assert.Equal(viewModel.Description, result.Description);
			Assert.Equal(viewModel.Kilometers, result.Kilometers);
			Assert.Equal(viewModel.EngineCapacity, result.EngineCapacity);
			Assert.Equal(viewModel.BodyType, result.BodyType);
			Assert.Equal(viewModel.FuelType, result.FuelType);
			Assert.Equal(viewModel.TransmissionType, result.TransmissionType);
			Assert.Equal(viewModel.Images, result.Images);
			Assert.Equal(viewModel.ComfortExtras, result.ComfortExtras);
			Assert.Equal(viewModel.SafetyExtras, result.SafetyExtras);
			Assert.Equal(viewModel.OtherExtras, result.OtherExtras);
			Assert.Equal(viewModel.LocationCity, result.LocationCity);
			Assert.Equal(viewModel.LocationTown, result.LocationTown);
		}


		[Fact]
		public void Should_Map_LatestPostsCarDTO_To_LatestPostsCarViewModel()
		{
			// Arrange
			var dto = new LatestPostsCarDTO
			{
				Id = 1,
				MakeId = 2,
				CarModelId = 3,
				EngineCapacity = 2.0m,
				FuelType = "Petrol",
				TransmissionType = "Automatic",
				CoverImage = "image.jpg",
				Year = 2022,
				Price = 25000m
			};

			// Act
			var viewModel = _mapper.Map<LatestPostsCarViewModel>(dto);

			// Assert
			Assert.Equal(dto.Id, viewModel.Id);
			Assert.Equal(dto.EngineCapacity, viewModel.EngineCapacity);
			Assert.Equal(dto.FuelType, viewModel.FuelType);
			Assert.Equal(dto.TransmissionType, viewModel.TransmissionType);
			Assert.Equal(dto.CoverImage, viewModel.CoverImage);
			Assert.Equal(dto.Year, viewModel.Year);
			Assert.Equal(dto.Price, viewModel.Price);
			Assert.Equal(dto.Make, viewModel.Make);
			Assert.Equal(dto.CarModel.Name, viewModel.CarModel.Name);
		}

		[Fact]
		public void Should_Map_LatestPostsCarViewModel_To_LatestPostsCarDTO()
		{
			// Arrange
			var viewModel = new LatestPostsCarViewModel
			{
				Id = 1,
				EngineCapacity = 2.0m,
				FuelType = "Petrol",
				TransmissionType = "Automatic",
				CoverImage = "image.jpg",
				Year = 2022,
				Price = 25000m,
				Make = "Toyota",
				CarModel = new CarModel { Name = "ModelT", Id = 7, MakeId = 1, Year = 2012, BodyTypeId = 1, FuelTypeId = 1 }
			};

			// Act
			var dto = _mapper.Map<LatestPostsCarDTO>(viewModel);

			// Assert
			Assert.Equal(viewModel.Id, dto.Id);
			Assert.Equal(viewModel.EngineCapacity, dto.EngineCapacity);
			Assert.Equal(viewModel.FuelType, dto.FuelType);
			Assert.Equal(viewModel.TransmissionType, dto.TransmissionType);
			Assert.Equal(viewModel.CoverImage, dto.CoverImage);
			Assert.Equal(viewModel.Year, dto.Year);
			Assert.Equal(viewModel.Price, dto.Price);
			Assert.Equal(viewModel.Make, dto.Make);
			Assert.Equal(viewModel.CarModel.Id, dto.CarModel.Id);
			Assert.Equal(viewModel.CarModel.MakeId, dto.CarModel.MakeId);
			Assert.Equal(viewModel.CarModel.BodyTypeId, dto.CarModel.BodyTypeId);
		}



		[Fact]
		public void Should_Map_CarByUserDTO_To_CarByUserViewModel()
		{
			// Arrange
			var dto = new CarByUserDTO
			{
				Id = 1,
				MakeId = 2,
				CarModelId = 3,
				CoverImage = "image.jpg",
				Year = 2022,
				Price = 25000m,
				CarModel = new CarModel { Name = "ModelT", Id = 7, MakeId = 1, Year = 2012, BodyTypeId = 1, FuelTypeId = 1 }
			};

			// Act
			var viewModel = _mapper.Map<CarByUserViewModel>(dto);

			// Assert
			Assert.Equal(dto.Id, viewModel.Id);
			Assert.Equal(dto.CoverImage, viewModel.CoverImage);
			Assert.Equal(dto.Year, viewModel.Year);
			Assert.Equal(dto.Price, viewModel.Price);
			Assert.Equal(dto.Make, viewModel.Make);
			Assert.Equal(dto.CarModel.Id, viewModel.CarModel.Id);
			Assert.Equal(dto.CarModel.MakeId, viewModel.CarModel.MakeId);
			Assert.Equal(dto.CarModel.BodyTypeId, viewModel.CarModel.BodyTypeId);
		}

		[Fact]
		public void Should_Map_CarByUserViewModel_To_CarByUserDTO()
		{
			// Arrange
			var viewModel = new CarByUserViewModel
			{
				Id = 1,
				CoverImage = "image.jpg",
				Year = 2022,
				Price = 25000m,
				Make = "Toyota",
				CarModel = new CarModel { Name = "ModelT", Id = 7, MakeId = 1, Year = 2012, BodyTypeId = 1, FuelTypeId = 1 }
			};

			// Act
			var dto = _mapper.Map<CarByUserDTO>(viewModel);

			// Assert
			Assert.Equal(viewModel.Id, dto.Id);
			Assert.Equal(viewModel.CoverImage, dto.CoverImage);
			Assert.Equal(viewModel.Year, dto.Year);
			Assert.Equal(viewModel.Price, dto.Price);
			Assert.Equal(viewModel.Make, dto.Make);
			Assert.Equal(viewModel.CarModel.Id, dto.CarModel.Id);
			Assert.Equal(viewModel.CarModel.MakeId, dto.CarModel.MakeId);
			Assert.Equal(viewModel.CarModel.BodyTypeId, dto.CarModel.BodyTypeId);
		}


		[Fact]
		public void Should_Map_UploadedImage_To_ImageFile()
		{
			// Arrange
			var uploadedImage = new UploadedImage
			{
				FileName = "test.png",
				Image = new byte[] { 1, 2, 3, 4 },
				IsCoverImage = true
			};

			// Act
			var imageFile = _mapper.Map<ImageFile>(uploadedImage);

			// Assert
			Assert.Equal(uploadedImage.Id, imageFile.Id);
			Assert.Equal(uploadedImage.IsCoverImage, imageFile.IsCoverImage);
			Assert.Equal(uploadedImage.Image, imageFile.Image);
			Assert.Equal(uploadedImage.FileName, imageFile.FileName);
		}

		[Fact]
		public void Should_Map_ImageFile_To_UploadedImage()
		{
			// Arrange
			var imageFile = new ImageFile
			{
				Image = new byte[] { 1, 2, 3, 4 },
				FileName = "test.png"
			};

			// Act
			var uploadedImage = _mapper.Map<UploadedImage>(imageFile);

			// Assert
			Assert.Equal(imageFile.Image, uploadedImage.Image);
			Assert.Equal(imageFile.FileName, uploadedImage.FileName);

		}

		[Fact]
		public void Should_Map_BaseCarPropertyListsDTO_To_BaseCarPropertyListsViewModel()
		{
			//this.CreateMap<BaseCarPropertyListsViewModel, BaseCarPropertyListsDTO>().ReverseMap();

			var dto = new BaseCarPropertyListsDTO
			{
				BodyTypes = new List<BaseCarSpecificationServiceModel>() { new BaseCarSpecificationServiceModel() { Id = 1, Name = "Sedan"} },
				CarExtras = new List<CarExtrasServiceModel>() { new CarExtrasServiceModel() { Id = 1, Name = "Comfort" } },
				CarModels = new List<BaseCarSpecificationServiceModel>() { new BaseCarSpecificationServiceModel() { Id = 1, Name = "Sedan" } },
				FuelTypes = new List<BaseCarSpecificationServiceModel>() { new BaseCarSpecificationServiceModel() { Id = 1, Name = "Sedan" } },
				Makes = new List<BaseCarSpecificationServiceModel>() { new BaseCarSpecificationServiceModel() { Id = 1, Name = "Sedan" } },
				TransmissionTypes = new List<BaseCarSpecificationServiceModel>() { new BaseCarSpecificationServiceModel() { Id = 1, Name = "Automatic" } }
			};

			// Act
			var mdl = _mapper.Map<BaseCarPropertyListsViewModel>(dto);

			// Assert
			Assert.NotNull(mdl);
			Assert.Equal(dto.BodyTypes.Count(), mdl.BodyTypes.Count());
			Assert.Equal(dto.BodyTypes.First().Name, mdl.BodyTypes.First().Name);
			Assert.Equal(dto.CarExtras.Count(), mdl.CarExtras.Count());
			Assert.Equal(dto.CarModels.Count(), mdl.CarModels.Count());
			Assert.Equal(dto.CarModels.First().Id, mdl.CarModels.First().Id);
			Assert.Equal(dto.FuelTypes.Count(), mdl.FuelTypes.Count());
			Assert.Equal(dto.TransmissionTypes.Count(), mdl.TransmissionTypes.Count());
			Assert.Equal(dto.Makes.Count(), mdl.Makes.Count());
		}


	}
}