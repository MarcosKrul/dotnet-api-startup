
using Microsoft.AspNetCore.Identity;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Extensions;

namespace TucaAPI.src.Mappers
{
    public static class IdentityErrorMapper
    {
        public static AppErrorDescriptor ToAppErrorDescriptor(this IdentityError error)
        {
            return new AppErrorDescriptor
            {
                Key = error.Code.ToErrorKeyFormat(),
                Description = error.Description,
            };
        }
    }
}