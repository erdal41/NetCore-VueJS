using AutoMapper;
using Microsoft.AspNetCore.Http;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using System;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class UploadProfile : Profile
    {
        public UploadProfile()
        {
            CreateMap<Upload, UploadDto>();
            CreateMap<UploadAddDto, Upload>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<IFormFile, UploadAddDto>();
            CreateMap<UploadUpdateDto, Upload>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Upload, UploadUpdateDto>();

            CreateMap<GalleryAddDto, Gallery>();
        }
    }
}