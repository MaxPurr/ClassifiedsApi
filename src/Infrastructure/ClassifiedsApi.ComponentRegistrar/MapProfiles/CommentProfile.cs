using System;
using AutoMapper;
using ClassifiedsApi.Contracts.Contexts.Comments;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class CommentProfile : Profile
{
    public CommentProfile(TimeProvider timeProvider)
    {
        CreateMap<CommentCreateRequest, Comment>(MemberList.None)
            .ForMember(comment => comment.Id, map => map.MapFrom(_ => Guid.NewGuid()))
            .ForMember(comment => comment.CreatedAt, map => map.MapFrom(_ => timeProvider.GetUtcNow().UtcDateTime))
            .ForMember(comment => comment.Text, map => map.MapFrom(request => request.CommentCreate.Text))
            .ForMember(comment => comment.ParentId, map => map.MapFrom(request => request.CommentCreate.ParentId))
            .ForMember(comment => comment.AdvertId, map => map.MapFrom(request => request.AdvertId))
            .ForMember(comment => comment.UserId, map => map.MapFrom(request => request.UserId));

        CreateMap<Comment, CommentInfo>(MemberList.None)
            .ForMember(info => info.User, map => map.MapFrom(comment => comment.User));

        CreateMap<User, CommentUserInfo>(MemberList.Destination);
    }
}