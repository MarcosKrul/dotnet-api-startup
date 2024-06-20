using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.UserManagement;
using TucaAPI.src.Exceptions;
using TucaAPI.src.Models;

namespace TucaAPI.src.Services.UserManagement
{
    public class DeleteUserService : IService<DeleteUserRequestDto, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;

        public DeleteUserService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ApiResponse> ExecuteAsync(DeleteUserRequestDto data)
        {
            var hasUser = await this.userManager.FindByIdAsync(data.Id);
            if (hasUser is null)
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.USER_NOT_FOUND);

            // delete

            return new ApiResponse { Success = true };
        }
    }
}
