using AutoMapper;
using UserManagement.Application.Commands;
using UserManagement.Application.DTOs.AcessLevels;
using UserManagement.Application.DTOs.Users;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
         CreateMap<User, UserDto>()
            .ConstructUsing(src => new UserDto(
                src.Id,
                src.Name,
                src.Email,
                src.CreatedAt,
                src.UpdatedAt,
                src.UserAccessLevels.Where(ual => ual.IsActive).Select(ual => ual.AccessLevel.Name).FirstOrDefault()
            ));


        CreateMap<CreateUserCommand, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UserAccessLevels, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // AccessLevel mappings
        CreateMap<AccessLevel, AccessLevelDto>();

        CreateMap<CreateAccessLevelCommand, AccessLevel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // UserAccessLevel mappings
        CreateMap<UserAccessLevel, UserAccessLevelDto>()
            .ForMember(dto => dto.AccessLevelName, opt => opt.MapFrom(src => src.AccessLevel.Name));
    }
}