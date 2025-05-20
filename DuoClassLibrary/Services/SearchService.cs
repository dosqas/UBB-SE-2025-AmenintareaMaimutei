using System;
using System.Collections.Generic;
using System.Linq;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.Services
{
    public class SearchService : ISearchService
    {
        // Similarity score thresholds
        private const double EXACT_MATCH_THRESHOLD = 0.9;
        private const double HIGH_SIMILARITY_THRESHOLD = 0.8;
        private const double DEFAULT_SIMILARITY_THRESHOLD = 0.6;

        // Tuple field names for better readability
        private const string MATCH_TEXT_FIELD = "Text";
        private const string MATCH_SCORE_FIELD = "Score";

        public double LevenshteinSimilarity(string source, string target)
        {
            // Initialize the distance matrix
            int[,] distanceMatrix = new int[source.Length + 1, target.Length + 1];

            // Initialize first row and column
            for (int sourceIndex = 0; sourceIndex <= source.Length; sourceIndex++)
                distanceMatrix[sourceIndex, 0] = sourceIndex;
            for (int targetIndex = 0; targetIndex <= target.Length; targetIndex++)
                distanceMatrix[0, targetIndex] = targetIndex;

            // Calculate minimum edit distance
            for (int sourceIndex = 1; sourceIndex <= source.Length; sourceIndex++)
            {
                for (int targetIndex = 1; targetIndex <= target.Length; targetIndex++)
                {
                    int substitutionCost = (source[sourceIndex - 1] == target[targetIndex - 1]) ? 0 : 1;

                    distanceMatrix[sourceIndex, targetIndex] = Math.Min(
                        Math.Min(
                            distanceMatrix[sourceIndex - 1, targetIndex] + 1,     // deletion
                            distanceMatrix[sourceIndex, targetIndex - 1] + 1),    // insertion
                        distanceMatrix[sourceIndex - 1, targetIndex - 1] + substitutionCost); // substitution
                }
            }

            // Calculate similarity score
            int maxStringLength = Math.Max(source.Length, target.Length);
            if (maxStringLength == 0) return 1.0; // Both strings empty = perfect match

            int levenshteinDistance = distanceMatrix[source.Length, target.Length];
            return 1.0 - ((double)levenshteinDistance / maxStringLength);
        }

        public List<string> FindFuzzySearchMatches(string searchQuery, IEnumerable<string> candidateStrings, double similarityThreshold = DEFAULT_SIMILARITY_THRESHOLD)
        {
            // Handle empty or null search query
            if (string.IsNullOrEmpty(searchQuery))
                return new List<string>();

            // Normalize input for case-insensitive comparison
            string normalizedQuery = searchQuery.ToLower();
            var matchesWithScores = new List<(string Text, double Score)>();

            foreach (var candidate in candidateStrings)
            {
                string normalizedCandidate = candidate.ToLower();
                double similarityScore = LevenshteinSimilarity(normalizedQuery, normalizedCandidate);

                // Check for high similarity match
                if (similarityScore >= HIGH_SIMILARITY_THRESHOLD)
                {
                    matchesWithScores.Add((candidate, similarityScore));
                    continue;
                }

                // Check for substring containment
                if (normalizedCandidate.Contains(normalizedQuery))
                {
                    matchesWithScores.Add((candidate, EXACT_MATCH_THRESHOLD));
                    continue;
                }

                // Check if query contains the candidate
                if (normalizedQuery.Contains(normalizedCandidate))
                {
                    matchesWithScores.Add((candidate, HIGH_SIMILARITY_THRESHOLD));
                    continue;
                }

                // Handle multi-word candidates
                if (candidate.Contains(" "))
                {
                    string[] candidateWords = normalizedCandidate.Split(' ');
                    foreach (var word in candidateWords)
                    {
                        double wordSimilarityScore = LevenshteinSimilarity(normalizedQuery, word);
                        if (wordSimilarityScore >= similarityThreshold)
                        {
                            matchesWithScores.Add((candidate, wordSimilarityScore));
                            break;
                        }
                    }
                    continue;
                }

                // Check against similarity threshold
                if (similarityScore >= similarityThreshold)
                {
                    matchesWithScores.Add((candidate, similarityScore));
                }
            }

            // Process and sort matches
            return matchesWithScores
                .GroupBy(match => match.Text)  // Remove duplicates
                .Select(group => group.OrderByDescending(match => match.Score).First())  // Keep highest score
                .OrderByDescending(match => match.Score)  // Sort by score
                .Select(match => match.Text)  // Extract text
                .ToList();
        }
    }
}