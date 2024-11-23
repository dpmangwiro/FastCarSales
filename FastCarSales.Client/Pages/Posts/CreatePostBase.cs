using FastCarSales.ComponentModels.Posts;
using Microsoft.AspNetCore.Components;
using AutoMapper;
using FastCarSales.Data.Models;
using FastCarSales.Data.Dtos;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;
using FastCarSales.ComponentModels.Images;
using Microsoft.AspNetCore.Components.Authorization;
using FastCarSales.ComponentModels.Cars.ViewModel;
using Microsoft.AspNetCore.Authorization;
using FastCarSales.Web.ViewModels.Images;
using BlazorBootstrap;


namespace FastCarSales.Client.Pages.Posts
{
    [Authorize]
    public class CreatePostBase : ComponentBase
    {
		protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>()
		{
			new BreadcrumbItem{Text = "Home", Href="/"},
			new BreadcrumbItem{Text = "New Post", Href="/createpost", IsCurrentPage= true}
		};

		#region Variable Declaration
		[Inject] HttpClient Http { get; set; } = null!;        
        [Inject] IMapper Mapper { get; set; } = null!;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] IJSRuntime jSRuntime { get; set; } = null!;
		[Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;

		protected PostFormInputModel PostModel { get; set; } = new PostFormInputModel();
		protected BaseCarPropertyListsViewModel PostDictionary { get; set; } = new BaseCarPropertyListsViewModel();

		protected IEnumerable<CarModel> ThisCarModels = new List<CarModel>();
		protected IEnumerable<CarModel> AllCarModels = new List<CarModel>();

		protected Dictionary<int, string> extraTypes = new Dictionary<int, string>();
        protected DateTime? selectedYear;
        protected ElementReference inputDate;

		#endregion
		
        protected override async void OnAfterRender(bool firsRender)
        {
            if (!firsRender)
            {
                return;
            }

			PostDictionary = await GetPostsDictionary();

			await GetAllCarModels();

			PostModel.Car = new Web.ViewModels.Cars.CarFormInputModel();

            PostModel.Car.Kilometers = default(int?);
            PostModel.Car.EngineCapacity = default(decimal?);
            PostModel.Car.Price = default(decimal?);
            PostModel.Car.Year = default(int?);

			PostModel.Car.MakeId = PostDictionary.Makes.First().Id;
			ThisCarModels = AllCarModels.ToList().Where(x => x.MakeId == PostModel.Car.MakeId);

			await InvokeAsync(StateHasChanged);

			await MakeChanged();
        }

       
        protected async Task MakeChanged()
        {
            try
            {
                if (PostModel.Car.MakeId <= 0) { return; }

				ThisCarModels = AllCarModels.ToList().Where(x => x.MakeId == PostModel.Car.MakeId);

				ThisCarModels = ThisCarModels ?? new List<CarModel>();
				                
                if (ThisCarModels is null)
                {
                    return;
                }

                if (ThisCarModels.Count() == 0)
                {
                    PostModel.Car.CarModelId = 0;
                    return;
                }

                PostModel.Car.CarModelId = ThisCarModels.First().Id;
                                                
                await ModelChanged();
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error while posting: " + ex);
			}
		}

		protected async Task ModelChanged()
		{
			try
			{
				//var authState = await AuthStateProvider.GetAuthenticationStateAsync();
				//var isAdmin = authState.User.HasClaim(c => c.Type == "User" && c.Value == "dpmangwiro@gmail.com");

				//canViewPage = user.HasClaim(c => c.Type == "Permission" && c.Value == "CanViewPage");

				if (PostModel.Car.CarModelId <= 0) { return; }

               if (ThisCarModels is null || ThisCarModels.Count() == 0) { return;}

               var thisModel = ThisCarModels.First(x=> x.Id == PostModel.Car.CarModelId);

                PostModel.Car.BodyTypeId = thisModel.BodyTypeId;

                PostModel.Car.TransmissionTypeId = thisModel.TransmissionTypeId;

				PostModel.Car.FuelTypeId = thisModel.FuelTypeId;

				PostModel.Car.EngineCapacity = thisModel.EngineCapacity;

                PostModel.Car.Year = thisModel.Year;

				await InvokeAsync(StateHasChanged);
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error while posting: " + ex);
			}
		}

		private async Task GetAllCarModels()
		{
			var url = Http.BaseAddress + $"api/Models/";

			var models = await Http.GetFromJsonAsync<IEnumerable<CarModel>>(url);

			AllCarModels = models ?? new List<CarModel>();
		}

		public async void PostCar()
        {
            try
            {
                if (PostModel is null || PostModel.Car is null)
                {
                    return;
                }				
				
				var images = Mapper.Map<HashSet<ImageFile>>(UploadedImages);

				PostModel.Car.Images = images;

                if (PostModel.Car.Images.Count <= 4)
                {
                    await jSRuntime.InvokeVoidAsync("alert", "Please choose 4 to 10 images!");
					
					return;
                }

                var url = Http.BaseAddress + $"api/Posts/";

                var response = await Http.PostAsJsonAsync(url, PostModel);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }

                var stringResponse = await response.Content.ReadAsStringAsync();
                var postId = System.Text.Json.JsonSerializer.Deserialize<int>(stringResponse, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                if (postId == 0) { return; }

                NavigationManager.NavigateTo($"/singlepostview/{postId}");
            }
            catch (Exception ex)
            {
                await jSRuntime.InvokeVoidAsync("alert", "Error while posting: " + ex);
            }
        }

        private async Task<BaseCarPropertyListsViewModel> GetPostsDictionary()
        {
            var url = Http.BaseAddress + $"api/BaseInput/";

            var createPostViewModel = await Http.GetFromJsonAsync<BaseCarPropertyListsViewModel>(url);

            return createPostViewModel ?? new BaseCarPropertyListsViewModel();
        }

        #region Images

        protected int nextImageId = 1;
        protected List<UploadedImage> UploadedImages = new List<UploadedImage>();

        protected async Task HandleSelectedFiles(InputFileChangeEventArgs e)
        {
            try
            {
				foreach (var file in e.GetMultipleFiles())
				{
					var buffer = new byte[file.Size];
					await file.OpenReadStream().ReadAsync(buffer);

					//using var image = SixLabors.ImageSharp.Image.Load(buffer);

					// Resize image to 800x600
					//image.Mutate(x => x.Resize(800, 600));
					//using var ms = new MemoryStream();
					//image.Save(ms, new PngEncoder());
					//var resizedImage = ms.ToArray();
					//imagePreview = $"data:image/png;base64,{Convert.ToBase64String(resizedImage)}";

					UploadedImages.Add(new UploadedImage
					{						
						FileName = file.Name,
						Image = buffer
					});
				}

				await InvokeAsync(StateHasChanged);
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error while posting: " + ex);
			}
		}

        protected void RemoveImage(string id)
        {
            var image = UploadedImages.FirstOrDefault(img => img.Id == id);
            if (image != null)
            {
                UploadedImages.Remove(image);
            }
        }

		protected void SetCoverImage(string imageId)
		{			
			foreach (UploadedImage img in UploadedImages)
			{
				if (img.Id == imageId)
				{
					img.IsCoverImage = true;
				}
				else
				{
					img.IsCoverImage = false;
				}
			}

			StateHasChanged();
		}

		#endregion


	}
}
