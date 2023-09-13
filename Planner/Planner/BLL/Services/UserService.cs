using Planner.BLL.Interfaces;
using Planner.Core.Helpers;
using Planner.Core.Models;
using Planner.DAL;

namespace Planner.BLL.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;
    private readonly IPasswordService _passwordService;

    public UserService(IRepository<User> repository, IPasswordService passwordService)
    {
        _repository = repository;
        _passwordService = passwordService;
    }

    public async Task<Result<bool>> Register(User user)
    {
        if (user == null)
        {
            return new Result<bool>(false, $"{nameof(user)} not found");
        }
        
        if (!(await IsFreeUsername(user.Username)))
        {
            return new Result<bool>(false, $"This username has already used");
        }

        if (!Validations.IsPasswordValid(user.Password) || !Validations.IsUsernameValid(user.Username))
        {
            return new Result<bool>(false, $"Incorrect password or username");
        }

        user.Password = _passwordService.HashPassword(user.Password);
        
        var addResult = await _repository.AddAsync(user);

        if (!addResult.IsSuccessful)
        {
            return new Result<bool>(false, $"Failed to register user with username: {user.Username}. " +
                                           $"Error: {addResult.Message}");
        }

        return new Result<bool>(true);
    }

    public async Task<Result<bool>> Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return new Result<bool>(false, "Username and password must no be empty");
        }

        var getResult = await _repository.GetAsync(u => u.Username.Equals(username));
        
        if (!getResult.IsSuccessful)
        {
            return new Result<bool>(false, getResult.Message);
        }

        var user = getResult.Data.FirstOrDefault();
        
        if (user == null)
        {
            return new Result<bool>(false, $"{nameof(user)} not found");
        }

        if (!_passwordService.VerifyPassword(password, user.Password))
        {
            return new Result<bool>(false, "Failed to log in. Incorrect password or username");
        }
        
        return new Result<bool>(true);
    }

    private async Task<bool> IsFreeUsername(string username)
    {
        var result = await _repository.GetAsync(u => u.Username.Equals(username));

        if (!result.IsSuccessful)
        {
            return false;
        }
        
        return result.Data.Count == 0;
    }
}