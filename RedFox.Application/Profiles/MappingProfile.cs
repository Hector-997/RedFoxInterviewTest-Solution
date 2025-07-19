#region

using AutoMapper;
using RedFox.Application.DTO;
using RedFox.Domain.Entities;
using RedFox.Domain.ValueObjects;

#endregion

namespace RedFox.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Company, CompanyDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
        CreateMap<Geolocation, GeolocationDto>()
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Lng, opt => opt.MapFrom(src => src.Longitude))
            .ReverseMap();
        CreateMap<UserCreationDto, User>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
    }
}