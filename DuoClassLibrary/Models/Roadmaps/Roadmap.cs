using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DuoClassLibrary.Models.Sections;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly

namespace Duo.Api.Models.Roadmaps
{
    /// <summary>
    /// Represents a roadmap consisting of multiple sections.
    /// A roadmap is a structured plan that organizes sections in a specific order.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Roadmap
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier for the roadmap.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the roadmap.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of sections associated with this roadmap.
        /// Each section represents a part of the roadmap.
        /// </summary>
        public virtual ICollection<Section> Sections { get; set; } = [];

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Roadmap"/> class.
        /// </summary>
        public Roadmap()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the roadmap, including its name and the number of sections.
        /// </summary>
        /// <returns>A string describing the roadmap.</returns>
        public override string ToString()
        {
            return $"Roadmap: {Name}, Sections Count: {Sections.Count}";
        }

        #endregion
    }
}