using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Models.Quizzes;

namespace Duo.Services
{
    public class UserServiceProxy : IUserServiceProxy
    {
        private readonly HttpClient httpClient;
        private const string BaseUrl = "https://localhost:7174/api/User";

        public UserServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var response = await httpClient.GetAsync($"{BaseUrl}/{userId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty.", nameof(username));
            }

            var response = await httpClient.GetAsync(BaseUrl);
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            return users.Find(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<int> CreateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var response = await httpClient.PostAsJsonAsync($"{BaseUrl}/register", user);
            response.EnsureSuccessStatusCode();

            var createdUser = await response.Content.ReadFromJsonAsync<User>();
            return createdUser.UserId;
        }

        public async Task UpdateUserSectionProgressAsync(int userId, int newNrOfSectionsCompleted, int newNrOfQuizzesInSectionCompleted)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var user = await GetByIdAsync(userId);
            user.NumberOfCompletedSections = newNrOfSectionsCompleted;
            user.NumberOfCompletedQuizzesInSection = newNrOfQuizzesInSectionCompleted;

            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task IncrementUserProgressAsync(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));
            }

            var user = await GetByIdAsync(userId);
            user.NumberOfCompletedQuizzesInSection++;

            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/update", user);
            response.EnsureSuccessStatusCode();
        }
    }
}
