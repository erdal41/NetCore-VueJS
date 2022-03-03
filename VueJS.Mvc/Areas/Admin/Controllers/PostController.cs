using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using VueJS.Shared.Utilities.Extensions;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : BaseController
    {
        private readonly IPostService _postService;
        private readonly ISeoService _seoService;
        //private readonly IFileHelper _fileHelper;
        //private readonly ICacheService _cacheService;

        public PostController(IPostService postService, ISeoService seoService, UserManager<User> userManager, IMapper mapper) : base(userManager, mapper)
        {
            _postService = postService;
            _seoService = seoService;
        }

        [HttpGet("/admin/post-allposts")]
        public async Task<JsonResult> AllPosts(SubObjectType postType, PostStatus? postStatus)
        {
            var result = await _postService.GetAllAsync(postType, postStatus);
            var publishPosts = await _postService.GetAllAsync(postType, PostStatus.publish);
            var draftPosts = await _postService.GetAllAsync(postType, PostStatus.draft);
            var trashPosts = await _postService.GetAllAsync(postType, PostStatus.trash);

            return Json(new PostViewModel
            {
                PostListDto = result,
                PublishPostsCount = publishPosts.Data.Posts.Count,
                DraftPostsCount = draftPosts.Data.Posts.Count,
                TrashPostsCount = trashPosts.Data.Posts.Count
            });
        }

        [HttpGet("/admin/post-getschnemapagetype")]
        public JsonResult GetSchnemaPageType()
        {
            return Json(Enum.GetValues(typeof(SchemaPageType))
                .Cast<SchemaPageType>()
                .Select(spt => new SchnemaTypeViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/post-getschnemaarticletype")]
        public JsonResult GetSchnemaArticleType()
        {
            return Json(Enum.GetValues(typeof(SchemaArticleType))
                .Cast<SchemaArticleType>()
                .Select(spt => new SchnemaTypeViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpPost("/admin/post-new")]
        public async Task<IActionResult> New(PostViewModel postViewModel)
        {
            if (string.IsNullOrEmpty(postViewModel.SeoObjectSettingAddDto.SeoTitle))
            {
                postViewModel.SeoObjectSettingAddDto.SeoTitle = postViewModel.PostAddDto.Title;
            }

            var postResult = await _postService.AddAsync(postViewModel.PostAddDto, LoggedInUser.Id, postViewModel.PostAddDto.PostType);
            //_cacheService.Clear();
            var seoResult = await _seoService.SeoObjectSettingAddAsync(ObjectType.post, postViewModel.PostAddDto.PostType, postResult.Data.Post.Id, postViewModel.SeoObjectSettingAddDto, LoggedInUser.Id);
            //await _fileHelper.CreateSitemapInRootDirectoryAsync();
            return Json(new PostViewModel
            {
                PostDto = postResult,
                SeoObjectSettingDto = seoResult
            });
        }

        [HttpGet("/admin/post-edit")]
        public async Task<IActionResult> Edit(int postId)
        {
            var postResult = await _postService.GetPostUpdateDtoAsync(postId);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            var seoResult = postResult.ResultStatus == ResultStatus.Success ? await _seoService.GetSeoObjectSettingUpdateDtoAsync(postId, postResult.Data.PostType) : null;
            return Json(new PostViewModel
            {
                PostUpdateDto = postResult,
                IsActivePageSeoSetting = seoGeneralResult.Data.IsActivePageSeoSetting,
                IsActiveArticleSeoSetting = seoGeneralResult.Data.IsActiveArticleSeoSetting,
                SeoObjectSettingUpdateDto = seoResult
            });
        }

        [HttpPost("/admin/post-edit")]
        public async Task<IActionResult> Edit(PostViewModel postViewModel)
        {
            var result = await _postService.UpdateAsync(postViewModel.PostUpdateDto.Data, LoggedInUser.Id);
            //_cacheService.Clear();
            //await _fileHelper.CreateSitemapInRootDirectoryAsync();
            await _seoService.SeoObjectSettingUpdateAsync(postViewModel.PostUpdateDto.Data.Id, postViewModel.PostUpdateDto.Data.PostType, postViewModel.SeoObjectSettingUpdateDto.Data, LoggedInUser.Id);
            return Json(new PostViewModel { PostDto = result });
        }

        [HttpGet("/admin/post-alltopposts")]
        public async Task<JsonResult> GetAllTopPosts(int? postId)
        {
            return Json(await _postService.GetAllTopPostsAsync(SubObjectType.page, postId));
        }

        [HttpGet("/admin/post-allsubposts")]
        public async Task<JsonResult> GetAllSubPosts(int? postId)
        {
            return Json(await _postService.GetAllSubPostsAsync(SubObjectType.page, postId));
        }

        [HttpGet("/admin/post-allsubpostsDetail")]
        public async Task<JsonResult> GetAllSubPostsDetail(int postId)
        {
            return Json(await _postService.GetAllSubPostDetailsAsync(postId));
        }

        [HttpGet("/admin/post-subpostDetail")]
        public async Task<JsonResult> GetSubPostDetail(int postId)
        {
            return Json(await _postService.GetAllSubPostDetailsAsync(postId));
        }

        [HttpPost("/admin/post-addgalleryimage")]
        public async Task<IActionResult> AddGalleryImage(int postId, List<int> galleryIds)
        {
            return Json(await _postService.GalleryImageAddAsync(postId, galleryIds));
        }

        [HttpPost("/admin/post-edittoppost")]
        public async Task<IActionResult> EditTopPost(int postId, int parentId)
        {
            return Json(await _postService.TopPostUpdateAsync(postId, parentId));
        }

        [HttpPost("/admin/post-editsubpost")]
        public async Task<IActionResult> EditSubPost(int postId, int? subPostParentId)
        {
            return Json(await _postService.SubPostUpdateAsync(postId, subPostParentId));
        }

        [HttpPost("/admin/post-editsubpostdetail")]
        public async Task<IActionResult> EditSubPostDetail(int postId, string description, int? featuredImageId)
        {
            return Json(await _postService.SubPostDetailUpdateAsync(postId, description, featuredImageId));
        }

        [HttpPost("/admin/post-poststatuschange")]
        public async Task<JsonResult> PostStatusChange(int postId, PostStatus status)
        {
            var result = await _postService.PostStatusChangeAsync(postId, status, LoggedInUser.Id);
            return Json(new PostViewModel { PostDto = result });
        }

        [HttpPost("/admin/post-delete")]
        public async Task<JsonResult> Delete(int postId)
        {
            var result = await _postService.DeleteAsync(postId);
            //_cacheService.Clear();
            return Json(new PostViewModel { PostDto = result });
        }
    }
}