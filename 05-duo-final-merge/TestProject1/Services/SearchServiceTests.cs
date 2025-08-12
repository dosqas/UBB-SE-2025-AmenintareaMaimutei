using System;
using System.Collections.Generic;
using System.Linq;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Xunit;
using Moq;
using Duo.Services;

namespace TestProject1.Services
{
    public class SearchServiceTests
    {
        private readonly ISearchService _searchService;
        private readonly Mock<ISearchService> _mockSearchService;

        // Similarity threshold constants
        private const double EXACT_MATCH_THRESHOLD = 1.0;
        private const double HIGH_SIMILARITY_THRESHOLD = 0.8;
        private const double MEDIUM_SIMILARITY_THRESHOLD = 0.5;
        private const double LOW_SIMILARITY_THRESHOLD = 0.3;
        private const double DEFAULT_SIMILARITY_THRESHOLD = 0.6;
        private const double STRICT_SIMILARITY_THRESHOLD = 0.9;

        // Test query strings
        private const string BASE_QUERY = "test";
        private const string LONG_QUERY = "testing";
        private const string EMPTY_QUERY = "";
        private const string SUBSTRING_QUERY = "te";

        // Levenshtein test strings
        private const string SOURCE_STRING = "kitten";
        private const string TARGET_STRING = "sitting";
        private const string DIFFERENT_STRING_1 = "abc";
        private const string DIFFERENT_STRING_2 = "xyz";

        // Test word pairs
        private static readonly string[] SIMILAR_WORDS = { "test", "tst" };
        private static readonly string[] CONTAINED_SUBSTRINGS = { "te", "ing" };
        private static readonly string[] MULTI_WORD_STRINGS = { "test world", "world test" };

        // Test candidate collections
        private static readonly List<string> TEST_CANDIDATES = new()
        { 
            SIMILAR_WORDS[0],     // Exact match
            SIMILAR_WORDS[1],     // Similar word
            LONG_QUERY,          // Contains query
            MULTI_WORD_STRINGS[0], // Multi-word with exact match
            "hello " + SIMILAR_WORDS[1],  // Multi-word with similar word
            DIFFERENT_STRING_2,   // No match
            SUBSTRING_QUERY       // Shorter string contained in query
        };

        public SearchServiceTests()
        {
            _mockSearchService = new Mock<ISearchService>();
            _searchService = new SearchService();
        }

        #region LevenshteinSimilarity Tests

        [Theory]
        [InlineData(SOURCE_STRING, TARGET_STRING, true)]     // Similar strings
        [InlineData(BASE_QUERY, BASE_QUERY, true)]          // Identical strings
        [InlineData(EMPTY_QUERY, EMPTY_QUERY, true)]        // Empty strings
        [InlineData(DIFFERENT_STRING_1, DIFFERENT_STRING_2, false)]  // Different strings
        [InlineData(BASE_QUERY, EMPTY_QUERY, false)]        // One empty string
        public void LevenshteinSimilarity_ReturnsExpectedSimilarity(string sourceText, string targetText, bool shouldBeHighSimilarity)
        {
            // Act
            double similarityScore = _searchService.LevenshteinSimilarity(sourceText, targetText);

            // Assert
            if (shouldBeHighSimilarity)
            {
                Assert.True(similarityScore > MEDIUM_SIMILARITY_THRESHOLD);
            }
            else
            {
                Assert.True(similarityScore <= MEDIUM_SIMILARITY_THRESHOLD);
            }
        }

        #endregion

        #region FindFuzzySearchMatches Tests

        [Theory]
        [InlineData(null)]
        [InlineData(EMPTY_QUERY)]
        public void FindFuzzySearchMatches_WithInvalidQuery_ReturnsEmptyList(string searchQuery)
        {
            // Act
            var matchResults = _searchService.FindFuzzySearchMatches(searchQuery, TEST_CANDIDATES);

            // Assert
            Assert.Empty(matchResults);
        }

        [Fact]
        public void FindFuzzySearchMatches_WithValidQuery_ReturnsMatchesInCorrectOrder()
        {
            // Act
            var matchResults = _searchService.FindFuzzySearchMatches(BASE_QUERY, TEST_CANDIDATES).ToList();

            // Assert
            Assert.NotEmpty(matchResults);
            
            // Verify exact match is first
            Assert.Equal(SIMILAR_WORDS[0], matchResults.First());
            
            // Verify expected matches are included
            Assert.Contains(LONG_QUERY, matchResults);
            Assert.Contains(MULTI_WORD_STRINGS[0], matchResults);
            
            // Verify non-matches are excluded
            Assert.DoesNotContain(DIFFERENT_STRING_2, matchResults);

            // Verify ordering (exact matches before similar matches)
            if (matchResults.Contains(SIMILAR_WORDS[0]) && matchResults.Contains(SIMILAR_WORDS[1]))
            {
                Assert.True(matchResults.IndexOf(SIMILAR_WORDS[0]) < matchResults.IndexOf(SIMILAR_WORDS[1]));
            }
        }

        [Fact]
        public void FindFuzzySearchMatches_WithCustomThreshold_RespectsThreshold()
        {
            // Act
            var strictMatchResults = _searchService.FindFuzzySearchMatches(BASE_QUERY, TEST_CANDIDATES, STRICT_SIMILARITY_THRESHOLD);
            var looseMatchResults = _searchService.FindFuzzySearchMatches(BASE_QUERY, TEST_CANDIDATES, LOW_SIMILARITY_THRESHOLD);

            // Assert
            Assert.True(strictMatchResults.Count < looseMatchResults.Count);
            Assert.Contains(SIMILAR_WORDS[0], strictMatchResults);
            Assert.Contains(SIMILAR_WORDS[1], looseMatchResults);
        }

        [Fact]
        public void FindFuzzySearchMatches_WhenQueryContainsCandidate_IncludesMatch()
        {
            // Arrange
            var substringsToMatch = new List<string> { SIMILAR_WORDS[0], CONTAINED_SUBSTRINGS[0], CONTAINED_SUBSTRINGS[1] };

            // Act
            var matchResults = _searchService.FindFuzzySearchMatches(LONG_QUERY, substringsToMatch).ToList();

            // Assert
            Assert.Contains(CONTAINED_SUBSTRINGS[0], matchResults);
            Assert.Contains(CONTAINED_SUBSTRINGS[1], matchResults);
        }

        #endregion
    }
} 