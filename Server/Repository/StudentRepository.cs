using ExScheduler_Server.Data;
using ExScheduler_Server.Dto;
using ExScheduler_Server.Interfaces;
using ExScheduler_Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ExScheduler_Server.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentRepository(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this._configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        private bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved >= 0 ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
        }
        public bool EmailExistsAlready(string crEmail)
        {
            return _context.Students.Any(cr => cr.StudentEmail == crEmail);
        }  
        public Students GetClassRepresentativeByEmail(string studentemail)
        {
            return _context.Students.FirstOrDefault(p => p.StudentEmail == studentemail);
        }
        public ICollection<Students> GetClassRepresentatives()
        {
            return _context.Students.OrderBy(p => p.StudentID).ToList();
        }
        public string generateJWTToken(Students classRepresentative)
        {
            //fetch programSemesterID from studentID
            var programSemesterID = from ProgrammeSemester in _context.ProgramSemesters
                                    join Students in _context.Students on ProgrammeSemester.programSemesterID equals Students.program.programSemesterID
                                    where Students.StudentID == classRepresentative.StudentID
                                    select ProgrammeSemester.programSemesterID;

            //fetch departmentID from programSemesterID
            var departmentID = from Department in _context.Departments
                               join ProgrammeSemester in _context.ProgramSemesters on Department.departmentID equals ProgrammeSemester.department.departmentID
                               where ProgrammeSemester.programSemesterID == programSemesterID.FirstOrDefault()
                               select Department.departmentID;

            // Generate JWT token
            var claims = new[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("StudentID", classRepresentative.StudentID.ToString()),
                        new Claim("StudentEmail", classRepresentative.StudentEmail),
                        new Claim("StudentName", classRepresentative.StudentName),
                        new Claim("DepartmentID", departmentID.FirstOrDefault().ToString()),
                        new Claim("ProgramSemesterID", programSemesterID.FirstOrDefault().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public bool CreateClassRepresentative(StudentDto CRDto)
        {
            //Check if program exists
            var program = _context.ProgramSemesters.FirstOrDefault(p => p.programSemesterName == CRDto.programeSemester);   
            if (program == null)
            {
                return false;
            }
            else
            {
                //Create new class representative
                Students classRepresentative = new Students();
                classRepresentative.StudentID = CRDto.StudentId;
                classRepresentative.StudentName = CRDto.StudentName;
                classRepresentative.StudentEmail = CRDto.StudentEmail;
                classRepresentative.StudentPassword = CRDto.StudentPassword;
                classRepresentative.Salt = CRDto.salt;
                classRepresentative.program = program;
                _context.Add(classRepresentative);
                return Save();
            }
        }

        public ICollection<linkedCoursesDto> GetLinkedCourses()
        {
            // Access JWT information from HttpContext
            var user = _httpContextAccessor?.HttpContext?.User;

            // Retrieve specific claim (e.g., ProgramSemesterID)
            var programSemesterIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "ProgramSemesterID");

            if (programSemesterIdClaim != null && Guid.TryParse(programSemesterIdClaim.Value, out var studentProgramSemester))
            {
                var query = from linkCourse in _context.LinkCourses
                            join course in _context.Courses on linkCourse.CourseID equals course.courseID
                            join link in _context.Links on linkCourse.LinkID equals link.linkID
                            join programsemester in _context.ProgramSemesters on course.ProgrammeSemester.programSemesterID equals programsemester.programSemesterID
                            where programsemester.programSemesterID == studentProgramSemester
                            select new { link.linkname, course.courseName };

                var groupedData = query.GroupBy(result => result.linkname)
                                       .Select(group => new linkedCoursesDto
                                       {
                                           LinkName = group.Key,
                                           Courses = group.Select(item => item.courseName).ToList()
                                       })
                                       .ToList();

                return groupedData;
            }
            else
            {
                // Handle the case where "ProgramSemesterID" claim is not present or not a valid Guid
                // You might want to return an error response or handle it accordingly
                return null;
            }
        }


        public bool GetIfSubmitted()
        {
            var user = _httpContextAccessor?.HttpContext?.User;

            // Retrieve specific claim (e.g., ProgramSemesterID)
            var programSemesterIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "ProgramSemesterID");

            if (programSemesterIdClaim != null && Guid.TryParse(programSemesterIdClaim.Value, out var studentProgramSemester))
            {
                var query = from linkExamDate in _context.LinkExamDates
                            where linkExamDate.ProgrammeSemesterID == studentProgramSemester
                            select linkExamDate;

                if (query.Count() == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                // Handle the case where "ProgramSemesterID" claim is not present or not a valid Guid
                // You might want to return an error response or handle it accordingly
                return false;
            }
        }


        public string postDatesWithPriority(ICollection<LinkDatesDto> linkDates)
        {
            //insert data in LinkExamDate table
            foreach (var linkDate in linkDates)
            {
                LinkExamDate linkExamDates = new LinkExamDate();
                linkExamDates.Link = _context.Links.FirstOrDefault(p => p.linkname == linkDate.linkName);
                linkExamDates.ExamSchedule = _context.ExamSchedules.FirstOrDefault(p => p.examDate == linkDate.examDate);
                linkExamDates.ProgrammeSemester = _context.ProgramSemesters.FirstOrDefault(p => p.programSemesterName == linkDate.programmeSemester);
                if(linkExamDates.Link == null || linkExamDates.ExamSchedule == null)
                {
                    return "Link or ExamDate does not exist";
                }
                linkExamDates.Priority = linkDate.priority;
                _context.Add(linkExamDates);
            }
            return Save() ? "Dates with priority added successfully" : "Preference already exists";
        }

        public ICollection<GetExamDateAndCourseDto> GetExamDatesAndCourses()
        {
            // Access JWT information from HttpContext
            var user = _httpContextAccessor?.HttpContext?.User;

            // Retrieve specific claim (e.g., ProgramSemesterID)
            var programSemesterIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "ProgramSemesterID");

            var examDatesAndCourses = from course in _context.Courses
                                      join examSchedule in _context.ExamSchedules on course.ExamSchedule.examScheduleID equals examSchedule.examScheduleID
                                      join programSemester in _context.ProgramSemesters on course.ProgrammeSemester.programSemesterID equals programSemester.programSemesterID
                                      where course.ProgrammeSemester.programSemesterID == Guid.Parse(programSemesterIdClaim.Value)
                                      select new GetExamDateAndCourseDto
                                      {
                                          courseName = course.courseName,
                                          examDate = examSchedule.examDate
                                      };
            return examDatesAndCourses.ToList();
        }

        public string postStudentPreference(ICollection<PostStudentPreferenceDto> postStudentPreferenceDtos)
        {
            //find the courses using the postStudentPreferenceDtos.courseName
            foreach (var postStudentPreferenceDto in postStudentPreferenceDtos)
            {
                var course = _context.Courses.FirstOrDefault(p => p.courseName == postStudentPreferenceDto.courseName);
                if (course == null)
                {
                    return "Course does not exist";
                }
                else
                {
                    //find the scheduleID using examschedule table
                    var schedule = _context.ExamSchedules.FirstOrDefault(p => p.examDate == postStudentPreferenceDto.examDate);
                    //update course with the schedule
                    if(course.ExamSchedule==null)
                    {
                        course.ExamSchedule = schedule;
                    }
                    else
                    {
                        return "Course already has an exam date";
                    }   
                }
            }
            return Save() ? "Student preferences added successfully" : "Something went wrong while adding student preferences";
        }

        public ICollection<string> GetCourses()
        {
            var user = _httpContextAccessor?.HttpContext?.User;
            var programSemesterIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "ProgramSemesterID");

            var courses = from course in _context.Courses
                          where course.ProgrammeSemester.programSemesterID == Guid.Parse(programSemesterIdClaim.Value)
                          select course.courseName;
            return courses.ToList();
        }

        public bool GetCheckPreferences()
        {
            var progSemCount = _context.LinkCourses.Count();
            var count = _context.LinkExamDates
                      .Select(x => x.ProgrammeSemesterID)
                      .Distinct()
                      .Count();
            if (count == progSemCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ICollection<string> GetAvailableDates()
        {
            var query = from es in _context.ExamSchedules
                        where es.examDate != null
                        select new availableDatesDto
                        {
                           examDate = es.examDate
                        };
            return query.Select(x => x.examDate).ToList();
        }
    }
}
