using AutoMapper;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;

namespace VueJS.Mvc.AutoMapper.Profiles
{
    public class ViewModelsProfile : Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<ArticleRightSideBarWidgetOptions, ArticleRightSideBarWidgetOptionsDto>();
        }
    }
}