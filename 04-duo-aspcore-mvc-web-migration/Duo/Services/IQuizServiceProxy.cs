using System.Collections.Generic;
using System.Threading.Tasks;
using DuoClassLibrary.Models.Quizzes;
using DuoClassLibrary.Models.Quizzes.API;

namespace Duo.Services.Interfaces
{
    public interface IQuizServiceProxy
    {
        Task<List<Quiz>> GetAsync();
        Task<List<Exam>> GetAllExams();
        Task<List<Quiz>> GetAllAvailableQuizzesAsync();
        Task<List<Exam>> GetAllAvailableExamsAsync();
        Task<Quiz> GetQuizByIdAsync(int id);
        Task<Exam> GetExamByIdAsync(int id);
        Task<List<Quiz>> GetAllQuizzesFromSectionAsync(int sectionId);
        Task<int> CountQuizzesFromSectionAsync(int sectionId);
        Task<int> LastOrderNumberFromSectionAsync(int sectionId);
        Task<Exam> GetExamFromSectionAsync(int sectionId);
        Task DeleteQuizAsync(int quizId);
        Task UpdateQuizAsync(Quiz quiz);
        Task CreateQuizAsync(Quiz quiz);
        Task AddExercisesToQuizAsync(int quizId, List<int> exerciseIds);
        Task AddExerciseToQuizAsync(int quizId, int exerciseId);
        Task AddExerciseToExamAsync(int quizId, int exerciseId);
        Task RemoveExerciseFromQuizAsync(int quizId, int exerciseId);
        Task RemoveExerciseFromExamAsync(int examId, int exerciseId);
        Task DeleteExamAsync(int examId);
        Task UpdateExamAsync(Exam exam);
        Task CreateExamAsync(Exam exam);
        Task<QuizResult> GetResultAsync(int quizId);
        Task SubmitQuizAsync(QuizSubmission submission);
    }
}
