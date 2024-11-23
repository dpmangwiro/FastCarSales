using BlazorBootstrap;
using FastCarSales.Client.Events;
using FastCarSales.Client.LocalService;
using FastCarSales.ComponentModels.Posts;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using static FastCarSales.Client.Pages.Posts.SortingBase;
using static System.Net.WebRequestMethods;

namespace FastCarSales.Client.Pages.Posts
{
	public class AdminBase : ComponentBase
	{
		protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>()
		{
			new BreadcrumbItem{Text = "Home", Href="/"},
			new BreadcrumbItem{Text = "Admin", Href="/postsadmin", IsCurrentPage= true}
		};
		[Inject] IJSRuntime jSRuntime { get; set; } = null!;
		[Inject] HttpClient Http { get; set; } = null!;
		[Inject] EventAggregator EventAggregator { get; set; } = null!;
		[Inject] NavigationManager NavgManager { get; set; } = null!;

		protected PostsListAdminAreaViewModel Posts = new PostsListAdminAreaViewModel();
		protected SortedClass SortModel { get; set; } = new SortedClass();
		protected SortedClass FilterModel { get; set; } = new SortedClass();

		protected bool NotViewingDeletedItems = false;
		protected bool CannotChangeFeatured = false;
		protected bool CannotApprove = false;

		protected List<SortedClass> SortedListTemplate = new List<SortedClass>(){
				new SortedClass { SortValue = 0, SortDescription = "Post created (newest first)"},
				new SortedClass { SortValue = 1, SortDescription = "Post created (oldest first)"},
				new SortedClass { SortValue = 2, SortDescription = "Price (highest first)"},
				new SortedClass { SortValue = 3, SortDescription = "Price (lowest first)"},				
				new SortedClass { SortValue = 6, SortDescription = "Car year (newest first)"},
				new SortedClass { SortValue = 7, SortDescription = "Car year (oldest first)"}
			  };

		protected List<SortedClass> FilterListTemplate = new List<SortedClass>(){
				new SortedClass { SortValue = 0, SortDescription = "All"},
				new SortedClass { SortValue = 6, SortDescription = "CanEmptyRecyleBin"},
				new SortedClass { SortValue = 1, SortDescription = "Deleted"},
				new SortedClass { SortValue = 2, SortDescription = "Expired"},
				new SortedClass { SortValue = 3, SortDescription = "Featured"},
				new SortedClass { SortValue = 4, SortDescription = "Hidden"},
				new SortedClass { SortValue = 5, SortDescription = "Public"},
				new SortedClass { SortValue = 7, SortDescription = "Todays"},
				new SortedClass { SortValue = 8, SortDescription = "Last 2 Todays"},
				new SortedClass { SortValue = 9, SortDescription = "Last 3 Todays"},
				new SortedClass { SortValue = 10, SortDescription = "Last 4 Todays"},
				new SortedClass { SortValue = 11, SortDescription = "Last 5 Todays"},
				new SortedClass { SortValue = 12, SortDescription = "Last 6 Todays"},
				new SortedClass { SortValue = 13, SortDescription = "Last 7 Todays"},
			  };

		protected override void OnInitialized()
		{


		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender) { return; }

			try
			{
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
			await jSRuntime.InvokeVoidAsync("saveAdminResults", Posts);
		}

		private async Task RecoverStoredSearchData()
		{
			try
			{
				var postsData = await jSRuntime.InvokeAsync<PostsListAdminAreaViewModel>("getAdminResults");

				if (postsData != null) { Posts = postsData; }
			}
			catch (Exception)
			{
				throw;
			}

		}

		protected async void LoadPosts()
		{
			try
			{
				int pageNumber = 1;
				int sorting = SortModel.SortValue;
				int filter = FilterModel.SortValue;

				NotViewingDeletedItems = true;
				CannotApprove = true;
				CannotChangeFeatured = true;

				NotViewingDeletedItems = filter != 1 && filter != 6;
				CannotApprove = filter == 1 || filter == 2 || filter == 6;
				CannotChangeFeatured = filter == 1 || filter == 2 || filter == 4 || filter == 6;

				var url = Http.BaseAddress + $"api/Admin/{pageNumber}/{sorting}/{filter}";

				var results = await Http.GetFromJsonAsync<PostsListAdminAreaViewModel>(url);

				Posts = results ?? new PostsListAdminAreaViewModel();

				await SaveScrollPosition();

				await InvokeAsync(StateHasChanged);
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
			}
		}
				
		protected async Task ChangeVisibility(int postId)
		{
			try
			{
				var url = Http.BaseAddress + $"api/Admin/";

				var response = await Http.PostAsJsonAsync(url, postId);

				if (!response.IsSuccessStatusCode)
				{
					throw new Exception(response.ReasonPhrase);
				}

				LoadPosts();
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
			}
		}

		protected async Task RestoreDeletePost(int postId)
		{
			try
			{
				var url = Http.BaseAddress + $"api/Admin/restore/{postId}";

				var response = await Http.DeleteFromJsonAsync<bool>(url);

				if (!response)
				{
					throw new Exception("failed to restore");
				}

				LoadPosts();
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
			}
		}

		protected async Task EmptyDeletedBin(int postId)
		{
			try
			{
				var url = Http.BaseAddress + $"api/Admin/{postId}";

				var response = await Http.DeleteFromJsonAsync<bool>(url);

				if (!response)
				{
					throw new Exception("failed to restore");
				}

			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
			}
		}

		protected async Task SetFeatured(int postId)
		{
			try
			{
				var url = Http.BaseAddress + $"api/Admin/";

				var response = await Http.PutAsJsonAsync(url, postId);

				if (!response.IsSuccessStatusCode)
				{
					throw new Exception(response.ReasonPhrase);
				}

				LoadPosts();
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
			}
		}

		protected async Task ManagePostDeletion(int postId)
		{
			var postToDelete = Posts.Posts.ToList().First(x => x.PostID == postId);

			if (postToDelete.IsDeleted)
			{
				await EmptyDeletedBin(postId);
			}
			else
			{
				NavgManager.NavigateTo($"/deletepost/{postId}");
			}

			LoadPosts();
		}




	}
}
