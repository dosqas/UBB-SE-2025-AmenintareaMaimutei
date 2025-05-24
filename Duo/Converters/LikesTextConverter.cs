using Microsoft.UI.Xaml.Data;
using System;

namespace Duo.Converters
{
    public class LikesTextConverter : IValueConverter
    {
        // Constants for text formatting
        private const string LIKES_FORMAT = "{0} likes";
        private const string DEFAULT_LIKES_TEXT = "0 likes";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Check if value is an integer
            if (value is int likeCount)
            {
                // Format the likes count
                return string.Format(LIKES_FORMAT, likeCount);
            }

            // Return default text for non-integer values
            return DEFAULT_LIKES_TEXT;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 