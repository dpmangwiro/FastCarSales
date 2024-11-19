using AutoMapper;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Web;
using System.Net.Http.Json;


namespace FastCarSales.Client.Pages.Posts
{
    public class SinglePostViewBase: ComponentBase
    {
		[Inject] NavigationManager navManager {  get; set; } = null!;
		[Inject] HttpClient Http { get; set; } = null!;
		[Inject] IMapper Mapper { get; set; } = null!;
		[Inject] IJSRuntime jSRuntime { get; set; } = null!;

		protected SinglePostViewModel? SinglePost = new SinglePostViewModel();
        protected int counter = 0;

		[Parameter] public int PostId { get;set;}

		//pass the following in url https://localhost:7182/singlepostview/?postid=2
		protected override async Task OnParametersSetAsync()
		{
			if (navManager is null) { return; }
			
			try
			{
				//var uri = navManager!.ToAbsoluteUri(navManager.Uri);
				//var queryParams = HttpUtility.ParseQueryString(uri.Query);

				//if (queryParams is null) { return; }

				//var id = queryParams["postid"] ?? "0";

				//if (string.IsNullOrEmpty(id)) { return; }

				//int.TryParse(id, out PostId);

				if (PostId == 0) { return; }

				var url = Http.BaseAddress + $"api/Posts/{PostId}";

				SinglePost = await Http.GetFromJsonAsync<SinglePostViewModel>(url);

				//await InvokeAsync(StateHasChanged);

			}
			catch (Exception ex)
			{
				
			}

		}







	}
}
