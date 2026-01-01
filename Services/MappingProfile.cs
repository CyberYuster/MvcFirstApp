using AutoMapper;
using MvcDatabaseApp.Models;
using MvcDatabaseApp.Models.ViewModels;

namespace MvcDatabaseApp.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity to ViewModel
            CreateMap<Product, ProductViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src.isActive))
                    .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                    .ReverseMap(); // ViewModel to Entity (bidirectional)

            // You can add more mappings here
            // CreateMap<Category, CategoryViewModel>().ReverseMap();
        }
    }
}
