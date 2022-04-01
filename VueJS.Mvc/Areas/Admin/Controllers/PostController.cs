using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models.Data;
using VueJS.Mvc.Areas.Admin.Models.View;
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
        public async Task<JsonResult> AllPosts(ObjectType postType, PostStatus? postStatus, int? userId, string category, string tag)
        {
            var postListResult = await _postService.GetAllAsync(userId, category, tag, postType, postStatus);
            var publishPosts = await _postService.GetAllAsync(null, null, null, postType, PostStatus.publish);
            var draftPosts = await _postService.GetAllAsync(null, null, null, postType, PostStatus.draft);
            var trashPosts = await _postService.GetAllAsync(null, null, null, postType, PostStatus.trash);

            return Json(new PostViewModel
            {
                PostListDto = postListResult,
                PublishPostsCount = publishPosts.Data.Posts.Count,
                DraftPostsCount = draftPosts.Data.Posts.Count,
                TrashPostsCount = trashPosts.Data.Posts.Count
            });
        }

        [HttpGet("/admin/post-getschemapagetype")]
        public JsonResult GetSchemaPageType()
        {
            return Json(Enum.GetValues(typeof(SchemaPageType))
                .Cast<SchemaPageType>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpGet("/admin/post-getschemaarticletype")]
        public JsonResult GetSchemaArticleType()
        {
            return Json(Enum.GetValues(typeof(SchemaArticleType))
                .Cast<SchemaArticleType>()
                .Select(spt => new EnumViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                }));
        }

        [HttpPost("/admin/post-new")]
        public async Task<JsonResult> New(PostDataModel postDataModel)
        {
            if (string.IsNullOrEmpty(postDataModel.SeoObjectSettingAddDto.SeoTitle))
            {
                postDataModel.SeoObjectSettingAddDto.SeoTitle = postDataModel.PostAddDto.Title;
            }

            var postResult = await _postService.AddAsync(postDataModel.PostAddDto, LoggedInUser.Id);
            //_cacheService.Clear();
            var seoResult = await _seoService.SeoObjectSettingAddAsync(postDataModel.PostAddDto.PostType, postResult.ResultStatus == ResultStatus.Success ? postResult.Data.Post.Id : -1, postDataModel.SeoObjectSettingAddDto, LoggedInUser.Id);
            //await _fileHelper.CreateSitemapInRootDirectoryAsync();
            return Json(new PostViewModel
            {
                PostDto = postResult,
                SeoObjectSettingDto = seoResult
            });
        }

        [HttpGet("/admin/post-edit")]
        public async Task<JsonResult> Edit(int postId)
        {
            var postUpdateResult = await _postService.GetPostUpdateDtoAsync(postId);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            var seoObjectSettingUpdateResult = postUpdateResult.ResultStatus == ResultStatus.Success ? await _seoService.GetSeoObjectSettingUpdateDtoAsync(postId, postUpdateResult.Data.PostType) : null;
            return Json(new PostViewModel
            {
                PostUpdateDto = postUpdateResult,
                IsActivePageSeoSetting = seoGeneralResult.Data.IsActivePageSeoSetting,
                IsActiveArticleSeoSetting = seoGeneralResult.Data.IsActiveArticleSeoSetting,
                SeoObjectSettingUpdateDto = seoObjectSettingUpdateResult
            });
        }

        [HttpPost("/admin/post-edit")]
        public async Task<JsonResult> Edit(PostDataModel postDataModel)
        {
            var postResult = await _postService.UpdateAsync(postDataModel.PostUpdateDto, LoggedInUser.Id);
            //_cacheService.Clear();
            //await _fileHelper.CreateSitemapInRootDirectoryAsync();
            var seoObjectSettingResult = postResult.ResultStatus == ResultStatus.Success ? await _seoService.SeoObjectSettingUpdateAsync(postDataModel.PostUpdateDto.Id, postDataModel.PostUpdateDto.PostType, postDataModel.SeoObjectSettingUpdateDto, LoggedInUser.Id) : null;
            return Json(new PostViewModel
            {
                PostDto = postResult,
                SeoObjectSettingDto = seoObjectSettingResult
            });
        }

        [HttpGet("/admin/post-alltopposts")]
        public async Task<JsonResult> GetAllTopPosts(int? postId)
        {
            return Json(new PostViewModel { PostListDto = await _postService.GetAllTopPostsAsync(postId) });
        }

        [HttpGet("/admin/post-allsubposts")]
        public async Task<JsonResult> GetAllSubPosts(int? postId)
        {
            return Json(new PostViewModel { PostListDto = await _postService.GetAllSubPostsAsync(postId) });
        }

        [HttpGet("/admin/post-allsubpostsDetail")]
        public async Task<JsonResult> GetAllSubPostsDetail(int postId)
        {
            return Json(new PostViewModel { PostListDto = await _postService.GetAllSubPostDetailsAsync(postId) });
        }

        [HttpGet("/admin/post-subpostDetail")]
        public async Task<JsonResult> GetSubPostDetail(int postId)
        {
            return Json(new PostViewModel { PostListDto = await _postService.GetAllSubPostDetailsAsync(postId) });
        }

        [HttpPost("/admin/post-addgalleryimage")]
        public async Task<JsonResult> AddGalleryImage(int postId, List<int> galleryIds)
        {
            return Json(new PostViewModel { PostDto = await _postService.GalleryImageAddAsync(postId, galleryIds) });
        }

        [HttpPost("/admin/post-edittoppost")]
        public async Task<JsonResult> EditTopPost(int postId, int parentId)
        {
            return Json(new PostViewModel { PostDto = await _postService.TopPostUpdateAsync(postId, parentId) });
        }

        [HttpPost("/admin/post-editsubpost")]
        public async Task<JsonResult> EditSubPost(int postId, int? subPostParentId)
        {
            return Json(await _postService.SubPostUpdateAsync(postId, subPostParentId));
        }

        [HttpPost("/admin/post-editsubpostdetail")]
        public async Task<JsonResult> EditSubPostDetail(int postId, string description, int? featuredImageId)
        {
            return Json(new PostViewModel { PostDto = await _postService.SubPostDetailUpdateAsync(postId, description, featuredImageId) });
        }

        [HttpPost("/admin/post-poststatuschange")]
        public async Task<JsonResult> PostStatusChange(int postId, PostStatus status)
        {
            return Json(new PostViewModel { PostDto = await _postService.PostStatusChangeAsync(postId, status, LoggedInUser.Id) });
        }

        [HttpPost("/admin/post-delete")]
        public async Task<JsonResult> Delete(int postId)
        {
            //_cacheService.Clear();
            return Json(new PostViewModel { PostDto = await _postService.DeleteAsync(postId) });
        }
    }
}