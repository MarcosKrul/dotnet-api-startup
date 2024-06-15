using TucaAPI.src.Common;
using TucaAPI.src.Dtos.Common;
using TucaAPI.src.Exceptions;

namespace TucaAPI.src.Extensions
{
    public static class StringExtensions
    {
        public static string GetNonNullable(this string? value)
        {
            if (value is null)
                throw new AppException(StatusCodes.Status500InternalServerError, new ErrorApiResponse<string>
                {
                    Errors = [MessageKey.STRING_NON_NULLABLE]
                });

            return value;
        }
    }
}