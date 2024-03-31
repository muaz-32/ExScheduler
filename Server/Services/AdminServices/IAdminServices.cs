using ExScheduler_Server.Dto;
using ExScheduler_Server.Models;

namespace ExScheduler_Server.Services.AdminServices
{
    public interface IAdminServices
    {
        public ICollection<Admin> GetAdmins();
        public Task<Admin> GetByEmail(String adminemail);
        string ValidateCR(int id);
        string AddDepartment(AddDepartmentDto departmentDto);
        string AddProgrammeSemester(ProgrammeSemesterDto programmeSemesterDto);
        string addExamSchedule(ExamScheduleDto examScheduleDto);
        string AddCourse(CourseDto courseDto);
        string AddLink(LinkDto linkDto);
        ICollection<RoutineDto> FetchExamSchedules();
        ICollection<linkedCoursesWithProgSem> GetLinkedCourses();
        ICollection<LinkCourseDatePriorityDto> GetLinkCourseDatePriority();
        ICollection<PrioritiesDto> GetPriorities();
        string PostLinkedCoursesSchedule(ICollection<PostLinkedCoursesScheduleDto> postLinkedCoursesScheduleDtos);
        ICollection<LinkedCoursesWIthoutPriority> GetLinkedCoursesWithoutPriority();
        ICollection<DepartmentDto> GetDepartments();
        ICollection<ProgSemDto> GetProgrammeSemesters();
        ICollection<DateDto> GetDates();
        ICollection<GetCourseDto> GetCourses();
        ICollection<GetLinkDto> GetLinks();
        bool DeleteDepartment(string DeptId);
        bool DeleteProgrammeSemester(string ProgSemId);
        bool DeleteCourse(string CourseId);
        bool DeleteExamSchedule(string ExamScheduleId);
        bool DeleteLink(string LinkId);
    }
}
