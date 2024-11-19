namespace FastCarSales.Services.Cars
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Models;
	using Data.Models;
	using Microsoft.EntityFrameworkCore.Storage;

	public interface ICarsService
	{
        Task<Car> GetCarFromInputModelAsync(CarFormInputModelDTO inputCar, List<int> selectedExtrasIds, string userId, string imageRootDirectoryPath);

        IEnumerable<BaseCarSpecificationServiceModel> GetAllBodyTypes();
        IEnumerable<BaseCarSpecificationServiceModel> GetAllCarModels();
        IEnumerable<BaseCarSpecificationServiceModel> GetAllMakes();
        IEnumerable<CarModel> GetCarModels(int makeID);
		IEnumerable<CarModel> GetCarModels();

		IEnumerable<BaseCarSpecificationServiceModel> GetAllFuelTypes();
        IEnumerable<BaseCarSpecificationServiceModel> GetBodyTypes(int ModelID);
		BaseCarSpecificationServiceModel? GetBodyType(int ModelID);
		IEnumerable<BaseCarSpecificationServiceModel> GetAllTransmissionTypes();

        IEnumerable<CarExtrasServiceModel> GetAllCarExtras();
		BaseCarPropertyListsDTO FillInputCarBaseProperties();
		Task UpdateCarDataFromInputModelAsync(int carId, CarFormInputModelDTO inputCar, List<int> selectedExtrasIds, List<string> deletedImagesIds, string userId, string imagePath, string SelectedCoverImageId);

		Task DeleteCarByIdAsync(int carId, IDbContextTransaction transaction);
		Task RestoreDeletedCar(int carId, IDbContextTransaction transaction);

	}
}