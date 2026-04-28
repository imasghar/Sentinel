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

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _UserService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("getByid{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _UserService.GetUserById(id);
                var response = _responseHelper.Success(user, "User retrieved successfully.");
                return Ok(response);
            } catch(Exception ex)
            {
                var ErrorResponse = _responseHelper.Error<SentinelUser>(ex.Message);
                return BadRequest(ErrorResponse);
            }
            
        }

        [HttpPost("createUser")]
        public async Task<IActionResult> CreateUser([FromBody] SentinelUserDTO user)
        {
            try
            {
                var createdUser = await _UserService.CreateUser(user);
                var response = _responseHelper.Success(createdUser, "User registered successfully.");
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, response);
            } catch(Exception ex)
            {
                var ErrorResponse = _responseHelper.Error<SentinelUser>(ex.Message);
                return BadRequest(ErrorResponse);
            }
        }

        [HttpDelete("deleteUser")]
        public IActionResult DeleteUser(int id)
        {
            var success = _UserService.DeleteUser(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
