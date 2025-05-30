using System.Diagnostics.CodeAnalysis;
using Duo.Api.Models;
using Duo.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable SA1009 // Closing parenthesis should be spaced correctly

namespace Duo.Api.Controllers
{
    /// <summary>
    /// Controller for managing users in the system.
    /// Provides endpoints for user registration, login, and CRUD operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UserController"/> class with the specified repository.
    /// </remarks>
    /// <param name="userRepository">The user repository instance for data access.</param>
    [ApiController]
    [Route("api/[controller]")]
    [ExcludeFromCodeCoverage]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet(Name = "GetAllUsers")]
        public async Task<IEnumerable<User>> Get()
        {
            return await _userRepository.GetUsers();
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<User> Get(int id)
        {
            var user = await _userRepository.GetUser(id);
            return user;
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<int> Create([FromBody] User user)
        {
            var userId = await _userRepository.CreateUser(user);
            return userId;
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task Update(int id, [FromBody] User user)
        {
            await _userRepository.UpdateUser(user);
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task Delete(int id)
        {
            await _userRepository.DeleteUser(id);
        }
    }
}
