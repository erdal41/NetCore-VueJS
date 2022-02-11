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
        public async Task<JsonResult> AllPosts(SubObjectType post_type, PostStatus? post_status)
        {
            var result = await _postService.GetAllAsync(post_type, post_status);
            var publishPostsCount = await _postService.PublishStatusCountAsync(post_type, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(post_type, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(post_type, PostStatus.trash);
            if (result.ResultStatus == ResultStatus.Success)
            {
                var postViewModel = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return new JsonResult(postViewModel);
            }
            else
            {
                var postViewModel = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return new JsonResult(postViewModel);
            }
        }

        [HttpGet("/admin/post-getschnemapagetype")]
        public JsonResult GetSchnemaPageType()
        {
            var schnemaPageType = Enum.GetValues(typeof(SchemaPageType))
                .Cast<SchemaPageType>()
                .Select(spt => new SchnemaTypeViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                });
            return new JsonResult(schnemaPageType);
        }

        [HttpGet("/admin/post-getschnemaarticletype")]
        public JsonResult GetSchnemaArticleType()
        {
            var schnemaArticleType = Enum.GetValues(typeof(SchemaArticleType))
                .Cast<SchemaArticleType>()
                .Select(spt => new SchnemaTypeViewModel
                {
                    Id = (int)spt,
                    Name = spt.GetDisplayName()
                });
            return new JsonResult(schnemaArticleType);
        }

        [HttpPost("/admin/post-new")]
        public async Task<IActionResult> New(PostViewModel postViewModel)
        {
            var postResult = await _postService.AddAsync(postViewModel.PostAddDto, LoggedInUser.Id, postViewModel.PostAddDto.PostType);
            if (string.IsNullOrEmpty(postViewModel.SeoObjectSettingAddDto.SeoTitle))
            {
                postViewModel.SeoObjectSettingAddDto.SeoTitle = postViewModel.PostAddDto.Title;
            }
            if (postResult.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var seoResult = await _seoService.SeoObjectSettingAddAsync(ObjectType.post, postViewModel.PostAddDto.PostType, postResult.Data.Post.Id, postViewModel.SeoObjectSettingAddDto, LoggedInUser.Id);
                //await _fileHelper.CreateSitemapInRootDirectoryAsync();
                var postAddViewModelJson = new PostViewModel
                {
                    PostDto = postResult.Data,
                    SeoObjectSettingDto = seoResult.Data

                };
                return Json(postAddViewModelJson);
            }
            else
            {
                var postAddViewModelJsonError = new PostViewModel
                {
                    PostDto = postResult.Data
                };
                return Json(postAddViewModelJsonError);
            }
        }

        [HttpGet("/admin/post-edit")]
        public async Task<IActionResult> Edit(int postId)
        {
            var postResult = await _postService.GetPostUpdateDtoAsync(postId);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            if (postResult.ResultStatus == ResultStatus.Success && seoGeneralResult.ResultStatus == ResultStatus.Success)
            {
                var seoResult = await _seoService.GetSeoObjectSettingUpdateDtoAsync(postId, postResult.Data.PostType);
                var postViewModelJson = new PostViewModel
                {
                    PostUpdateDto = postResult.Data,
                    IsActivePageSeoSetting = seoGeneralResult.Data.IsActivePageSeoSetting,
                    IsActiveArticleSeoSetting = seoGeneralResult.Data.IsActiveArticleSeoSetting,
                    SeoObjectSettingUpdateDto = seoResult.Data
                };
                return new JsonResult(postViewModelJson);
            }
            var postViewModelJsonError = new PostViewModel
            {
                PostUpdateDto = postResult.Data
            };
            return new JsonResult(postViewModelJsonError);
        }

        [HttpPost("/admin/post-edit")]
        public async Task<IActionResult> Edit(PostViewModel postViewModel)
        {
            var postResult = await _postService.UpdateAsync(postViewModel.PostUpdateDto, LoggedInUser.Id);
            if (postResult.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                //await _fileHelper.CreateSitemapInRootDirectoryAsync();
                await _seoService.SeoObjectSettingUpdateAsync(postViewModel.PostUpdateDto.Id, postViewModel.PostUpdateDto.PostType, postViewModel.SeoObjectSettingUpdateDto, LoggedInUser.Id);

                var postViewModelJson = new PostViewModel
                {
                    PostDto = postResult.Data
                };
                return new JsonResult(postViewModelJson);
            }
            var postViewModelJsonError = new PostViewModel
            {
                PostDto = postResult.Data
            };
            return new JsonResult(postViewModelJsonError);
        }

        [HttpGet("/admin/post-alltopposts")]
        public async Task<JsonResult> GetAllTopPosts(int? postId)
        {
            var result = await _postService.GetAllAnotherPostsAsync(SubObjectType.page, postId);
            return new JsonResult(result.Data);
        }

        [HttpGet("/admin/post-allsubposts")]
        public async Task<JsonResult> GetAllSubPosts(int? postId)
        {
            var result = await _postService.GetAllSubPostsAsync(SubObjectType.page, postId);
            return new JsonResult(result.Data);
        }

        [HttpGet("/admin/post-allsubpostsDetail")]
        public async Task<JsonResult> GetAllSubPostsDetail(int postId)
        {
            var result = await _postService.GetAllSubPostDetailsAsync(postId);
            return new JsonResult(result.Data);
        }

        [HttpGet("/admin/post-subpostDetail")]
        public async Task<JsonResult> GetSubPostDetail(int postId)
        {
            var result = await _postService.GetAllSubPostDetailsAsync(postId);
            return new JsonResult(result);
        }

        [HttpPost("/admin/post-addgalleryimage")]
        public async Task<IActionResult> AddGalleryImage(int postId, List<int> galleryIds)
        {
            var result = await _postService.GalleryImageAddAsync(postId, galleryIds);
            return new JsonResult(result.Data);
        }

        [HttpPost("/admin/post-edittoppost")]
        public async Task<IActionResult> EditTopPost(int postId, int parentId)
        {
            var result = await _postService.TopPostUpdateAsync(postId, parentId);
            return new JsonResult(result.Data);
        }

        [HttpPost("/admin/post-editsubpost")]
        public async Task<IActionResult> EditSubPost(int postId, int? subPostParentId)
        {
            var result = await _postService.SubPostUpdateAsync(postId, subPostParentId);
            return new JsonResult(result);
        }

        [HttpPost("/admin/post-editsubpostdetail")]
        public async Task<IActionResult> EditSubPostDetail(int postId, string description, int? featuredImageId)
        {
            var result = await _postService.SubPostDetailUpdateAsync(postId, description, featuredImageId);
            return new JsonResult(result.Data);
        }

        [HttpPost("/admin/post-poststatuschange")]
        public async Task<JsonResult> PostStatusChange(int postId, PostStatus status)
        {
            var result = await _postService.PostStatusChangeAsync(postId, status, LoggedInUser.Id);
            var postViewModel = new PostViewModel
            {
                PostDto = result.Data,
            };
            return Json(postViewModel);
        }

        [HttpPost("/admin/post-delete")]
        public async Task<JsonResult> Delete(int postId)
        {
            var result = await _postService.DeleteAsync(postId);
            //_cacheService.Clear();
            var postViewModel = new PostViewModel
            {
                PostDto = result.Data,
            };
            return Json(postViewModel);
        }
    }
}