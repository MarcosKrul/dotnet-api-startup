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
                Title = comment.Title
            };
        }
    }
}