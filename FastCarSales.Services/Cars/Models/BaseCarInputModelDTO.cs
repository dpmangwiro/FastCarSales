namespace FastCarSales.Services.Cars.Models
{
    using System.Collections.Generic;

    public class BaseCarInputModelDTO
    {
        public int MakeId { get; init; }
        
        public int CarModelId { get; init; }
       
        public int BodyTypeId { get; init; }
               
        public int FuelTypeId { get; init; }       
        
        public int TransmissionTypeId { get; init; }       
        
        public int CarExtraId { get; init; }

    }
}
