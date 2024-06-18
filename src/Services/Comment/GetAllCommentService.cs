using TucaAPI.src.Dtos.Comment;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Mappers;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Comment
{
    public class GetAllCommentService
        : IService<object?, SuccessApiResponse<IEnumerable<CommentDto>>>
    {
        private readonly ICommentRepository commentRepository;

        public GetAllCommentService(ICommentRepository repository)
        {
            this.commentRepository = repository;
        }

        public async Task<SuccessApiResponse<IEnumerable<CommentDto>>> ExecuteAsync(object? data)
        {
            var comments = await this.commentRepository.GetAllAsync();
            var formatted = comments.Select(i => i.ToCommentDto());

            return new SuccessApiResponse<IEnumerable<CommentDto>> { Content = formatted };
        }
    }
}
