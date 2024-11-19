using FastCarSales.Client.Events;
using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace FastCarSales.Client.LocalService
{
    public class SearchService
    {
        private readonly HttpClient Http;
        private readonly IJSRuntime jSRuntime;
      
        public event EventHandler<SearchResultsArgs> ResultsReceivedEvent;

        //public PostsListViewModel ResultsView { get; set; } = new PostsListViewModel();

        //private readonly EventAggregator EvntAggregator;

        public SearchService(HttpClient http, IJSRuntime jSRuntime, EventAggregator eventAggregator)
        {
            Http = http;
            this.jSRuntime = jSRuntime;
            
            //EvntAggregator = eventAggregator;
            //EvntAggregator.Subscribe<BeginSearchArgs>(HandleBeginSearch);
           
        }

        //public void PublishBeginSearchEvent(SearchCarInputModel SearchInput, int pageNumber, int sorting)
        //{
        //    EvntAggregator.publish(new BeginSearchArgs(searchInput: SearchInput, pageNumber:pageNumber, sorting: sorting));
        //}

        public  async Task HandleBeginSearch(BeginSearchArgs args)
        {
            try
            {
                var pageId = args.PageNumber;
                var sorting = args.Sorting;

                var url = $"{Http.BaseAddress}api/Search?pageId={pageId}&sorting={sorting}";
				
				//only way we can send a complext objext to controller is cheating it and making the get method into a post method
				var response = await Http.PostAsJsonAsync(url, args.SearchInput);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                var stringResponse = await response.Content.ReadAsStringAsync();
                var result = System.Text.Json.JsonSerializer.Deserialize<PostsListViewModel>(stringResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                result = result ?? new PostsListViewModel();
                
                //ResultsView = result;

                Console.WriteLine("Firing HandleSearchResultsReceived in service");

                ResultsReceivedEvent?.Invoke(this, new SearchResultsArgs(resultsModel: result, searchData: args.SearchInput, pageNumber: pageId, sorting: sorting));

                Console.WriteLine("Firing Completed HandleSearchResultsReceived in Service");

                
            }
            catch (Exception ex)
            {
                await jSRuntime.InvokeVoidAsync("alert", ex.Message);
            }

        }










    }
}
