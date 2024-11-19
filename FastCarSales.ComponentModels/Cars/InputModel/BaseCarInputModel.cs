namespace FastCarSales.ComponentModels.Cars.InputModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BaseCarInputModel
    {
        [Display(Name = "Make:")]
        public int MakeId { get; set; }

        [Display(Name = "CarModel:")]
        public int CarModelId { get; set; }

        [Display(Name = "BodyType:")]
        public int BodyTypeId { get; set; }

        [Display(Name = "Fuel type:")]
        public int FuelTypeId { get; set; }

        [Display(Name = "Transmission type:")]
        public int TransmissionTypeId { get; set; }

        [Display(Name = "Extras:")]
        public int CarExtraId { get; init; }

    }
}
