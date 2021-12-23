using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VueJS.Data.Abstract;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Services.Extensions;
using VueJS.Services.Utilities;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using VueJS.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueJS.Services.Concrete
{
    public class PostManager : ManagerBase, IPostService
    {
        #region AP
        public PostManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IDataResult<PostDto>> GetAsync(int postId)
        {
            var page = await UnitOfWork.Posts.GetAsync(p => p.Id == postId && p.PostType == SubObjectType.page, p => p.Parent, p => p.Children, p => p.Galleries, p => p.User);
            if (page != null)
            {
                return new DataResult<PostDto>(ResultStatus.Success, new PostDto
                {
                    ResultStatus = ResultStatus.Success,
                    Post = page
                });
            }

            var article = await UnitOfWork.Posts.GetAsync(a => a.Id == postId && a.PostType == SubObjectType.article, a => a.PostTerms, a => a.Comments, a => a.User);
            if (article != null)
            {
                article.PostTerms = await UnitOfWork.PostTerms.GetAllAsync(p => p.PostId == postId);
                return new DataResult<PostDto>(ResultStatus.Success, new PostDto
                {
                    ResultStatus = ResultStatus.Success,
                    Post = article
                });
            }
            return new DataResult<PostDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), null);
        }

        public async Task<IDataResult<PostDto>> GetAsync(string postName)
        {
            var postResult = await UnitOfWork.Posts.GetAsync(a => a.PostName == postName);

            if (postResult != null)
            {
                Post post = null;
                if (postResult.PostType == SubObjectType.article)
                {
                    post = await UnitOfWork.Posts.GetIncludeAsync(p => p.PostName == postName, p => p
               .Include(p => p.PostTerms)
               .Include(p => p.FeaturedImage)
               .Include(p => p.Comments.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.Children.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.User)
               .Include(p => p.Comments.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.Children.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.Children.Where(c => c.CommentStatus == CommentStatus.approved))
               .Include(p => p.Comments.Where(c => c.CommentStatus == CommentStatus.approved)).ThenInclude(c => c.User)
               .Include(p => p.User));
                }
                else if (postResult.PostType == SubObjectType.page || postResult.PostType == SubObjectType.basepage)
                {
                    post = await UnitOfWork.Posts.GetIncludeAsync(p => p.PostName == postName, p => p
               .Include(p => p.FeaturedImage)
               .Include(p => p.Parent).ThenInclude(p => p.User)
               .Include(p => p.Children).ThenInclude(p => p.User)
               .Include(p => p.User));
                }

                return new DataResult<PostDto>(ResultStatus.Success, new PostDto
                {
                    ResultStatus = ResultStatus.Success,
                    Post = post
                });
            }
            return new DataResult<PostDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), null);
        }

        public async Task<IDataResult<PostUpdateDto>> GetBasePageUpdateDtoAsync(string postName)
        {
            var basePage = await UnitOfWork.Posts.GetAsync(p => p.PostName == postName && p.PostType == SubObjectType.basepage);
            var basePageUpdateDto = Mapper.Map<PostUpdateDto>(basePage);
            return new DataResult<PostUpdateDto>(ResultStatus.Success, basePageUpdateDto);
        }

        public async Task<IDataResult<PostListDto>> GetAllAsync(SubObjectType postType, PostStatus? postStatus)
        {
            if (postStatus == null)
            {
                var posts = postType == SubObjectType.page
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
                .Include(x => x.Parents).ThenInclude(x => x.Parent)
                .Include(x => x.Parent)
                .Include(x => x.Children).ThenInclude(x => x.FeaturedImage)
                .Include(x => x.FeaturedImage)
                .Include(x => x.Galleries).ThenInclude(x => x.Upload)
                .Include(x => x.User))
                : postType == SubObjectType.article
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
                .Include(x => x.PostTerms).ThenInclude(x => x.Term)
                .Include(x => x.FeaturedImage)
                .Include(x => x.Comments)
                .Include(x => x.User))
                : postType == SubObjectType.basepage
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus != PostStatus.trash, p => p
                .Include(x => x.FeaturedImage)
                .Include(x => x.User)) :
                null;

                return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto
                {
                    Posts = posts,
                    ResultStatus = ResultStatus.Success
                });
            }
            else
            {
                var posts = postType == SubObjectType.page
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p
                .Include(x => x.Parents).ThenInclude(x => x.Parent)
                .Include(x => x.Parent)
                .Include(x => x.Children).ThenInclude(x => x.FeaturedImage)
                .Include(x => x.FeaturedImage)
                .Include(x => x.Galleries).ThenInclude(x => x.Upload)
                .Include(x => x.User))
                : postType == SubObjectType.article
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p
                .Include(x => x.PostTerms).ThenInclude(x => x.Term)
                .Include(x => x.FeaturedImage)
                .Include(x => x.Comments)
                .Include(x => x.User))
                : postType == SubObjectType.basepage
                ? await UnitOfWork.Posts.GetAllIncludeAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p
                .Include(x => x.FeaturedImage)
                .Include(x => x.User))
                : null;

                return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto
                {
                    Posts = posts,
                    ResultStatus = ResultStatus.Success
                });
            }
        }

        public async Task<IDataResult<PostListDto>> GetAllPostStatusAsync(SubObjectType postType, PostStatus postStatus)
        {
            var posts = postType == SubObjectType.page
                ? await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p.Parent, p => p.Children, p => p.FeaturedImage, p => p.Galleries, p => p.User)
                : postType == SubObjectType.article
                ? await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p.FeaturedImage, p => p.PostTerms, p => p.Comments, p => p.User)
                : null;

            return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto
            {
                ResultStatus = ResultStatus.Success,
                Posts = posts
            });
        }

        public async Task<IDataResult<PostListDto>> GetAllAnotherPostsAsync(SubObjectType postType, int? postId, string search)
        {
            if (postType == SubObjectType.page)
            {
                IList<Post> posts = null;

                if (postId == null)
                {
                    posts = await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType);
                }
                else
                {
                    var currentPost = await UnitOfWork.Posts.GetAsync(p => p.Id == postId.Value, p => p.Children);
                    if (currentPost.Children != null)
                    {
                        List<int> childIds = new List<int>();
                        foreach (var child in currentPost.Children)
                        {
                            childIds.Add(child.Id);
                        }
                        posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == SubObjectType.page && c.Id != postId.Value && !childIds.Contains(c.Id));
                    }
                    else
                    {
                        posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == SubObjectType.page && c.Id != postId.Value);
                    }
                }

                if (!(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search)))
                {
                    posts = posts.Where(c => c.Title.ToLower().StartsWith(search.ToLower())).ToList();
                }

                if (posts != null)
                {
                    return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto
                    {
                        Posts = posts,
                        ResultStatus = ResultStatus.Success
                    });
                }
            }
            return new DataResult<PostListDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: true), new PostListDto
            {
                Posts = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Product.NotFound(isPlural: true)
            });
        }

        public async Task<IDataResult<PostListDto>> GetAllSubPostsAsync(SubObjectType postType, int? postId, string search)
        {
            if (postType == SubObjectType.page)
            {
                IList<Post> posts = null;

                if (postId == null)
                {
                    posts = await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType);
                }
                else
                {
                    var currentPost = await UnitOfWork.Posts.GetAsync(p => p.Id == postId.Value, p => p.Parent);
                    if (currentPost.Parent != null)
                    {
                        Post parent = currentPost.Parent;
                        currentPost.Parents = new List<Post>();
                        while (parent != null)
                        {
                            currentPost.Parents.Add(parent);
                            parent = await UnitOfWork.Posts.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                        }

                        List<int> parentIds = new List<int>();
                        foreach (var par in currentPost.Parents)
                        {
                            parentIds.Add(par.Id);
                        }
                        posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == SubObjectType.page && c.Id != postId.Value && !parentIds.Contains(c.Id));
                    }
                    else
                    {
                        posts = await UnitOfWork.Posts.GetAllAsync(c => c.PostType == SubObjectType.page && c.Id != postId.Value);
                    }
                }

                if (!(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search)))
                {
                    posts = posts.Where(c => c.Title.ToLower().StartsWith(search.ToLower())).ToList();
                }

                if (posts != null)
                {
                    return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto
                    {
                        Posts = posts,
                        ResultStatus = ResultStatus.Success
                    });
                }
            }

            return new DataResult<PostListDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: true), new PostListDto
            {
                Posts = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Product.NotFound(isPlural: true)
            });
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

            return new DataResult<PostListDto>(ResultStatus.Success, new PostListDto
            {
                Posts = posts,
                ResultStatus = ResultStatus.Success
            });
        }

        public Task<IDataResult<PostDto>> GetByIdAsync(int postId, bool includeCategories, bool includeTags, bool includeComments, bool includeGalleries, bool includeUser)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<PostUpdateDto>> GetPostUpdateDtoAsync(int postId)
        {
            var postResult = await UnitOfWork.Posts.GetAsync(p => p.Id == postId);
            if (postResult != null)
            {
                Post post = null;
                switch (postResult.PostType)
                {
                    case SubObjectType.page:
                        post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postId && p.PostType == SubObjectType.page, p => p
                                .Include(x => x.Parent)
                                .Include(x => x.Children).ThenInclude(x => x.FeaturedImage)
                                .Include(x => x.FeaturedImage)
                                .Include(x => x.Galleries).ThenInclude(x => x.Upload)
                                .Include(x => x.User));

                        Post parent = post.Parent;
                        post.Parents = new List<Post>();
                        while (parent != null)
                        {
                            post.Parents.Add(parent);
                            parent = await UnitOfWork.Posts.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                        }
                        break;
                    case SubObjectType.article:
                        post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postId && p.PostType == SubObjectType.article, p => p
                                .Include(x => x.PostTerms).ThenInclude(x => x.Term)
                                .Include(x => x.FeaturedImage)
                                .Include(x => x.User));
                        break;
                    case SubObjectType.basepage:
                        post = await UnitOfWork.Posts.GetIncludeAsync(p => p.Id == postId && p.PostType == SubObjectType.basepage, p => p
                                .Include(x => x.FeaturedImage)
                                .Include(x => x.User));
                        break;
                }
                var postUpdateDto = Mapper.Map<PostUpdateDto>(post);
                return new DataResult<PostUpdateDto>(ResultStatus.Success, postUpdateDto);
            }
            else
            {
                return new DataResult<PostUpdateDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), null);
            }
        }

        public Task<IDataResult<PostListDto>> GetSubPostUpdateDtoAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<PostDto>> AddAsync(PostAddDto postAddDto, int userId, SubObjectType postType)
        {
            string postName = UrlExtensions.FriendlySEOUrl(postAddDto.Title);
            var postNameCheck = await UnitOfWork.Posts.GetAllAsync(p => p.PostName == postName);

            if (postNameCheck.Count == 0)
            {
                if (postType == SubObjectType.page)
                {
                    var page = Mapper.Map<Post>(postAddDto);
                    page.PostStatus = PostStatus.publish;
                    page.CreatedDate = DateTime.Now;
                    page.ModifiedDate = DateTime.Now;
                    page.UserId = userId;
                    page.PostName = postName;
                    page.PostType = SubObjectType.page;
                    page.CommentCount = 0;

                    await UnitOfWork.Posts.AddAsync(page);
                    await UnitOfWork.SaveAsync();
                    return new DataResult<PostDto>(ResultStatus.Success, Messages.Post.Add(page.Title), new PostDto
                    {
                        Post = page,
                        ResultStatus = ResultStatus.Success,
                        Message = Messages.Post.Add(page.Title)
                    });
                }
                else if (postType == SubObjectType.article)
                {
                    var post = Mapper.Map<Post>(postAddDto);
                    post.UserId = userId;
                    post.PostName = postName;
                    post.PostType = SubObjectType.article;
                    post.CommentCount = 0;
                    await UnitOfWork.Posts.AddAsync(post);
                    await UnitOfWork.SaveAsync();
                    return new DataResult<PostDto>(ResultStatus.Success, Messages.Post.Add(post.Title), new PostDto
                    {
                        Post = post,
                        ResultStatus = ResultStatus.Success,
                        Message = Messages.Post.Add(post.Title)
                    });
                }
                else if (postType == SubObjectType.basepage)
                {
                    var post = Mapper.Map<Post>(postAddDto);
                    post.UserId = userId;
                    post.PostName = postName;
                    post.PostType = SubObjectType.basepage;
                    post.CommentCount = 0;
                    await UnitOfWork.Posts.AddAsync(post);
                    await UnitOfWork.SaveAsync();
                    return new DataResult<PostDto>(ResultStatus.Success, Messages.Post.Add(post.Title), new PostDto
                    {
                        Post = post,
                        ResultStatus = ResultStatus.Success,
                        Message = Messages.Post.Add(post.Title)
                    });
                }
            }
            return new DataResult<PostDto>(ResultStatus.Error, Messages.Post.TitleCheck(), new PostDto
            {
                Post = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Post.UrlCheck()
            });
        }

        public async Task<IDataResult<PostDto>> UpdateAsync(PostUpdateDto postUpdateDto, int userId)
        {
            var oldPost = await UnitOfWork.Posts.GetAsync(p => p.Id == postUpdateDto.Id);

            if (oldPost != null)
            {
                var posts = await UnitOfWork.Posts.GetAllAsync(p => oldPost.PostName == p.PostName || p.PostName == postUpdateDto.PostName);
                if (posts.Count == 1 || posts.Count == 0)
                {
                    var post = Mapper.Map<PostUpdateDto, Post>(postUpdateDto, oldPost);

                    post.UserId = userId;
                    post.ModifiedDate = DateTime.Now;
                    await UnitOfWork.Posts.UpdateAsync(post);
                    await UnitOfWork.SaveAsync();
                    return new DataResult<PostDto>(ResultStatus.Success, Messages.Post.Update(post.Title), new PostDto
                    {
                        Post = post,
                        ResultStatus = ResultStatus.Success,
                        Message = Messages.Post.Update(post.Title)
                    });
                }
                else
                {
                    return new DataResult<PostDto>(ResultStatus.Error, Messages.Post.UrlCheck(), new PostDto
                    {
                        Post = oldPost,
                        ResultStatus = ResultStatus.Error,
                        Message = Messages.Post.UrlCheck()
                    });
                }
            }
            else
            {
                return new DataResult<PostDto>(ResultStatus.Error, Messages.Post.NotFound(false), new PostDto
                {
                    Post = null,
                    ResultStatus = ResultStatus.Error,
                    Message = Messages.Post.NotFound(false)
                });
            }
        }

        public async Task<IResult> BasePageUpdateAsync(PostUpdateDto postUpdateDto, int userId)
        {
            var postNameCheck = await UnitOfWork.Posts.GetAllAsync(a => (a.PostName == postUpdateDto.PostName && a.Id == postUpdateDto.Id) || (a.PostName != postUpdateDto.PostName && a.Id == postUpdateDto.Id));

            if (postNameCheck.Count == 1)
            {
                Post oldPost = await UnitOfWork.Posts.GetAsync(p => p.Id == postUpdateDto.Id, p => p.FeaturedImage, p => p.User);
                var post = Mapper.Map<PostUpdateDto, Post>(postUpdateDto, oldPost);

                post.UserId = userId;
                post.ModifiedDate = DateTime.Now;
                await UnitOfWork.Posts.UpdateAsync(post);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, "Güncellendi");
            }
            else
            {
                return new Result(ResultStatus.Error, Messages.Post.UrlCheck());
            }
        }

        public async Task<IDataResult<PostListDto>> MultiPublishAsync(List<int> postIds, int userId)
        {
            var posts = await UnitOfWork.Posts.GetAllAsync(p => postIds.Contains(p.Id));
            if (posts.Count > -1)
            {
                List<Post> publishedPosts = new List<Post>();
                foreach (var post in posts)
                {
                    post.PostStatus = PostStatus.publish;
                    post.UserId = userId;
                    post.ModifiedDate = DateTime.Now;
                    publishedPosts.Add(post);
                }
                await UnitOfWork.Posts.MultiUpdateAsync(publishedPosts);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostListDto>(ResultStatus.Success, Messages.Post.MultiTrash(postIds.Count), new PostListDto
                {
                    Posts = posts
                });
            }
            return new DataResult<PostListDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), new PostListDto
            {
                Posts = null,
            });
        }

        public async Task<IDataResult<PostListDto>> MultiDraftAsync(List<int> postIds, int userId)
        {
            var posts = await UnitOfWork.Posts.GetAllAsync(p => postIds.Contains(p.Id));
            if (posts.Count > -1)
            {
                List<Post> draftedPosts = new List<Post>();
                foreach (var post in posts)
                {
                    post.PostStatus = PostStatus.draft;
                    post.UserId = userId;
                    post.ModifiedDate = DateTime.Now;
                    draftedPosts.Add(post);
                }
                await UnitOfWork.Posts.MultiUpdateAsync(draftedPosts);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostListDto>(ResultStatus.Success, Messages.Post.MultiTrash(postIds.Count), new PostListDto
                {
                    Posts = posts
                });
            }
            return new DataResult<PostListDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), new PostListDto
            {
                Posts = null,
            });
        }

        public async Task<IDataResult<PostDto>> TrashAsync(int postId, int userId)
        {
            var post = await UnitOfWork.Posts.GetAsync(p => p.Id == postId);
            if (post.PostType != SubObjectType.basepage)
            {
                post.PostStatus = PostStatus.trash;
                post.UserId = userId;
                post.ModifiedDate = DateTime.Now;
                await UnitOfWork.Posts.UpdateAsync(post);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostDto>(ResultStatus.Success, Messages.Post.Trash(post.Title), new PostDto
                {
                    Post = post,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Post.Trash(post.Title)
                });
            }
            else
            {
                return new DataResult<PostDto>(ResultStatus.Warning, Messages.Post.BasePage(), new PostDto
                {
                    Post = post,
                    ResultStatus = ResultStatus.Warning,
                    Message = Messages.Post.BasePage()
                });
            }
        }

        public async Task<IDataResult<PostListDto>> MultiTrashAsync(List<int> postIds, int userId)
        {
            var posts = await UnitOfWork.Posts.GetAllAsync(p => postIds.Contains(p.Id));
            if (posts.Count > -1)
            {
                List<Post> trashedPosts = new List<Post>();
                foreach (var post in posts)
                {
                    if (post.PostType != SubObjectType.basepage)
                    {
                        post.PostStatus = PostStatus.trash;
                        post.UserId = userId;
                        post.ModifiedDate = DateTime.Now;
                        trashedPosts.Add(post);
                    }
                    else
                    {
                        return new DataResult<PostListDto>(ResultStatus.Warning, Messages.Post.BasePage(), new PostListDto
                        {
                            Posts = posts,
                            ResultStatus = ResultStatus.Warning,
                            Message = Messages.Post.BasePage()
                        });
                    }
                }
                await UnitOfWork.Posts.MultiUpdateAsync(trashedPosts);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostListDto>(ResultStatus.Success, Messages.Post.MultiTrash(postIds.Count), new PostListDto
                {
                    Posts = posts
                });
            }
            return new DataResult<PostListDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), new PostListDto
            {
                Posts = null,
            });
        }

        public async Task<IDataResult<PostDto>> UnTrashAsync(int postId, int userId)
        {
            var post = await UnitOfWork.Posts.GetAsync(p => p.Id == postId);
            if (post.PostType != SubObjectType.basepage)
            {
                post.PostStatus = PostStatus.draft;
                post.UserId = userId;
                post.ModifiedDate = DateTime.Now;
                await UnitOfWork.Posts.UpdateAsync(post);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostDto>(ResultStatus.Success, Messages.Post.UnTrash(post.Title), new PostDto
                {
                    Post = post,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Post.UnTrash(post.Title)
                });
            }
            else
            {
                return new DataResult<PostDto>(ResultStatus.Warning, Messages.Post.BasePage(), new PostDto
                {
                    Post = post,
                    ResultStatus = ResultStatus.Warning,
                    Message = Messages.Post.BasePage()
                });
            }
        }

        public async Task<IDataResult<PostListDto>> MultiUnTrashAsync(List<int> postIds, int userId)
        {
            var posts = await UnitOfWork.Posts.GetAllAsync(p => postIds.Contains(p.Id));
            if (posts.Count > -1)
            {
                List<Post> unTrashedPosts = new List<Post>();
                foreach (var post in posts)
                {
                    if (post.PostType != SubObjectType.basepage)
                    {
                        post.PostStatus = PostStatus.draft;
                        post.UserId = userId;
                        post.ModifiedDate = DateTime.Now;
                        unTrashedPosts.Add(post);
                    }
                    else
                    {
                        return new DataResult<PostListDto>(ResultStatus.Warning, Messages.Post.BasePage(), new PostListDto
                        {
                            Posts = posts,
                            ResultStatus = ResultStatus.Warning,
                            Message = Messages.Post.BasePage()
                        });
                    }
                }
                await UnitOfWork.Posts.MultiUpdateAsync(unTrashedPosts);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostListDto>(ResultStatus.Success, Messages.Post.MultiUnTrash(postIds.Count), new PostListDto
                {
                    Posts = posts
                });
            }
            return new DataResult<PostListDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), new PostListDto
            {
                Posts = null,
            });
        }

        public async Task<IDataResult<PostDto>> DeleteAsync(int postId)
        {
            var post = await UnitOfWork.Posts.GetAsync(p => p.Id == postId);
            if (post.PostType != SubObjectType.basepage)
            {
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
                return new DataResult<PostDto>(ResultStatus.Success, Messages.Post.Delete(post.Title), new PostDto
                {
                    Post = post,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Post.Delete(post.Title)
                });
            }
            else
            {
                return new DataResult<PostDto>(ResultStatus.Warning, Messages.Post.BasePage(), new PostDto
                {
                    Post = post,
                    ResultStatus = ResultStatus.Warning,
                    Message = Messages.Post.BasePage()
                });
            }
        }

        public async Task<IDataResult<PostListDto>> MultiDeleteAsync(List<int> postIds)
        {
            var posts = await UnitOfWork.Posts.GetAllAsync(p => p.PostStatus == PostStatus.trash && postIds.Contains(p.Id));
            if (posts.Count > -1)
            {
                foreach (var post in posts)
                {
                    if (post.PostType == SubObjectType.basepage)
                    {
                        return new DataResult<PostListDto>(ResultStatus.Warning, Messages.Post.BasePage(), new PostListDto
                        {
                            Posts = posts,
                            ResultStatus = ResultStatus.Warning,
                            Message = Messages.Post.BasePage()
                        });
                    }
                }
                await UnitOfWork.Posts.MultiDeleteAsync(posts);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostListDto>(ResultStatus.Success, Messages.Post.MultiDelete(postIds.Count), new PostListDto
                {
                    Posts = posts,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<PostListDto>(ResultStatus.Error, Messages.Post.NotFound(isPlural: false), new PostListDto
            {
                Posts = null,
                ResultStatus = ResultStatus.Error,
                Message = "Hata oluştu. Lütfen sayfayı yenileyip tekrar deneyiniz."
            });
        }

        public async Task<int> PublishStatusCountAsync(SubObjectType postType, PostStatus? postStatus)
        {
            var posts = postType == SubObjectType.page
                ? await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p.Parent, p => p.Children, p => p.FeaturedImage, p => p.Galleries, p => p.User)
                : postType == SubObjectType.article
                ? await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p.FeaturedImage, p => p.PostTerms, p => p.Comments, p => p.User)
                : postType == SubObjectType.basepage
                ? await UnitOfWork.Posts.GetAllAsync(p => p.PostType == postType && p.PostStatus == postStatus, p => p.FeaturedImage, p => p.User)
                : null;

            return posts.Count;
        }

        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var postCount = await UnitOfWork.Posts.CountAsync(null);
            if (postCount > -1)
            {
                return new DataResult<int>(ResultStatus.Success, postCount);
            }
            else
            {
                return new DataResult<int>(ResultStatus.Error, $"Beklenmeyen bir hata ile karşılaşıldı.", -1);
            }
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

            if (subPost != null)
            {
                subPost.Description = description;
                subPost.FeaturedImageId = featuredImageId;
                await UnitOfWork.Posts.UpdateAsync(subPost);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostDto>(ResultStatus.Success, "Alt sayfa güncellendi.", new PostDto
                {
                    Post = subPost,
                    ResultStatus = ResultStatus.Success,
                    Message = "Alt sayfa güncellendi."
                });
            }
            return new DataResult<PostDto>(ResultStatus.Error, "Hata oluştu. Sayfayı yenileyip tekrar deneyiniz.", new PostDto
            {
                Post = subPost,
                ResultStatus = ResultStatus.Error,
                Message = "Hata oluştu. Sayfayı yenileyip tekrar deneyiniz."
            });
        }

        public async Task<IDataResult<PostDto>> TopPostUpdateAsync(int postId, int parentId)
        {
            var post = await UnitOfWork.Posts.GetAsync(x => x.Id == postId);

            if (post != null)
            {
                post.ParentId = parentId == 0 ? 0 : parentId;
                await UnitOfWork.Posts.UpdateAsync(post);
                await UnitOfWork.SaveAsync();
                return new DataResult<PostDto>(ResultStatus.Success, new PostDto
                {
                    Post = post,
                    ResultStatus = ResultStatus.Success,
                    Message = "Ebeveyn sayfa güncellendi"
                });
            }
            return new DataResult<PostDto>(ResultStatus.Error, null);
        }

        public async Task<IResult> SubPostUpdateAsync(int postId, List<int> subPostId)
        {
            var subPosts = await UnitOfWork.Posts.GetAllAsync(x => x.ParentId == postId);

            foreach (var subPost in subPosts)
            {
                if (!subPostId.Contains(subPost.Id))
                {
                    subPost.ParentId = null;
                    await UnitOfWork.Posts.UpdateAsync(subPost);
                }
            }

            foreach (var spId in subPostId)
            {
                var subPost = await UnitOfWork.Posts.GetAsync(p => p.Id == spId);
                if (subPost != null)
                {
                    subPost.ParentId = postId;
                    await UnitOfWork.Posts.UpdateAsync(subPost);
                }
            }
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

                    Term parent = pterm.Term.Parent;
                    pterm.Term.Parents = new List<Term>();
                    while (parent != null)
                    {
                        pterm.Term.Parents.Add(parent);
                        parent = await UnitOfWork.Terms.GetAsync(p => p.Id == parent.ParentId, p => p.Parent, p => p.Children);
                    }
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