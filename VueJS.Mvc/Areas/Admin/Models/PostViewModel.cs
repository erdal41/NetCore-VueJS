using VueJS.Entities.Dtos;

namespace VueJS.Mvc.Areas.Admin.Models
{
    public class PostViewModel
    {
        public PostDto PostDto { get; set; }
        public PostListDto PostListDto { get; set; }
        public int PublishPostsCount { get; set; }
        public int DraftPostsCount { get; set; }
        public int TrashPostsCount { get; set; }
        public PostAddDto PostAddDto { get; set; }
        public SeoObjectSettingDto SeoObjectSettingDto { get; set; }
        public SeoObjectSettingAddDto SeoObjectSettingAddDto { get; set; }
        public PostUpdateDto PostUpdateDto { get; set; }
        public PostListDto ChildrenListDto { get; set; }
        public PostTermListDto PostTermListDto { get; set; }
        public GalleryListDto GalleryListDto { get; set; }
        public UploadDto UploadDto { get; set; }
        public SeoObjectSettingUpdateDto SeoObjectSettingUpdateDto { get; set; }
        public bool IsActivePageSeoSetting { get; set; }
        public bool IsActiveArticleSeoSetting { get; set; }
    }
}