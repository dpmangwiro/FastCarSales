using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastCarSales.Services.ExtensionMethods
{
	public static class Extensions
	{

		public static string ToBase64(this byte[] Image)
		{
			string Base64Content = $"data:image/png;base64,{Convert.ToBase64String(Image)}";
			
			return Base64Content;
		}
	}
}
