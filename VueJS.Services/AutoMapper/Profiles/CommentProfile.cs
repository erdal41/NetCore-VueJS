using AutoMapper;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using System;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentAddDto, Comment>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(dest => dest.CommentStatus, opt => opt.MapFrom(x => CommentStatus.moderated));
            CreateMap<CommentUpdateDto, Comment>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Comment, CommentUpdateDto>();
        }
    }
}