using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.CustomAttributes.ValidationAttributes;
using FastCarSales.Data.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static FastCarSales.Data.Constants.DataConstants;



namespace FastCarSales.Web.ViewModels.Cars
{


    public class CarFormInputModel : BaseCarInputModel
    {
        [Required(ErrorMessage = "The car description field is required.")]
        [StringLength(
            int.MaxValue,
            MinimumLength = CarDescriptionMinLength,
            ErrorMessage = "The car description must be at least {2} characters long.")]
        [Display(Name = "Description:")]
        public string Description { get; set; }

        [RangeUntilCurrentYear(
            CarYearMinValue,
            ErrorMessage = "The car year must be between {1} and {2}.")]
        [Display(Name = "Year of first registration:")]
        public int? Year { get; set; }

        [RangeWithCustomFormat(CarPriceMinValue, CarPriceMaxValue, "car price")]
        [Display(Name = "Price:")]
        public decimal? Price { get; set; }

        [RangeWithCustomFormat(CarKilometersMinValue, CarKilometersMaxValue, "car kilometers")]
        [Display(Name = "Mileage (in kilometers):")]
        public int? Kilometers { get; set; }

        [Range(
            CarEngineCapacityMinValue,
            CarEngineCapacityMaxValue,
            ErrorMessage = "The car engine capacity must be between {1} and {2}.")]
        [Display(Name = "EngineCapacity:")]
        public decimal? EngineCapacity { get; set; }


        [Required(ErrorMessage = "The car city field is required.")]
        [StringLength(
            CarLocationCityMaxLength,
            MinimumLength = CarLocationCityMinLength,
            ErrorMessage = "The city name must be between {2} and {1} characters long.")]
        [Display(Name = "Car location - city:")]
        public string LocationCity { get; set; }

        [Required(ErrorMessage = "The town field is required.")]
        [StringLength(
            CarLocationTownMaxLength,
            MinimumLength = CarLocationTownMinLength,
            ErrorMessage = "The town name must be between {2} and {1} characters long.")]
        [Display(Name = "Car location - Town:")]
        public string LocationTown { get; set; }
        
        [Display(Name = "Images:")]
		public HashSet<ImageFile> Images { get; set; } = new HashSet<ImageFile>();
	}
}
