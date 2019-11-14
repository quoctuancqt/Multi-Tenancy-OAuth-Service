using AutoMapper;
using System;
using System.Text;
using OAuthService.Server.Domain;
using OAuthService.Server.Dto;

namespace OAuthService.Server.MapperProfiles
{
    public class ClientMapperProfile : Profile
    {
        public ClientMapperProfile()
        {
            #region Dto To Entity

            CreateMap<AddOrEditClientDto, Client>().AfterMap((src, dest) =>
            {
                dest.Secret = Convert.ToBase64String(Encoding.UTF8.GetBytes(src.SubDomain));
            });

            CreateMap<EditClientCommonInfoDto, Client>();

            #endregion

            #region Entity To Dto
            #endregion
        }
    }

    public static class ClientMapper
    {
        static ClientMapper()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ClientMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static Client ToEntity(this AddOrEditClientDto dto)
        {
            return Mapper.Map<Client>(dto);
        }

        public static Client ToEntity(this EditClientCommonInfoDto dto, Client entity)
        {
            return Mapper.Map(dto, entity);
        }

        //public static UserProfileDto ToDto(this User entity)
        //{
        //    return Mapper.Map<UserProfileDto>(entity);
        //}
    }
}
