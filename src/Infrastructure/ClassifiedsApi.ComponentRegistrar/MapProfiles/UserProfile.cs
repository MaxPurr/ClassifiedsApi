using AutoMapper;
using ClassifiedsApi.Contracts.Contexts.Users;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserInfo>(MemberList.Destination);
        
        CreateMap<User, UserContactsInfo>(MemberList.Destination);
    }
}