using AutoMapper;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;

namespace VueJS.Services.AutoMapper.Profiles
{
    public class TermProfile : Profile
    {
        public TermProfile()
        {
            CreateMap<TermAddDto, Term>();
            CreateMap<TermUpdateDto, Term>();
            CreateMap<Term, TermUpdateDto>();

            CreateMap<PostTermAddDto, PostTerm>();
        }
    }
}