#region

using AutoMapper;
using RedFox.Application.DTO;
using RedFox.Domain.Entities;

#endregion

namespace RedFox.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<Company, CompanyDto>().ReverseMap();
        CreateMap<UserCreationDto, User>().ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company));
    }
}