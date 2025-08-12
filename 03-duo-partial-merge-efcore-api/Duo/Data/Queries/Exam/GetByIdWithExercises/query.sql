CREATE OR ALTER PROCEDURE sp_GetExamByIdWithExercises
    @examId INT
AS
BEGIN
    SELECT 
        ex.*,
        e.Id as ExerciseId,
        e.Type as Type,
        e.Question as Question,
        e.DifficultyId as DifficultyId,
        -- Multiple Choice Exercise data
        mce.CorrectAnswer as MultipleChoiceCorrectAnswer,
        mco.OptionText as MultipleChoiceOption,
        -- Fill in the Blank Exercise data
        fba.CorrectAnswer as FillInTheBlankAnswer,
        -- Association Exercise data
        ap.FirstAnswer as AssociationFirstAnswer,
        ap.SecondAnswer as AssociationSecondAnswer,
        -- Flashcard Exercise data
        fe.Answer as FlashcardAnswer
    FROM Exams ex
    LEFT JOIN ExamExercises ee ON ex.Id = ee.ExamId
    LEFT JOIN Exercises e ON ee.ExerciseId = e.Id
    -- Multiple Choice Exercise joins
    LEFT JOIN MultipleChoiceExercises mce ON e.Id = mce.ExerciseId
    LEFT JOIN MultipleChoiceOptions mco ON mce.ExerciseId = mco.ExerciseId
    -- Fill in the Blank Exercise joins
    LEFT JOIN FillInTheBlankExercises fbe ON e.Id = fbe.ExerciseId
    LEFT JOIN FillInTheBlankAnswers fba ON fbe.ExerciseId = fba.ExerciseId
    -- Association Exercise joins
    LEFT JOIN AssociationExercises ae ON e.Id = ae.ExerciseId
    LEFT JOIN AssociationPairs ap ON ae.ExerciseId = ap.ExerciseId
    -- Flashcard Exercise joins
    LEFT JOIN FlashcardExercises fe ON e.Id = fe.ExerciseId
    WHERE ex.Id = @examId;
END; 