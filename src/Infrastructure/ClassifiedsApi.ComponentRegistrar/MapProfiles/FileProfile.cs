using System.IO;
using AutoMapper;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.DataAccess.Helpers;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using FileInfo = ClassifiedsApi.Contracts.Contexts.Files.FileInfo;

namespace ClassifiedsApi.ComponentRegistrar.MapProfiles;

public class FileProfile : Profile
{
    public FileProfile()
    {
        CreateMap<GridFSFileInfo, FileInfo>(MemberList.Destination)
            .ForMember(info => info.Id, map => map.MapFrom(info => MongoDbHelper.ParseStringFromObjectId(info.Id)))
            .ForMember(info => info.CreatedAt, map => map.MapFrom(info => info.UploadDateTime))
            .ForMember(info => info.Name, map => map.MapFrom(info => info.Filename));
        
        CreateMap<GridFSDownloadStream<ObjectId>, FileDownload>()
            .ForMember(download => download.Name, map => map.MapFrom(download => download.FileInfo.Filename))
            .ForMember(download => download.ReadStream, map => map.MapFrom(download => (Stream)download))
            .ForMember(download => download.ContentType, map => map.MapFrom(download => download.FileInfo.Metadata["content-type"].AsString));
    }
}