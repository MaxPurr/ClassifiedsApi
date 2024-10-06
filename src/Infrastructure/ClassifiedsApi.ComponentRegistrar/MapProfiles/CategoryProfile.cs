using System;
using AutoMapper;
using ClassifiedsApi.Contracts.Contexts.Categories;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile(TimeProvider timeProvider)
    {
        CreateMap<CategoryCreate, Category>(MemberList.Source)
            .ForMember(category => category.Id, map => map.MapFrom(_ => Guid.NewGuid()))
            .ForMember(category => category.CreatedAt, map => map.MapFrom(_ => timeProvider.GetUtcNow().UtcDateTime));
        
        CreateMap<Category, CategoryInfo>(MemberList.Destination);
    }
}