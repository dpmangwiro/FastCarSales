namespace FastCarSales.Data.Constants
{
    public class DataConstants
    {
        //Car constants
        public const int CarMakeMaxLength = 30;
        public const int CarMakeMinLength = 2;
        public const int CarModelMaxLength = 30;
        public const int CarModelMinLength = 2;
        public const int CarDescriptionMinLength = 20;
        public const int CarYearMinValue = 1950;
        public const double CarPriceMinValue = 1;
        public const double CarPriceMaxValue = 10000000;
        public const int CarKilometersMinValue = 0;
        public const int CarKilometersMaxValue = 2000000;
        public const double CarEngineCapacityMinValue = 0;
        public const double CarEngineCapacityMaxValue = 5000;
        public const int CarLocationCityMaxLength = 20;
        public const int CarLocationCityMinLength = 3;
        public const int CarLocationTownMaxLength = 30;
        public const int CarLocationTownMinLength = 3;

        //Post constants
        public const int PostSellerNameMaxLength = 30;
        public const int PostSellerNameMinLength = 2;
        public const int PostSellerPhoneNumberMaxLength = 20;
        public const int PostSellerPhoneNumberMinLength = 6;

        //Other data entities constants
        public const int BodyTypeNameMaxLength = 30;
        public const int ExtraNameMaxLength = 100;
        public const int ExtraTypeNameMaxLength = 20;
        public const int FuelTypeNameMaxLength = 40;
        public const int TransmissionTypeNameMaxLength = 30;
    }
}
