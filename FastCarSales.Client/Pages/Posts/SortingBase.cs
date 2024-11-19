using FastCarSales.Client.Events;
using FastCarSales.ComponentModels.Posts;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FastCarSales.Client.Pages.Posts
{
    public class SortingBase: ComponentBase
    {       
       
        [Parameter] public EventCallback<int> OnSortValueChanged { get; set; }
                
        [Parameter] public SortedClass SortModel { get; set;} = new SortedClass();

        public List<SortedClass> SortedListTemplate = new List<SortedClass>(){
                new SortedClass { SortValue = 0, SortDescription = "Post created (newest first)"},
                new SortedClass { SortValue = 1, SortDescription = "Post created (oldest first)"},
                new SortedClass { SortValue = 2, SortDescription = "Price (highest first)"},
                new SortedClass { SortValue = 3, SortDescription = "Price (lowest first)"},
                new SortedClass { SortValue = 4, SortDescription = "Engine Capacity (most powerful first)"},
                new SortedClass { SortValue = 5, SortDescription = "Engine Capacity (least powerful first)"},
                new SortedClass { SortValue = 6, SortDescription = "Car year (newest first)"},
                new SortedClass { SortValue = 7, SortDescription = "Car year (oldest first)"}
              };

        //[Inject] EventAggregator EvntAggregator { get; set; } = null!;

        //protected override void OnInitialized()
        //{
        //    EvntAggregator.publish(new BeginSearchArgs(searchInput: SearchInput, pageNumber: pageNumber, sorting: sorting));
        //}

        protected async void SortChanged()
        {
            await OnSortValueChanged.InvokeAsync(SortModel.SortValue);
        }



    }
}
