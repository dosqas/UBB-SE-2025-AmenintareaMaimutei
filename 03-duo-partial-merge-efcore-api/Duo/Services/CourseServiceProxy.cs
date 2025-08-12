using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System;
using System.Threading.Tasks;
using Duo.Models;
using Duo.Services.Interfaces;

namespace Duo.Services
{
    public class CourseServiceProxy : ICourseServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7174";

        public CourseServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Course>> GetAllCourses()
        {
            return await httpClient.GetFromJsonAsync<List<Course>>($"{url}/api/Course/list");
        }

        public async Task<Course> GetCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<Course>($"{url}/api/Course/get?id={courseId}");
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"{url}/api/Tag/get-all");
        }

        public async Task<List<Tag>> GetTagsForCourse(int courseId)
        {
            return await httpClient.GetFromJsonAsync<List<Tag>>($"{url}/api/Course/{courseId}/tags");
        }

        public async Task OpenModule(int userId, int moduleId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Module/add-open-module", new { UserId = userId, ModuleId = moduleId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Module>> GetModulesByCourseId(int courseId)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Module>>($"{url}/api/Module/get-all/by-course/{courseId}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<Module>();
            }
        }

        public async Task<Module> GetModule(int moduleId)
        {
            return await httpClient.GetFromJsonAsync<Module>($"{url}/api/Module/get-module-by-id/{moduleId}");
        }

        public async Task<bool> IsModuleOpen(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/Module/get-open-status?userId={userId}&moduleId={moduleId}");
        }

        public async Task EnrollUser(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Course/enroll", new { userId, courseId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsUserEnrolled(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/Course/is-enrolled?userId={userId}&courseId={courseId}");
        }

        public async Task CompleteModule(int userId, int moduleId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Module/complete", new { UserId = userId, ModuleId = moduleId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsCourseCompleted(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/Course/isCompleted?userId={userId}&courseId={courseId}");
        }

        public async Task MarkCourseAsCompleted(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Course/complete", new { UserId = userId, CourseId = courseId });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateTimeSpent(int userId, int courseId, int seconds)
        {
            var response = await httpClient.PutAsJsonAsync($"{url}/api/Course/update-time", new { userId, courseId, seconds });
            response.EnsureSuccessStatusCode();
        }

        public async Task<int> GetTimeSpent(int userId, int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/Course/get-time?userId={userId}&courseId={courseId}");
        }

        public async Task ClickModuleImage(int userId, int moduleId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Module/clickImage", new { UserId = userId, ModuleId = moduleId });
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsModuleImageClicked(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/Module/imageClicked?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleAvailable(int userId, int moduleId)
        {
            return await httpClient.GetFromJsonAsync<bool>($"{url}/api/Module/isAvailable?userId={userId}&moduleId={moduleId}");
        }

        public async Task<bool> IsModuleCompleted(int userId, int moduleId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<bool>($"{url}/api/Module/is-completed?userId={userId}&moduleId={moduleId}");
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
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/Course/completedModules?userId={userId}&courseId={courseId}");
        }

        public async Task<int> GetRequiredModulesCount(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/Course/requiredModules?courseId={courseId}");
        }

        public async Task<bool> ClaimCompletionReward(int userId, int courseId)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Course/claim-completion", new { UserId = userId, CourseId = courseId });
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<bool> ClaimTimedReward(int userId, int courseId, int timeSpent)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}/api/Course/claim-time", new { UserId = userId, CourseId = courseId, TimeSpent = timeSpent });
            response.EnsureSuccessStatusCode();
            return true;
        }

        public async Task<int> GetCourseTimeLimit(int courseId)
        {
            return await httpClient.GetFromJsonAsync<int>($"{url}/api/Course/timeLimit?courseId={courseId}");
        }

        public async Task<bool> BuyBonusModule(int userId, int moduleId, int courseId)
        {
            var requestContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    CourseId = courseId
                }),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync($"{url}/api/Course/buyBonusModule", requestContent);
            response.EnsureSuccessStatusCode();
            return true;
        }
    }
}
