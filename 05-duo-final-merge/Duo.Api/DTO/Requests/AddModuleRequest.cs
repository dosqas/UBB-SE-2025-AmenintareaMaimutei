using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.DTO.Requests
{
    /// <summary>
    /// Represents the data required to add a new module to the system.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddModuleRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the title of the module.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the module is a bonus module.
        /// </summary>
        public required bool IsBonus { get; set; }

        /// <summary>
        /// Gets or sets the cost of the module in coins.
        /// </summary>
        public required int Cost { get; set; }

        /// <summary>
        /// Gets or sets the URL of the image associated with the module.
        /// </summary>
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the ID of the course to which the module belongs (optional).
        /// </summary>
        public int? CourseId { get; set; }

        #endregion
    }
}

