using AutoMapper;
using BlazorBootstrap;
using FastCarSales.Services.Cars;
using FastCarSales.Services.Posts;
using FastCarSales.Services.Posts.Models;
using FastCarSales.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using System.Collections.Specialized;
using System.Net.Http.Json;

namespace FastCarSales.Client.Pages.Posts
{
	public class MineBase : ComponentBase
	{
		protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>()
		{
			new BreadcrumbItem{Text = "Home", Href="/"},
			new BreadcrumbItem{Text = "My Posts", Href="/mine", IsCurrentPage= true}
		};

		[Inject] HttpClient Http { get; set; } = null!;
		
		protected PostsByUserViewModel? MyPostsView = new PostsByUserViewModel();

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender) { return; }


			try
			{
				var url = Http.BaseAddress + $"api/Mine/";

				MyPostsView = await Http.GetFromJsonAsync<PostsByUserViewModel>(url);

				if (MyPostsView == null)
				{
					MyPostsView = new PostsByUserViewModel();

				}

				await InvokeAsync(StateHasChanged);

			}
			catch (Exception ex)
			{

			}
		}

		protected void Create()
		{

		}



		protected void Offer()
		{

		}

		protected void Edit()
		{

		}

		protected void Delete()
		{

		}


	}
}
