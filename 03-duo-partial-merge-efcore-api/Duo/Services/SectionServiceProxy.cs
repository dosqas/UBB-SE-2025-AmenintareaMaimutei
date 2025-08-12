using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure;
using Duo.Models.Roadmap;
using Duo.Models.Sections;
using Duo.Models.Sections.DTO;
using Duo.Services.Interfaces;

namespace Duo.Services
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
        private readonly string url = "https://localhost:7174";

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
            var response = await this.httpClient.PostAsync(
                    $"{this.url}/api/Section/add",
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
            var response = await httpClient.GetAsync($"{url}/api/Section/get-section-count-on-roadmap");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task DeleteSection(int sectionId)
        {
            var response = await this.httpClient.DeleteAsync($"{url}/api/Section/{sectionId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Section>> GetAllSections()
        {
            var list = await this.httpClient
                    .GetFromJsonAsync<List<Section>>($"{url}/api/Section/get-all")
                    .ConfigureAwait(false);
            if (list == null)
            {
                throw new InvalidOperationException("Empty or invalid response from server.");
            }
            return list;
        }

        public async Task<List<Section>> GetByRoadmapId(int roadmapId)
        {
            var response = await this.httpClient.GetAsync($"{this.url}/api/Section/list/roadmap/{roadmapId}");
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseJson);
            var result = doc.RootElement.GetProperty("result");
            var sections = JsonSerializer.Deserialize<List<Section>>(result, new JsonSerializerOptions
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
            var response = await httpClient.GetAsync($"{url}/api/Section/get-section-by-id?id={sectionId}");

            response.EnsureSuccessStatusCode();
            var section = await response.Content.ReadFromJsonAsync<Section>();
            if (section == null)
            {
                throw new Exception($"Section with ID {sectionId} not found.");
            }
            return section;
        }

        public async Task<int> LastOrderNumberFromRoadmap(int roadmapId)
        {
            var response = await httpClient.GetAsync($"{url}/api/Section/get-last-from-roadmap");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task UpdateSection(Section section)
        {
            var response = await httpClient.PutAsJsonAsync($"{url}/api/Section/patch", section);
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> TrackCompletion(int sectionId, bool isCompleted)
        {
            var response = await this.httpClient
                    .PostAsJsonAsync(
                        $"{this.url}/api/sections/completion/{sectionId}/{isCompleted}",
                        new { })
                    .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<bool>().ConfigureAwait(false);
        }

        public async Task<List<SectionDependency>> GetSectionDependencies(int sectionId)
        {
            var response = await httpClient.GetAsync($"{url}/api/sections/dependencies/{sectionId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<SectionDependency>>() ?? new List<SectionDependency>();
        }
    }
}
