using System;
using AutoMapper;
using ClassifiedsApi.Contracts.Common.Requests;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class CharacteristicProfile : Profile
{
    public CharacteristicProfile(TimeProvider timeProvider)
    {
        CreateMap<Characteristic, CharacteristicInfo>(MemberList.Destination);

        CreateMap<UserAdvertRequest<CharacteristicAdd>, Characteristic>(MemberList.None)
            .ForMember(characteristic => characteristic.Id, map => map.MapFrom(_ => Guid.NewGuid()))
            .ForMember(characteristic => characteristic.CreatedAt, map => map.MapFrom(_ => timeProvider.GetUtcNow().UtcDateTime))
            .ForMember(characteristic => characteristic.Name, map => map.MapFrom(request => request.Model.Name))
            .ForMember(characteristic => characteristic.Value, map => map.MapFrom(request => request.Model.Value));
    }
}