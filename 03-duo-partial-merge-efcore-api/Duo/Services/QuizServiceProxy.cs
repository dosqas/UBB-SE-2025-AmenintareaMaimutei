using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using Duo.Exceptions;
using Duo.Models.Quizzes;
using Duo.Models.Quizzes.API;
using Duo.Services.Interfaces;
using Duo.Helpers;
using Azure;
using Duo.Models.Exercises;

namespace Duo.Services
{
    public class QuizServiceProxy : IQuizServiceProxy
    {
        private readonly HttpClient httpClient;
        private readonly string url = "https://localhost:7174/api/";

        public QuizServiceProxy(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<Quiz>> GetAsync()
        {
            var result = await httpClient.GetAsync($"{url}Quiz/get-all");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            var quizzes = new List<Quiz>();
            using JsonDocument doc = JsonDocument.Parse(responseJson);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var quizJsonString = element.GetRawText();
                var quiz = JsonSerializationUtil.DeserializeQuiz(quizJsonString);
                quizzes.Add(quiz);
            }
            return quizzes;
        }

        public async Task<List<Quiz>> GetAllAvailableQuizzesAsync()
        {
            var result = await httpClient.GetAsync($"{url}Quiz/get-all-available");
            if (result == null)
            {
                throw new QuizServiceProxyException("Received null response when fetching quiz list.");
            }
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            var quizzes = new List<Quiz>();
            using JsonDocument doc = JsonDocument.Parse(responseJson);

            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var quizJsonString = element.GetRawText();
                var quiz = JsonSerializationUtil.DeserializeQuiz(quizJsonString);
                quizzes.Add(quiz);
            }

            return quizzes;
        }

        public async Task<List<Exam>> GetAllExams()
        {
            var result = await httpClient.GetAsync($"{url}Exam/get-all");
            if (result == null)
            {
                throw new QuizServiceProxyException("Received null response when fetching available exams.");
            }
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            var exams = new List<Exam>();
            using JsonDocument doc = JsonDocument.Parse(responseJson);

            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var examJsonString = element.GetRawText();
                var exam = JsonSerializationUtil.DeserializeExamWithTypedExercises(examJsonString);
                exams.Add(exam);
            }

            return exams;
        }

        public async Task<List<Exam>> GetAllAvailableExamsAsync()
        {
            var result = await httpClient.GetAsync($"{url}Exam/get-all-available");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            var exams = new List<Exam>();
            using JsonDocument doc = JsonDocument.Parse(responseJson);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var examJsonString = element.GetRawText();
                var exam = JsonSerializationUtil.DeserializeExamWithTypedExercises(examJsonString);
                exams.Add(exam);
            }
            return exams;
        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            var result = await httpClient.GetAsync($"{url}Quiz/get-quiz-by-id?id={id}");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            return JsonSerializationUtil.DeserializeQuiz(responseJson);
        }

        public async Task<Exam> GetExamByIdAsync(int id)
        {
            var result = await httpClient.GetAsync($"{url}Exam/get-exam-by-id?id={id}");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            return JsonSerializationUtil.DeserializeExamWithTypedExercises(responseJson);
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Quiz>>($"{url}Quiz/get-all-by-section?sectionId={sectionId}");
            return result ?? throw new QuizServiceProxyException($"Received null response for section {sectionId} quizzes.");
        }

        public async Task<int> CountQuizzesFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetFromJsonAsync<int?>($"{url}Quiz/count-quizzes-from-section?sectionId={sectionId}");
            return result ?? throw new QuizServiceProxyException($"Received null response when counting quizzes in section {sectionId}.");
        }

        public async Task<int> LastOrderNumberFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetFromJsonAsync<int?>($"{url}Quiz/get-last-order-number-from-section?sectionId={sectionId}");
            return result ?? throw new QuizServiceProxyException($"Received null response when getting last order number from section {sectionId}.");
        }

        public async Task<Exam> GetExamFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetAsync($"{url}Exam/get-exams-from-section?sectionId={sectionId}");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            return JsonSerializationUtil.DeserializeExamWithTypedExercises(responseJson);
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            var response = await httpClient.DeleteAsync($"{url}Quiz/delete?id={quizId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            string serialized = JsonSerializationUtil.SerializeQuiz(quiz);
            var response = await httpClient.PutAsync($"{url}Quiz/modify", new StringContent(serialized, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            string serialized = JsonSerializationUtil.SerializeQuiz(quiz);
            var response = await httpClient.PostAsync($"{url}Quiz/add", new StringContent(serialized, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(quizId.ToString()), "quizId");
            foreach (var exerciseId in exerciseIds)
            {
                formData.Add(new StringContent(exerciseId.ToString()), "exercises");
            }
            var response = await httpClient.PostAsync($"{url}Quiz/add-exercises", formData);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddExerciseToQuizAsync(int quizId, int exerciseId)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(quizId.ToString()), "quizId");
            formData.Add(new StringContent(exerciseId.ToString()), "exerciseId");
            var response = await httpClient.PostAsync($"{url}Quiz/add-exercise", formData);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddExerciseToExamAsync(int examId, int exerciseId)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(examId.ToString()), "examId");
            formData.Add(new StringContent(exerciseId.ToString()), "exerciseId");
            var response = await httpClient.PostAsync($"{url}Exam/add-exercise", formData);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId)
        {
            var response = await httpClient.DeleteAsync($"{url}Quiz/delete-exercise?quizId={quizId}&exerciseId={exerciseId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveExerciseFromExamAsync(int examId, int exerciseId)
        {
            var response = await httpClient.DeleteAsync($"{url}Exam/remove-exercise?examId={examId}&exerciseId={exerciseId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteExamAsync(int examId)
        {
            var response = await httpClient.DeleteAsync($"{url}Exam/delete?id={examId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            var response = await httpClient.PutAsJsonAsync($"{url}Exam/update", exam);
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateExamAsync(Exam exam)
        {
            string serialized = JsonSerializationUtil.SerializeExamWithTypedExercises(exam);
            var response = await httpClient.PostAsync($"{url}Exam/add", new StringContent(serialized, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task<QuizResult> GetResultAsync(int quizId)
        {
            var result = await httpClient.GetFromJsonAsync<QuizResult>($"{url}Quiz/get-quiz-result?quizId={quizId}");
            return result ?? throw new QuizServiceProxyException($"Received null response for result of quiz {quizId}.");
        }

        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}Quiz/add-submit", submission);
            response.EnsureSuccessStatusCode();
        }
    }
}
