using System.Text;
using TucaAPI.src.Common;
using TucaAPI.src.Exceptions;

namespace TucaAPI.src.Extensions
{
    public static class StringExtensions
    {
        public static string GetNonNullable(this string? value)
        {
            if (value is null)
                throw new AppException(
                    StatusCodes.Status500InternalServerError,
                    MessageKey.STRING_NON_NULLABLE
                );

            return value;
        }

        public static string ToErrorKeyFormat(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder();
            bool lastWasUpperCase = false;

            foreach (char c in input)
            {
                if (char.IsUpper(c))
                {
                    if (!lastWasUpperCase && result.Length > 0)
                        result.Append('_');

                    result.Append(char.ToUpper(c));
                    lastWasUpperCase = true;
                }
                else
                {
                    result.Append(char.ToUpper(c));
                    lastWasUpperCase = false;
                }
            }

            return result.ToString();
        }
    }
}