using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Mvc.Areas.Admin.Models;
using VueJS.Mvc.Helpers.Abstract;
using VueJS.Services.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using Newtonsoft.Json;
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

        public PostController(IPostService postService, ISeoService seoService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _postService = postService;
            _seoService = seoService;
        }

        [HttpGet]
        [Route("/admin/post/allposts")]
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

        [HttpGet]
        [Route("/admin/post/getschnemapagetype")]
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

        [HttpGet]
        [Route("/admin/post/getschnemaarticletype")]
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

        [HttpPost]
        [Route("/admin/post/new")]
        public async Task<IActionResult> New(PostViewModel postViewModel)
        {
            var postResult = await _postService.AddAsync(postViewModel.PostAddDto, 1, postViewModel.PostAddDto.PostType);
            if (string.IsNullOrEmpty(postViewModel.SeoObjectSettingAddDto.SeoTitle))
            {
                postViewModel.SeoObjectSettingAddDto.SeoTitle = postViewModel.PostAddDto.Title;
            }
            if (postResult.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var seoResult = await _seoService.SeoObjectSettingAddAsync(ObjectType.post, postViewModel.PostAddDto.PostType, postResult.Data.Post.Id, postViewModel.SeoObjectSettingAddDto, 1);
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

        [HttpGet]
        [Route("/admin/post/edit")]
        public async Task<IActionResult> Edit(int post)
        {
            var postResult = await _postService.GetPostUpdateDtoAsync(post);
            var seoGeneralResult = await _seoService.GetSeoGeneralSettingUpdateDtoAsync();
            if (postResult.ResultStatus == ResultStatus.Success && seoGeneralResult.ResultStatus == ResultStatus.Success)
            {
                var seoResult = await _seoService.GetSeoObjectSettingUpdateDtoAsync(post, postResult.Data.PostType);
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

        [HttpPost]
        [Route("/admin/post/edit")]
        public async Task<IActionResult> Edit(PostViewModel postViewModel)
        {
            var postResult = await _postService.UpdateAsync(postViewModel.PostUpdateDto, 1);
            if (postResult.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                //await _fileHelper.CreateSitemapInRootDirectoryAsync();
                await _seoService.SeoObjectSettingUpdateAsync(postViewModel.PostUpdateDto.Id, postViewModel.PostUpdateDto.PostType, postViewModel.SeoObjectSettingUpdateDto, 1);

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

        [HttpGet]
        [Route("/admin/post/getalltopposts")]
        public async Task<JsonResult> GetAllTopPosts(int? postId)
        {
            var result = await _postService.GetAllAnotherPostsAsync(SubObjectType.page, postId);
            return new JsonResult(result.Data);
        }

        [HttpGet]
        [Route("/admin/post/getallsubposts")]
        public async Task<JsonResult> GetAllSubPosts(int? postId)
        {
            var result = await _postService.GetAllSubPostsAsync(SubObjectType.page, postId);
            return new JsonResult(result.Data);
        }

        [HttpGet]
        [Route("/admin/post/getallsubpostsDetail")]
        public async Task<JsonResult> GetAllSubPostsDetail(int postId)
        {
            var result = await _postService.GetAllSubPostDetailsAsync(postId);
            return new JsonResult(result.Data);
        }

        [HttpGet]
        [Route("/admin/post/getsubpostDetail")]
        public async Task<JsonResult> GetSubPostDetail(int postId)
        {
            var result = await _postService.GetAllSubPostDetailsAsync(postId);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("/admin/post/addgalleryimage")]
        public async Task<IActionResult> AddGalleryImage(int postId, List<int> galleryIds)
        {
            var result = await _postService.GalleryImageAddAsync(postId, galleryIds);
            return new JsonResult(result.Data);
        }

        [HttpPost]
        [Route("/admin/post/edittoppost")]
        public async Task<IActionResult> EditTopPost(int postId, int parentId)
        {
            var result = await _postService.TopPostUpdateAsync(postId, parentId);
            return new JsonResult(result.Data);
        }

        [HttpPost]
        [Route("/admin/post/editsubpost")]
        public async Task<IActionResult> EditSubPost(int postId, List<int> subPostIds)
        {
            var result = await _postService.SubPostUpdateAsync(postId, subPostIds);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("/admin/post/editsubpostdetail")]
        public async Task<IActionResult> EditSubPostDetail(int postId, string description, int? featuredImageId)
        {
            var result = await _postService.SubPostDetailUpdateAsync(postId, description, featuredImageId);
            return new JsonResult(result.Data);
        }

        [HttpPost]
        [Route("/admin/post/multipublish")]
        public async Task<JsonResult> MultiPublish(List<int> postIds, SubObjectType postType)
        {
            var result = await _postService.MultiPublishAsync(postIds, LoggedInUser.Id);
            var publishPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.trash);

            //await _fileHelper.CreateSitemapInRootDirectoryAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var postViewModelJson = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return Json(postViewModelJson);
            }
            else
            {
                var postViewModelJsonError = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return Json(postViewModelJsonError);
            }
        }

        [HttpPost]
        [Route("/admin/post/multidraft")]
        public async Task<JsonResult> MultiDraft(List<int> postIds, SubObjectType postType)
        {
            var result = await _postService.MultiDraftAsync(postIds, LoggedInUser.Id);
            var publishPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.trash);

            //await _fileHelper.CreateSitemapInRootDirectoryAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var postViewModelJson = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return Json(postViewModelJson);
            }
            else
            {
                var postViewModelJsonError = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return Json(postViewModelJsonError);
            }
        }

        [HttpPost]
        [Route("/admin/post/trash")]
        public async Task<JsonResult> Trash(int postId)
        {
            var result = await _postService.TrashAsync(postId, LoggedInUser.Id);
            var publishPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.trash);
            if (result.ResultStatus == ResultStatus.Success)
            {
                //    _cacheService.Clear();
                //    await _fileHelper.CreateSitemapInRootDirectoryAsync();
                var postViewModel = new PostViewModel
                {
                    PostDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return Json(postViewModel);
            }
            else
            {
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
        }

        [HttpPost]
        [Route("/admin/post/multitrash")]
        public async Task<JsonResult> MultiTrash(List<int> postIds, SubObjectType postType)
        {
            var result = await _postService.MultiTrashAsync(postIds, LoggedInUser.Id);
            var publishPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.trash);

            //await _fileHelper.CreateSitemapInRootDirectoryAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
            else
            {
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
        }

        [HttpPost]
        [Route("/admin/post/untrash")]
        public async Task<JsonResult> UnTrash(int postId)
        {
            var result = await _postService.UnTrashAsync(postId, LoggedInUser.Id);
            var publishPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.trash);
            if (result.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
            else
            {
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
        }

        [HttpPost]
        [Route("/admin/post/multiuntrash")]
        public async Task<JsonResult> MultiUnTrash(List<int> postIds, SubObjectType postType)
        {
            var result = await _postService.MultiUnTrashAsync(postIds, LoggedInUser.Id);
            var publishPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.trash);
            if (result.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
            else
            {
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
        }

        [HttpPost]
        [Route("/admin/post/delete")]
        public async Task<JsonResult> Delete(int postId)
        {
            var result = await _postService.DeleteAsync(postId);
            var publishPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(result.Data.Post.PostType, PostStatus.trash);
            if (result.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
            else
            {
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
        }

        [HttpPost]
        [Route("/admin/post/multidelete")]
        public async Task<JsonResult> MultiDelete(List<int> postIds, SubObjectType postType)
        {
            var result = await _postService.MultiDeleteAsync(postIds);
            var publishPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(postType, PostStatus.trash);
            if (result.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
            else
            {
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                });
                return Json(postViewModel);
            }
        }
    }
}