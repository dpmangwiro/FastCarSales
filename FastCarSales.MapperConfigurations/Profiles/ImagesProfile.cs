using AutoMapper;
using FastCarSales.ComponentModels.Images;
using FastCarSales.Data.Dtos;
using FastCarSales.Services.Images.Models;
using FastCarSales.Web.ViewModels.Images;

namespace FastCarSales.MapperConfigurations.Profiles
{ 
    public class ImagesProfile : Profile
    {
        public ImagesProfile()
        {
            this.CreateMap<ImageInfoDTO, ImageInfoViewModel>().ReverseMap();
			this.CreateMap<UploadedImage, ImageFile>().ReverseMap();
		}
    }
}