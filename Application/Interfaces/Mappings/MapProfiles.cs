using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using System.Threading.Tasks;
using Application.DTO.User;
using Domain.Models.UserModels;
using Application.DTO.Permission;
using Application.DTO.Role;
using System.Net;
using Application.DTO.RolePermission;
using Application.DTO.UserRole;
using Application.DTO.Phone;
using Domain.Models.Entities;
using Domain.Models.IdentityEntites;

namespace Application.Mappings
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            MapPermission();
            MapRole();
            MapRolePermission();
            MapUser();
            MapUserRole();
            MapPhone();
        }
        public void MapPermission()
        {
            CreateMap<PermissionCreateDTO, Permission>().ReverseMap();
            CreateMap<PermissionUpdateDTO, Permission>().ReverseMap()
            .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PermissionGetDTO, Permission>().ReverseMap()
            .ForMember(dest => dest.PermissionId, opt => opt.MapFrom(src => src.Id));
        }
        public void MapRolePermission()
        {
            CreateMap<RolePermissionCreateDTO, RolePermission>().ReverseMap();
            CreateMap<RolePermissionGetDTO, RolePermission>().ReverseMap()
                .ForMember(dest => dest.RolePermissionId, opt => opt.MapFrom(src => src.Id));
            CreateMap<RolePermissionUpdateDTO, RolePermission>().ReverseMap()
                .ForMember(dest => dest.RolePermissionId, opt => opt.MapFrom(src => src.Id));
        }
        public void MapUserRole()
        {
            CreateMap<UserRoleCreateDTO, UserRole>().ReverseMap();
            CreateMap<UserRoleGetDTO, UserRole>().ReverseMap()
                .ForMember(dest => dest.UserRoleId, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserRoleUpdateDTO, UserRole>().ReverseMap()
                .ForMember(dest => dest.UserRoleId, opt => opt.MapFrom(src => src.Id));
        }

        public void MapRole()
        {
            CreateMap<RoleCreateDTO, Role>().ReverseMap();
            CreateMap<RoleUpdateDTO, Role>().ReverseMap()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            CreateMap<RoleGetDTO, Role>().ReverseMap()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
        }

        public void MapUser()
        {
            CreateMap<UserCreateDTO, User>().ReverseMap();
            CreateMap<UserUpdateDTO, User>().ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            CreateMap<UserGetDTO, User>().ReverseMap()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }

        public void MapPhone()
        {
            CreateMap<PhoneCreateDTO, Phone>().ReverseMap();
            CreateMap<PhoneGetDTO, Phone>().ReverseMap()
                .ForMember(dest => dest.PhoneId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PhoneUpdateDTO, Phone>().ReverseMap()
                .ForMember(dest => dest.PhoneId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
