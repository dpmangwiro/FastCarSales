using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace FastCarSales.Client.Pages.Posts
{
    public class AllPostsBase: ComponentBase
    {
        [Inject] HttpClient Http { get; set; } = null!;
        [Inject] IJSRuntime jSRuntime { get; set; } = null!;
        public SearchPostInputModel? SearchModel { get; set; } = new SearchPostInputModel();
        public PostsListViewModel? PostsModel { get; set; } = new PostsListViewModel();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
            if (!firstRender)
            {
                return;
            }

            try
            {
                var url = Http.BaseAddress + $"api/AllPosts/?{1}/?{0}";

                PostsModel = await Http.GetFromJsonAsync<PostsListViewModel>(url);

                if (PostsModel == null)
                {
                    PostsModel = new PostsListViewModel();
                }

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                await jSRuntime.InvokeVoidAsync("alert", "Error fetching search results: " + ex.Message);
            }
        }
    }
}
