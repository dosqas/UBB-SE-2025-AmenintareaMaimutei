using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Api.Models.Exercises
{
    /// <summary>
    /// Represents an association exercise where users match items from two lists.
    /// Inherits from the <see cref="Exercise"/> base class.
    /// Configured for Table-Per-Hierarchy (TPH) inheritance.
    /// </summary>
    [Index(nameof(Question))] // Ensures efficient search queries on the Question field
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class AssociationExercise : Exercise
    {
        #region Properties

        /// <summary>
        /// Gets or sets the first list of answers for the association exercise.
        /// </summary>
        [Required]
        public List<string> FirstAnswersList { get; set; } = [];

        /// <summary>
        /// Gets or sets the second list of answers for the association exercise.
        /// </summary>
        [Required]
        public List<string> SecondAnswersList { get; set; } = [];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssociationExercise"/> class.
        /// This parameterless constructor is required for Entity Framework.
        /// </summary>
        [JsonConstructorAttribute]
        public AssociationExercise()
        {
            //Type = "Association";
        }

        #endregion
    }
}