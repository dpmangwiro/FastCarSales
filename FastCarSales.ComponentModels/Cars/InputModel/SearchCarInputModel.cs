using FastCarSales.CustomAttributes.ValidationAttributes;
using System.ComponentModel.DataAnnotations;


using static FastCarSales.Data.Constants.DataConstants;

namespace FastCarSales.ComponentModels.Cars.InputModel
{


    public class SearchCarInputModel 
    {
        [Display(Name = "Make, model or/and specification:")]
        public string TextSearchTerm { get; set; } = string.Empty;

        [RangeUntilCurrentYear(
            CarYearMinValue,
            ErrorMessage = "The car year must be between {1} and {2}.")]
        public int? FromYear { get; set; }

        [RangeUntilCurrentYear(
            CarYearMinValue,
            ErrorMessage = "The car year must be between {1} and {2}.")]
        public int? ToYear { get; set; }

        [Range(
            CarEngineCapacityMinValue,
            CarEngineCapacityMaxValue,
            ErrorMessage = "The car engine capacity must be between {1} and {2}.")]
        public decimal? MinEngineCapacity { get; set; }

        [Range(
            CarEngineCapacityMinValue,
            CarEngineCapacityMaxValue,
            ErrorMessage = "The car engine capacity must be between {1} and {2}.")]
        public decimal? MaxEngineCapacity { get; set; }

        [RangeWithCustomFormat(CarPriceMinValue, CarPriceMaxValue, "car price")]
        [Display(Name = "Minimum price (in Euro):")]
        public decimal? MinPrice { get; set; }

        [RangeWithCustomFormat(CarPriceMinValue, CarPriceMaxValue, "car price")]
        [Display(Name = "Maximum price (in Euro):")]
        public decimal? MaxPrice { get; set; }
    }
}