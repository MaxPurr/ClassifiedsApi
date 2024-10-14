using System;
using System.Linq;
using AutoMapper;
using ClassifiedsApi.Contracts.Contexts.Accounts;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class AccountProfile : Profile
{
    public AccountProfile(TimeProvider timeProvider)
    {
        CreateMap<AccountRegisterRequest, User>(MemberList.None)
            .ForMember(user => user.Id, map => map.MapFrom(_ => Guid.NewGuid()))
            .ForMember(user => user.CreatedAt, map => map.MapFrom(_ => timeProvider.GetUtcNow().UtcDateTime));

        CreateMap<User, AccountInfo>(MemberList.None)
            .ForMember(info => info.RoleNames, map => map.MapFrom(user => user.Roles.Select(role => role.Name)));
    }
}