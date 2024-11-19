namespace FastCarSales.Services.Cars.Models
{
    public class SearchCarInputModelDTO
    {
        public string TextSearchTerm { get; set; } = string.Empty;
        
        public int? FromYear { get; set; }
        
        public int? ToYear { get; set; }
        
        public decimal? MinEngineCapacity { get; set; }
        
        public decimal? MaxEngineCapacity { get; set; }
        
        public decimal? MinPrice { get; set; }
        
        public decimal? MaxPrice { get; set; }
    }
}