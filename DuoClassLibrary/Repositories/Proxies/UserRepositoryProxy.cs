using DuoClassLibrary.Models;
using DuoClassLibrary.Repositories.Interfaces;
using System.Text.Json;
using System.Text;
using DuoClassLibrary.Constants;
using System.Net.Http;
using System.Net.Http.Json;

namespace DuoClassLibrary.Repositories.Proxies
{
    public class UserRepositoryProxy : IUserRepository, IDisposable
    {
        private readonly HttpClient _httpClient;

        public UserRepositoryProxy()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting to connect to API at: {Constants.Environment.BaseUrl}user");
                var response = await _httpClient.GetAsync(Constants.Environment.BaseUrl + "User");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error Response: {errorContent}");
                    throw new Exception($"Failed to fetch users. Status code: {response.StatusCode}, Message: {errorContent}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {jsonResponse}");
                var result = JsonSerializer.Deserialize<List<User>>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result ?? new List<User>();
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"Request timed out: {ex.Message}");
                throw new Exception($"Request to API timed out. Please check if the API is running at {Constants.Environment.BaseUrl}", ex);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                throw new Exception($"Failed to connect to the API. Please make sure the API is running at {Constants.Environment.BaseUrl}. Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw new Exception($"An error occurred while fetching users: {ex.Message}", ex);
            }
        }

        public async Task<User> GetUser(int id)
        {
            var response = await _httpClient.GetAsync($"{Constants.Environment.BaseUrl}User/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch user. Status code: {response.StatusCode}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<User>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? throw new Exception("User not found");
        }

        public async Task<int> CreateUser(User user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Constants.Environment.BaseUrl + "User", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to create user. Status code: {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();
            return int.TryParse(result, out int userId) ? userId : 0;
        }

        public async Task UpdateUser(User user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{Constants.Environment.BaseUrl}User/{user.UserId}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update user. Status code: {response.StatusCode}");
            }
        }

        public async Task DeleteUser(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Constants.Environment.BaseUrl}User/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete user. Status code: {response.StatusCode}");
            }
        }


        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var response = await _httpClient.PutAsJsonAsync($"{Constants.Environment.BaseUrl}/update", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var user = await GetUser(userId);
            user.NumberOfCompletedSections = newNrOfSectionsCompleted;
            user.NumberOfCompletedQuizzesInSection = newNrOfQuizzesInSectionCompleted;

            var response = await _httpClient.PutAsJsonAsync($"{Constants.Environment.BaseUrl}/update", user);
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
} 