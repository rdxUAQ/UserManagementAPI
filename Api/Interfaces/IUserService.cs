using UserManagementAPI.Api.Models;
using UserManagementAPI.Api.Models.Dtos;

namespace UserManagementAPI.Api.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUsersByIdAsync(string Id);
        public Task<List<User>> GetAllUsersAsync();
        public Task<List<User>> GetUsersByXnYIndex(int x, int y);
        public Task<bool> CreateNewUser(UserDto newUser);
        public Task<bool> UpdateUserById(string Id, UserDto userNewData);
        public Task<bool> DeleteUserById(string Id);
    }
}