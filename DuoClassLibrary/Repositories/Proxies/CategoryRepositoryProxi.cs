using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using System.Text.Json;
using DuoClassLibrary.Constants;

public class CategoryRepositoryProxi : ICategoryRepository, IDisposable
{
    private readonly HttpClient _httpClient;

    public CategoryRepositoryProxi()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync(DuoClassLibrary.Constants.Environment.BaseUrl + "category");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch categories. Status code: {response.StatusCode}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<Category>>(jsonResponse, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}