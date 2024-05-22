using TucaAPI.Dtos.Comment;
using TucaAPI.Models;

namespace TucaAPI.Mappers
{
    public static class CommentMapper
    {
        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
                Title = comment.Title,
                CreatedBy = comment.AppUser.UserName
            };
        }

        public static Comment ToCommentFromRequestDto(this CreateCommentRequestDto comment, int stockId, string userId)
        {
            return new Comment
            {
                Content = comment.Content,
                Title = comment.Title,
                StockId = stockId,
                AppUserId = userId
            };
        }
    }
}