using FastCarSales.Client.Events;
using FastCarSales.Client.LocalService;
using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.ComponentModels.Cars.ViewModel;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Net.Http.Json;
using System.Web;

namespace FastCarSales.Client.Pages.Posts
{
    public class SearchPaneBase: ComponentBase
    {
        [Inject] HttpClient Http { get; set; } = null!;       
        [Inject] IJSRuntime jSRuntime { get; set; } = null!;
		[Inject] NavigationManager NavManager { get; set; } = null!;
		[Inject] SearchService _SearchService { get; set; } = null!;
		protected BaseCarPropertyListsViewModel PostDictionary { get; set; } = new BaseCarPropertyListsViewModel();
		protected SearchCarInputModel SearchModel { get; set; } = new SearchCarInputModel();
       
        //protected bool UserIsAdmin = false;
		protected string QueryParam_SearchText = string.Empty;
				

		protected override void OnInitialized()
		{
			GetQueryString();
		}

		protected void GetQueryString()
		{
			if (this.NavManager is null)
			{
				return;
			}

			var uri = NavManager!.ToAbsoluteUri(NavManager.Uri);
			var queryParams = HttpUtility.ParseQueryString(uri.Query);

			if (queryParams is null) { return; }

			QueryParam_SearchText = queryParams["searchText"] ?? "";		
			
			
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender) { return; }

            try
            {
				PostDictionary = await GetPostsDictionary();
								
				if (string.IsNullOrEmpty(QueryParam_SearchText))
				{
					await LoadStoredSearchData();					
				}
				else
				{
					SearchModel = new SearchCarInputModel()
					{
						TextSearchTerm = QueryParam_SearchText
					};

					QueryParam_SearchText = "";

					SearchButtonClicked();
				}

			}
            catch (Exception ex)
			{
                await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
			}
		}

		private async Task<BaseCarPropertyListsViewModel> GetPostsDictionary()
		{
			var url = Http.BaseAddress + $"api/BaseInput/";

			var createPostViewModel = await Http.GetFromJsonAsync<BaseCarPropertyListsViewModel>(url);

			return createPostViewModel ?? new BaseCarPropertyListsViewModel();
		}

		//protected async void SearchButtonClicked()
		//{
		//	Console.WriteLine("SearchPane: SearchButtonClicked: checking for searchmodel null");

		//	if (SearchModel is null) { return; }

		//	Console.WriteLine("SearchPane: SearchButtonClicked: Searchmodel check passed: now firing begin search");

		//	await SaveSearchData();

		//	await OnBeginSearch.InvokeAsync(new BeginSearchArgs(searchInput: SearchModel, pageNumber: 1, sorting: 0));

		//}

		protected async void SearchButtonClicked()
		{
			Console.WriteLine("SearchPane: SearchButtonClicked: checking for searchmodel null");

			if (SearchModel is null) { return; }

			Console.WriteLine("SearchPane: SearchButtonClicked: Searchmodel check passed: now firing begin search");

			await SaveSearchData();

			var searchArgs = new BeginSearchArgs(searchInput: SearchModel, pageNumber: 1, sorting: 0);

			await _SearchService.HandleBeginSearch(searchArgs);

			//await OnBeginSearch.InvokeAsync(new BeginSearchArgs(searchInput: SearchModel, pageNumber: 1, sorting: 0));

		}

		protected async Task SaveSearchData()
		{			
			await jSRuntime.InvokeVoidAsync("saveSearchData", SearchModel);			
		}


		private async Task LoadStoredSearchData()
		{
			var searchData = await jSRuntime.InvokeAsync<SearchCarInputModel>("getSearchData");						

			if (searchData != null)
			{
				//this.SearchModel = searchData;				

				SearchModel = new SearchCarInputModel
				{
					FromYear = searchData.FromYear,
					ToYear = searchData.ToYear,
					MaxEngineCapacity = searchData.MaxEngineCapacity,
					MinEngineCapacity = searchData.MinEngineCapacity,
					MaxPrice = searchData.MaxPrice,
					MinPrice = searchData.MinPrice,
					TextSearchTerm = searchData.TextSearchTerm					
				};
								
				await InvokeAsync(StateHasChanged);
			}
			
		}


	}
}
