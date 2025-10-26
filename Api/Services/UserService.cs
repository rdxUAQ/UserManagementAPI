using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using UserManagementAPI.Api.Models;
using UserManagementAPI.Api.Models.Dtos;
using UserManagementAPI.Api.Interfaces;

namespace UserManagementAPI.Api.Services
{

    public class UserService : IUserService
    {

        private readonly ILogger<UserService> _logger;
        private static readonly string currenDir = AppContext.BaseDirectory;
        private static readonly string usersPath = Path.Combine(currenDir, "Api", "Repositories", "Users.json");

        // in-memory store
        private readonly List<User> _users;
        private static readonly object _fileLock = new();

        public UserService(ILogger<UserService> logger)
{
    _logger = logger;
    try
    {
        var json = File.Exists(usersPath) ? File.ReadAllText(usersPath) : "[]";
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var loaded = JsonSerializer.Deserialize<List<User>>(json, opts) ?? new List<User>();

        // keep only entries with required fields
        var cleaned = loaded.Where(u =>
            u != null &&
            !string.IsNullOrWhiteSpace(u.Id) &&
            !string.IsNullOrWhiteSpace(u.Email) &&
            !string.IsNullOrWhiteSpace(u.Password) &&
            !string.IsNullOrWhiteSpace(u.Fullname)
        ).ToList();

        _users = cleaned;

        // if there were invalid entries, overwrite file with cleaned data
        if (cleaned.Count != loaded.Count)
        {
            // persist synchronously during startup
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(usersPath, JsonSerializer.Serialize(_users, options));
            _logger.LogInformation("Cleaned invalid entries from users.json");
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed reading users.json, initializing empty store.");
        _users = new List<User>();
    }
}


        public Task<bool> CreateNewUser(UserDto newUser)
        {

            var result = _users.Where(u =>
                u.Email!.Equals(newUser.Email) || u.Fullname!.Equals(newUser.Fullname)
            );

            if (result.Count() > 0) {

                _logger.LogInformation("There is already a user with this Email or Name");
                return Task.FromResult(false);
            }

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = newUser.Email!,
                Password = newUser.Password!,
                Fullname = newUser.Fullname!

            };

            lock (_fileLock)
            {
                try {

                    _users.Add(user);

                    File.WriteAllText(
                        usersPath,
                        JsonSerializer.
                        Serialize(_users, new JsonSerializerOptions { WriteIndented = true })
                    );
                } catch (Exception ex)
                {
                    _logger.LogError("Error saving user: " + ex.Message);
                    return Task.FromResult(false);
                }
            }
            

            return Task.FromResult(true);
        }

        public Task<bool> DeleteUserById(string Id)
        {
            var user = _users.Where(c => c.Id == Id).FirstOrDefault();

            if (user == null)
            {
                _logger.LogWarning("User not found");
                return Task.FromResult(false);
            }

            lock (_fileLock)
            {
                
                try {

                    _users.Remove(user);

                    File.WriteAllText(
                        usersPath,
                        JsonSerializer.
                        Serialize(_users, new JsonSerializerOptions { WriteIndented = true })
                    );
                } catch (Exception ex)
                {
                    _logger.LogError("Error deleting user: " + ex.Message);
                    return Task.FromResult(false);
                }
            }
            
            return Task.FromResult(true);
        }

        public Task<List<User>> GetAllUsersAsync()
    {
        // defensive: return only valid users (copy)
        var valid = _users.Where(u =>
            u != null &&
            !string.IsNullOrWhiteSpace(u.Id) &&
            !string.IsNullOrWhiteSpace(u.Email) &&
            !string.IsNullOrWhiteSpace(u.Password) &&
            !string.IsNullOrWhiteSpace(u.Fullname)
        ).Select(u => new User
        {
            Id = u.Id,
            Email = u.Email,
            Password = u.Password,
            Fullname = u.Fullname
        }).ToList();

        return Task.FromResult(valid);
    }

        public async Task<User?> GetUsersByIdAsync(string Id)
        {
            

            await Task.Delay(200);

            var result = _users.Where(c => c.Id == Id).FirstOrDefault();

            if (result is null)
            {
                return null;
            }

            return result;
        }

        public Task<List<User>> GetUsersByXnYIndex(int x, int y)
        {

            return Task.FromResult(_users);
            
        }

        public async Task<bool> UpdateUserById(string id, UserDto userNewData)
        {

            
            var oldUser = _users.Where(u => u.Id!.Equals(id)).FirstOrDefault();

            if (oldUser is null)
            {
                _logger.LogError($"user {id} not found");
                return false;
            }

            oldUser.Email = userNewData.Email;
            oldUser.Fullname = userNewData.Fullname;
            oldUser.Password = userNewData.Password;

            lock (_fileLock)
            {

                try
                {

                    File.WriteAllTextAsync(
                        usersPath,
                        JsonSerializer.
                        Serialize(_users, new JsonSerializerOptions { WriteIndented = true })
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error updating user: " + ex.Message);
                    return false;
                }
            }

            return true;
            
        }
    }
}