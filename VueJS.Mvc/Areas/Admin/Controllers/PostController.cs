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

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : BaseController
    {
        private readonly IPostService _postService;
        private readonly ITermService _termService;
        private readonly ISeoService _seoService;
        //private readonly IFileHelper _fileHelper;
        //private readonly ICacheService _cacheService;

        public PostController(IPostService postService, ITermService termService, ISeoService seoService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
        {
            _postService = postService;
            _termService = termService;
            _seoService = seoService;
        }

        #region Post

        [Authorize(Roles = "SuperAdmin,Post.Read")]
        [HttpGet]
        [Route("admin/post")]
        public async Task<IActionResult> Index(SubObjectType post_type, PostStatus? post_status)
        {
            var result = await _postService.GetAllAsync(post_type, post_status);
            var publishPostsCount = await _postService.PublishStatusCountAsync(post_type, PostStatus.publish);
            var draftPostsCount = await _postService.PublishStatusCountAsync(post_type, PostStatus.draft);
            var trashPostsCount = await _postService.PublishStatusCountAsync(post_type, PostStatus.trash);
            if (result.ResultStatus == ResultStatus.Success)
            {
                var postListViewModel = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return View(postListViewModel);
            }
            else
            {
                var postErrorListViewModel = new PostViewModel
                {
                    PostListDto = result.Data,
                    PublishPostsCount = publishPostsCount,
                    DraftPostsCount = draftPostsCount,
                    TrashPostsCount = trashPostsCount
                };
                return View(postErrorListViewModel);
            }
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
        [Route("admin/post/allstatusedposts")]
        public async Task<JsonResult> AllPostStatusedPosts(SubObjectType post_type, PostStatus post_status)
        {
            var posts = await _postService.GetAllPostStatusAsync(post_type, post_status);
            return new JsonResult(posts);
        }

        [Authorize(Roles = "SuperAdmin,Post.Read")]
        [HttpGet]
        [Route("admin/post/getalltopposts")]
        public async Task<JsonResult> GetAllTopPosts(int? postId, string search)
        {
            var result = await _postService.GetAllAnotherPostsAsync(SubObjectType.page, postId, search);

            var topPost = JsonConvert.SerializeObject(result.Data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(topPost);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        [Route("admin/post/getallsubposts")]
        public async Task<JsonResult> GetAllSubPosts(int? postId, string search)
        {
            var result = await _postService.GetAllSubPostsAsync(SubObjectType.page, postId, search);

            var subPost = JsonConvert.SerializeObject(result.Data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(subPost);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        [Route("admin/post/getallsubpostsDetail")]
        public async Task<JsonResult> GetAllSubPostsDetail(int postId)
        {
            var subPostsResult = await _postService.GetAllSubPostDetailsAsync(postId);
            var subPost = JsonConvert.SerializeObject(subPostsResult.Data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(subPost);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        [HttpGet]
        [Route("admin/post/getsubpostDetail")]
        public async Task<JsonResult> GetSubPostDetail(int postId)
        {
            var subPostsResult = await _postService.GetAllSubPostDetailsAsync(postId);
            var subPost = JsonConvert.SerializeObject(subPostsResult.Data, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(subPost);
        }

        [Authorize(Roles = "SuperAdmin,Post.Create")]
        [HttpGet]
        [Route("admin/post/new")]
        public IActionResult New(SubObjectType post_type)
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin,Post.Create")]
        [HttpPost]
        [Route("admin/post/new")]
        public async Task<IActionResult> New(PostViewModel postAddViewModel, SubObjectType post_type)
        {
            var postResult = await _postService.AddAsync(postAddViewModel.PostAddDto, LoggedInUser.Id, post_type);
            if (postResult.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                var seoResult = await _seoService.SeoObjectSettingAddAsync(ObjectType.post, post_type, postResult.Data.Post.Id, postAddViewModel.SeoObjectSettingAddDto, LoggedInUser.Id);
                //await _fileHelper.CreateSitemapInRootDirectoryAsync();
                var postAddAjaxViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostDto = postResult.Data,
                    SeoObjectSettingDto = seoResult.Data

                });
                return Json(postAddAjaxViewModel);
            }
            else
            {
                var postAddAjaxErrorViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostDto = postResult.Data
                });
                return Json(postAddAjaxErrorViewModel);
            }
        }

        public async Task<IActionResult> AddGalleryImage(int postId, List<int> galleryIds)
        {
            var result = await _postService.GalleryImageAddAsync(postId, galleryIds);
            var postResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(postResult);
        }

        [Authorize(Roles = "SuperAdmin,Post.Update")]
        [HttpGet]
        [Route("admin/post/edit")]
        public async Task<IActionResult> Edit(int post)
        {
            var postResult = await _postService.GetPostUpdateDtoAsync(post);
            if (postResult.ResultStatus == ResultStatus.Success)
            {
                if (postResult.Data.PostType == SubObjectType.page)
                {
                    ViewBag.Title = "Sayfayı Düzenle > " + postResult.Data.Title;
                }
                else if (postResult.Data.PostType == SubObjectType.article)
                {
                    ViewBag.Title = "Makaleyi Düzenle > " + postResult.Data.Title;
                }
                else if (postResult.Data.PostType == SubObjectType.basepage)
                {
                    ViewBag.Title = "Temel Sayfayı Düzenle > " + postResult.Data.Title;
                }
            }
            return View();
        }

        [HttpGet]
        [Route("admin/post/getajaxedit")]
        public async Task<JsonResult> GetAjaxEdit(int post)
        {
            var postResult = await _postService.GetPostUpdateDtoAsync(post);

            if (postResult.ResultStatus == ResultStatus.Success)
            {
                var seoResult = await _seoService.GetSeoObjectSettingUpdateDtoAsync(post, postResult.Data.PostType);
                var postViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostUpdateDto = postResult.Data,
                    SeoObjectSettingUpdateDto = seoResult.Data
                }, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(postViewModel);
            }
            else
            {
                var postErrorViewModel = JsonConvert.SerializeObject(new PostViewModel
                {
                    PostUpdateDto = postResult.Data
                });
                return Json(postErrorViewModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Product.Update")]
        [HttpPost]
        [Route("admin/post/edit")]
        public async Task<IActionResult> Edit(PostViewModel postViewModel)
        {
            var postResult = await _postService.UpdateAsync(postViewModel.PostUpdateDto, LoggedInUser.Id);
            var seoResult = await _seoService.SeoObjectSettingUpdateAsync(postViewModel.PostUpdateDto.Id, postViewModel.PostUpdateDto.PostType, postViewModel.SeoObjectSettingUpdateDto, LoggedInUser.Id);
            if (postResult.ResultStatus == ResultStatus.Success && seoResult.ResultStatus == ResultStatus.Success)
            {
                //_cacheService.Clear();
                //await _fileHelper.CreateSitemapInRootDirectoryAsync();
                var result = new { postResult, seoResult };
                var postUpdateAjaxViewModel = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(postUpdateAjaxViewModel);
            }
            else
            {
                var result = new { postResult, seoResult };
                var postUpdateAjaxErrorViewModel = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return Json(postUpdateAjaxErrorViewModel);
            }
        }

        [Route("admin/post/editpostterm")]
        public async Task<IActionResult> EditPostTerm(int postId, SubObjectType termType, List<int> termIds)
        {
            var result = await _termService.PostTermUpdateAsync(postId, termType, termIds);
            var postTermResult = JsonConvert.SerializeObject(result);
            return Json(postTermResult);
        }

        [Route("admin/post/edittoppost")]
        public async Task<IActionResult> EditTopPost(int postId, int parentId)
        {
            var result = await _postService.TopPostUpdateAsync(postId, parentId);
            var postResult = JsonConvert.SerializeObject(result);
            return Json(postResult);
        }

        [Route("admin/post/editsubpost")]
        public async Task<IActionResult> EditSubPost(int postId, List<int> subPostIds)
        {
            var result = await _postService.SubPostUpdateAsync(postId, subPostIds);
            var postResult = JsonConvert.SerializeObject(result);
            return Json(postResult);
        }

        [Route("admin/post/editsubpostdetail")]
        public async Task<IActionResult> EditSubPostDetail(int postId, string description, int? featuredImageId)
        {
            var result = await _postService.SubPostDetailUpdateAsync(postId, description, featuredImageId);
            var postResult = JsonConvert.SerializeObject(result);
            return Json(postResult);
        }

        [Authorize(Roles = "SuperAdmin,Post.Edit")]
        [HttpPost]
        [Route("admin/post/multipublish")]
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

        [Authorize(Roles = "SuperAdmin,Post.Edit")]
        [HttpPost]
        [Route("admin/post/multidraft")]
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

        [Authorize(Roles = "SuperAdmin,Post.Delete")]
        [HttpPost]
        [Route("admin/post/trash")]
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

        [Authorize(Roles = "SuperAdmin,Post.Delete")]
        [HttpPost]
        [Route("admin/post/multitrash")]
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

        [Authorize(Roles = "SuperAdmin,Post.Update")]
        [HttpPost]
        [Route("admin/post/untrash")]
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

        [Authorize(Roles = "SuperAdmin,Post.Update")]
        [HttpPost]
        [Route("admin/post/multiuntrash")]
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

        [Authorize(Roles = "SuperAdmin,Post.Delete")]
        [HttpPost]
        [Route("admin/post/delete")]
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

        [Authorize(Roles = "SuperAdmin,Post.Delete")]
        [HttpPost]
        [Route("admin/post/multidelete")]
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

        #endregion
    }
}