using Duo.Api.Models.Exercises;

namespace Duo.Api.Helpers
{
    public class ExerciseMerger
    {
        public static List<Exercise> MergeExercises(List<Exercise> exercises)
        {
            var mergedExercises = new List<Exercise>();
            var exerciseMap = new Dictionary<int, Exercise>();

            foreach (var exercise in exercises)
            {
                if (!exerciseMap.TryGetValue(exercise.ExerciseId, out var existingExercise))
                {
                    exerciseMap[exercise.ExerciseId] = exercise switch
                    {
                        MultipleChoiceExercise mc => new MultipleChoiceExercise { ExerciseId = mc.ExerciseId, Question = mc.Question!, Difficulty = mc.Difficulty, Choices = [.. mc.Choices!] },
                        FillInTheBlankExercise fb => new FillInTheBlankExercise { ExerciseId = fb.ExerciseId, Question = fb.Question!, Difficulty = fb.Difficulty, PossibleCorrectAnswers = [.. fb.PossibleCorrectAnswers!] },
                        AssociationExercise assoc => new AssociationExercise { ExerciseId = assoc.ExerciseId, Question = assoc.Question!, Difficulty = assoc.Difficulty, FirstAnswersList = [.. assoc.FirstAnswersList], SecondAnswersList = [.. assoc.SecondAnswersList] },
                        FlashcardExercise flash => new FlashcardExercise { ExerciseId = flash.ExerciseId, Question = flash.Question!, Answer = flash.Answer, Difficulty = flash.Difficulty },
                        _ => exercise
                    };
                }
                else
                {
                    // Merge the data
                    switch (existingExercise)
                    {
                        case MultipleChoiceExercise existingMC when exercise is MultipleChoiceExercise newMC:
                            newMC.Choices!.RemoveAll(c => c.IsCorrect);
                            existingMC.Choices!.AddRange(newMC.Choices);
                            break;

                        case FillInTheBlankExercise existingFB when exercise is FillInTheBlankExercise newFB:
                            existingFB.PossibleCorrectAnswers!.AddRange(newFB.PossibleCorrectAnswers!);
                            break;

                        case AssociationExercise existingAssoc when exercise is AssociationExercise newAssoc:
                            existingAssoc.FirstAnswersList.AddRange(newAssoc.FirstAnswersList);
                            existingAssoc.SecondAnswersList.AddRange(newAssoc.SecondAnswersList);
                            break;
                    }
                }
            }

            mergedExercises.AddRange(exerciseMap.Values);

            return mergedExercises;
        }
    }
}
