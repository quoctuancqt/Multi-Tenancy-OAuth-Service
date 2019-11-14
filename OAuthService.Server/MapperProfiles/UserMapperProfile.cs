using AutoMapper;
using System;
using System.Text;
using OAuthService.Server.Domain;
using OAuthService.Server.Dto;

namespace OAuthService.Server.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            #region Dto To Entity

            CreateMap<AddOrEditAccountDto, User>().AfterMap((src, dest) =>
            {

            });

            #endregion

            #region Entity To Dto
            #endregion
        }
    }

    public static class UserMapper
    {
        static UserMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static User ToEntity(this AddOrEditAccountDto dto)
        {
            return Mapper.Map<User>(dto);
        }

        public static User ToEntity(this AddOrEditAccountDto dto, User entity)
        {
            return Mapper.Map(dto, entity);
        }

        //public static UserProfileDto ToDto(this User entity)
        //{
        //    return Mapper.Map<UserProfileDto>(entity);
        //}
    }
}
