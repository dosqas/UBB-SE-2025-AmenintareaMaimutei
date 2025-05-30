using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System;
using System.Threading.Tasks;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services.Interfaces;
using DuoClassLibrary.Constants;

namespace DuoClassLibrary.Services
{
    public class CourseServiceProxy : ICourseServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = Constants.Environment.BaseUrl;

        public CourseServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Course>> GetAllCourses()
        {
            return await httpClient.GetFromJsonAsync<List<Course>>($"{url}Course/list");
        }

        public async Task<Course> GetCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<Course>($"{url}Course/get?id={courseId}");
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"{url}Tag/list");
        }

        public async Task<List<Tag>> GetTagsForCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"{url}Course/{courseId}/tags");
        }

        public async Task OpenModule(int userId, int moduleId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Module/open", new { UserId = userId, ModuleId = moduleId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Module>> GetModulesByCourseId(int courseId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Module>>($"{url}Module/list/by-course/{courseId}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<Module>();
            }
        }

        public async Task<Module> GetModule(int moduleId)
        {
            return await httpClient.GetFromJsonAsync<Module>($"{url}Module/{moduleId}");
        }

        public async Task<bool> IsModuleOpen(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}Module/isOpen?userId={userId}&moduleId={moduleId}");
        }

        public async Task EnrollUser(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Course/enroll", new { userId, courseId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsUserEnrolled(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}Course/is-enrolled?userId={userId}&courseId={courseId}");
        }

        public async Task CompleteModule(int userId, int moduleId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Module/complete", new { UserId = userId, ModuleId = moduleId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsCourseCompleted(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}Course/is-completed?userId={userId}&courseId={courseId}");
        }

        public async Task MarkCourseAsCompleted(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Course/complete", new { UserId = userId, CourseId = courseId });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTimeSpent(int userId, int courseId, int seconds)
        {
            var response = await httpClient.PutAsJsonAsync($"{url}Course/update-time", new { userId, courseId, seconds });
            response.EnsureSuccessStatusCode();
        }

        public async Task<int> GetTimeSpent(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}Course/get-time?userId={userId}&courseId={courseId}");
        }

        public async Task ClickModuleImage(int userId, int moduleId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Module/clickImage", new { UserId = userId, ModuleId = moduleId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsModuleImageClicked(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}Module/imageClicked?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleAvailable(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}Module/isAvailable?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleCompleted(int userId, int moduleId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<bool>($"{url}Module/is-completed?userId={userId}&moduleId={moduleId}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to check IsModuleCompleted: {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetCompletedModulesCount(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}Course/completedModules?userId={userId}&courseId={courseId}");
        }

        public async Task<int> GetRequiredModulesCount(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}Course/requiredModules?courseId={courseId}");
        }

        public async Task<bool> ClaimCompletionReward(int userId, int courseId, int coins)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Course/claim-completion", new { UserId = userId, CourseId = courseId, Coins = coins });
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> ClaimTimedReward(int userId, int courseId, int timeSpent, int coins)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Course/claim-time", new { UserId = userId, CourseId = courseId, TimeSpent = timeSpent, Coins = coins });
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<int> GetCourseTimeLimit(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}Course/timeLimit?courseId={courseId}");
        }

        public async Task<bool> BuyBonusModule(int userId, int moduleId)
        {
            var requestContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    UserId = userId,
                    ModuleId = moduleId,
                }),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync($"{url}Course/buyBonusModule", requestContent);
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
