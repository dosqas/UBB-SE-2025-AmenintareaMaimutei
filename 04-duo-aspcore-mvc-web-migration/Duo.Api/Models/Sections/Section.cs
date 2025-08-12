using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models.Quizzes;
using Duo.Api.Models.Exercises;
using Duo.Api.Models.Roadmaps;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Api.Models.Sections
{
    /// <summary>
    /// Represents a section within the system.
    /// A section is a part of a roadmap and can contain quizzes and an optional exam.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Section
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier for the section.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the subject associated with the section (optional).
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the title of the section.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the section.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the roadmap to which the section belongs.
        /// </summary>
        public int RoadmapId { get; set; }

        /// <summary>
        /// Gets or sets the order number of the section within the roadmap (optional).
        /// This determines the sequence in which sections appear.
        /// </summary>
        public int? OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the collection of quizzes associated with this section.
        /// </summary>
        public ICollection<Quiz> Quizzes { get; set; } = [];

        /// <summary>
        /// Gets or sets the exam associated with this section (optional).
        /// </summary>
        public Exam? Exam { get; set; }

        public Roadmap? Roadmap { get; internal set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the section, including its title and the number of quizzes.
        /// </summary>
        /// <returns>A string describing the section.</returns>
        public override string ToString()
        {
            return $"Section: {Title}, Quizzes Count: {Quizzes.Count}, Exam: {(Exam != null ? "Yes" : "No")}";
        }

        #endregion
    }
}
