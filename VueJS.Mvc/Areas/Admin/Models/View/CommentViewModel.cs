using VueJS.Entities.Dtos;
using VueJS.Shared.Utilities.Results.Abstract;

namespace VueJS.Mvc.Areas.Admin.Models.View
{
    public class CommentViewModel
    {
        public IDataResult<CommentDto> CommentDto { get; set; }
        public IDataResult<CommentListDto> CommentListDto { get; set; }
        public IDataResult<CommentUpdateDto> CommentUpdateDto { get; set; }
        public int MineCommentsCount { get; set; }
        public int ModeratedCommentsCount { get; set; }
        public int ApprovedCommentsCount { get; set; }
        public int TrashCommentsCount { get; set; }
    }
}