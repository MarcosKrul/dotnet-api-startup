using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Dtos.UserManagement;
using TucaAPI.src.Models;
using TucaAPI.src.Utilities.Extensions;

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
            var user = await this.userManager.FindNonNullableUserAsync(data.Id);

            // delete

            return new ApiResponse { Success = true };
        }
    }
}
