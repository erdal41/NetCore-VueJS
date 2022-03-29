using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VueJS.Entities.ComplexTypes;
using VueJS.Entities.Concrete;
using VueJS.Entities.Dtos;
using VueJS.Mvc.Areas.Admin.Models.Data;
using VueJS.Mvc.Areas.Admin.Models.View;
using VueJS.Services.Abstract;
using System.Threading.Tasks;
using VueJS.Shared.Utilities.Results.ComplexTypes;

namespace VueJS.Mvc.Areas.Admin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(UserManager<User> userManager, IMapper mapper, ICommentService commentService) : base(userManager, mapper)
        {
            _commentService = commentService;
        }

        [HttpGet("/admin/comment-allcomments")]
        public async Task<JsonResult> AllComments(CommentStatus? status, int? userId)
        {
            var result = await _commentService.GetAllAsync(status, userId);
            var myComments = await _commentService.GetAllAsync(null, LoggedInUser.Id);
            var moderatedComments = await _commentService.GetAllAsync(CommentStatus.moderated, null);
            var approvedComments = await _commentService.GetAllAsync(CommentStatus.approved, null);
            var trashComments = await _commentService.GetAllAsync(CommentStatus.trash, null);

            var commentListViewModel = new CommentViewModel
            {
                CommentListDto = result,
                MineCommentsCount = myComments.ResultStatus == ResultStatus.Success ? myComments.Data.Comments.Count : 0,
                ModeratedCommentsCount = moderatedComments.ResultStatus == ResultStatus.Success ? moderatedComments.Data.Comments.Count : 0,
                ApprovedCommentsCount = approvedComments.ResultStatus == ResultStatus.Success ? approvedComments.Data.Comments.Count : 0,
                TrashCommentsCount = trashComments.ResultStatus == ResultStatus.Success ? trashComments.Data.Comments.Count : 0
            };
            return Json(commentListViewModel);

        }

        [HttpGet("/admin/comment-edit")]
        public async Task<JsonResult> Edit(int commentId)
        {
            var result = await _commentService.GetCommentUpdateDtoAsync(commentId);
            return Json(new CommentViewModel { CommentUpdateDto = result });
        }

        [HttpPost("/admin/comment-edit")]
        public async Task<JsonResult> Edit(CommentDataModel commentDataModel)
        {
            var result = await _commentService.UpdateAsync(commentDataModel.CommentUpdateDto, LoggedInUser.Id);
            return Json(new CommentViewModel { CommentDto = result });
        }

        [HttpPost("/admin/comment-commentstatuschange")]
        public async Task<JsonResult> CommentStatusChange(int commentId, CommentStatus commentStatus)
        {
            var result = await _commentService.CommentStatusChangeAsync(commentId, commentStatus, LoggedInUser.Id);
            return Json(new CommentViewModel { CommentDto = result });
        }

        [HttpPost("/admin/comment-delete")]
        public async Task<JsonResult> Delete(int commentId)
        {
            return Json(await _commentService.DeleteAsync(commentId));
        }
    }
}