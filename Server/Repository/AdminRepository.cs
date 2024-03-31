using AutoMapper.Execution;
using ExScheduler_Server.Data;
using ExScheduler_Server.Dto;
using ExScheduler_Server.Interfaces;
using ExScheduler_Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Npgsql;


namespace ExScheduler_Server.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AdminRepository(DataContext context,IConfiguration configuration)
        {
            _context = context;
            this._configuration = configuration;
        }
        private bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }

        public Admin GetAdminByEmail(string adminemail)
        {
            return _context.Admins.FirstOrDefault(p => p.AdminEmail == adminemail);
        }
        public ICollection<Admin> GetAdmins()
        {
            return _context.Admins.OrderBy(p => p.AdminID).ToList();
        }
        public bool CreateAdmin(string AdminName, string AdminEmail, string AdminPassword,string Salt)
        {
            Admin admin = new Admin();
            admin.AdminName = AdminName;
            admin.AdminEmail = AdminEmail;
            admin.AdminPassword = AdminPassword;
            admin.Salt = Salt;
            _context.Add(admin);
            return Save();
        }
        public bool EmailExistsAlready(string adminEmail)
        {
            return _context.Admins.Any(admin => admin.AdminEmail == adminEmail);
        }
        public string generateJWTToken(Admin admin)
        {
            // Generate JWT token
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("AdminID", admin.AdminID.ToString()),
                        new Claim("AdminEmail", admin.AdminEmail),
                        new Claim("AdminName", admin.AdminName)
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
        public bool ValidateCrByID(int id)
        {
            var cr = _context.Students.FirstOrDefault(p => p.StudentID == id);
            if (cr != null)
            {
                cr.Validity = true;
                _context.Update(cr);
                return Save();
            }
            return false;
        }
        public bool AddDepartment(AddDepartmentDto departmentdto)
        {
            //find if department already exists
            var dept = _context.Departments.FirstOrDefault(p => p.departmentName == departmentdto.departmentName);
           
            if (dept != null)
            {
                return false;
            }
            else
            {
                Department department = new Department();
                department.departmentName = departmentdto.departmentName;
                _context.Add(department);
                return Save();
            }
        }
        public bool AddProgrammeSemester(ProgrammeSemesterDto programmeSemesterDto)
        {
            //Find deparmentID by departmentName
            var dept = _context.Departments.FirstOrDefault(p => p.departmentName == programmeSemesterDto.departmentName);

            //Find if programme already exists
            var prog = _context.ProgramSemesters.FirstOrDefault(p => p.programSemesterName == programmeSemesterDto.programmeSemesterName && p.department == dept);
            if (prog != null || dept==null)
            {
                return false;
            }
            else
            {
                ProgrammeSemester programmeSemester = new ProgrammeSemester();
                programmeSemester.programSemesterName = programmeSemesterDto.programmeSemesterName;
                programmeSemester.department = dept;
                _context.Add(programmeSemester);
                return Save();
            }
        }
        public bool AddExamSchedule(ExamScheduleDto examScheduleDto)
        {
            //Find if examSchedule already exists
            var examSchedule = _context.ExamSchedules.FirstOrDefault(p => p.examDate == examScheduleDto.examDate);
            if (examSchedule != null)
            {
                return false;
            }
            else
            {
                ExamSchedule examSchedule1 = new ExamSchedule();
                examSchedule1.examDate = examScheduleDto.examDate;
                _context.Add(examSchedule1);
                return Save();
            }
        }
        public bool AddCourse(CourseDto courseDto)
        {
            //Caution: Course Name should be uniquely identifyable

            //Find programmeSemesterID by programmeSemesterName
            var prog = _context.ProgramSemesters.FirstOrDefault(p => p.programSemesterName == courseDto.programSemesterName);

            //Find if course already exists
            var course = _context.Courses.FirstOrDefault(p => p.courseName == courseDto.CourseName && p.ProgrammeSemester == prog);
            if (course != null || prog == null)
            {
                return false;
            }
            else
            {
                Course course1 = new Course();
                course1.courseName = courseDto.CourseName;
                course1.ProgrammeSemester = prog;
                course1.ExamSchedule = null;
                _context.Add(course1);
                return Save();
            }
        }
        public bool AddLink(LinkDto linkDto)
        {
            //check if linkname already exists
            var linkdummy1 = _context.Links.FirstOrDefault(p => p.linkname == linkDto.linkname);
            if (linkdummy1 != null)
            {
                return false;
            }
            //check if courses in linkdto exists
            foreach (var coursedummy in linkDto.courses)
            {
                var course = _context.Courses.FirstOrDefault(p => p.courseName == coursedummy);
                if (course == null)
                {
                    return false;
                }
            }
            //add linkname to Links
            Links link = new Links();
            link.linkname = linkDto.linkname;
            _context.Add(link);
            Save();

            //Fetch linkID using linkname
            var linkdummy = _context.Links.FirstOrDefault(p => p.linkname == linkDto.linkname);

            //add the courses in database of linkdto 
            foreach (var coursedummy in linkDto.courses)
            {
                //Find courseID by courseName
                var course = _context.Courses.FirstOrDefault(p => p.courseName == coursedummy);

                //add linkID and courseID to LinkCourses
                LinkCourse linkCourses = new LinkCourse();
                linkCourses.LinkID = linkdummy.linkID;
                linkCourses.CourseID = course.courseID;
                _context.Add(linkCourses);
                Save();
            }
            return Save();
        }
        public ICollection<RoutineDto> FetchExamSchedules()
        {
            var query = from course in _context.Courses
                        join examSchedule in _context.ExamSchedules on course.ExamSchedule.examScheduleID equals examSchedule.examScheduleID
                        join programSemester in _context.ProgramSemesters on course.ProgrammeSemester.programSemesterID equals programSemester.programSemesterID
                        orderby examSchedule.examDate, programSemester.programSemesterName, course.courseName
                        group new { examSchedule.examDate, programSemester.programSemesterName, course.courseName } by new { examSchedule.examDate } into grouped
                        select new RoutineDto
                        {
                            ExamDate = grouped.Key.examDate,
                            ExamSchedules = grouped.Select(x =>
                                new GenerateExamScheduleDto
                                {
                                    ProgramSemesterName = x.programSemesterName,
                                    Course = new GenerateCourseDto { CourseName = x.courseName }
                                }).ToList()
                        };


            return query.ToList();
        }
        public ICollection<linkedCoursesWithProgSem> GetLinkedCourses()
        {
            var linkCourses = from linkCourse in _context.LinkCourses
                              join course in _context.Courses on linkCourse.CourseID equals course.courseID
                              select new linkedCoursesWithProgSem
                              {
                                  CourseName = course.courseName,
                                  ProgrammeSemester = course.ProgrammeSemester.programSemesterName
                              };

            return linkCourses.ToList();
        }

        public ICollection<LinkCourseDatePriorityDto> GetLinkCourseDatePriority()
        {
            var linkCourseDatePriority = from course in _context.Courses
                                         join linkCourse in _context.LinkCourses on course.courseID equals linkCourse.CourseID
                                         join link in _context.Links on linkCourse.LinkID equals link.linkID
                                         join linkExamDate in _context.LinkExamDates on link.linkID equals linkExamDate.LinkID
                                         join examSchedule in _context.ExamSchedules on linkExamDate.ExamScheduleID equals examSchedule.examScheduleID
                                         select new LinkCourseDatePriorityDto
                                         {
                                             LinkName = link.linkname,
                                             CourseName = course.courseName,
                                             ExamDate = examSchedule.examDate,
                                             Priority = linkExamDate.Priority
                                         };

            return linkCourseDatePriority.ToList();
        }

        public ICollection<PrioritiesDto> GetPriorities()
        {
            var query = from linkExamDate in _context.LinkExamDates
                        join link in _context.Links on linkExamDate.LinkID equals link.linkID
                        join examSchedule in _context.ExamSchedules on linkExamDate.ExamScheduleID equals examSchedule.examScheduleID
                        join programSemester in _context.ProgramSemesters on linkExamDate.ProgrammeSemesterID equals programSemester.programSemesterID
                        select new PrioritiesDto
                        {
                            linkname = link.linkname,
                            examDate = examSchedule.examDate,
                            programmeSemesterName = programSemester.programSemesterName,
                            priority = linkExamDate.Priority.ToString()
                        };

            return query.ToList();
        }

        public string PostLinkedCoursesSchedule(ICollection<PostLinkedCoursesScheduleDto> postLinkedCoursesScheduleDtos)
        {
            foreach (var postLinkedCoursesScheduleDto in postLinkedCoursesScheduleDtos)
            {
                var course = _context.Courses.FirstOrDefault(p => p.courseName == postLinkedCoursesScheduleDto.courseName);
                if(course == null)
                {
                    return "Course doesn't exist";
                }
                else
                {
                    var schedule = _context.ExamSchedules.FirstOrDefault(p => p.examDate == postLinkedCoursesScheduleDto.date);
                    course.ExamSchedule = schedule;
                }
            }
            return Save() ? "Exam Schedule added successfully" : "course doesn't exist";
        }

        public ICollection<LinkedCoursesWIthoutPriority> GetLinkedCoursesWithoutPriority()
        {
            var linkedCoursesWithoutPriority = from linkCourse in _context.LinkCourses
                                               join link in _context.Links on linkCourse.LinkID equals link.linkID
                                               join course in _context.Courses on linkCourse.CourseID equals course.courseID
                                               group course.courseName by new { link.linkname } into grouped
                                               select new LinkedCoursesWIthoutPriority
                                               {
                                                   linkName = grouped.Key.linkname,
                                                   course1 = grouped.FirstOrDefault(),
                                                   course2 = grouped.Skip(1).FirstOrDefault()
                                               };

            return linkedCoursesWithoutPriority.ToList();
        }

        public ICollection<DepartmentDto> GetDepartments()
        {
            var query = from department in _context.Departments orderby department.departmentName ascending
                        select new DepartmentDto
                        {
                            DeptId = department.departmentID.ToString(),
                            DeptName = department.departmentName
                        };
            
            return query.ToList();
        }

        public ICollection<ProgSemDto> GetProgrammeSemesters()
        {
            var query = from programmeSemester in _context.ProgramSemesters
                        join department in _context.Departments on programmeSemester.department.departmentID equals department.departmentID orderby programmeSemester.programSemesterName ascending
                        select new ProgSemDto
                        {
                            ProgSemId = programmeSemester.programSemesterID.ToString(),
                            ProgSemName = programmeSemester.programSemesterName,
                            DeptName = department.departmentName
                        };

            return query.ToList();
        }

        public ICollection<DateDto> GetDates()
        {
            var query = from examSchedule in _context.ExamSchedules orderby examSchedule.examDate ascending
                        select new DateDto
                        {
                            DateId = examSchedule.examScheduleID.ToString(),
                            Date = examSchedule.examDate
                        };

            return query.ToList();
        }

        public ICollection<GetCourseDto> GetCourses()
        {
            var query = from course in _context.Courses
                        join programmeSemester in _context.ProgramSemesters on course.ProgrammeSemester.programSemesterID equals programmeSemester.programSemesterID orderby course.courseName ascending
                        select new GetCourseDto
                        {
                            CourseId = course.courseID.ToString(),
                            CourseName = course.courseName,
                            ProgSemName = programmeSemester.programSemesterName
                        };

            return query.ToList();
        }

        public ICollection<GetLinkDto> GetLinks()
        {
            var query = from link in _context.Links
                        join linkCourse in _context.LinkCourses on link.linkID equals linkCourse.LinkID
                        join course in _context.Courses on linkCourse.CourseID equals course.courseID orderby link.linkname ascending
                        select new GetLinkDto
                        {
                            LinkId = link.linkID.ToString(),
                            LinkName = link.linkname,
                            CourseName = course.courseName
                        };

            return query.ToList();
        }

        public bool DeleteDepartment(string DeptId)
        {
            var department = _context.Departments.FirstOrDefault(p => p.departmentID.ToString() == DeptId);
            if (department != null)
            {
                _context.Remove(department);
                return Save();
            }
            return false;
        }

        public bool DeleteProgrammeSemester(string ProgSemId)
        {
            var programmeSemester = _context.ProgramSemesters.FirstOrDefault(p => p.programSemesterID.ToString() == ProgSemId);
            if (programmeSemester != null)
            {
                _context.Remove(programmeSemester);
                return Save();
            }
            return false;
        }

        public bool DeleteCourse(string CourseId)
        {
            var course = _context.Courses.FirstOrDefault(p => p.courseID.ToString() == CourseId);
            if (course != null)
            {
                _context.Remove(course);
                return Save();
            }
            return false;
        }

        public bool DeleteExamSchedule(string ExamScheduleId)
        {
            var examSchedule = _context.ExamSchedules.FirstOrDefault(p => p.examScheduleID.ToString() == ExamScheduleId);
            if (examSchedule != null)
            {
                _context.Remove(examSchedule);
                return Save();
            }
            return false;
        }

        public bool DeleteLink(string LinkId)
        {
            string connectionString = "Host=localhost;Username=muaz;Password=test123;Database=exscheduler";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Your DELETE command goes here
                string deleteCommand = "DELETE FROM \"Links\" WHERE \"linkID\" = '" + LinkId + "'";
                using (NpgsqlCommand command = new NpgsqlCommand(deleteCommand, connection))
                {
                    // check if the command executed properly
                    if (command.ExecuteNonQuery() > 0)
                    {
                        string deleteCommand2 = "update \"Courses\" set \"examScheduleID\" = null;";
                        using (NpgsqlCommand command2 = new NpgsqlCommand(deleteCommand2, connection))
                        {
                            if (command2.ExecuteNonQuery() > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }

        }
    }
}
