CREATE OR ALTER PROCEDURE sp_UpdateExam
    @examId INT,
    @sectionId INT = NULL
AS
BEGIN
    BEGIN TRY
        -- Check if exam exists
        IF NOT EXISTS (SELECT 1 FROM Exams WHERE Id = @examId)
        BEGIN
            RAISERROR ('Exam not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section exists
        IF @sectionId IS NOT NULL AND NOT EXISTS (SELECT 1 FROM Sections WHERE Id = @sectionId)
        BEGIN
            RAISERROR ('Section not found', 16, 1) WITH NOWAIT;
        END

        -- Check if section already has an exam
        IF @sectionId IS NOT NULL AND EXISTS (SELECT 1 FROM Exams WHERE SectionId = @sectionId)
        BEGIN
            RAISERROR ('Section already has an exam', 16, 1) WITH NOWAIT;
        END

        -- Update the exam
        UPDATE Exams
        SET SectionId = @sectionId
        WHERE Id = @examId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState) WITH NOWAIT;
    END CATCH
END; 