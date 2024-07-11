namespace TucaAPI.src.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string GetReadableString(this DateTime date)
        {
            var difference = DateTime.Now - date;

            if (difference.TotalDays >= 30)
            {
                int months = (int)(difference.TotalDays / 30);
                return $"{months} {(months == 1 ? "month" : "months")}";
            }

            if (difference.TotalDays >= 1)
            {
                int days = (int)difference.TotalDays;
                return $"{days} {(days == 1 ? "day" : "days")}";
            }

            if (difference.TotalHours >= 1)
            {
                int hours = (int)difference.TotalHours;
                return $"{hours} {(hours == 1 ? "hour" : "hours")}";
            }

            int minutes = (int)difference.TotalMinutes;
            return $"{minutes} {(minutes == 1 ? "minute" : "minutes")}";
        }
    }
}
