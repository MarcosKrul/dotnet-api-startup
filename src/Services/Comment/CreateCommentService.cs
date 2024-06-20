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
using TucaAPI.src.Utilities.Extensions;

namespace TucaAPI.src.Services.Comment
{
    public class CreateCommentService
        : IService<CreateCommentRequestDto, SuccessApiResponse<CommentDto>>
    {
        private readonly ICommentRepository commentRepository;
        private readonly IStockRepository stockRepository;
        private readonly UserManager<AppUser> userManager;

        public CreateCommentService(
            ICommentRepository repository,
            IStockRepository stockRepository,
            UserManager<AppUser> userManager
        )
        {
            this.commentRepository = repository;
            this.stockRepository = stockRepository;
            this.userManager = userManager;
        }

        public async Task<SuccessApiResponse<CommentDto>> ExecuteAsync(CreateCommentRequestDto data)
        {
            if (!await this.stockRepository.StockExistsAsync(data.StockId))
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.STOCK_NOT_FOUND);

            var user = await this.userManager.FindNonNullableUserAsync(
                data.GetNonNullableUser().GetEmail().GetNonNullable()
            );

            var comment = data.ToCommentFromRequestDto(data.StockId, user.Id);

            await this.commentRepository.CreateAsync(comment);

            var newComment = await new GetCommentByIdService(this.commentRepository).ExecuteAsync(
                new GetCommentByIdRequestDto { Id = comment.Id }
            );

            return newComment;
        }
    }
}
