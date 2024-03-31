using ExScheduler_Server.Dto;

namespace ExScheduler_Server.Services.ClassRepresentativeAuthentication
{
    public interface IStudentAuthenticationService
    {
        Task<string> Signup(StudentDto CRDto);
        Task<string> Login(StudentLoginDto CRLoginDto);
    }
}
