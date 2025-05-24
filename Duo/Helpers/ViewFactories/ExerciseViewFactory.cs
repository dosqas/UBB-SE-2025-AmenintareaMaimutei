// <copyright file="ExerciseViewFactory.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Helpers.ViewFactories
{
    using Duo.Views.Components.CreateExerciseComponents;
    using Duo.Helpers.Interfaces;
    using Microsoft.UI.Xaml.Controls;

    /// <summary>
    /// Factory class for creating views corresponding to different exercise types.
    /// </summary>
    public class ExerciseViewFactory : IExerciseViewFactory
    {
        private readonly CreateAssociationExercise associationExerciseView;
        private readonly CreateFillInTheBlankExercise fillInTheBlankExerciseView;
        private readonly CreateMultipleChoiceExercise multipleChoiceExerciseView;
        private readonly CreateFlashcardExercise flashcardExerciseView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExerciseViewFactory"/> class.
        /// Pre-instantiates views for different exercise types.
        /// </summary>
        public ExerciseViewFactory()
        {
            // Instantiate the views
            this.associationExerciseView = new CreateAssociationExercise();
            this.fillInTheBlankExerciseView = new CreateFillInTheBlankExercise();
            this.multipleChoiceExerciseView = new CreateMultipleChoiceExercise();
            this.flashcardExerciseView = new CreateFlashcardExercise();
        }

        /// <summary>
        /// Creates and returns a new instance of the appropriate view for the given exercise type.
        /// </summary>
        /// <param name="exerciseType">The type of exercise (e.g., "Association", "Fill in the blank").</param>
        /// <returns>
        /// A new instance of the corresponding exercise view, or a fallback <see cref="TextBlock"/>
        /// prompting the user to select a valid exercise type if the input is unrecognized.
        /// </returns>
        public object CreateExerciseView(string exerciseType)
        {
            return exerciseType switch
            {
                "Association" => new CreateAssociationExercise(),
                "Fill in the blank" => new CreateFillInTheBlankExercise(),
                "Multiple Choice" => new CreateMultipleChoiceExercise(),
                "Flashcard" => new CreateFlashcardExercise(),
                _ => new TextBlock { Text = "Select an exercise type." },
            };
        }
    }
}