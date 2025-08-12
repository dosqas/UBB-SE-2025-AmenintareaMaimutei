using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.DTO.Requests
{
    /// <summary>
    /// Represents the data required to add a new section to the system.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddSectionRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ID of the subject associated with the section (optional).
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the title of the section.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the section.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Gets or sets the ID of the roadmap to which the section belongs.
        /// </summary>
        public required int RoadmapId { get; set; }

        /// <summary>
        /// Gets or sets the order number of the section within the roadmap (optional).
        /// </summary>
        public int? OrderNumber { get; set; }

        #endregion
    }
}
