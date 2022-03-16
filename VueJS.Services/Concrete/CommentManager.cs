using AutoMapper;
using VueJS.Data.Abstract;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Services.Abstract;
using VueJS.Services.Utilities;
using VueJS.Shared.Utilities.Results.Concrete;
using VueJS.Shared.Utilities.Results.Abstract;
using VueJS.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Threading.Tasks;
using VueJS.Entities.ComplexTypes;
using System.Collections.Generic;

namespace VueJS.Services.Concrete
{
    public class CommentManager : ManagerBase, ICommentService
    {
        public CommentManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<IDataResult<CommentDto>> GetAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment == null) return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(false), null);
            return new DataResult<CommentDto>(ResultStatus.Success, new CommentDto { Comment = comment });
        }

        public async Task<IDataResult<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await UnitOfWork.Comments.AnyAsync(c => c.Id == commentId);
            if (result) return new DataResult<CommentUpdateDto>(ResultStatus.Error, Messages.Comment.NotFound(false), null);

            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            var commentUpdateDto = Mapper.Map<CommentUpdateDto>(comment);
            return new DataResult<CommentUpdateDto>(ResultStatus.Success, commentUpdateDto);
        }

        public async Task<IDataResult<CommentListDto>> GetAllAsync(CommentStatus? commentStatus, int? userId)
        {
            if (commentStatus == null)
            {
                if (userId == null)
                {
                    var comments = await UnitOfWork.Comments.GetAllAsync(c => c.CommentStatus != CommentStatus.trash, c => c.Post, c => c.User);
                    if (comments.Count > 0)
                    {
                        return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                        {
                            Comments = comments,
                        });
                    }
                }
                else
                {
                    var comments = await UnitOfWork.Comments.GetAllAsync(c => c.UserId == userId.Value, c => c.Post, c => c.User);
                    if (comments.Count > 0)
                    {
                        return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                        {
                            Comments = comments,
                        });
                    }
                }
            }
            else
            {
                var comments = await UnitOfWork.Comments.GetAllAsync(c => c.CommentStatus == commentStatus, c => c.Post, c => c.User);
                if (comments.Count > 0)
                {
                    return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto { Comments = comments });
                }
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(true), null);
        }

        public async Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, int userId)
        {
            var oldComment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentUpdateDto.Id, c => c.User);
            if (oldComment == null) return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(false), null);


            var comment = Mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);
            comment.UserId = userId;
            await UnitOfWork.Comments.UpdateAsync(comment);
            comment.Post = await UnitOfWork.Posts.GetAsync(a => a.Id == comment.PostId);
            await UnitOfWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Update(comment.User.UserName), new CommentDto { Comment = comment });
        }

        public async Task<IDataResult<CommentDto>> CommentStatusChangeAsync(int commentId, CommentStatus commentStatus, int userId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(p => p.Id == commentId);
            if (comment == null) return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(false), null);

            var post = comment.Post;
            comment.CommentStatus = commentStatus;
            comment.UserId = userId;
            await UnitOfWork.Comments.UpdateAsync(comment);
            post.CommentCount = await UnitOfWork.Comments.CountAsync(c => c.PostId == post.Id && c.CommentStatus == CommentStatus.approved);
            await UnitOfWork.Posts.UpdateAsync(post);
            await UnitOfWork.SaveAsync();

            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentStatusChange(commentStatus, comment.Id), new CommentDto { Comment = comment });
        }

        public async Task<IResult> DeleteAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId && c.CommentStatus == CommentStatus.trash);
            if (comment == null) return new Result(ResultStatus.Error, Messages.Comment.NotFound(false));

            await UnitOfWork.Comments.DeleteAsync(comment);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Comment.Delete());
        }

        public async Task<IDataResult<int>> CountAsync()
        {
            return new DataResult<int>(ResultStatus.Success, await UnitOfWork.Comments.CountAsync());
        }

        #region WEB

        public async Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto, int? userId)
        {
            var post = await UnitOfWork.Posts.GetAsync(a => a.Id == commentAddDto.PostId, c => c.User);
            if (post == null) return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), null);

            var comment = Mapper.Map<Comment>(commentAddDto);
            if (userId == 1)
            {
                comment.CommentStatus = CommentStatus.approved;
            }

            comment.UserId = userId;
            var addedComment = await UnitOfWork.Comments.AddAsync(comment);
            post.CommentCount += 1;
            await UnitOfWork.Posts.UpdateAsync(post);
            await UnitOfWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Add(commentAddDto.Name), new CommentDto
            {
                Comment = addedComment,
            });
        }

        public async Task<IDataResult<CommentDto>> CommentReplyAddAsync(CommentAddDto commentAddDto, int parentId, int? userId)
        {
            var post = await UnitOfWork.Posts.GetAsync(a => a.Id == commentAddDto.PostId, c => c.User);
            if (post == null)
            {
                return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), null);
            }

            var comment = Mapper.Map<Comment>(commentAddDto);

            if (userId == 1)
            {
                comment.CommentStatus = CommentStatus.approved;
            }

            comment.ParentId = parentId;
            comment.UserId = userId;
            var commentReply = await UnitOfWork.Comments.AddAsync(comment);
            post.CommentCount += 1;
            await UnitOfWork.Posts.UpdateAsync(post);
            await UnitOfWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Add(commentAddDto.Name), new CommentDto
            {
                Comment = commentReply,
            });
        }

        public async Task<IDataResult<CommentListDto>> GetAllArticleCommentsAsync(int postId)
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(p => p.CommentStatus == CommentStatus.approved && p.PostId == postId, p => p.Parent, p => p.Children, p => p.Post, p => p.User);

            return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
            {
                Comments = comments
            });
        }

        public async Task<IDataResult<CommentListDto>> GetCommentReplysAsync(int commentId)
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => c.ParentId == commentId, c => c.Children, c => c.Parents, c => c.User);
            if (comments.Count > 0)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                Comments = null,
            });
        }


        #endregion
    }
}