using ExScheduler_Server.Dto;
using ExScheduler_Server.Models;
using ExScheduler_Server.Services.AdminServices;
using ExScheduler_Server.Services.ClassRepresentativeAuthentication;
using ExScheduler_Server.Services.StudentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExScheduler_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentAuthenticationService _CRAuthentication;
        private readonly IStudentServices _CRServices;

        public StudentController(IStudentAuthenticationService cRAuthentication, IStudentServices CRServices)
        {
            _CRAuthentication = cRAuthentication;
            _CRServices = CRServices;
        }

        [HttpPost("CRSignup")]
        [ProducesResponseType(201, Type = typeof(Students))]
        public async Task<IActionResult> CreateCR([FromBody] StudentDto CRDto)
        {
            return Ok(new { message = await _CRAuthentication.Signup(CRDto) });
        }

        [HttpPost("CRLogin")]
        [ProducesResponseType(201, Type = typeof(Students))]
        public async Task<IActionResult> Login([FromBody] StudentLoginDto cRLoginDto)
        {
            return Ok(new { message = await _CRAuthentication.Login(cRLoginDto) });
        }

        //Check the JWTtoken information
        [HttpGet("CheckToken")]
        // [Authorize]
        public IActionResult CheckToken()
        {
            var studentInfo = User.Claims
            .Where(x => x.Type == "StudentID" && x.Value == "200042158")
            .Select(x => new { x.Type, x.Value });
            return Ok(studentInfo);
        }

        //Get the list of linked courses
        [HttpGet("GetLinkedCourses")]
        // [Authorize]
        public IActionResult GetLinkedCourses()
        {   
            return Ok(new { info = _CRServices.GetLinkedCourses() });
        }

        [HttpGet("GetIfSubmitted")]
        // [Authorize]
        public IActionResult GetIfSubmitted()
        {
            return Ok(new { info = _CRServices.GetIfSubmitted() });
        }

        //Post the list of linked courses
        [HttpPost("PostLinkedCourses")]
        // [Authorize]
        public IActionResult postDatesWithPriority([FromBody] ICollection<LinkDatesDto> linkDates)
        {
            return Ok(new { message = _CRServices.postDatesWithPriority(linkDates) });
        }


        //Get the dates and corresponding Courses
        [HttpGet("GetExamDates")]
        // [Authorize]
        public IActionResult GetExamDatesAndCourses()
        {
            return Ok(new { info = _CRServices.GetExamDatesAndCourses() });
        }

        //Post Student's preferences
        [HttpPost("PostStudentPreferences")]
        // [Authorize]
        public IActionResult PostStudentPreferences([FromBody] ICollection<PostStudentPreferenceDto> postStudentPreferenceDtos)
        {
            return Ok(new { message = _CRServices.postStudentPreference(postStudentPreferenceDtos) });
        }

        [HttpGet("GetCourses")]
        // [Authorize]
        public IActionResult GetCourses()
        {
            return Ok(new { info = _CRServices.GetCourses() });
        }

        [HttpGet("GetAvailableCourses")]
        // [Authorize]
        public IActionResult GetAvailableCourses()
        {
            return Ok(new { info = _CRServices.GetAvailableDates() });
        }

        [HttpGet("GetCheckPreferences")]
        // [Authorize]
        public IActionResult GetCheckPreferences()
        {
            return Ok(_CRServices.GetCheckPreferences());
        }
    }
}
