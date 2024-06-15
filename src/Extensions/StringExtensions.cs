using TucaAPI.src.Common;

namespace TucaAPI.src.Extensions
{
    public static class StringExtensions
    {
        public static string GetNonNullable(this string? value)
        {
            if (value is null)
                throw new InvalidOperationException(MessageKey.INTERNAL_SERVER_ERROR);

            return value;
        }
    }
}