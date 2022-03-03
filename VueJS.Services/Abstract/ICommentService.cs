using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace VueJS.Services.Abstract
{
    public interface ICommentService
    {
        Task<IDataResult<CommentDto>> GetAsync(int commentId);
        Task<IDataResult<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId);
        Task<IDataResult<CommentListDto>> GetAllAsync(CommentStatus? commentStatus, int? userId);
        Task<IDataResult<CommentListDto>> GetCommentReplysAsync(int commentId);
        Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto, int? userId);
        Task<IDataResult<CommentDto>> CommentReplyAddAsync(CommentAddDto commentAddDto, int parentId, int? userId);
        Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, int userId);
        Task<IDataResult<CommentDto>> CommentStatusChangeAsync(int commentId, CommentStatus commentStatus, int userId);
        Task<IResult> DeleteAsync(int commentId);
        Task<IDataResult<int>> CountAsync();
    }
}