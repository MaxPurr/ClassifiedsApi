using System;
using System.Linq;
using AutoMapper;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class AdvertProfile : Profile
{
    public AdvertProfile(TimeProvider timeProvider)
    {
        CreateMap<AdvertCreateRequest, Advert>(MemberList.None)
            .ForMember(advert => advert.Id, map => map.MapFrom(_ => Guid.NewGuid()))
            .ForMember(advert => advert.CreatedAt, map => map.MapFrom(_ => timeProvider.GetUtcNow().UtcDateTime))
            .ForMember(advert => advert.Title, map => map.MapFrom(request => request.AdvertCreate.Title))
            .ForMember(advert => advert.Description, map => map.MapFrom(request => request.AdvertCreate.Description))
            .ForMember(advert => advert.Price, map => map.MapFrom(request => request.AdvertCreate.Price))
            .ForMember(advert => advert.Disabled, map => map.MapFrom(_ => false))
            .ForMember(advert => advert.CategoryId, map => map.MapFrom(request => request.AdvertCreate.CategoryId))
            .AfterMap((request, advert) =>
                {
                    advert.Characteristics = request.AdvertCreate.Characteristics.Select(pair => new Characteristic
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = timeProvider.GetUtcNow().UtcDateTime,
                        AdvertId = advert.Id,
                        Name = pair.Key,
                        Value = pair.Value
                    }).ToArray();
                }
            );

        CreateMap<Advert, AdvertInfo>(MemberList.None)
            .ForMember(info => info.ImageIds, map => map.MapFrom(advert => advert.Images.Select(advertImage => advertImage.ImageId)));
        
        CreateMap<Advert, ShortAdvertInfo>(MemberList.None)
            .ForMember(info => info.ImageIds, map => map.MapFrom(advert => advert.Images.Select(advertImage => advertImage.ImageId)));

        CreateMap<Advert, UpdatedAdvertInfo>(MemberList.Destination);
    }
}