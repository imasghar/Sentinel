using Sentinel.DTOs;
using Sentinel.Helpers;
using Sentinel.Models;
namespace Sentinel.Services
{
    public interface IUserService
    {
        Task<SentinelUserDTO> CreateUser(SentinelUserDTO user);
        Task<string> Login(LoginDTO dto);
    }
}
