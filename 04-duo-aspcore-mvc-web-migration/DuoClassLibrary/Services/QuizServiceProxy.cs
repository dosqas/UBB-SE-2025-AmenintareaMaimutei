using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Diagnostics;
using System.Text;
using DuoClassLibrary.Exceptions;

using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Helpers;
using DuoClassLibrary.Models.Quizzes.API;
using DuoClassLibrary.Models;

namespace DuoClassLibrary.Services
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
            var result = await httpClient.GetAsync($"{url}quiz/list");
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
            var result = await httpClient.GetAsync($"{url}quiz/get-available");
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
            var result = await httpClient.GetAsync($"{url}exam/list");
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
            var result = await httpClient.GetAsync($"{url}exam/get-available");
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
            var result = await httpClient.GetAsync($"{url}quiz/get?id={id}");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            return JsonSerializationUtil.DeserializeQuiz(responseJson);
        }

        public async Task<Exam> GetExamByIdAsync(int id)
        {
            var result = await httpClient.GetAsync($"{url}Exam/get?id={id}");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            return JsonSerializationUtil.DeserializeExamWithTypedExercises(responseJson);
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetFromJsonAsync<List<Quiz>>($"{url}Quiz/get-all-section?sectionId={sectionId}");
            return result ?? throw new QuizServiceProxyException($"Received null response for section {sectionId} quizzes.");
        }

        public async Task<int> CountQuizzesFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetFromJsonAsync<int?>($"{url}quiz/count-from-section?sectionId={sectionId}");
            return result ?? throw new QuizServiceProxyException($"Received null response when counting quizzes in section {sectionId}.");
        }

        public async Task<int> LastOrderNumberFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetFromJsonAsync<int?>($"{url}quiz/last-order?sectionId={sectionId}");
            return result ?? throw new QuizServiceProxyException($"Received null response when getting last order number from section {sectionId}.");
        }

        public async Task<Exam> GetExamFromSectionAsync(int sectionId)
        {
            var result = await httpClient.GetAsync($"{url}Exam/get-from-section?sectionId={sectionId}");
            result.EnsureSuccessStatusCode();
            string responseJson = await result.Content.ReadAsStringAsync();
            return JsonSerializationUtil.DeserializeExamWithTypedExercises(responseJson);
        }

        public async Task DeleteQuizAsync(int quizId)
        {
            var response = await httpClient.DeleteAsync($"{url}quiz/delete?id={quizId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateQuizAsync(Quiz quiz)
        {
            string serialized = JsonSerializationUtil.SerializeQuiz(quiz);
            var response = await httpClient.PutAsync($"{url}quiz/update", new StringContent(serialized, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
        }

        public async Task CreateQuizAsync(Quiz quiz)
        {
            string serialized = JsonSerializationUtil.SerializeQuiz(quiz);
            var response = await httpClient.PostAsync($"{url}quiz/add", new StringContent(serialized, Encoding.UTF8, "application/json"));
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
            var response = await httpClient.PostAsync($"{url}quiz/add-exercises", formData);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddExerciseToQuizAsync(int quizId, int exerciseId)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(quizId.ToString()), "quizId");
            formData.Add(new StringContent(exerciseId.ToString()), "exerciseId");
            var response = await httpClient.PostAsync($"{url}quiz/add-exercise", formData);
            response.EnsureSuccessStatusCode();
        }

        public async Task AddExerciseToExamAsync(int examId, int exerciseId)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(examId.ToString()), "examId");
            formData.Add(new StringContent(exerciseId.ToString()), "exerciseId");
            var response = await httpClient.PostAsync($"{url}exam/add-exercise", formData);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId)
        {
            var response = await httpClient.DeleteAsync($"{url}quiz/remove-exercise?quizId={quizId}&exerciseId={exerciseId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveExerciseFromExamAsync(int examId, int exerciseId)
        {
            var response = await httpClient.DeleteAsync($"{url}exam/remove-exercise?examId={examId}&exerciseId={exerciseId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteExamAsync(int examId)
        {
            var response = await httpClient.DeleteAsync($"{url}exam/delete?id={examId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateExamAsync(Exam exam)
        {
            var response = await httpClient.PutAsJsonAsync($"{url}exam/update", exam);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Exam> CreateExamAsync(Exam exam)
        {
            string serialized = JsonSerializationUtil.SerializeExamWithTypedExercises(exam);
            var response = await httpClient.PostAsync($"{url}exam/add", new StringContent(serialized, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            string responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializationUtil.DeserializeExamWithTypedExercises(responseJson);
        }


        public async Task<QuizResult> GetResultAsync(int quizId)
        {
            var result = await httpClient.GetFromJsonAsync<QuizResult>($"{url}quiz/get-result?quizId={quizId}");
            return result ?? throw new QuizServiceProxyException($"Received null response for result of quiz {quizId}.");
        }

        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
            var response = await httpClient.PostAsJsonAsync($"{url}quiz/submit", submission);
            response.EnsureSuccessStatusCode();
        }

        public Task<bool> IsQuizCompletedAsync(int userId, int quizId)
        {
            var response = httpClient.GetFromJsonAsync<QuizCompletionDTO>($"{url}Quiz/is-completed?userId={userId}&quizId={quizId}");
            //Get boolean result from response
            if (response.Result == null)
            {
                throw new InvalidOperationException("Empty or invalid response from server.");
            }

            return Task.FromResult(response.Result.IsCompleted);
        }

        public Task<bool> IsExamCompletedAsync(int userId, int examId)
        {
            var response = httpClient.GetFromJsonAsync<ExamCompletionDTO>($"{url}Exam/is-completed?userId={userId}&examId={examId}");
            //Get boolean result from response
            if (response.Result == null)
            {
                throw new InvalidOperationException("Empty or invalid response from server.");
            }

            return Task.FromResult(response.Result.IsCompleted);
        }

        public async Task CompleteQuizAsync(int userId, int quizId)
        {
            var response = httpClient.PostAsJsonAsync($"{url}Quiz/add-completed-quiz?userId={userId}&quizId={quizId}", new { });
            response.Result.EnsureSuccessStatusCode();
        }

        public async Task CompleteExamAsync(int userId, int examId)
        {
            var response = httpClient.PostAsJsonAsync($"{url}Exam/add-completed-exam?userId={userId}&examId={examId}", new { });
            response.Result.EnsureSuccessStatusCode();
        }
    }
}