using Sentinel.DTOs;
using Sentinel.Models;
namespace Sentinel.Services
{
    public interface IUserService
    {
        Task<SentinelUser> CreateUser(SentinelUserDTO user);
        List<SentinelUser> GetAllUsers();
        Task<SentinelUser> GetUserById(int id);
        SentinelUser UpdateUser(int id, SentinelUser updatedUser);
        bool DeleteUser(int id);
    }
}
