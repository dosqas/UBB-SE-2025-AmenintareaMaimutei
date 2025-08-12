using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DuoClassLibrary.Helpers
{
    public static class ValidationHelper
    {
        public static bool ValidateNotNullOrEmpty(string stringToCheck, string parameterName)
        {
            if (string.IsNullOrEmpty(stringToCheck))
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.");
            }
            return true;
        }

        public static bool ValidateRange<T>(T valueToCheck, T min, T max, string parameterName) where T : IComparable<T>
        {
            if (valueToCheck.CompareTo(min) < 0 || valueToCheck.CompareTo(max) > 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"{parameterName} must be between {min} and {max}.");
            }
            return true;
        }

        public static bool ValidateCollectionNotEmpty<T>(ICollection<T> collectionToCheck, string parameterName)
        {
            if (collectionToCheck == null || collectionToCheck.Count == 0)
            {
                throw new ArgumentException($"{parameterName} cannot be null or empty.");
            }
            return true;
        }

        public static bool ValidateCondition(bool conditionToCheck, string errorMessage)
        {
            if (!conditionToCheck)
            {
                throw new ArgumentException(errorMessage);
            }
            return true;
        }

        public static bool ValidatePost(string contentToCheck, string? title = null)
        {

            ValidateNotNullOrEmpty(contentToCheck, nameof(contentToCheck));

            ValidateRange(contentToCheck.Length, 1, 4000, "Post content length");

            if (title != null)
            {
                ValidateRange(title.Length, 1, 100, "Post title length");
            }

            return true;
        }

        public static bool ValidateComment(string commentContent)
        {

            ValidateNotNullOrEmpty(commentContent, nameof(commentContent));

            ValidateRange(commentContent.Length, 1, 1000, "Comment length");

            return true;
        }

        public static bool ValidateHashtag(string hashtagToValidate)
        {

            ValidateNotNullOrEmpty(hashtagToValidate, nameof(hashtagToValidate));

            ValidateCondition(!string.IsNullOrEmpty(hashtagToValidate), "Hashtag cannot be just a # symbol.");

            ValidateCondition(Regex.IsMatch(hashtagToValidate, @"^[a-zA-Z0-9]+$"), 
                "Hashtag can only contain letters and numbers.");

            ValidateRange(hashtagToValidate.Length, 1, 30, "Hashtag length");

            return true;
        }

        public static bool ValidateUsername(string usernameToValidate)
        {
            ValidateNotNullOrEmpty(usernameToValidate, nameof(usernameToValidate));

            ValidateRange(usernameToValidate.Length, 1, 30, "Username length");

            ValidateCondition(Regex.IsMatch(usernameToValidate, @"^[a-zA-Z0-9]+$"), 
                "Username can only contain letters and numbers.");

            ValidateCondition(!usernameToValidate.Contains(" "), "Username cannot contain spaces.");

            return true;
        }


        public static (bool IsValid, string ErrorMessage) ValidatePostTitle(string postTitle)
        {
            if (string.IsNullOrWhiteSpace(postTitle))
            {
                return (false, "Title cannot be empty.");
            }
            
            if (postTitle.Length < 3)
            {
                return (false, "Title should be at least 3 characters long.");
            }

            if (postTitle.Length > 50)
            {
                return (false, "Title cannot exceed 50 characters.");
            }
            
            return (true, string.Empty);
        }

        public static (bool IsValid, string ErrorMessage) ValidatePostContent(string postContent)
        {
            if (string.IsNullOrWhiteSpace(postContent))
            {
                return (false, "Content cannot be empty.");
            }
            
            if (postContent.Length < 10)
            {
                return (false, "Content should be at least 10 characters long.");
            }
            
            if (postContent.Length > 4000)
            {
                return (false, "Content cannot exceed 4000 characters.");
            }
            
            return (true, string.Empty);
        }

        public static (bool IsValid, string ErrorMessage) ValidateHashtagInput(string newInputHashtag)
        {
            if (string.IsNullOrWhiteSpace(newInputHashtag))
            {
                return (true, string.Empty); 
            }
            
            string cleanHashtag = newInputHashtag.StartsWith("#") ? newInputHashtag.Substring(1) : newInputHashtag;
            
            if (string.IsNullOrWhiteSpace(cleanHashtag))
            {
                return (false, "Hashtag cannot be just a # symbol.");
            }
            
            if (!Regex.IsMatch(cleanHashtag, @"^[a-zA-Z0-9]+$"))
            {
                return (false, "Hashtag can only contain letters and numbers.");
            }
            
            if (cleanHashtag.Length > 30)
            {
                return (false, "Hashtag cannot exceed 30 characters.");
            }
            
            return (true, string.Empty);
        }

    }
}