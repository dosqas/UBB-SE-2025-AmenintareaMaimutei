// <copyright file="IExerciseViewFactory.cs" company="DuoISS">
// Copyright (c) DuoISS. All rights reserved.
// </copyright>

namespace Duo.Helpers.Interfaces
{
    /// <summary>
    /// Defines a factory for creating exercise view objects based on exercise type.
    /// </summary>
    internal interface IExerciseViewFactory
    {
        /// <summary>
        /// Creates and returns an exercise view object for the specified exercise type.
        /// </summary>
        /// <param name="exerciseType">The type of exercise to create a view for.</param>
        /// <returns>An object representing the exercise view.</returns>
        object CreateExerciseView(string exerciseType);
    }
}
