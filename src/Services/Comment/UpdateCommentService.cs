using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Comment;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Mappers;
using TucaAPI.src.Models;
using TucaAPI.src.Repositories;

namespace TucaAPI.src.Services.Comment
{
    public class UpdateCommentService
        : IService<UpdateCommentRequestDto, SuccessApiResponse<CommentDto>>
    {
        private readonly ICommentRepository commentRepository;
        private readonly UserManager<AppUser> userManager;

        public UpdateCommentService(ICommentRepository repository, UserManager<AppUser> userManager)
        {
            this.commentRepository = repository;
            this.userManager = userManager;
        }

        public async Task<SuccessApiResponse<CommentDto>> ExecuteAsync(UpdateCommentRequestDto data)
        {
            var email = data.User.GetEmail();
            var hasUser = await this.userManager.FindByEmailAsync(email.GetNonNullable());
            if (hasUser is null)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.USER_NOT_FOUND
                );

            var comment = await this.commentRepository.UpdateAsync(data.Id, data, hasUser);

            if (comment is null)
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.COMMENT_NOT_FOUND);

            return new SuccessApiResponse<CommentDto> { Content = comment.ToCommentDto() };
        }
    }
}
