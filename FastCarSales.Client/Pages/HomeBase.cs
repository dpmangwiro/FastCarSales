using AutoMapper;
using FastCarSales.Services.Posts.Models;
using System.Collections;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Routing;
using FastCarSales.Client.Pages.Posts;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using BlazorBootstrap;


namespace FastCarSales.Client.Pages
{
	public class HomeBase : ComponentBase, IDisposable
	{
		protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>()
		{			
			new BreadcrumbItem{Text = "Home", Href="/", IsCurrentPage= true}
		};
		[Inject] HttpClient Http { get; set; } = null!;
		[Inject] IMapper Mapper { get; set; } = null!;
		[Inject] IJSRuntime jSRuntime { get; set; } = null!;
		[Inject] NavigationManager NavigationManager { get; set; } = null!;
		//[Inject] ILogger<HomeBase> Logger { get; set; } = null!;

		protected LatestPostsViewModel LatestPosts = new LatestPostsViewModel();
		protected IEnumerable<SinglePostViewModel> FeaturedPosts = new List<SinglePostViewModel>();

		protected Dictionary<int,bool> DictCarousel = new Dictionary<int, bool>()
		{
			{1, false }, {2, false}, {3, false}, {4, false}, {5, false}, {6, false}
		};
		
		protected string SearchText = string.Empty;
		
		protected override void OnInitialized()
		{
			currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
			NavigationManager.LocationChanged += OnLocationChanged;

			LatestPosts = ProvideEmptyPostView();

			//generate random number between 1 and 6 use that to set one of the pics in carousel active
			//int activeItemNo = Math.
			var timeinsec = DateTime.UtcNow.Minute;
			while (timeinsec > 6)
			{
				timeinsec /= 6;
			}

			if (timeinsec <= 0) { timeinsec = 1;}

			DictCarousel[timeinsec] = true;

			//Logger.LogInforma
			//tion("Completed initilize on homebase");

		}

		protected async override Task OnAfterRenderAsync(bool firsRender)
		{
			if (!firsRender)
			{
				return;
			}

			LatestPosts = await GetLatestPosts();
			await GetFeatured();

			StateHasChanged();
		}
				
		protected void BeginSearch(KeyboardEventArgs args)
		{			
			//StateHasChanged();
			if (args.Key.ToString() != "Enter") { return; }
			if (string.IsNullOrEmpty(SearchText)) { return;}

			NavigationManager.NavigateTo($"/searchresults?searchText={SearchText}");
		}

		[JSInvokable]
		protected void Searchchanged()
		{			
			if (string.IsNullOrEmpty(SearchText)) { return; }

			NavigationManager.NavigateTo($"/searchresults?searchText={SearchText}");
		}

		public async Task<LatestPostsViewModel> GetLatestPosts()
		{
			try
			{
				var url = Http.BaseAddress + $"api/Home/{false}";

				var latestPosts = await Http.GetFromJsonAsync<LatestPostsViewModel>(url);

				return latestPosts ?? new LatestPostsViewModel();
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "An error occurred: " + ex);
			}
			return new LatestPostsViewModel();
		}

		protected async Task GetFeatured()
		{
			try
			{
				var url = Http.BaseAddress + $"api/FeaturedPosts/";

				var featuredPosts = await Http.GetFromJsonAsync<IEnumerable<SinglePostViewModel>>(url);

				if (featuredPosts is null)
				{
					//FeaturedPost1 = null;
					//FeaturedPost2 = null;

					return;
				}

				FeaturedPosts = featuredPosts;

				//FeaturedPost1 = FeaturedPosts!.Count() > 0 ? FeaturedPosts!.ToList()[0] : null;
				//FeaturedPost2 = FeaturedPosts!.Count() > 1 ? FeaturedPosts!.ToList()[1] : null;

				return;
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "An error occurred: " + ex);
			}
			return;
		}

		public LatestPostsViewModel ProvideEmptyPostView()
		{
			var latestPosts = new LatestPostsViewModel()
			{
				LatestPosts = new List<PostInLatestListViewModel>(),
			};

			return latestPosts;
		}

		protected List<SinglePostViewModel> ProvideEmptyFeaturedViewList()
		{
			var featuredPosts = new List<SinglePostViewModel>
			{
				new SinglePostViewModel()
				{
					Car = new Web.ViewModels.Cars.SingleCarViewModel()
				},
				new SinglePostViewModel()
				{
					Car = new Web.ViewModels.Cars.SingleCarViewModel()
				}
			};

			return featuredPosts;
		}

		protected SinglePostViewModel ProvideEmptyFeaturedView() => new SinglePostViewModel()
		{
			Car = new Web.ViewModels.Cars.SingleCarViewModel()
		};

		private string? currentUrl;

		private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
		{
			currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
			StateHasChanged();
		}

		public void Dispose()
		{
			NavigationManager.LocationChanged -= OnLocationChanged;
		}



	}
}
