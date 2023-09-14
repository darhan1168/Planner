using Planner.Core.Models;

namespace Planner.BLL.Interfaces;

public interface IUserService
{
    Task<Result<bool>> Register(User user);
    Task<Result<bool>> Login(string username, string password);
    Task<Result<User>> GetUserByUsername(string username);
}