using AutoMapper;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using System;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostAddDto, Post>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<PostUpdateDto, Post>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Post, PostUpdateDto>();
            CreateMap<SubPostUpdateDto, Post>().ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Post, SubPostUpdateDto>();
        }
    }
}