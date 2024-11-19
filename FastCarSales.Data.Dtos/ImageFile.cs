
namespace FastCarSales.Data.Dtos
{
    public class ImageFile
    {
		public ImageFile()
		{
			this.Id = Guid.NewGuid().ToString();
		}
		public string Id { get; set; }
        public byte[] Image { get; set; }
        public string FileName { get; set; }
		public bool IsCoverImage { get; set; }
		public string Base64Content => $"data:image/png;base64,{Convert.ToBase64String(Image)}";

	}
}
