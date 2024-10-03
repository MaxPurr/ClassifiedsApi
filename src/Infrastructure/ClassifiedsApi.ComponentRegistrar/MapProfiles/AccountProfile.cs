using System;
using System.Linq;
using AutoMapper;
using ClassifiedsApi.AppServices.Helpers;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<AccountRegister, User>(MemberList.None)
            .ForMember(user => user.Id, map => map.MapFrom(_ => Guid.NewGuid()))
            .ForMember(user => user.CreatedAt, map => map.MapFrom(_ => DateTime.UtcNow))
            .ForMember(user => user.PasswordHash, 
                map => map.MapFrom(register => CryptoHelper.GetBase64Hash(register.Password)));

        CreateMap<User, AccountInfo>(MemberList.None)
            .ForMember(info => info.RoleNames, map => map.MapFrom(user => user.Roles.Select(role => role.Name)));
    }
}