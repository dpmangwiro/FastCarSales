using Azure;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Text.Json;

namespace FastCarSales.Client.Pages.Posts
{
    public class DeletePostBase: ComponentBase
    {
        [Inject] HttpClient Http { get; set; } = null!;
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
		[Inject] NavigationManager NavManager { get; set; } = null!;
		[Inject] IJSRuntime JSRuntime { get; set; } = null!;
		//[Inject] ILogger<DeletePostBase> Logger { get; set; } = null!;


		protected PostByUserViewModel? MyPostView = new PostByUserViewModel();
        [Parameter] public int PostId { get; set; } 

        protected bool UserIsAdmin = false;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           if (!firstRender) { return; }

            try
            {
				//Logger.LogInformation($"MyPostView onAfterRender enter");
				var url = $"{Http.BaseAddress}api/DeletePost/{PostId}";
				//var url = Http.BaseAddress + $"api/DeletePost/{PostId}";

                //Logger.LogInformation("DeletePostBase: Getting info from " + url);

                MyPostView = await Http.GetFromJsonAsync<PostByUserViewModel>(url);

				//Logger.LogInformation("DeletePostBase: Completed Getting the info from " + url);

				if (MyPostView == null)
                {
                    MyPostView = new PostByUserViewModel();
                }
                 
                //Logger.LogInformation($"MyPostView car make: {MyPostView.Car.Make}" );

                var authStat = await AuthStateProvider.GetAuthenticationStateAsync();
                UserIsAdmin = authStat.User.HasClaim(x => x.Value == "Admin");

                await InvokeAsync(StateHasChanged);

            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Error fetching post" + ex.Message);
                //Logger.LogError("Error gettin deletepost infor" + ex.Message);
            }
        }

		protected async Task DeletePost()
		{			
			try
			{
				//Logger.LogInformation($"MyPostView onAfterRender enter");
				var url = $"{Http.BaseAddress}api/DeletePost/{PostId}";
				//var url = Http.BaseAddress + $"api/DeletePost/{PostId}";

				//Logger.LogInformation("DeletePostBase: Getting info from " + url);

				var response = await Http.DeleteAsync(url);

				//Logger.LogInformation("DeletePostBase: Completed Getting the info from " + url);

				if (!response.IsSuccessStatusCode) { throw new Exception(response.ReasonPhrase); }

				//Logger.LogInformation($"MyPostView car make: {MyPostView.Car.Make}" );
								
				if (UserIsAdmin)
				{
					NavManager.NavigateTo("/allposts");
				}
				else
				{
					NavManager.NavigateTo("/mine");
				}				

			}
			catch (Exception ex)
			{
				await JSRuntime.InvokeVoidAsync("alert", "Error deleting post" + ex.Message);
				//Logger.LogError("Error gettin deletepost infor" + ex.Message);
			}
		}





	}
}
