using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.Models
{
    /// <summary>
    /// Represents a learning module within a course.
    /// A module contains structured content and metadata such as its position, cost, and bonus status.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class Module
    {
        #region Fields and Properties

        /// <summary>
        /// Gets or sets the unique identifier for the module.
        /// </summary>
        [Key]
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the course to which this module belongs.
        /// </summary>
        [ForeignKey(nameof(Course))]
        public int? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the title of the module.
        /// </summary>
        [Required]
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the full description of the module.
        /// </summary>
        [Required]
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the position of the module within the course.
        /// This determines the order in which modules are presented.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the module is a bonus (optional) module.
        /// Bonus modules are not required to complete the course.
        /// </summary>
        public bool IsBonus { get; set; }

        /// <summary>
        /// Gets or sets the cost to unlock the module.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image associated with the module.
        /// </summary>
        [Required]
        public required string ImageUrl { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Module"/> class.
        /// </summary>
        public Module()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of the module, including its title and position.
        /// </summary>
        /// <returns>A string describing the module.</returns>
        public override string ToString()
        {
            return $"Module: {Title}, Position: {Position}, Is Bonus: {IsBonus}, Cost: {Cost}";
        }

        #endregion
    }
}
