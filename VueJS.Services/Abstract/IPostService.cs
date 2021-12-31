using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface IPostService
    {
        Task<IDataResult<PostDto>> GetAsync(int postId);
        Task<IDataResult<PostDto>> GetAsync(string postName);
        Task<IDataResult<PostDto>> GetByIdAsync(int postId, bool includeCategories, bool includeTags, bool includeComments, bool includeGalleries, bool includeUser);
        Task<IDataResult<PostUpdateDto>> GetBasePageUpdateDtoAsync(string pageName);
        Task<IDataResult<PostListDto>> GetAllAsync(SubObjectType postType, PostStatus? postStatus);
        Task<IDataResult<PostListDto>> GetAllPostStatusAsync(SubObjectType postType, PostStatus postStatus);
        Task<IDataResult<PostListDto>> GetAllAnotherPostsAsync(SubObjectType postType, int? postId);
        Task<IDataResult<PostListDto>> GetAllSubPostsAsync(SubObjectType postType, int? postId);
        Task<IDataResult<PostListDto>> GetSubPostUpdateDtoAsync(int parentId);
        Task<IDataResult<PostListDto>> GetAllSubPostDetailsAsync(int postId);
        Task<IDataResult<PostUpdateDto>> GetPostUpdateDtoAsync(int postId);
        Task<IDataResult<PostDto>> AddAsync(PostAddDto postAddDto, int userId, SubObjectType postType);
        Task<IDataResult<PostDto>> UpdateAsync(PostUpdateDto postUpdateDto, int userId);
        Task<IResult> BasePageUpdateAsync(PostUpdateDto postUpdateDto, int userId);
        Task<IDataResult<PostDto>> TopPostUpdateAsync(int postId, int parentId);
        Task<IResult> SubPostUpdateAsync(int postId, List<int> subPostId);
        Task<IDataResult<PostDto>> SubPostDetailUpdateAsync(int postId, string description, int? featuredImageId);
        Task<IDataResult<PostDto>> GalleryImageAddAsync(int postId, List<int> galleryIds);
        Task<IDataResult<PostListDto>> MultiPublishAsync(List<int> postIds, int userId);
        Task<IDataResult<PostListDto>> MultiDraftAsync(List<int> postIds, int userId);
        Task<IDataResult<PostDto>> TrashAsync(int postId, int userId);
        Task<IDataResult<PostListDto>> MultiTrashAsync(List<int> postIds, int userId);
        Task<IDataResult<PostDto>> UnTrashAsync(int postId, int userId);
        Task<IDataResult<PostListDto>> MultiUnTrashAsync(List<int> postIds, int userId);
        Task<IDataResult<PostDto>> DeleteAsync(int postId);
        Task<IDataResult<PostListDto>> MultiDeleteAsync(List<int> postIds);
        Task<int> PublishStatusCountAsync(SubObjectType postType, PostStatus? postStatus);
        Task<IDataResult<int>> CountByNonDeletedAsync();
        Task<IDataResult<PostListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize);
        Task<IDataResult<PostListDto>> GetAllByPagingAsync(string categoryGuid, string tagGuid, int currentPage = 1, int pageSize = 5,
            bool isAscending = false);
        Task<IDataResult<PostListDto>> GetAllByTermArticlesAsync(int termId, int currentPage = 1);
        Task<IDataResult<PostListDto>> GetAllByTagArticlesAsync(int tagId, int currentPage = 1);
        Task<IDataResult<PostListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy,
            bool isAscending, int takeSize, int categoryId, int tagId, DateTime startAt, DateTime endAt, int minViewCount,
            int maxViewCount, int minCommentCount, int maxCommentCount);
        Task<IDataResult<PostListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5,
            bool isAscending = false);
        Task<IResult> IncreaseViewCountAsync(int postId);
    }
}