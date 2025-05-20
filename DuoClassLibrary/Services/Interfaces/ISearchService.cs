
namespace DuoClassLibrary.Services.Interfaces
{
    public interface ISearchService
    {
        public double LevenshteinSimilarity(string source, string target);
        public List<string> FindFuzzySearchMatches(string searchQuery, IEnumerable<string> candidateStrings, double similarityThreshold = 0.6);

    }
} 