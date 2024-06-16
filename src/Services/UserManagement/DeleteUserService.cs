using Microsoft.AspNetCore.Identity;
using TucaAPI.Models;
using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;
using TucaAPI.Src.Services;

namespace TucaAPI.src.Services.UserManagement
{
    public class DeleteUserService : IService<string, ApiResponse>
    {
        private readonly UserManager<AppUser> userManager;

        public DeleteUserService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ApiResponse> ExecuteAsync(string data)
        {
            var hasUser = await this.userManager.FindByIdAsync(data);
            if (hasUser is null)
                throw new AppException(StatusCodes.Status404NotFound, MessageKey.USER_NOT_FOUND);

            // delete

            return new ApiResponse { Success = true };
        }
    }
}
