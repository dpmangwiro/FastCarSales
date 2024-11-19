
using FastCarSales.ComponentModels.Posts;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Web.ViewModels.Cars;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Posts;
using Microsoft.Extensions.Hosting;
using FastCarSales.Data.Models;
using FastCarSales.Web.ViewModels.Posts;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using FastCarSales.ComponentModels.Cars.ViewModel;
using FastCarSales.ComponentModels.Images;
using Microsoft.AspNetCore.Components.Forms;
using FastCarSales.Data.Dtos;
using System.Web;
using FastCarSales.Web.ViewModels.Images;
using System.Collections.Generic;

namespace FastCarSales.Client.Pages.Posts
{
    public class EditPostBase: ComponentBase
    {
		#region Paremeters And Variables

		
        [Inject] HttpClient Http { get; set; } = null!;       
        [Inject] IMapper Mapper { get; set; } = null!;
        [Inject] NavigationManager NavManager { get; set; } = null!;
        [Inject] IJSRuntime jSRuntime { get; set; } = null!;
        protected EditPostViewModel PostModel { get; set; } = new EditPostViewModel();
		protected BaseCarPropertyListsViewModel PostDictionary { get; set; } = new BaseCarPropertyListsViewModel();

		protected IEnumerable<CarModel> ThisCarModels = new List<CarModel>();
		protected IEnumerable<CarModel> AllCarModels = new List<CarModel>();

		protected Dictionary<int, string> extraTypes = new Dictionary<int, string>();
		protected string SelectedCoverImageId= "";
		protected string SelectedCoverImageId_Og = "";

		[Parameter] public int PostId {get; set;}

		#endregion
		protected void OnInItialized()
		{
			PostDictionary = new BaseCarPropertyListsViewModel();
			PostModel = new EditPostViewModel();
		}

		//pass the following in url https://localhost:7082/editpost/2
		
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender) { return;}

			if (NavManager is null) { return; }

			try
			{
				PostDictionary = await PrepareModelForInput();

				await GetAllCarModels();

				if (PostId == 0) { return; }

				PostModel = await GetEditModel(PostId);		
				
				this.SelectedCoverImageId_Og = PostModel.SelectedCoverImageId;

				ThisCarModels = AllCarModels.ToList().Where(x => x.MakeId == PostModel.Car.MakeId);

				await InvokeAsync(StateHasChanged);

			}
			catch (Exception ex)
			{

			}
		}

		protected async Task MakeChanged()
		{
			try
			{
				if (PostModel.Car.MakeId <= 0) { return; }

				ThisCarModels = AllCarModels.ToList().Where(x => x.MakeId == PostModel.CarId);
				
				ThisCarModels = ThisCarModels ?? new List<CarModel>();

				if (ThisCarModels is null || ThisCarModels.Count() == 0)
				{
					PostModel.Car.CarModelId = 0;
					return;
				}

				if (!ThisCarModels.ToList().Exists(x => x.Id == PostModel.Car.CarModelId))
				{
					PostModel.Car.CarModelId = ThisCarModels.First().Id;
					await ModelChanged();
				}				
				
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

				if (ThisCarModels is null || ThisCarModels.Count() == 0) { return; }

				var thisModel = ThisCarModels.First(x => x.Id == PostModel.Car.CarModelId);

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

		public async void EditPost()
		{
			try
			{
				if (PostModel is null || PostModel.Car is null)
				{
					return;
				}

				var images = Mapper.Map<HashSet<ImageFile>>(UploadedImages);

				PostModel.Car.Images = images;

				SetSelectedImageId();

				var url = Http.BaseAddress + $"api/EditPosts/";

				var response = await Http.PutAsJsonAsync(url, PostModel);

				if (!response.IsSuccessStatusCode)
				{
					throw new Exception(response.ReasonPhrase);
				}
								
				NavManager.NavigateTo($"/singlepostview/{PostId}");
			}
			catch (Exception ex)
			{
				await jSRuntime.InvokeVoidAsync("alert", "Error while posting: " + ex);
			}
		}

		private void SetSelectedImageId()
		{
			var images = Mapper.Map<HashSet<ImageFile>>(UploadedImages);

			this.SelectedCoverImageId = images.FirstOrDefault(x => x.IsCoverImage)?.Id;

			if (string.IsNullOrEmpty(this.SelectedCoverImageId))
			{
				this.SelectedCoverImageId = PostModel.CurrentImages.FirstOrDefault(x => x.IsCoverImage)?.Id;
			}

			if (string.IsNullOrEmpty(this.SelectedCoverImageId))
			{
				SelectedCoverImageId = SelectedCoverImageId_Og;
			}

			PostModel.SelectedCoverImageId = SelectedCoverImageId;
		}
				
		private async Task<BaseCarPropertyListsViewModel> PrepareModelForInput()
		{
			var url = Http.BaseAddress + $"api/BaseInput/";

			var createPostViewModel = await Http.GetFromJsonAsync<BaseCarPropertyListsViewModel>(url);

			return createPostViewModel ?? new BaseCarPropertyListsViewModel();
		}

		private async Task<EditPostViewModel> GetEditModel(int postId)
		{
			var url = Http.BaseAddress + $"api/EditPosts/{postId}";

			var editModel = await Http.GetFromJsonAsync<EditPostViewModel>(url);

			return editModel ?? new EditPostViewModel();
		}

		private async Task GetAllCarModels()
		{
			var url = Http.BaseAddress + $"api/Models/";

			var models = await Http.GetFromJsonAsync<IEnumerable<CarModel>>(url);

			AllCarModels = models ?? new List<CarModel>();
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

		protected void RemoveCurrentImage(ImageInfoViewModel image)
		{
			var removeImage = PostModel.CurrentImages.FirstOrDefault(img => img.Id == image.Id);

			if (removeImage != null)
			{
				var newList = PostModel.CurrentImages;
				newList.Remove(removeImage);
				
				PostModel.CurrentImages = newList;

				PostModel.DeletedImagesIds.Add(image.Id);
			}
		}

		/// <summary>
		/// we need only one cover image defined from current images and new uploaded images
		/// </summary>
		/// <param name="imageId"></param>
		protected void SetCoverImage(string imageId)
		{
			foreach (ImageInfoViewModel img in PostModel.CurrentImages)
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
