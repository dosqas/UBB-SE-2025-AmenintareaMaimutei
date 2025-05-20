using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DuoClassLibrary.Helpers
{
    public static class DateTimeHelper
    {
        public const string DefaultDateFormat = "yyyy-MM-dd";
        public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        public static DateTime GetCurrentTimeUtc()
        {
            return DateTime.UtcNow;
        }

        public static DateTime ConvertUtcToLocal(DateTime utcDateTime)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {

                utcDateTime = DateTime.SpecifyKind(utcDateTime, DateTimeKind.Utc);
            }
            return utcDateTime.ToLocalTime();
        }

        public static DateTime ConvertLocalToUtc(DateTime localDateTime)
        {
            if (localDateTime.Kind != DateTimeKind.Local)
            {
                localDateTime = DateTime.SpecifyKind(localDateTime, DateTimeKind.Local);
            }
            return localDateTime.ToUniversalTime();
        }

        public static bool TryParseDateTime(string dateTimeString, out DateTime result)
        {
            return DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, 
                DateTimeStyles.AdjustToUniversal, out result);
        }

        public static DateTime? ParseDateTime(string dateTimeString, string? format = null)
        {
            
            if (string.IsNullOrWhiteSpace(dateTimeString))
                return null;

            if (format != null)
            {
                if (DateTime.TryParseExact(dateTimeString, format, CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            if (DateTime.TryParse(dateTimeString, CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }

            return null;
        }

        public static string FormatAsDate(DateTime? dateTime)
        {
            return dateTime?.ToString(DefaultDateFormat) ?? string.Empty;
        }

        public static string FormatAsDateTime(DateTime? dateTime)
        {
            return dateTime?.ToString(DefaultDateTimeFormat) ?? string.Empty;
        }

        public static string Format(DateTime? dateTime, string format)
        {
            return dateTime?.ToString(format) ?? string.Empty;
        }

        public static string GetRelativeTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
                return string.Empty;

            var timeSpan = DateTime.Now - dateTime.Value;

            if (timeSpan.TotalDays > 365)
            {
                int years = (int)Math.Floor(timeSpan.TotalDays / 365);
                return years == 1 ? "1 year ago" : $"{years} years ago";
            }

            if (timeSpan.TotalDays > 30)
            {
                int months = (int)Math.Floor(timeSpan.TotalDays / 30);
                return months == 1 ? "1 month ago" : $"{months} months ago";
            }

            if (timeSpan.TotalDays > 7)
            {
                int weeks = (int)Math.Floor(timeSpan.TotalDays / 7);
                return weeks == 1 ? "1 week ago" : $"{weeks} weeks ago";
            }

            if (timeSpan.TotalDays >= 1)
            {
                int days = (int)Math.Floor(timeSpan.TotalDays);
                return days == 1 ? "1 day ago" : $"{days} days ago";
            }

            if (timeSpan.TotalHours >= 1)
            {
                int hours = (int)Math.Floor(timeSpan.TotalHours);
                return hours == 1 ? "1 hour ago" : $"{hours} hours ago";
            }

            if (timeSpan.TotalMinutes >= 1)
            {
                int minutes = (int)Math.Floor(timeSpan.TotalMinutes);
                return minutes == 1 ? "1 minute ago" : $"{minutes} minutes ago";
            }

            if (timeSpan.TotalSeconds >= 10)
            {
                int seconds = (int)Math.Floor(timeSpan.TotalSeconds);
                return seconds == 1 ? "1 second ago" : $"{seconds} seconds ago";
            }

            return "Just now";
        }

        public static DateTime EnsureUtcKind(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;

            if (dateTime.Kind == DateTimeKind.Local)
                return dateTime.ToUniversalTime();

            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        public static string FormatDbDateTimeForDisplay(DateTime? utcDateTime, string? format = null)
        {
            if (!utcDateTime.HasValue)
                return string.Empty;

            var localDateTime = ConvertUtcToLocal(utcDateTime.Value);
            return format == null ? 
                FormatAsDateTime(localDateTime) : 
                Format(localDateTime, format);
        }
        public static string FormatDate(DateTime date)
        {
            if (date.Date == DateTime.Today)
            {
                return "Today";
            }
            else if (date.Date == DateTime.Today.AddDays(-1))
            {
                return "Yesterday";
            }
            else if ((DateTime.Today - date.Date).TotalDays < 7)
            {
                return date.ToString("ddd"); // Day of week
            }

            DateTime localDate = DuoClassLibrary.Helpers.DateTimeHelper.ConvertUtcToLocal(date);
            return date.ToString("MMM d"); // Month + day
        }
    }
}