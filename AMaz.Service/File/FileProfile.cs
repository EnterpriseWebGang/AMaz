using AMaz.Util;
using AMaz.Models;
using AutoMapper;
using File = AMaz.Entity.File;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace AMaz.Service
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<File, FileViewModel>().
                ForMember(dest => dest.ContributionId, opt => opt.MapFrom(src => src.Contribution == null? src.Contribution.ContributionId.ToString() : "")).
                ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FileId.ToString()));

            CreateMap<IFormFile, CreateFileModel>().
                ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetFileName(src.FileName))).
                ForMember(dest => dest.FileType, opt => opt.MapFrom(src => GetFileType(src.ContentType))).
                ForMember(dest => dest.MIMEType, opt => opt.MapFrom(src => src.ContentType)).
                ForMember(dest => dest.File, opt => opt.MapFrom(src => src));

            CreateMap<CreateFileModel, File>().ReverseMap(); //2 ways

            CreateMap<CreateFileModel, SaveFileRequest>().ReverseMap();
        }

        private string GetFileName(string fileName)
        {
            var name = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var dateTime = DateTime.Now.ToString("yyyyMMddHHmm");

            if (name == null)
            {
                return dateTime + "_" + extension;
            }

            var result = string.Join("_", dateTime, name, RandomNumberGenerator.GetInt32(1000, 9999).ToString()) + extension;

            return result;
        }

        private FileType GetFileType(string contentType)
        {
            switch (true)
            {
                case bool _ when contentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return FileType.Docx;
                case bool _ when Regex.IsMatch(contentType, @"^image\/(jpeg|png|gif|bmp)$", RegexOptions.IgnoreCase):
                    //TODO: Need to add more formats
                    return FileType.Image;

                default:
                    return FileType.Undefined;
            }

        }
    }
}
