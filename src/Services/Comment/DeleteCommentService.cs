using Microsoft.AspNetCore.Identity;
using TucaAPI.Extensions;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Comment;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Extensions;
using TucaAPI.src.Models;
using TucaAPI.src.Repositories;
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Comment
{
    public class DeleteCommentService : IService<DeleteCommentRequestDto, ApiResponse>
    {
        private readonly ICommentRepository commentRepository;
        private readonly UserManager<AppUser> userManager;

        public DeleteCommentService(ICommentRepository repository, UserManager<AppUser> userManager)
        {
            this.commentRepository = repository;
            this.userManager = userManager;
        }

        public async Task<ApiResponse> ExecuteAsync(DeleteCommentRequestDto data)
        {
            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            var comment = await this.commentRepository.GetByIdAsync(data.Id);

            if (comment is null)
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.COMMENT_NOT_FOUND);

            if (comment.AppUserId != user.Id)
                throw new AppException(
                    StatusCodes.Status401Unauthorized,
                    MessageKey.USER_NOT_FOUND
                );

            await this.commentRepository.DeleteAsync(comment);

            return new ApiResponse { Success = true };
        }
    }
}
