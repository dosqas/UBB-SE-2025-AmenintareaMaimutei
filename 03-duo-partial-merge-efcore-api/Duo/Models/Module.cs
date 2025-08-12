using System;

namespace Duo.Models
{
    /// <summary>
    /// Represents a learning module within a course.
    /// </summary>
    public class Module
    {
        /// <summary>
        /// Gets or sets the unique identifier for the module.
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the course to which this module belongs.
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets the title of the module.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the full description of the module.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the position of the module within the course.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the module is a bonus (optional) module.
        /// </summary>
        public bool IsBonus { get; set; }

        /// <summary>
        /// Gets or sets the cost to unlock the module.
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Gets a shortened version of the module description, limited to 23 characters.
        /// </summary>
        public string ShortDescription => Description.Length > 23 ? string.Concat(Description.AsSpan(0, 23), "...") : Description;

        /// <summary>
        /// Gets or sets the URL of the image associated with the module.
        /// </summary>
        public required string ImageUrl { get; set; }
    }
}