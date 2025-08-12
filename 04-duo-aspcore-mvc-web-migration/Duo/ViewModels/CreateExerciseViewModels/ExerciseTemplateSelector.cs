﻿using Duo.ViewModels.ExerciseViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Duo.ViewModels.CreateExerciseViewModels
{
    public class ExerciseTemplateSelector : DataTemplateSelector
    {
        public DataTemplate AssociationExerciseTemplate { get; set; }
        public DataTemplate FillInTheBlankExerciseTemplate { get; set; }
        public DataTemplate MultipleChoiceExerciseTemplate { get; set; }
        public DataTemplate FlashcardExerciseTemplate { get; set; }

        protected override DataTemplate? SelectTemplateCore(object item)
        {
            if (item is AssociationExerciseViewModel)
            {
                return AssociationExerciseTemplate;
            }
            else if (item is FillInTheBlankExerciseViewModel)
            {
                return FillInTheBlankExerciseTemplate;
            }
            else if (item is MultipleChoiceExerciseViewModel)
            {
                return MultipleChoiceExerciseTemplate;
            }
            else if (item is CreateFlashcardExerciseViewModel)
            {
                return FlashcardExerciseTemplate;
            }

            return null;
        }
    }
}
