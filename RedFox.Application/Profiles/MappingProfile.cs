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

        CreateMap<Address, AddressDto>()
            .ConstructUsing(src => new AddressDto(
                src.Street,
                src.Suite,
                src.City,
                src.Zipcode,
                new GeolocationDto(src.Geo.Latitude, src.Geo.Longitude)
            ));

        CreateMap<AddressDto, Address>()
            .ConstructUsing(dto => new Address {
                Street = dto.Street,
                Suite = dto.Suite,
                City = dto.City,
                Zipcode = dto.Zipcode,
                Geo = new Geolocation { 
                    Latitude = dto.Geo.Lat, 
                    Longitude = dto.Geo.Lng 
                }
            });

        CreateMap<Geolocation, GeolocationDto>()
            .ConstructUsing(src => new GeolocationDto(src.Latitude, src.Longitude));

        CreateMap<GeolocationDto, Geolocation>()
            .ConstructUsing(dto => new Geolocation { 
                Latitude = dto.Lat, 
                Longitude = dto.Lng 
            });

        CreateMap<UserCreationDto, User>()
            .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
    }
}