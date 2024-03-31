using ExScheduler_Server.Dto;
using ExScheduler_Server.Helper;
using ExScheduler_Server.Interfaces;
using ExScheduler_Server.Models;

namespace ExScheduler_Server.Services.ClassRepresentativeAuthentication
{
    public class StudentAuthenticationService : IStudentAuthenticationService
    {
        private readonly IStudentRepository _CRRepository;
        public StudentAuthenticationService(IStudentRepository CRRepository)
        {
            _CRRepository = CRRepository;
        }
        public Task<string> Signup(StudentDto CRDto)
        {
            if (CRDto.StudentPassword != CRDto.StudentConfirmPassword)
            {
                return Task.FromResult("Password and Confirm Password does not match");
            }
            else if (!_CRRepository.EmailExistsAlready(CRDto.StudentEmail))
            {
                string salt;
                string password = PassworHasher.HashPassword(CRDto.StudentPassword, out salt);
                CRDto.salt = salt;
                CRDto.StudentPassword = password;

                _CRRepository.CreateClassRepresentative(CRDto);


                return Task.FromResult("Request sent to admin");
            }
            else if (_CRRepository.EmailExistsAlready(CRDto.StudentEmail))
            {
                return Task.FromResult("Email already exists");
            }
            else
            {
                return Task.FromResult("Something went wrong");
            }
        }
        public Task<string> Login(StudentLoginDto CRDto)
        {
            Students cr = _CRRepository.GetClassRepresentativeByEmail(CRDto.StudentEmail);
            if (cr == null)
            {
                // return Task.FromResult("Email does not exist");
                throw new Exception("Email does not exist");
            }

            bool isPasswordValid = PassworHasher.VerifyPassword(CRDto.StudentPassword, cr.StudentPassword, cr.Salt);

            if (isPasswordValid && cr.Validity==true)
            {
                string token = _CRRepository.generateJWTToken(cr);
                Console.WriteLine(token);
                return Task.FromResult(token);
            }
            else if(cr.Validity==false)
            {
                // return Task.FromResult("Sorry you are not authorized");
                throw new Exception("Sorry you are not authorized");
            }
            else
            {
                // return Task.FromResult("Password is incorrect");
                throw new Exception("Password is incorrect");
            }
        }
    }
}
