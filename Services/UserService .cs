using Sentinel.Data;
using Sentinel.DTOs;
using Sentinel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace Sentinel.Services
{
    public class UserService : IUserService
    {
        private readonly List<SentinelUser> Users = new();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SentinelDbContext _context;
        
        private int _nextId = 1;
        public UserService(SentinelDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SentinelUser> CreateUser(SentinelUserDTO user)
        {

            string? ImagePath = null;
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
            var sentinelUser = new SentinelUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Dob = user.Dob,
                Email = user.Email,
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
            return createdUser;
        }

        public List<SentinelUser> GetAllUsers()
        {
            return Users;
        }

        public async Task<SentinelUser>  GetUserById(int id)
        {
            var User = await _context.SentinelUsers.FindAsync(id);
            if(User != null)
            {
                return User;
            } else
            {
                throw new Exception("User Not Found");
            }
        }

        public SentinelUser UpdateUser(int id, SentinelUser updatedUser)
        {
            var User = Users.Find(u => u.Id == id);
            if (User != null)
            {
                updatedUser.Id = id;
                Users.Remove(User);
                Users.Add(updatedUser);
            }

            return updatedUser;
        }


        public bool DeleteUser(int id)
        {
            var User = Users.Find(u => u.Id == id);
            if (User != null)
            {
                Users.Remove(User);
                return true;
            }
            return false;
        }

    }
}
