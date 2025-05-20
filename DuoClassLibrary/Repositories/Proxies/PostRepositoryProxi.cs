using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using System.Text.Json;
using System.Text;
using DuoClassLibrary.Constants;

public class PostRepositoryProxi : IPostRepository, IDisposable
{
    private readonly HttpClient _httpClient;

    public PostRepositoryProxi()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Post>> GetPosts()
    {
        var response = await _httpClient.GetAsync(Enviroment.BaseUrl + "post");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch posts. Status code: {response.StatusCode}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Post>>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result ?? new List<Post>();
    }

    public async Task<int> CreatePost(Post post)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(Enviroment.BaseUrl + "post", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to create post. Status code: {response.StatusCode}");
        }

        var result = await response.Content.ReadAsStringAsync();
        return int.TryParse(result, out int postId) ? postId : 0;
    }

    public async Task UpdatePost(Post post)
    {
        var jsonContent = new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{Enviroment.BaseUrl}post/{post.Id}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to update post. Status code: {response.StatusCode}");
        }
    }

    public async Task DeletePost(int id)
    {
        var response = await _httpClient.DeleteAsync($"{Enviroment.BaseUrl}post/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to delete post. Status code: {response.StatusCode}");
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}