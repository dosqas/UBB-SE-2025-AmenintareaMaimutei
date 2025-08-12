using System.Diagnostics.CodeAnalysis;
using Duo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Represents the base controller for all API controllers in the application.
    /// Provides common functionality and access to the repository layer.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ExcludeFromCodeCoverage]
    public class BaseController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The repository instance used for data access.
        /// </summary>
        protected readonly IRepository repository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class with the specified repository.
        /// </summary>
        /// <param name="repository">The repository instance to be used for data access.</param>
        public BaseController(IRepository repository)
        {
            this.repository = repository;
        }

        #endregion
    }
}