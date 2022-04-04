using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VueJS.Data.Abstract;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Services.Utilities;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueJS.Services.Helper.Abstract;

namespace VueJS.Services.Concrete
{
    public class PostManager : ManagerBase, IPostService
    {
        public PostManager(IUnitOfWork unitOfWork, IMapper mapper, IExtensionsHelper extensionsHelper) : base(unitOfWork, mapper, extensionsHelper)
        {
        }

        #region AP

        public async Task<IDataResult<PostDto>> GetAsync(int postId, ObjectType postType)
        {
            var post = await UnitOfWork.Posts.GetAsync(p => p.Id == postId && p.PostType == postType, p => p.Parent, p => p.Children, p => p.Galleries, p => p.User);
            if (post == null) return new DataResult<PostDto>(ResultStatus.Error, Messages.NotFound(postType, false), null);

            if (postType == ObjectType.article)
            {
                post.PostTerms = await UnitOfWork.PostTerms.GetAllAsync(p => p.PostId == postId);
            }
            return new DataResult<PostDto>(ResultStatus.Success, new PostDto { Post = post });
        }

        public async Task<IDataResult<PostDto>> GetAsync(string postName)
        {
            var postResult = await UnitOfWork.Posts.GetAsync(a => a.PostName == postName);

            if (postResult == null) return new DataResult<PostDto>(ResultStatus.Error, Messages.NotFound(postResult.PostType, false), null);

            Post post = null;
            if (postResult.PostType == ObjectType.article)
            {
                post = await UnitOfWork.Posts.GetIncludeAsync(p => p.PostName == postName, p => p
           .Include(p => p.PostTerms)
           .Include(p => p.FeaturedImage)
           .Include(p => p.Comments.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.Children.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.User)
           .Include(p => p.Comments.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.Children.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.Children.Where(c => c.CommentStatus == CommentStatus.approved))
           .Include(p => p.Comments.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.User)
           .Include(p => p.User));
            }
            else if (postResult.PostType == ObjectType.page || postResult.PostType == ObjectType.basepage)
            {
                post = await UnitOfWork.Posts.GetIncludeAsync(p => p.PostName == postName, p => p
           .Include(p => p.FeaturedImage)
           .Include(p => p.Parent).ThenInclude(p => p.User)
           .Include(p => p.Children).ThenInclude(p => p.User)
           .Include(p => p.User));
            }

            return new DataResult<PostDto>(ResultStatus.Success, new PostDto { Post = post });
        }

        public async Task<IDataResult<PostUpdateDto>> GetBasePageUpdateDtoAsync(string postName)
        {
            var basePage = await UnitOfWork.Posts.GetAsync(p => p.PostName == postName && p.PostType == ObjectType.basepage);
            var basePageUpdateDto = Mapper.Map<PostUpdateDto>(basePage);
            return new DataResult<PostUpdateDto>(ResultStatus.Success, basePageUpdateDto);
        }

        public async Task<IDataResult<PostListDto>> GetAllAsync(int? userId, string category, string tag, ObjectType postType, PostStatus? postStatus)
        {
            if (userId != null)
            {
                var posts = postType == ObjectType.page
            ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.UserId == userId.Value && p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
            .Include(x => x.Parents).ThenInclude(x => x.Parent)
            .Include(x => x.Parent)
            .Include(x => x.Children).ThenInclude(x => x.FeaturedImage)
            .Include(x => x.FeaturedImage)
            .Include(x => x.User))
            : postType == ObjectType.article
            ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.UserId == userId.Value && p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
            .Include(x => x.PostTerms).ThenInclude(x => x.Term)
            .Include(x => x.FeaturedImage)
            .Include(x => x.Comments)
            .Include(x => x.User))
            : postType == ObjectType.basepage
            ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.UserId == userId.Value && p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
            .Include(x => x.FeaturedImage)
            .Include(x => x.User)) :
            null;
                return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
            }
            else if (!string.IsNullOrWhiteSpace(category))
            {
                IList<Post> posts = new List<Post>(); 
                var postTerms = await UnitOfWork.PostTerms.GetAllAsync(pt => pt.Term.Slug == category, pt => pt.Post);
                foreach (var postTerm in postTerms)
                {
                    var post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postTerm.Post.Id, p => p
            .Include(x => x.PostTerms).ThenInclude(x => x.Term)
            .Include(x => x.FeaturedImage)
            .Include(x => x.Comments)
            .Include(x => x.User));
                    posts.Add(post);
                }
                return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
            }
            else if (!string.IsNullOrWhiteSpace(tag))
            {
                IList<Post> posts = new List<Post>();
                var postTerms = await UnitOfWork.PostTerms.GetAllAsync(pt => pt.Term.Slug == tag, pt => pt.Post);

                foreach (var postTerm in postTerms)
                {
                    var post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postTerm.Post.Id, p => p
           .Include(x => x.PostTerms).ThenInclude(x => x.Term)
           .Include(x => x.FeaturedImage)
           .Include(x => x.Comments)
           .Include(x => x.User));
                    posts.Add(post);
                }
                return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
            }
            else
            {
                if (postStatus == null)
                {
                    var posts = postType == ObjectType.page
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
                .Include(x => x.Parents).ThenInclude(x => x.Parent)
                .Include(x => x.Parent)
                .Include(x => x.Children).ThenInclude(x => x.FeaturedImage)
                .Include(x => x.FeaturedImage)
                .Include(x => x.User))
                : postType == ObjectType.article
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
                .Include(x => x.PostTerms).ThenInclude(x => x.Term)
                .Include(x => x.FeaturedImage)
                .Include(x => x.Comments)
                .Include(x => x.User))
                : postType == ObjectType.basepage
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
                .Include(x => x.FeaturedImage)
                .Include(x => x.User)) :
                null;
                    return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
                }
                else
                {
                    var posts = postType == ObjectType.page
                    ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p
                    .Include(x => x.Parents).ThenInclude(x => x.Parent)
                    .Include(x => x.Parent)
                    .Include(x => x.Children).ThenInclude(x => x.FeaturedImage)
                    .Include(x => x.FeaturedImage)
                    .Include(x => x.User))
                    : postType == ObjectType.article
                    ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p
                    .Include(x => x.PostTerms).ThenInclude(x => x.Term)
                    .Include(x => x.FeaturedImage)
                    .Include(x => x.Comments)
                    .Include(x => x.User))
                    : postType == ObjectType.basepage
                    ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p
                    .Include(x => x.FeaturedImage)
                    .Include(x => x.User))
                    : null;
                    return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
                }
            }
        }

        public async Task<IDataResult<PostListDto>> GetAllPostStatusAsync(ObjectType postType, PostStatus postStatus)
        {
            var posts = postType == ObjectType.page
                ? await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p.Parent, p => p.Children, p => p.FeaturedImage, p => p.Galleries, p => p.User)
                : postType == ObjectType.article
                ? await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p.FeaturedImage, p => p.PostTerms, p => p.Comments, p => p.User)
                : null;

            if (posts.Count == 0) return new DataResult<PostListDto>(ResultStatus.Error, Messages.NotFound(postType, true), null);
            return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
        }

        public async Task<IDataResult<PostListDto>> GetAllTopPostsAsync(int? postId)
        {
            IList<Post> posts = null;

            if (postId == null)
            {
                posts = await UnitOfWork.Posts.GetAllAsync(p => p.PostType == ObjectType.page);
            }
            else
            {
                var currentPost = await UnitOfWork.Posts.GetAsync(p => p.Id == postId.Value, p => p.Children);
                if (currentPost != null && currentPost.Children != null)
                {
                    List<int> childIds = new List<int>();
                    foreach (var child in currentPost.Children)
                    {
                        childIds.Add(child.Id);
                    }
                    posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == ObjectType.page && c.Id != postId.Value && !childIds.Contains(c.Id));
                }
                else
                {
                    posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == ObjectType.page && c.Id != postId.Value);
                }
            }

            if (posts == null) return new DataResult<PostListDto>(ResultStatus.Error, Messages.NotFound(ObjectType.page, true), null);
            return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
        }

        public async Task<IDataResult<PostListDto>> GetAllSubPostsAsync(int? postId)
        {
            IList<Post> posts = null;

            if (postId == null)
            {
                posts = await UnitOfWork.Posts.GetAllAsync(p => p.PostType == ObjectType.page);
            }
            else
            {
                var currentPost = await UnitOfWork.Posts.GetAsync(p => p.Id == postId.Value, p => p.Parent);
                if (currentPost != null && currentPost.Parent != null)
                {
                    Post postParent = (Post)await ExtensionsHelper.GetParentsAsync(ObjectType.page, currentPost);
                    currentPost.Parents = postParent.Parents;

                    List<int> parentIds = new List<int>();
                    foreach (var par in currentPost.Parents)
                    {
                        parentIds.Add(par.Id);
                    }
                    posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == ObjectType.page && c.Id != postId.Value && !parentIds.Contains(c.Id));
                }
                else
                {
                    posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == ObjectType.page && c.Id != postId.Value);
                }
            }

            if (posts == null) return new DataResult<PostListDto>(ResultStatus.Error, Messages.NotFound(ObjectType.page, true), null);
            return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
        }

        public Task<IDataResult<PostListDto>> GetAllByPagingAsync(string categoryGuid, string tagGuid, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<PostListDto>> GetAllByTagArticlesAsync(int tagId, int currentPage = 1)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<PostListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, int tagId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<PostListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<PostListDto>> GetAllSubPostDetailsAsync(int postId)
        {
            var posts = await UnitOfWork.Posts.GetAllIncludeAsync(p => p.ParentId == postId, p => p
                        .Include(x => x.FeaturedImage));
            if (posts.Count < 1) return new DataResult<PostListDto>(ResultStatus.Error, Messages.NotFound(ObjectType.page, true), null);
            return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto { Posts = posts });
        }

        public Task<IDataResult<PostDto>> GetByIdAsync(int postId, bool includeCategories, bool includeTags, bool includeComments, bool includeGalleries, bool includeUser)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<PostUpdateDto>> GetPostUpdateDtoAsync(int postId)
        {
            var postResult = await UnitOfWork.Posts.GetAsync(p => p.Id == postId);
            if (postResult == null) return new DataResult<PostUpdateDto>(ResultStatus.Error, null);

            Post post = null;
            switch (postResult.PostType)
            {
                case ObjectType.page:
                    post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postId && p.PostType == ObjectType.page, p => p
                            .Include(x => x.Parent)
                            .Include(x => x.Children).ThenInclude(x => x.FeaturedImage)
                            .Include(x => x.Children).ThenInclude(x => x.Children).ThenInclude(x => x.Children)
                            .Include(x => x.FeaturedImage)
                            .Include(x => x.Galleries).ThenInclude(x => x.Upload)
                            .Include(x => x.User));

                    Post postParent = (Post)await ExtensionsHelper.GetParentsAsync(postResult.PostType, post);
                    post.Parents = postParent.Parents;
                    break;
                case ObjectType.article:
                    post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postId && p.PostType == ObjectType.article, p => p
                            .Include(x => x.PostTerms).ThenInclude(x => x.Term)
                            .Include(x => x.FeaturedImage)
                            .Include(x => x.User));
                    break;
                case ObjectType.basepage:
                    post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postId && p.PostType == ObjectType.basepage, p => p
                            .Include(x => x.FeaturedImage)
                            .Include(x => x.User));
                    break;
            }
            var postUpdateDto = Mapper.Map<PostUpdateDto>(post);
            return new DataResult<PostUpdateDto>(ResultStatus.Success, postUpdateDto);
        }

        public Task<IDataResult<PostListDto>> GetSubPostUpdateDtoAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<PostDto>> AddAsync(PostAddDto postAddDto, int userId)
        {
            string postName = postAddDto.PostName == null ? ExtensionsHelper.FriendlySEOString(postAddDto.Title) : postAddDto.PostName;
            var postNameCheck = await UnitOfWork.Posts.GetAllAsync(p => p.PostName == postName);
            if (postNameCheck.Count != 0) return new DataResult<PostDto>(ResultStatus.Error, Messages.UrlCheck(postAddDto.PostType), null);

            var post = Mapper.Map<Post>(postAddDto);
            post.UserId = userId;
            post.PostName = postName;
            post.CommentCount = 0;
            await UnitOfWork.Posts.AddAsync(post);
            await UnitOfWork.SaveAsync();
            return new DataResult<PostDto>(ResultStatus.Success, Messages.Add(post.PostType, post.Title), new PostDto { Post = post });
        }

        public async Task<IDataResult<PostDto>> UpdateAsync(PostUpdateDto postUpdateDto, int userId)
        {
            var oldPost = await UnitOfWork.Posts.GetAsync(p => p.Id == postUpdateDto.Id);
            if (oldPost == null) return new DataResult<PostDto>(ResultStatus.Error, Messages.NotFound(postUpdateDto.PostType, false), null);

            var posts = await UnitOfWork.Posts.GetAllAsync(p => p.PostName == oldPost.PostName || p.PostName == postUpdateDto.PostName);
            if (posts.Count >= 1 && posts.Count <= 0) return new DataResult<PostDto>(ResultStatus.Error, Messages.UrlCheck(postUpdateDto.PostType), null);

            var post = Mapper.Map<PostUpdateDto, Post>(postUpdateDto, oldPost);
            post.UserId = userId;

            var menuDetails = await UnitOfWork.MenuDetails.GetAllAsync(md => md.ObjectId == post.Id && md.ObjectType == post.PostType);

            if (menuDetails.Count > 0)
            {
                foreach (var menuDetail in menuDetails)
                {
                    menuDetail.CustomURL = await ExtensionsHelper.GetParentsURLAsync(post.PostType, post);
                    await UnitOfWork.MenuDetails.UpdateAsync(menuDetail);
                }
            }


            await UnitOfWork.Posts.UpdateAsync(post);
            await UnitOfWork.SaveAsync();
            return new DataResult<PostDto>(ResultStatus.Success, Messages.Update(postUpdateDto.PostType, post.Title), new PostDto { Post = post });
        }

        public async Task<IDataResult<PostDto>> PostStatusChangeAsync(int postId, PostStatus postStatus, int userId)
        {
            var post = await UnitOfWork.Posts.GetAsync(p => p.Id == postId);
            if (post == null) return new DataResult<PostDto>(ResultStatus.Error, Messages.NotFound(post.PostType, false), null);
            if (post.PostType == ObjectType.basepage && postStatus == PostStatus.trash) return new DataResult<PostDto>(ResultStatus.Error, Messages.BasePage.NoDelete(), null);

            post.PostStatus = postStatus;
            post.UserId = userId;
            await UnitOfWork.Posts.UpdateAsync(post);
            await UnitOfWork.SaveAsync();

            return new DataResult<PostDto>(ResultStatus.Success, Messages.StatusChange(post.PostType, postStatus, post.Title), new PostDto { Post = post });
        }

        public async Task<IDataResult<PostDto>> DeleteAsync(int postId)
        {
            var post = await UnitOfWork.Posts.GetAsync(p => p.Id == postId && p.PostStatus == PostStatus.trash);
            if (post == null) return new DataResult<PostDto>(ResultStatus.Error, Messages.NotFound(post.PostType, false), null);
            if (post.PostType == ObjectType.basepage) return new DataResult<PostDto>(ResultStatus.Error, Messages.BasePage.NoDelete(), null);

            if (post.Galleries != null)
            {
                var galleries = post.Galleries.Where(pg => pg.PostId == postId).ToList();
                foreach (var gallery in galleries)
                {
                    await UnitOfWork.Galleries.DeleteAsync(gallery);
                }
            }

            await UnitOfWork.Posts.DeleteAsync(post);
            await UnitOfWork.SaveAsync();
            return new DataResult<PostDto>(ResultStatus.Success, Messages.Delete(post.PostType, post.Title), new PostDto { Post = post });
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var postCount = await UnitOfWork.Posts.CountAsync(null);
            if (postCount < 0) return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            return new DataResult<int>(ResultStatus.Success, postCount);
        }

        public Task<IDataResult<PostDto>> GalleryImageAddAsync(int postId, List<int> galleryIds)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> IncreaseViewCountAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<PostListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<PostDto>> SubPostDetailUpdateAsync(int postId, string description, int? featuredImageId)
        {
            var subPost = await UnitOfWork.Posts.GetAsync(x => x.Id == postId);

            if (subPost == null) return new DataResult<PostDto>(ResultStatus.Error, "Hata oluştu. Sayfayı yenileyip tekrar deneyiniz.", null);

            subPost.Description = description;
            subPost.FeaturedImageId = featuredImageId;
            await UnitOfWork.Posts.UpdateAsync(subPost);
            await UnitOfWork.SaveAsync();
            return new DataResult<PostDto>(ResultStatus.Success, "Alt sayfa güncellendi.", new PostDto { Post = subPost });
        }

        public async Task<IDataResult<PostDto>> TopPostUpdateAsync(int postId, int parentId)
        {
            var post = await UnitOfWork.Posts.GetAsync(x => x.Id == postId);
            if (post == null) return new DataResult<PostDto>(ResultStatus.Error, null);

            post.ParentId = parentId == 0 ? 0 : parentId;
            await UnitOfWork.Posts.UpdateAsync(post);
            await UnitOfWork.SaveAsync();
            return new DataResult<PostDto>(ResultStatus.Success, "Ebeveyn sayfa güncellendi", new PostDto { Post = post });
        }

        public async Task<IResult> SubPostUpdateAsync(int subPostId, int? subPostParentId)
        {
            var subPost = await UnitOfWork.Posts.GetAsync(x => x.Id == subPostId);
            if (subPost == null) return new Result(ResultStatus.Error, "Alt sayfa güncellenemedi");

            if (subPostParentId == null)
            {
                subPost.ParentId = null;
            }
            else
            {
                subPost.ParentId = subPostParentId.Value;
            }
            await UnitOfWork.Posts.UpdateAsync(subPost);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, "Alt sayfa güncellendi");
        }

        #endregion

        #region WEB

        public async Task<IDataResult<PostListDto>> GetAllByTermArticlesAsync(int termId, int currentPage = 1)
        {

            List<Post> posts = new List<Post>();
            var postTerms = await UnitOfWork.PostTerms.GetAllAsync(pt => pt.TermId == termId, pt => pt.Post, pt => pt.Term);

            foreach (var postTerm in postTerms)
            {
                var post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postTerm.PostId && p.PostStatus == PostStatus.publish, p => p
                .Include(p => p.PostTerms).ThenInclude(p => p.Term).ThenInclude(p => p.Parent)
                .Include(p => p.FeaturedImage)
                .Include(p => p.User));
                posts.Add(post);

                foreach (var pterm in post.PostTerms)
                {
                    Term term = (Term)await ExtensionsHelper.GetParentsAsync(null, pterm.Term);
                    pterm.Term.Parents = term.Parents;
                }
            }

            if (posts.Count > -1)
            {
                return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto
                {
                    Posts = posts,
                    CurrentPage = currentPage,
                    TotalCount = posts.Count,
                });
            }
            else
            {
                return new DataResult<PostListDto>(ResultStatus.Error, new PostListDto
                {
                    Posts = null,
                    CurrentPage = currentPage,
                    TotalCount = posts.Count,
                });
            }
        }

        #endregion
    }
}