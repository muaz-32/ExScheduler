using ExScheduler_Server.Dto;
using ExScheduler_Server.Models;

namespace ExScheduler_Server.Services.Authentication
{
    public interface IAuthenticationService
    {
        public Task<string>Signup(AdminDto adminDto);
        public Task<string>Login(AdminLoginDto adminLoginDto);
        
    }
}
