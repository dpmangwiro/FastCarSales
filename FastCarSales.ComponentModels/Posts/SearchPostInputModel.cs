namespace FastCarSales.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Cars;
    using FastCarSales.ComponentModels.Cars.InputModel;

    public class SearchPostInputModel
    {
        [Required]
        public SearchCarInputModel Car { get; set; } = new SearchCarInputModel();
		public int PostID { get; init; }	

        public int PageId { get; set;} = 1;
        public int Sorting {  get; set; } = 0;
    }
}