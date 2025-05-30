using System;
using System.Collections.Generic;
using Xunit;
using Duo.Helpers;
using DuoClassLibrary.Helpers;

namespace TestProject1.Helpers
{
    public class ValidationHelperTests
    {
        [Fact]
        public void ValidateNotNullOrEmpty_ValidString_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateNotNullOrEmpty("test", "testParam");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateNotNullOrEmpty_NullString_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateNotNullOrEmpty(null, "testParam"));
            Assert.Contains("testParam cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateNotNullOrEmpty_EmptyString_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateNotNullOrEmpty("", "testParam"));
            Assert.Contains("testParam cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateRange_ValidValue_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateRange(5, 1, 10, "testParam");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateRange_ValueBelowMin_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                ValidationHelper.ValidateRange(0, 1, 10, "testParam"));
            Assert.Contains("testParam must be between 1 and 10", exception.Message);
        }

        [Fact]
        public void ValidateRange_ValueAboveMax_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                ValidationHelper.ValidateRange(11, 1, 10, "testParam"));
            Assert.Contains("testParam must be between 1 and 10", exception.Message);
        }

        [Fact]
        public void ValidateCollectionNotEmpty_ValidCollection_ReturnsTrue()
        {
            // Arrange
            var collection = new List<int> { 1, 2, 3 };

            // Act
            var result = ValidationHelper.ValidateCollectionNotEmpty(collection, "testParam");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCollectionNotEmpty_NullCollection_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateCollectionNotEmpty<int>(null, "testParam"));
            Assert.Contains("testParam cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateCollectionNotEmpty_EmptyCollection_ThrowsArgumentException()
        {
            // Arrange
            var collection = new List<int>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateCollectionNotEmpty(collection, "testParam"));
            Assert.Contains("testParam cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateCondition_TrueCondition_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateCondition(true, "error message");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateCondition_FalseCondition_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateCondition(false, "error message"));
            Assert.Contains("error message", exception.Message);
        }

        [Fact]
        public void ValidatePost_ValidPost_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidatePost("Valid content", "Valid title");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePost_NullContent_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidatePost(null, "title"));
            Assert.Contains("contentToCheck cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidatePost_EmptyContent_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidatePost("", "title"));
            Assert.Contains("contentToCheck cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidatePost_ContentTooLong_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var longContent = new string('x', 4001);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                ValidationHelper.ValidatePost(longContent, "title"));
            Assert.Contains("Post content length must be between 1 and 4000", exception.Message);
        }

        [Fact]
        public void ValidatePost_TitleTooLong_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var longTitle = new string('x', 101);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                ValidationHelper.ValidatePost("content", longTitle));
            Assert.Contains("Post title length must be between 1 and 100", exception.Message);
        }

        [Fact]
        public void ValidateComment_ValidComment_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateComment("Valid comment");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateComment_NullComment_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateComment(null));
            Assert.Contains("commentContent cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateComment_EmptyComment_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateComment(""));
            Assert.Contains("commentContent cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateComment_CommentTooLong_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var longComment = new string('x', 1001);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                ValidationHelper.ValidateComment(longComment));
            Assert.Contains("Comment length must be between 1 and 1000", exception.Message);
        }

        [Fact]
        public void ValidateHashtag_ValidHashtag_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateHashtag("ValidHashtag123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateHashtag_NullHashtag_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateHashtag(null));
            Assert.Contains("hashtagToValidate cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateHashtag_EmptyHashtag_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateHashtag(""));
            Assert.Contains("hashtagToValidate cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateHashtag_InvalidCharacters_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateHashtag("Invalid@Hashtag"));
            Assert.Contains("Hashtag can only contain letters and numbers", exception.Message);
        }

        [Fact]
        public void ValidateHashtag_TooLong_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var longHashtag = new string('x', 31);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                ValidationHelper.ValidateHashtag(longHashtag));
            Assert.Contains("Hashtag length must be between 1 and 30", exception.Message);
        }

        [Fact]
        public void ValidateUsername_ValidUsername_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateUsername("ValidUsername123");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidateUsername_NullUsername_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateUsername(null));
            Assert.Contains("usernameToValidate cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateUsername_EmptyUsername_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateUsername(""));
            Assert.Contains("usernameToValidate cannot be null or empty", exception.Message);
        }

        [Fact]
        public void ValidateUsername_InvalidCharacters_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateUsername("Invalid@Username"));
            Assert.Contains("Username can only contain letters and numbers", exception.Message);
        }

        [Fact]
        public void ValidateUsername_ContainsSpace_ThrowsArgumentException()
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => 
                ValidationHelper.ValidateUsername("Invalid Username"));
            Assert.Contains("Username can only contain letters and numbers", exception.Message);
        }

        [Fact]
        public void ValidateUsername_TooLong_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var longUsername = new string('x', 31);

            // Act & Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
                ValidationHelper.ValidateUsername(longUsername));
            Assert.Contains("Username length must be between 1 and 30", exception.Message);
        }

        [Fact]
        public void ValidatePostTitle_ValidTitle_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidatePostTitle("Valid Title");

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostTitle_NullTitle_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidatePostTitle(null);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Title cannot be empty", result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostTitle_EmptyTitle_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidatePostTitle("");

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Title cannot be empty", result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostTitle_TooShort_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidatePostTitle("ab");

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Title should be at least 3 characters long", result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostTitle_TooLong_ReturnsFalse()
        {
            // Arrange
            var longTitle = new string('x', 51);

            // Act
            var result = ValidationHelper.ValidatePostTitle(longTitle);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Title cannot exceed 50 characters", result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostContent_ValidContent_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidatePostContent("Valid content that is long enough");

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostContent_NullContent_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidatePostContent(null);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Content cannot be empty", result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostContent_EmptyContent_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidatePostContent("");

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Content cannot be empty", result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostContent_TooShort_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidatePostContent("Too short");

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Content should be at least 10 characters long", result.ErrorMessage);
        }

        [Fact]
        public void ValidatePostContent_TooLong_ReturnsFalse()
        {
            // Arrange
            var longContent = new string('x', 4001);

            // Act
            var result = ValidationHelper.ValidatePostContent(longContent);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Content cannot exceed 4000 characters", result.ErrorMessage);
        }

        [Fact]
        public void ValidateHashtagInput_ValidHashtag_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateHashtagInput("ValidHashtag123");

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        [Fact]
        public void ValidateHashtagInput_NullHashtag_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateHashtagInput(null);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        [Fact]
        public void ValidateHashtagInput_EmptyHashtag_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateHashtagInput("");

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }

        [Fact]
        public void ValidateHashtagInput_OnlyHashSymbol_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidateHashtagInput("#");

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Hashtag cannot be just a # symbol", result.ErrorMessage);
        }

        [Fact]
        public void ValidateHashtagInput_InvalidCharacters_ReturnsFalse()
        {
            // Act
            var result = ValidationHelper.ValidateHashtagInput("Invalid@Hashtag");

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Hashtag can only contain letters and numbers", result.ErrorMessage);
        }

        [Fact]
        public void ValidateHashtagInput_TooLong_ReturnsFalse()
        {
            // Arrange
            var longHashtag = new string('x', 31);

            // Act
            var result = ValidationHelper.ValidateHashtagInput(longHashtag);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains("Hashtag cannot exceed 30 characters", result.ErrorMessage);
        }

        [Fact]
        public void ValidateHashtagInput_HashtagWithHash_ReturnsTrue()
        {
            // Act
            var result = ValidationHelper.ValidateHashtagInput("#ValidHashtag123");

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.ErrorMessage);
        }
    }
} 