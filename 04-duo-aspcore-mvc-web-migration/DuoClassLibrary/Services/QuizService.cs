using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DuoClassLibrary.Exceptions;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Quizzes.API;
using DuoClassLibrary.Services.Interfaces;

namespace DuoClassLibrary.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizServiceProxy serviceProxy;
            
        /// <summary>
        /// For production: accepts the concrete proxy, up-casts to the interface.
        /// </summary>
        public QuizService(QuizServiceProxy concreteProxy)
            : this((IQuizServiceProxy)concreteProxy)
        {
        }

        /// <summary>
        /// Testable constructor: accepts the proxy interface directly.
        /// </summary>
        public QuizService(IQuizServiceProxy serviceProxy)
        {
            this.serviceProxy = serviceProxy ?? throw new ArgumentNullException(nameof(serviceProxy));
        }

        public async Task<List<Quiz>> GetAllAvailableQuizzes()
        {
                return await serviceProxy.GetAllAvailableQuizzesAsync().ConfigureAwait(false);
        }

        public async Task<List<Exam>> GetAllAvailableExams()
        {
                return await serviceProxy.GetAllAvailableExamsAsync().ConfigureAwait(false);
        }

        public async Task<Quiz> GetQuizById(int quizId)
        {
                return await serviceProxy.GetQuizByIdAsync(quizId).ConfigureAwait(false);
        }

        public async Task<Exam> GetExamById(int examId)
        {
                return await serviceProxy.GetExamByIdAsync(examId).ConfigureAwait(false);
        }

        public async Task<List<Quiz>> GetAllQuizzesFromSection(int sectionId)
        {
                return await serviceProxy.GetAllQuizzesFromSectionAsync(sectionId).ConfigureAwait(false);
        }

        public async Task<int> CountQuizzesFromSection(int sectionId)
        {
                return await serviceProxy.CountQuizzesFromSectionAsync(sectionId).ConfigureAwait(false);
        }

        public async Task<int> LastOrderNumberFromSection(int sectionId)
        {
                return await serviceProxy.LastOrderNumberFromSectionAsync(sectionId).ConfigureAwait(false);
        }

        public async Task<Exam?> GetExamFromSection(int sectionId)
        {
                return await serviceProxy.GetExamFromSectionAsync(sectionId).ConfigureAwait(false);
        }

        public async Task DeleteQuiz(int quizId)
        {
                await serviceProxy.DeleteQuizAsync(quizId).ConfigureAwait(false);
        }

        public async Task UpdateQuiz(Quiz quiz)
        {
                await serviceProxy.UpdateQuizAsync(quiz).ConfigureAwait(false);
        }

        public async Task<int> CreateQuiz(Quiz quiz)
        {
            // ValidationHelper.ValidateQuiz(quiz);
            await serviceProxy.CreateQuizAsync(quiz).ConfigureAwait(false);
            return quiz.Id;
        }

        public async Task AddExercisesToQuiz(int quizId, List<Exercise> exercises)
        {
                var ids = new List<int>();
                foreach (var ex in exercises)
                {
                    ids.Add(ex.ExerciseId);
                }

                await serviceProxy.AddExercisesToQuizAsync(quizId, ids).ConfigureAwait(false);
        }

        public async Task AddExerciseToExam(int examId, int exerciseId)
        {
            await serviceProxy.AddExerciseToExamAsync(examId, exerciseId).ConfigureAwait(false);
        }

        public async Task AddExerciseToQuiz(int quizId, int exerciseId)
        {
                await serviceProxy.AddExerciseToQuizAsync(quizId, exerciseId).ConfigureAwait(false);
        }

        public async Task RemoveExerciseFromQuiz(int quizId, int exerciseId)
        {
                await serviceProxy.RemoveExerciseFromQuizAsync(quizId, exerciseId).ConfigureAwait(false);
        }

        public async Task RemoveExerciseFromExam(int examId, int exerciseId)
        {
            await serviceProxy.RemoveExerciseFromExamAsync(examId, exerciseId).ConfigureAwait(false);
        }

        public async Task DeleteExam(int examId)
        {
                await serviceProxy.DeleteExamAsync(examId).ConfigureAwait(false);
        }

        public async Task UpdateExam(Exam exam)
        {
                await serviceProxy.UpdateExamAsync(exam).ConfigureAwait(false);
        }

        public async Task<int> CreateExam(Exam exam)
        {
            var createdExam = await serviceProxy.CreateExamAsync(exam);
            return createdExam.Id;
        }



        public async Task SubmitQuizAsync(QuizSubmission submission)
        {
                await serviceProxy.SubmitQuizAsync(submission).ConfigureAwait(false);
        }

        public async Task<QuizResult> GetResultAsync(int quizId)
        {
                return await serviceProxy.GetResultAsync(quizId).ConfigureAwait(false);
        }

        public async Task<List<Exam>> GetAllExams()
        {
            return await serviceProxy.GetAllExams().ConfigureAwait(false);
        }

        public async Task<List<Quiz>> GetAllQuizzes()
        {
            return await serviceProxy.GetAsync().ConfigureAwait(false);
        }

        public async Task<bool> IsQuizCompleted(int userId, int quizId)
        {
            bool result = await this.serviceProxy.IsQuizCompletedAsync(userId, quizId);
            return result;
        }

        public async Task CompleteQuiz(int userId, int quizId)
        {
            await this.serviceProxy.CompleteQuizAsync(userId, quizId);
        }

        public async Task<bool> IsExamCompleted(int userId, int examId)
        {
            bool result = await this.serviceProxy.IsExamCompletedAsync(userId, examId);
            return result;
        }

        public async Task CompleteExam(int userId, int examId)
        {
            await this.serviceProxy.CompleteExamAsync(userId, examId);
        }
    }
}
