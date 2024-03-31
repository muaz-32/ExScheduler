using ExScheduler_Server.Dto;
using ExScheduler_Server.Interfaces;
using ExScheduler_Server.Models;

namespace ExScheduler_Server.Services.AdminServices
{
    public class AdminServices : IAdminServices
    {
        private readonly IAdminRepository _adminRepository;
        
        public AdminServices(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public ICollection<Admin> GetAdmins()
        {
            return _adminRepository.GetAdmins();
        }

        public Task<Admin> GetByEmail(string adminemail)
        {
            return Task.FromResult(_adminRepository.GetAdminByEmail(adminemail));
        }
        public string ValidateCR(int id)
        {
            var cr = _adminRepository.ValidateCrByID(id);
            if (cr == null)
            {
                return "CR not found";
            }
            else
            {
                if (_adminRepository.ValidateCrByID(id))
                {
                    return "CR has been made valid";
                }
                else {
                    return "CR not found in the database";
                }
            }
        }

        public string AddDepartment(AddDepartmentDto departmentDto)
        {
            if (_adminRepository.AddDepartment(departmentDto))
            {
                return "Department added successfully";
            }
            else
            {
                return "Department already exists";
            }
        }
        
        public string AddProgrammeSemester(ProgrammeSemesterDto programmeSemesterDto)
        {
            if (_adminRepository.AddProgrammeSemester(programmeSemesterDto))
            {
                return "ProgrammeSemester added successfully";
            }
            else
            {
                return "ProgrammeSemester already exists or department doesn't exist";
            }
        }
        public string AddCourse(CourseDto courseDto)
        {
            if (_adminRepository.AddCourse(courseDto))
            {
                return "Course added successfully";
            }
            else
            {
                return "Course already exists or ProgramSemester doesn't exists";
            }
        }

        public string addExamSchedule(ExamScheduleDto examScheduleDto)
        {
            if (_adminRepository.AddExamSchedule(examScheduleDto))
            {
                return "ExamSchedule added successfully";
            }
            else
            {
                return "ExamSchedule already exists or course doesn't exist";
            }
        }

        public string AddLink(LinkDto linkDto)
        {
            if (_adminRepository.AddLink(linkDto))
            {
                return "Link added successfully";
            }
            else
            {
                return "Link already exists or course doesn't exist";
            }
        }

        public ICollection<RoutineDto> FetchExamSchedules()
        {
            return _adminRepository.FetchExamSchedules();
        }

        public ICollection<linkedCoursesWithProgSem> GetLinkedCourses()
        {
            return _adminRepository.GetLinkedCourses();
        }
        public ICollection<LinkCourseDatePriorityDto> GetLinkCourseDatePriority()
        {
            return _adminRepository.GetLinkCourseDatePriority();
        }

        public ICollection<PrioritiesDto> GetPriorities()
        {
            return _adminRepository.GetPriorities();
        }
        public string PostLinkedCoursesSchedule(ICollection<PostLinkedCoursesScheduleDto> postLinkedCoursesScheduleDtos)
        {
            return _adminRepository.PostLinkedCoursesSchedule(postLinkedCoursesScheduleDtos);
        }
        public ICollection<LinkedCoursesWIthoutPriority> GetLinkedCoursesWithoutPriority()
        {
            return _adminRepository.GetLinkedCoursesWithoutPriority();
        }

        public ICollection<DepartmentDto> GetDepartments()
        {
            return _adminRepository.GetDepartments();
        }

        public ICollection<ProgSemDto> GetProgrammeSemesters()
        {
            return _adminRepository.GetProgrammeSemesters();
        }

        public ICollection<DateDto> GetDates()
        {
            return _adminRepository.GetDates();
        }

        public ICollection<GetCourseDto> GetCourses()
        {
            return _adminRepository.GetCourses();
        }

        public ICollection<GetLinkDto> GetLinks()
        {
            return _adminRepository.GetLinks();
        }

        public bool DeleteDepartment(string DeptId)
        {
            return _adminRepository.DeleteDepartment(DeptId);
        }

        public bool DeleteProgrammeSemester(string ProgSemId)
        {
            return _adminRepository.DeleteProgrammeSemester(ProgSemId);
        }

        public bool DeleteCourse(string CourseId)
        {
            return _adminRepository.DeleteCourse(CourseId);
        }

        public bool DeleteExamSchedule(string ExamScheduleId)
        {
            return _adminRepository.DeleteExamSchedule(ExamScheduleId);
        }

        public bool DeleteLink(string LinkId)
        {
            return _adminRepository.DeleteLink(LinkId);
        }
    }
}
