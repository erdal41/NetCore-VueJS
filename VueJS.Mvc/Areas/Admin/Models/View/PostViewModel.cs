using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models.View
{
    public class PostViewModel
    {
        public IDataResult<PostDto> PostDto { get; set; }
        public IDataResult<PostListDto> PostListDto { get; set; }
        public IDataResult<PostUpdateDto> PostUpdateDto { get; set; }
        public IDataResult<SeoObjectSettingDto> SeoObjectSettingDto { get; set; }
        public IDataResult<SeoObjectSettingUpdateDto> SeoObjectSettingUpdateDto { get; set; }
        public int PublishPostsCount { get; set; }
        public int DraftPostsCount { get; set; }
        public int TrashPostsCount { get; set; }
        public bool IsActivePageSeoSetting { get; set; }
        public bool IsActiveArticleSeoSetting { get; set; }

    }
}