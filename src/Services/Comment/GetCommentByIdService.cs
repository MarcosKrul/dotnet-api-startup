using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Comment;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Mappers;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Comment
{
    public class GetCommentByIdService
        : IService<GetCommentByIdRequestDto, SuccessApiResponse<CommentDto>>
    {
        private readonly ICommentRepository commentRepository;

        public GetCommentByIdService(ICommentRepository repository)
        {
            this.commentRepository = repository;
        }

        public async Task<SuccessApiResponse<CommentDto>> ExecuteAsync(
            GetCommentByIdRequestDto data
        )
        {
            var comment = await this.commentRepository.GetByIdAsync(data.Id);

            if (comment is null)
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.COMMENT_NOT_FOUND);

            return new SuccessApiResponse<CommentDto> { Content = comment.ToCommentDto() };
        }
    }
}
