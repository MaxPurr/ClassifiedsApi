using System;
using AutoMapper;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.Domain.Entities;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class FileProfile : Profile
{
    public FileProfile(TimeProvider timeProvider)
    {
        CreateMap<FileUpload, File>(MemberList.None)
            .ForMember(file => file.Id, map => map.MapFrom(_ => Guid.NewGuid()))
            .ForMember(file => file.CreatedAt, map => map.MapFrom(_ => timeProvider.GetUtcNow().UtcDateTime));
        
        CreateMap<File, FileInfo>(MemberList.Destination);

        CreateMap<File, FileDownload>(MemberList.Destination);
    }
}