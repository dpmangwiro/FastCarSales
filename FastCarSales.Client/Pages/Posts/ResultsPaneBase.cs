using FastCarSales.Client.Events;
using FastCarSales.Client.LocalService;
using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.ComponentModels.Posts;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using static FastCarSales.Client.Pages.Posts.SortingBase;

namespace FastCarSales.Client.Pages.Posts
{
    public class ResultsPaneBase : ComponentBase, IDisposable
    {
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject] IJSRuntime jSRuntime { get; set; } = null!;

        [Inject] EventAggregator EventAggregator { get; set; } = null!;
        [Inject] SearchService _SearchService { get; set; } = null!;
        protected BeginSearchArgs SearchArgs { get; set; }

        protected PostsListViewModel ResultsView = new PostsListViewModel();
		[Parameter] public SortedClass SortModel { get; set; } = new SortedClass();

		//[CascadingParameter(Name = "ParentComponent")]
        //SearchAndResultsBase? ParentComponent { get; set; }

        protected bool UserIsAdmin = false;
        private event EventHandler<BeginSearchArgs>? BeginSearchEvent;

        protected override void OnInitialized()
        {
            //if (this.ParentComponent != null)
            //{
            //    Console.WriteLine("Subscribing to begin search");
            //    ParentComponent.OnBeginSearch += HandleBeginSearch;
            //}
            //else
            //{
            //    Console.WriteLine("parent component is null");
            //}

            if (_SearchService != null)
            {
                Console.WriteLine("Subscribing to ResultsReceivedEvent");
                this._SearchService.ResultsReceivedEvent += HandleSearchResultsReceived;
            }
            else { Console.WriteLine("SearchService is null"); }
            
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) { return; }

            try
            {
                var authStat = await AuthStateProvider.GetAuthenticationStateAsync();
                UserIsAdmin = authStat.User.HasClaim(x => x.Value == "Admin");
                               
                await RecoverStoredSearchData();

                await InvokeAsync(StateHasChanged);

                await jSRuntime.InvokeVoidAsync("restoreScrollPosition");

            }
            catch (Exception ex)
            {
                await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
            }
        }

        protected async Task SaveScrollPosition()
        {
            await jSRuntime.InvokeVoidAsync("saveScrollPosition");
            await jSRuntime.InvokeVoidAsync("saveSearchArgs", SearchArgs);
            await jSRuntime.InvokeVoidAsync("saveSearchResults", ResultsView);
        }

        private async Task RecoverStoredSearchData()
        {
            try
            {
                var searchData = await jSRuntime.InvokeAsync<BeginSearchArgs>("getSearchArgs");

                var postsData = await jSRuntime.InvokeAsync<PostsListViewModel>("getSearchResults");

                if (searchData != null)
                {
                    SearchArgs = searchData;
                    this.SortModel.SortValue = SearchArgs.Sorting;

                }

                if (postsData != null) { ResultsView = postsData; }
            }
            catch (Exception)
            {

                throw;
            }                     

        }

        protected async void HandleSearchResultsReceived(object? sender, SearchResultsArgs e)
        {
            Console.WriteLine("HandleSearchResultsReceived fired in component");

            this.ResultsView = e.SearchResults;

			this.SearchArgs = new BeginSearchArgs(searchInput: e.SearchData, pageNumber: e.PageNumber, sorting: e.Sorting);

            SortModel.SortValue = e.Sorting;

			await SaveScrollPosition();

            await InvokeAsync(StateHasChanged);
        }

        //protected async void HandleBeginSearch(object? sender, BeginSearchArgs args)
        //{            
        //    Console.WriteLine("Begining Search in Results pane for " + args.SearchInput.TextSearchTerm);

        //    this.SearchArgs = new BeginSearchArgs(searchInput: args.SearchInput, pageNumber: args.PageNumber, sorting: args.Sorting);

        //    await _SearchService.HandleBeginSearch(args);

        //    Console.WriteLine($"HandleSearchResultsReceived fired in component received {ResultsView.Posts.Count()} posts");

        //}

        protected async Task HandleSortValueChanged(int sortValue)
        {
            if (this.SearchArgs is null)
            {
                return; 
            }

            this.SearchArgs = new BeginSearchArgs(searchInput: SearchArgs.SearchInput, pageNumber: SearchArgs.PageNumber, sorting: sortValue);

			await _SearchService.HandleBeginSearch(SearchArgs);
		}


		public void Dispose()
        {
            _SearchService.ResultsReceivedEvent -= HandleSearchResultsReceived;

            //if (ParentComponent != null)
            //{
            //    ParentComponent.OnBeginSearch -= HandleBeginSearch;
            //}

        }



    }
}
