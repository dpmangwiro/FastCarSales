
using BlazorBootstrap;
using FastCarSales.Client.Events;
using Microsoft.AspNetCore.Components;


namespace FastCarSales.Client.Pages.Posts
{
	

	public class SearchAndResultsBase : ComponentBase
	{
		protected List<BreadcrumbItem> BreadcrumbItems = new List<BreadcrumbItem>()
		{
			new BreadcrumbItem{Text = "Home", Href="/"},
			new BreadcrumbItem{Text = "Search", Href="/searchresults", IsCurrentPage= true}
		};

		//public event EventHandler<BeginSearchArgs>? OnBeginSearch;

		//protected void BeginSearchHandler(BeginSearchArgs args)
		//{
		//	Console.WriteLine("In parent invoking begin search event");
		//	OnBeginSearch?.Invoke(this, args);

		//	StateHasChanged();
		//}


	}
}
