using TucaAPI.src.Dtos.Account;
using TucaAPI.src.Dtos.Common;

namespace TucaAPI.src.Services.Account
{
    public class UpdatePasswordService : IService<UpdatePasswordRequestDto, ApiResponse>
    {
        public async Task<ApiResponse> ExecuteAsync(UpdatePasswordRequestDto data)
        {
            throw new NotImplementedException();
        }
    }
}
