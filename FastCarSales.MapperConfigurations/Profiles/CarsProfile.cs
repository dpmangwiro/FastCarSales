using AutoMapper;
using FastCarSales.ComponentModels.Cars.InputModel;
using FastCarSales.ComponentModels.Cars.ViewModel;
using FastCarSales.ComponentModels.Images;
using FastCarSales.Data.Dtos;
using FastCarSales.Data.Models;
using FastCarSales.Services.Cars.Models;
using FastCarSales.Web.ViewModels.Cars;


namespace FastCarSales.MapperConfigurations.Profiles
{
    public class CarsProfile : Profile
    {
        public CarsProfile()
        {
            this.CreateMap<BodyType, BaseCarSpecificationServiceModel>();

            this.CreateMap<FuelType, BaseCarSpecificationServiceModel>();

			this.CreateMap<Make, BaseCarSpecificationServiceModel>();

			this.CreateMap<CarModel, BaseCarSpecificationServiceModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(dest => dest.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();

			this.CreateMap<TransmissionType, BaseCarSpecificationServiceModel>();

            this.CreateMap<Extra, CarExtrasServiceModel>();

            this.CreateMap<BaseCarSpecificationServiceModel, BaseCarSpecificationViewModel>().ReverseMap();

			this.CreateMap<BaseCarPropertyListsViewModel, BaseCarPropertyListsDTO>().ReverseMap();

			this.CreateMap<CarExtrasServiceModel, CarExtrasViewModel>().ReverseMap();

            this.CreateMap<BaseCarDTO, BaseCarViewModel>().ReverseMap();

            this.CreateMap<CarFormInputModelDTO, CarFormInputModel>().ReverseMap();

            this.CreateMap<SearchCarInputModelDTO, SearchCarInputModel>().ReverseMap();

			//this.CreateMap<CarInListDTO, CarInListViewModel>().ReverseMap();
			CreateMap<CarInListDTO, CarInListViewModel>()
                .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Make))
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.CarModel))
                .ReverseMap();

			//this.CreateMap<SingleCarDTO, SingleCarViewModel>().ReverseMap();
			
				CreateMap<SingleCarDTO, SingleCarViewModel>()
					.ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Make))
					.ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.CarModel))
					.ReverseMap();
		
            this.CreateMap<CarByUserDTO, CarByUserViewModel>().ReverseMap();

            this.CreateMap<LatestPostsCarDTO, LatestPostsCarViewModel>().ReverseMap();

            this.CreateMap<BaseCarInputModelDTO, BaseCarInputModel>().ReverseMap();

			


		}
    }
}