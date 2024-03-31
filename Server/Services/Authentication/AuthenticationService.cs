using ExScheduler_Server.Dto;
using ExScheduler_Server.Interfaces;
using ExScheduler_Server.Helper;
using ExScheduler_Server.Models;

namespace ExScheduler_Server.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAdminRepository _adminRepository;
        public AuthenticationService(IAdminRepository adminRepository)
        {
              _adminRepository = adminRepository;
        }
        public Task<string> Login(AdminLoginDto adminLoginDto)
        {
            Admin admin = _adminRepository.GetAdminByEmail(adminLoginDto.AdminEmail);
            if (admin == null)
            {
                // return Task.FromResult("Email does not exist");
                throw new Exception("Email does not exist");
            }

            bool isPasswordValid = PassworHasher.VerifyPassword(adminLoginDto.AdminPassword, admin.AdminPassword, admin.Salt);

            if (isPasswordValid)
            {
                string token = _adminRepository.generateJWTToken(admin);
                Console.WriteLine(token);
                return Task.FromResult(token);
            }
            else
            {
                // return Task.FromResult("Password is incorrect");
                throw new Exception("Password is incorrect");
            }
        }


        public Task<string> Signup(AdminDto adminDto)
        {
            if (adminDto.AdminPassword != adminDto.AdminConfirmPassword)
            {
                return Task.FromResult("Password and Confirm Password does not match");
            }
            else if (!_adminRepository.EmailExistsAlready(adminDto.AdminEmail))
            {
                string salt;
                string password = PassworHasher.HashPassword(adminDto.AdminPassword, out salt);

                _adminRepository.CreateAdmin(adminDto.AdminName, adminDto.AdminEmail, password, salt);

                return Task.FromResult("Admin Created Successfully");
            }
            else if (_adminRepository.EmailExistsAlready(adminDto.AdminEmail))
            {
                return Task.FromResult("Email already exists");
            }
            else
            {
                return Task.FromResult("Something went wrong");
            }
        }
    }
}
