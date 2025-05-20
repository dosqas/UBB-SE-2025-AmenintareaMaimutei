using Duo.Api.Models;

namespace Duo.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<User>> GetUsers();
        public Task<User> GetUser(int id);

        public Task<int> CreateUser(User user);

        public Task DeleteUser(int id);

        public Task UpdateUser(User post);
    }
}
