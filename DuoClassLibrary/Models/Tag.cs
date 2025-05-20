using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DuoClassLibrary.Models
{
    /// <summary>
    /// Represents a tag that can be assigned to courses or modules.
    /// Tags are used to categorize or label content for better organization and searchability.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Tag
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier for the tag.
        /// </summary>
        [Key]
        public int TagId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        public ICollection<CourseTag> CourseTags { get; set; } = new List<CourseTag>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        public Tag()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the tag, including its name.
        /// </summary>
        /// <returns>A string describing the tag.</returns>
        public override string ToString()
        {
            return $"Tag: {Name}";
        }

        #endregion
    }
}