using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using System.Text.Json;
using System.Text;
using DuoClassLibrary.Constants;

public class CommentRepositoryProxi : ICommentRepository, IDisposable
{
    private readonly HttpClient _httpClient;

    public CommentRepositoryProxi()
    {
        _httpClient = new HttpClient();
    }

    public async Task<Comment?> GetCommentById(int commentId)
    {
        var response = await _httpClient.GetAsync(Enviroment.BaseUrl + $"api/Comment/{commentId}");
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to fetch comment. Status code: {response.StatusCode}");
        var jsonResponse = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Comment>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<List<Comment>> GetCommentsByPostId(int postId)
    {
        try
        {
            var url = Enviroment.BaseUrl + $"api/Comment/ByPost/{postId}";
            
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
         
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch comments. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}, Content: {responseContent}");
            }

            if (string.IsNullOrEmpty(responseContent))
            {
                Console.WriteLine("Empty response received from API");
                return new List<Comment>();
            }

            try
            {
                var result = JsonSerializer.Deserialize<List<Comment>>(responseContent, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                });

                return result ?? new List<Comment>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization error: {ex.Message}");
                Console.WriteLine($"Problematic JSON content: {responseContent}");
                throw new Exception($"Failed to deserialize comments. Content: {responseContent}", ex);
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP request error: {ex.Message}");
            throw new Exception($"Network error while fetching comments: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            throw new Exception($"An error occurred while fetching comments: {ex.Message}", ex);
        }
    }

    public async Task<int> CreateComment(Comment comment)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(comment), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(Enviroment.BaseUrl + "api/comment", jsonContent);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to create comment. Status code: {response.StatusCode}");
        var result = await response.Content.ReadAsStringAsync();
        return int.TryParse(result, out int commentId) ? commentId : 0;
    }

    public async Task DeleteComment(int id)
    {
        var response = await _httpClient.DeleteAsync(Enviroment.BaseUrl + $"api/comment/{id}");
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to delete comment. Status code: {response.StatusCode}");
    }

    public async Task<List<Comment>> GetRepliesByCommentId(int parentCommentId)
    {
        var response = await _httpClient.GetAsync(Enviroment.BaseUrl + $"api/comment/Replies/{parentCommentId}");
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to fetch replies. Status code: {response.StatusCode}");
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Comment>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return result ?? new List<Comment>();
    }

    public async Task IncrementLikeCount(int commentId)
    {
        var response = await _httpClient.PostAsync(Enviroment.BaseUrl + $"api/comment/Like/{commentId}", null);
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to like comment. Status code: {response.StatusCode}");
    }

    public async Task<int> GetCommentsCountForPost(int postId)
    {
        var response = await _httpClient.GetAsync(Enviroment.BaseUrl + $"api/comment/Count/{postId}");
        if (!response.IsSuccessStatusCode)
            throw new Exception($"Failed to fetch comment count. Status code: {response.StatusCode}");
        var result = await response.Content.ReadAsStringAsync();
        return int.TryParse(result, out int count) ? count : 0;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
} 