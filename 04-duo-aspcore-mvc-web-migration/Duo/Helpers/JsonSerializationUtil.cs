using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Duo.Exceptions;
using DuoClassLibrary.Models.Exercises;
using DuoClassLibrary.Models.Quizzes;

namespace Duo.Helpers
{
    public class JsonSerializationUtil
    {
        public static string SerializeExamWithTypedExercises(Exam exam)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
            };

            var examDto = new
            {
                exam.Id,
                exam.SectionId,
                Exercises = exam.Exercises.Select(e => e.ExerciseId).ToList(),
            };

            return JsonSerializer.Serialize(examDto, options);
        }
        public static string SerializeQuiz(Quiz quiz)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
            };

            var quizDTO = new
            {
                quiz.Id,
                quiz.SectionId,
                quiz.OrderNumber,
                Exercises = quiz.Exercises.Select(q => q.ExerciseId).ToList(),
            };

            return JsonSerializer.Serialize(quizDTO, options);
        }

        public static Quiz DeserializeQuiz(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var root = doc.RootElement;

            int id = root.GetProperty("id").GetInt32();
            int? sectionId = root.TryGetProperty("sectionId", out var sectionProp) && sectionProp.ValueKind != JsonValueKind.Null
            ? sectionProp.GetInt32()
            : null;
            int? orderNumber = root.TryGetProperty("OrderNumber", out var orderProp) && orderProp.ValueKind != JsonValueKind.Null
                ? orderProp.GetInt32()
                : null;

            var quiz = new Quiz(id, sectionId, orderNumber);

            foreach (var exercise in root.GetProperty("exercises").EnumerateArray())
            {
                if (!exercise.TryGetProperty("type", out var typeProp))
                {
                    throw new JsonException("Exercise is missing a 'Type' property.");
                }

                string type = typeProp.GetString();

                if (type == null)
                {
                    throw new JsonException("Exercise 'Type' property is null.");
                }

                Exercise? ex = type switch
                {
                    "MultipleChoice" => exercise.Deserialize<MultipleChoiceExercise>(options),
                    "FillInTheBlank" => exercise.Deserialize<FillInTheBlankExercise>(options),
                    "Association" => exercise.Deserialize<AssociationExercise>(options),
                    "Flashcard" => exercise.Deserialize<FlashcardExercise>(options),
                    _ => throw new JsonException($"Unknown type: {type}")
                };

                if (ex == null)
                {
                    throw new JsonException($"Failed to deserialize exercise of type: {type}");
                }
                quiz.AddExercise(ex);
            }
            return quiz;
        }

        public static Exam DeserializeExamWithTypedExercises(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            int id = root.GetProperty("id").GetInt32();
            int? sectionId = root.TryGetProperty("sectionId", out var sectionProp) && sectionProp.ValueKind != JsonValueKind.Null
                ? sectionProp.GetInt32()
                : null;

            var exam = new Exam(id, sectionId);

            var exercises = root.GetProperty("exercises");
            foreach (var elem in exercises.EnumerateArray())
            {
                if (!elem.TryGetProperty("type", out var typeProp))
                {
                    throw new JsonException("Exercise is missing a 'Type' property.");
                }

                string type = typeProp.GetString();

                if (type == null)
                {
                    throw new JsonException("Exercise 'Type' property is null.");
                }

                Exercise? exercise = type switch
                {
                    "Association" => elem.Deserialize<AssociationExercise>(options),
                    "Flashcard" => elem.Deserialize<FlashcardExercise>(options),
                    "MultipleChoice" => elem.Deserialize<MultipleChoiceExercise>(options),
                    "FillInTheBlank" => elem.Deserialize<FillInTheBlankExercise>(options),
                    _ => throw new JsonException($"Unknown exercise type: {type}")
                };

                if (exercise == null)
                {
                    throw new JsonException($"Failed to deserialize exercise of type: {type}");
                }

                exam.AddExercise(exercise);
            }

            return exam;
        }

        public static List<Exercise> DeserializeExerciseList(string exercisesJson)
        {
            var exercises = new List<Exercise>();
            using JsonDocument doc = JsonDocument.Parse(exercisesJson);

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
            return exercises;
        }
    }
}
