using Sentinel.Data;
using Sentinel.DTOs;
using Sentinel.Models;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Sentinel.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Sentinel.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SentinelDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ApiResponseHelper _responseHelper;

        private int _nextId = 1;
        public UserService(SentinelDbContext context, IHttpContextAccessor httpContextAccessor, ApiResponseHelper apiResponseHelper, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _responseHelper = apiResponseHelper;
            _configuration = configuration;
        }

        public async Task<SentinelUserDTO> CreateUser(SentinelUserDTO user)
        {

            //string? ImagePath = null;
            //if(user.ProfilePicUrl != null && user.ProfilePicUrl.Length > 0)
            //{
            //var uploadFolders = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
            //if (!Directory.Exists(uploadFolders)) Directory.CreateDirectory(uploadFolders);
            //var uniqueFileName = Guid.NewGuid().ToString() + "_" + user.ProfilePicUrl.FileName;
            //var filePath = Path.Combine(uploadFolders, uniqueFileName);
            //using (var fileStream = new FileStream(filePath, FileMode.Create))
            //{
            //    await user.ProfilePicUrl.CopyToAsync(fileStream);
            //}
            //ImagePath = $"/uploads/profiles/{uniqueFileName}";
            //}

            var existedUser = await _context.SentinelUsers.FirstOrDefaultAsync(u => u.Email == user.Email || u.CNIC == user.CNIC);
            if (existedUser != null)
            {
                throw new Exception("User already exists with this Email or CNIC");
            }
            var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            var HashedPwd = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash, salt);

            var sentinelUser = new SentinelUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Dob = user.Dob,
                Email = user.Email,
                PasswordHash = HashedPwd,
                Biography = user.Biography,
                CNIC = user.CNIC,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                CreatedAt = DateTime.UtcNow,
                ProfilePicUrl = user.ProfilePicUrl,
                RegistrationIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
            };

            var result = await _context.SentinelUsers.AddAsync(sentinelUser);
            await _context.SaveChangesAsync();
            var createdUser = result.Entity;
            var SuccessDto = new SentinelUserDTO
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                CNIC = createdUser.CNIC,
                Address = createdUser.Address,
                Biography = createdUser.Biography,
                Dob = createdUser.Dob,
                Gender = createdUser.Gender,
                PhoneNumber = createdUser.PhoneNumber,
                ProfilePicUrl = createdUser.ProfilePicUrl
            };
            return SuccessDto;
        }

        public async Task<string> Login(LoginDTO dto)
        {
            var user = await _context.SentinelUsers.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.PasswordHash, user.PasswordHash))
            {
                throw new Exception("Invalid email or password.");
            }

            var toekHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = toekHandler.CreateToken(TokenDescriptor);
            var tokenString = toekHandler.WriteToken(token);
            return tokenString;
        }
    }
}
