using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DuoClassLibrary.Constants;
using DuoClassLibrary.Models;
using DuoClassLibrary.Models.Sections;
using DuoClassLibrary.Models.Sections.DTO;
using DuoClassLibrary.Services.Interfaces;

namespace DuoClassLibrary.Services
{
    /// <summary>
    /// Provides methods to interact with the Sections API.
    /// </summary>
    /// <remarks>
    /// Implements <see cref="ISectionServiceProxy"/> so that callers
    /// can depend on the interface and tests can inject mocks.
    /// </remarks>
    public class SectionServiceProxy : ISectionServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = Constants.Environment.BaseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionServiceProxy"/> class.
        /// </summary>
        /// <param name="httpClient">HTTP client used to call the backend API.</param>
        public SectionServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<int> AddSection(Section section)
        {
            SectionDTO dto = SectionDTO.ToDto(section);
            string json = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(
                    $"{url}Section/add",
                    content).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadFromJsonAsync<SectionAddResponse>().ConfigureAwait(false);

            if (responseBody == null)
            {
                throw new InvalidOperationException("Empty or invalid response from server.");
            }

            return responseBody.Id;
        }

        public async Task<int> CountSectionsFromRoadmap(int roadmapId)
        {
            var response = await httpClient.GetAsync($"{url}Section/count-on-roadmap?roadmapId={roadmapId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task DeleteSection(int sectionId)
        {
            var response = await httpClient.DeleteAsync($"{url}Section/{sectionId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Section>> GetAllSections()
        {
            var list = await httpClient
                    .GetFromJsonAsync<List<Section>>($"{url}Section/list")
                    .ConfigureAwait(false);
            if (list == null)
            {
                throw new InvalidOperationException("Empty or invalid response from server.");
            }
            return list;
        }

        public async Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            var response = await httpClient.GetAsync($"{url}Section/list/roadmap/{roadmapId}");
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseJson);
            var result = doc.RootElement.GetProperty("result");
            var sections = result.Deserialize<List<Section>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            });
            return sections ?? new List<Section>();
        }

        public async Task<Section> GetSectionById(int sectionId)
        {
            var response = await httpClient.GetAsync($"{url}Section/{sectionId}?id={sectionId}");
            response.EnsureSuccessStatusCode();
            string s = await response.Content.ReadAsStringAsync();
            var section = await response.Content.ReadFromJsonAsync<Section>();
            if (section == null)
            {
                throw new Exception($"Section with ID {sectionId} not found.");
            }
            return section;
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            var response = await httpClient.GetAsync($"{url}Section/last-from-roadmap?roadmapId={roadmapId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateSection(Section section)
        {
            var response = await httpClient.PutAsJsonAsync($"{url}Section/patch", section);
            response.EnsureSuccessStatusCode();
        }

        public Task<bool> IsSectionCompleted(int userId, int sectionId)
        {
            var response = httpClient.GetFromJsonAsync<SectionCompletionDTO>($"{url}Section/is-completed?userId={userId}&sectionId={sectionId}");
            //Get boolean result from response
            if (response.Result == null)
            {
                throw new InvalidOperationException("Empty or invalid response from server.");
            }

            return Task.FromResult(response.Result.IsCompleted);
        }

        public async Task CompleteSection(int userId, int sectionId)
        {
            var response = httpClient.PostAsJsonAsync($"{url}Section/add-completed-section?userId={userId}&sectionId={sectionId}", new { });
            response.Result.EnsureSuccessStatusCode();
        }
    }
}
