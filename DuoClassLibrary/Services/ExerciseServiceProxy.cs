using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DuoClassLibrary.Constants;
using DuoClassLibrary.Helpers;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Exercises.DTO;

namespace DuoClassLibrary.Services
{
    public class ExerciseServiceProxy : IExerciseServiceProxy
    {
        private readonly HttpClient httpClient;
        private string url = Constants.Environment.BaseUrl;

        public ExerciseServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task CreateExercise(Exercise exercise)
        {
            if (exercise == null)
            {
                throw new ArgumentNullException(nameof(exercise));
            }
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNameCaseInsensitive = true,
                };

                var jsonExercise = exercise.Type switch
                {
                    "Association" => JsonSerializer.Serialize((AssociationExercise)exercise, options),
                    "Flashcard" => JsonSerializer.Serialize((FlashcardExercise)exercise, options),
                    "MultipleChoice" => JsonSerializer.Serialize((MultipleChoiceExercise)exercise, options),
                    "FillInTheBlank" => JsonSerializer.Serialize((FillInTheBlankExercise)exercise, options),
                    _ => throw new NotSupportedException($"Exercise type '{exercise.Type}' is not supported.")
                };
                var response = await httpClient.PostAsync($"{url}Exercise", new StringContent(jsonExercise, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();

                // Deserialize the response to get the Id
                string responseBody = await response.Content.ReadAsStringAsync();

                var idObj = JsonSerializer.Deserialize<IdResponse>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                });

                if (idObj != null)
                {
                    exercise.ExerciseId = idObj.ExerciseId;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error creating exercise: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating exercise: {ex.Message}");
            }
        }

        public async Task DeleteExercise(int exerciseId)
        {
            var response = await httpClient.DeleteAsync($"{url}Exercise/{exerciseId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<Exercise>> GetAllExercises()
        {
            try
            {
                var response = await httpClient.GetAsync($"{url}Exercise");
                response.EnsureSuccessStatusCode();

                string responseJson = await response.Content.ReadAsStringAsync();

                // var exercises = await response.Content.ReadFromJsonAsync<List<Exercise>>();
                var exercises = new List<Exercise>();
                using JsonDocument doc = JsonDocument.Parse(responseJson);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                foreach (var element in doc.RootElement.EnumerateArray())
                {
                    string? type = element.GetProperty("type").GetString();

                    Exercise? mc = type switch
                    {
                        "MultipleChoice" => element.Deserialize<MultipleChoiceExercise>(options),
                        "FillInTheBlank" => element.Deserialize<FillInTheBlankExercise>(options),
                        "Association" => element.Deserialize<AssociationExercise>(options),
                        "Flashcard" => element.Deserialize<FlashcardExercise>(options),
                        _ => throw new Exception($"Unknown type: {type}")
                    };

                    if (mc == null)
                    {
                        throw new Exception($"Failed to deserialize exercise of type: {type}");
                    }
                    exercises.Add(mc);
                }
                return exercises ?? new List<Exercise>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error fetching exercises: {ex.Message}");
                return new List<Exercise>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return new List<Exercise>();
            }
        }

        public async Task<List<Exercise>> GetAllExercisesFromExam(int examId)
        {
            var response = await httpClient.GetAsync($"{url}Exercise/exam/{examId}");
            response.EnsureSuccessStatusCode();
            string responseJson = await response.Content.ReadAsStringAsync();
            var exercises = JsonSerializationUtil.DeserializeExerciseList(responseJson);
            return exercises ?? new List<Exercise>();
        }

        public async Task<List<Exercise>> GetAllExercisesFromQuiz(int quizId)
        {
            var response = await httpClient.GetAsync($"{url}Exercise/quiz/{quizId}");
            response.EnsureSuccessStatusCode();
            string responseJson = await response.Content.ReadAsStringAsync();
            var exercises = JsonSerializationUtil.DeserializeExerciseList(responseJson);
            return exercises ?? new List<Exercise>();
        }

        public async Task<Exercise?> GetExerciseById(int exerciseId)
        {
            var response = await httpClient.GetAsync($"{url}Exercise/{exerciseId}");
            response.EnsureSuccessStatusCode();
            var exercise = await response.Content.ReadFromJsonAsync<Exercise>();
            return exercise ?? throw new InvalidOperationException("Exercise not found.");
        }
    }
}
