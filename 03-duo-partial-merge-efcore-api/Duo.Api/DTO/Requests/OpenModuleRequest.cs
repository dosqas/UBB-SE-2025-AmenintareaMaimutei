using System.Diagnostics.CodeAnalysis;

namespace Duo.Api.DTO.Requests
{
    /// <summary>
    /// Represents the data required to open a module for a user.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OpenModuleRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the ID of the user who is opening the module.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the module to be opened.
        /// </summary>
        public int ModuleId { get; set; }

        #endregion
    }
}