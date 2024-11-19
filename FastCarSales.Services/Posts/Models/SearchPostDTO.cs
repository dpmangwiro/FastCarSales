namespace FastCarSales.Services.Posts.Models
{
    using System.Collections.Generic;
    using Cars.Models;

    public class SearchPostDTO
    {
        public SearchCarInputModelDTO Car { get; set; } = new SearchCarInputModelDTO();
		public int PostID { get; init; }

		public IEnumerable<int> SelectedBodyTypeIDs { get; set; } = new HashSet<int>();

        public IEnumerable<int> SelectedFuelTypesIds { get; set; } = new HashSet<int>();

        public IEnumerable<int> SelectedTransmissionTypesIds { get; set; } = new HashSet<int>();

        public IEnumerable<int> SelectedExtrasIds { get; set; } = new HashSet<int>();
    }
}