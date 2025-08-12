using System;
using System.Globalization;
using Xunit;
using Duo.Helpers;
using DuoClassLibrary.Helpers;

namespace TestProject1.Helpers
{
    public class DateTimeHelperTests
    {
        [Fact]
        public void GetCurrentTime_ReturnsCurrentTime()
        {
            // Arrange
            var before = DateTime.Now;

            // Act
            var result = DateTimeHelper.GetCurrentTime();

            // Assert
            Assert.True(result >= before);
            Assert.True(result <= DateTime.Now);
        }

        [Fact]
        public void GetCurrentTimeUtc_ReturnsCurrentUtcTime()
        {
            // Arrange
            var before = DateTime.UtcNow;

            // Act
            var result = DateTimeHelper.GetCurrentTimeUtc();

            // Assert
            Assert.True(result >= before);
            Assert.True(result <= DateTime.UtcNow);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void ConvertUtcToLocal_WithUtcDateTime_ConvertsCorrectly()
        {
            // Arrange
            var utcDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var expectedLocalTime = utcDateTime.ToLocalTime();

            // Act
            var result = DateTimeHelper.ConvertUtcToLocal(utcDateTime);

            // Assert
            Assert.Equal(expectedLocalTime, result);
            Assert.Equal(DateTimeKind.Local, result.Kind);
        }

        [Fact]
        public void ConvertUtcToLocal_WithUnspecifiedDateTime_SpecifiesUtcAndConverts()
        {
            // Arrange
            var unspecifiedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

            // Act
            var result = DateTimeHelper.ConvertUtcToLocal(unspecifiedDateTime);

            // Assert
            Assert.Equal(DateTimeKind.Local, result.Kind);
        }

        [Fact]
        public void ConvertLocalToUtc_WithLocalDateTime_ConvertsCorrectly()
        {
            // Arrange
            var localDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local);
            var expectedUtcTime = localDateTime.ToUniversalTime();

            // Act
            var result = DateTimeHelper.ConvertLocalToUtc(localDateTime);

            // Assert
            Assert.Equal(expectedUtcTime, result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void ConvertLocalToUtc_WithUnspecifiedDateTime_SpecifiesLocalAndConverts()
        {
            // Arrange
            var unspecifiedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

            // Act
            var result = DateTimeHelper.ConvertLocalToUtc(unspecifiedDateTime);

            // Assert
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void TryParseDateTime_WithValidString_ReturnsTrueAndParsesCorrectly()
        {
            // Arrange
            var dateString = "2024-01-01 12:00:00";

            // Act
            var success = DateTimeHelper.TryParseDateTime(dateString, out var result);

            // Assert
            Assert.True(success);
            Assert.Equal(new DateTime(2024, 1, 1, 12, 0, 0), result);
        }

        [Fact]
        public void TryParseDateTime_WithInvalidString_ReturnsFalse()
        {
            // Arrange
            var invalidDateString = "invalid-date";

            // Act
            var success = DateTimeHelper.TryParseDateTime(invalidDateString, out var result);

            // Assert
            Assert.False(success);
            Assert.Equal(default(DateTime), result);
        }

        [Fact]
        public void ParseDateTime_WithValidStringAndFormat_ReturnsParsedDateTime()
        {
            // Arrange
            var dateString = "01/01/2024";
            var format = "MM/dd/yyyy";

            // Act
            var result = DateTimeHelper.ParseDateTime(dateString, format);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new DateTime(2024, 1, 1), result.Value);
        }

        [Fact]
        public void ParseDateTime_WithInvalidString_ReturnsNull()
        {
            // Arrange
            var invalidDateString = "invalid-date";

            // Act
            var result = DateTimeHelper.ParseDateTime(invalidDateString);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ParseDateTime_WithEmptyString_ReturnsNull()
        {
            // Arrange
            var emptyString = string.Empty;

            // Act
            var result = DateTimeHelper.ParseDateTime(emptyString);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void FormatAsDate_WithValidDateTime_ReturnsFormattedString()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 1);

            // Act
            var result = DateTimeHelper.FormatAsDate(dateTime);

            // Assert
            Assert.Equal("2024-01-01", result);
        }

        [Fact]
        public void FormatAsDate_WithNullDateTime_ReturnsEmptyString()
        {
            // Act
            var result = DateTimeHelper.FormatAsDate(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void FormatAsDateTime_WithValidDateTime_ReturnsFormattedString()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);

            // Act
            var result = DateTimeHelper.FormatAsDateTime(dateTime);

            // Assert
            Assert.Equal("2024-01-01 12:00:00", result);
        }

        [Fact]
        public void FormatAsDateTime_WithNullDateTime_ReturnsEmptyString()
        {
            // Act
            var result = DateTimeHelper.FormatAsDateTime(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Format_WithValidDateTimeAndFormat_ReturnsFormattedString()
        {
            // Arrange
            var dateTime = new DateTime(2024, 1, 1);
            var format = "MM/dd/yyyy";

            // Act
            var result = DateTimeHelper.Format(dateTime, format);

            // Assert
            Assert.Equal("01/01/2024", result);
        }

        [Fact]
        public void Format_WithNullDateTime_ReturnsEmptyString()
        {
            // Arrange
            var format = "MM/dd/yyyy";

            // Act
            var result = DateTimeHelper.Format(null, format);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData(365, "1 year ago")]
        [InlineData(730, "2 years ago")]
        [InlineData(30, "1 month ago")]
        [InlineData(60, "2 months ago")]
        [InlineData(7, "1 week ago")]
        [InlineData(14, "2 weeks ago")]
        [InlineData(1, "1 day ago")]
        [InlineData(2, "2 days ago")]
        [InlineData(1.0/24, "1 hour ago")]
        [InlineData(2.0/24, "2 hours ago")]
        [InlineData(1.0/1440, "1 minute ago")]
        [InlineData(2.0/1440, "2 minutes ago")]
        [InlineData(10.0/86400, "10 seconds ago")]
        [InlineData(5.0/86400, "Just now")]
        public void GetRelativeTime_WithVariousTimeSpans_ReturnsCorrectString(double daysAgo, string expected)
        {
            // Arrange
            var dateTime = DateTime.Now.AddDays(-daysAgo);

            // Act
            var result = DateTimeHelper.GetRelativeTime(dateTime);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetRelativeTime_WithNullDateTime_ReturnsEmptyString()
        {
            // Act
            var result = DateTimeHelper.GetRelativeTime(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EnsureUtcKind_WithUtcDateTime_ReturnsSameDateTime()
        {
            // Arrange
            var utcDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            // Act
            var result = DateTimeHelper.EnsureUtcKind(utcDateTime);

            // Assert
            Assert.Equal(utcDateTime, result);
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void EnsureUtcKind_WithLocalDateTime_ConvertsToUtc()
        {
            // Arrange
            var localDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local);

            // Act
            var result = DateTimeHelper.EnsureUtcKind(localDateTime);

            // Assert
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void EnsureUtcKind_WithUnspecifiedDateTime_SpecifiesUtc()
        {
            // Arrange
            var unspecifiedDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);

            // Act
            var result = DateTimeHelper.EnsureUtcKind(unspecifiedDateTime);

            // Assert
            Assert.Equal(DateTimeKind.Utc, result.Kind);
        }

        [Fact]
        public void FormatDbDateTimeForDisplay_WithValidUtcDateTime_ReturnsLocalFormattedString()
        {
            // Arrange
            var utcDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var expectedLocalTime = utcDateTime.ToLocalTime();

            // Act
            var result = DateTimeHelper.FormatDbDateTimeForDisplay(utcDateTime);

            // Assert
            Assert.Equal(expectedLocalTime.ToString(DateTimeHelper.DefaultDateTimeFormat), result);
        }

        [Fact]
        public void FormatDbDateTimeForDisplay_WithValidUtcDateTimeAndFormat_ReturnsLocalFormattedString()
        {
            // Arrange
            var utcDateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var format = "MM/dd/yyyy HH:mm";
            var expectedLocalTime = utcDateTime.ToLocalTime();

            // Act
            var result = DateTimeHelper.FormatDbDateTimeForDisplay(utcDateTime, format);

            // Assert
            Assert.Equal(expectedLocalTime.ToString(format), result);
        }

        [Fact]
        public void FormatDbDateTimeForDisplay_WithNullDateTime_ReturnsEmptyString()
        {
            // Act
            var result = DateTimeHelper.FormatDbDateTimeForDisplay(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ParseDateTime_WithValidStringButInvalidFormat_ReturnsParsedDateTime()
        {
            // Arrange
            var dateString = "2024-01-01 12:00:00";
            var invalidFormat = "MM/dd/yyyy"; // This format doesn't match the date string

            // Act
            var result = DateTimeHelper.ParseDateTime(dateString, invalidFormat);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(new DateTime(2024, 1, 1, 12, 0, 0), result.Value);
        }

        [Fact]
        public void FormatDate_WithToday_ReturnsToday()
        {
            // Arrange
            var today = DateTime.Today;

            // Act
            var result = DateTimeHelper.FormatDate(today);

            // Assert
            Assert.Equal("Today", result);
        }

        [Fact]
        public void FormatDate_WithYesterday_ReturnsYesterday()
        {
            // Arrange
            var yesterday = DateTime.Today.AddDays(-1);

            // Act
            var result = DateTimeHelper.FormatDate(yesterday);

            // Assert
            Assert.Equal("Yesterday", result);
        }

        [Fact]
        public void FormatDate_WithDateWithinLastWeek_ReturnsDayOfWeek()
        {
            // Arrange
            var threeDaysAgo = DateTime.Today.AddDays(-3);

            // Act
            var result = DateTimeHelper.FormatDate(threeDaysAgo);

            // Assert
            Assert.Equal(threeDaysAgo.ToString("ddd"), result);
        }

        [Fact]
        public void FormatDate_WithDateMoreThanWeekAgo_ReturnsMonthAndDay()
        {
            // Arrange
            var date = new DateTime(2024, 1, 1);

            // Act
            var result = DateTimeHelper.FormatDate(date);

            // Assert
            Assert.Equal("Jan 1", result);
        }
    }
}
