using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.DTO.Requests
{
    /// <summary>
    /// Represents the data required to update an existing section in the system.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UpdateSectionRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the updated ID of the subject associated with the section (optional).
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the updated title of the section (optional).
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the updated description of the section (optional).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the updated ID of the roadmap to which the section belongs (optional).
        /// </summary>
        public int? RoadmapId { get; set; }

        /// <summary>
        /// Gets or sets the updated order number of the section within the roadmap (optional).
        /// </summary>
        public int? OrderNumber { get; set; }

        #endregion
    }
}

