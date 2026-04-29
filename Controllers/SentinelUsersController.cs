using Microsoft.AspNetCore.Mvc;
using Sentinel.DTOs;
using Sentinel.Helpers;
using Sentinel.Models;
using Sentinel.Services;
namespace Sentinel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SentinelUsersController : Controller
    {
        private readonly IUserService _UserService;
        private readonly ApiResponseHelper _responseHelper;
        public SentinelUsersController(IUserService UserService, ApiResponseHelper responseHelper)
        {
            _UserService = UserService;
            _responseHelper = responseHelper;
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] SentinelUserDTO user)
        {
            try
            {
                var createdUser = await _UserService.CreateUser(user);
                var response = _responseHelper.Success(createdUser, "User registered successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var ErrorResponse = _responseHelper.Error<SentinelUser>(ex.Message);
                return BadRequest(ErrorResponse);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var token = await _UserService.Login(dto);
                //var tokenObj = new
                //{
                //    token = token
                //};
                var response = _responseHelper.SuccessData<object>(token, "Login successful.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var ErrorResponse = _responseHelper.Error<SentinelUser>(ex.Message);
                return BadRequest(ErrorResponse);
            }
        }
    }
}
