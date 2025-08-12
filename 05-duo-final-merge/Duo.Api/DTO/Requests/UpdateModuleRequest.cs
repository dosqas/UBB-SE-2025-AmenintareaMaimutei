using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.DTO.Requests
{
    /// <summary>
    /// Represents the data required to update an existing module in the system.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UpdateModuleRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the updated title of the module (optional).
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the updated description of the module (optional).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the module is a bonus module (optional).
        /// </summary>
        public bool? IsBonus { get; set; }

        /// <summary>
        /// Gets or sets the updated cost of the module in coins (optional).
        /// </summary>
        public int? Cost { get; set; }

        /// <summary>
        /// Gets or sets the updated URL of the image associated with the module (optional).
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the updated ID of the course to which the module belongs (optional).
        /// </summary>
        public int? CourseId { get; set; }

        #endregion
    }
}
