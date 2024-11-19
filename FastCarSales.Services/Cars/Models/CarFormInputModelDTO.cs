

namespace FastCarSales.Services.Cars.Models
{
    using FastCarSales.Data.Dtos;   
    using System.Collections.Generic;

    public class CarFormInputModelDTO : BaseCarInputModelDTO
    {        
        public string Description { get; set; }
        
        public int? Year { get; set; }
        
        public decimal? Price { get; set; }
        
        public int? Kilometers { get; set; }
        
        public decimal? EngineCapacity { get; set; }
        
        public string LocationCity { get; set; }
        
        public string LocationTown { get; set; }
        
        public HashSet<ImageFile> Images { get; set; } = new HashSet<ImageFile>();
    }
}
