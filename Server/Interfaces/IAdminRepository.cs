using ExScheduler_Server.Dto;
using ExScheduler_Server.Models;

namespace ExScheduler_Server.Interfaces
{
    public interface IAdminRepository
    {
        ICollection<Admin> GetAdmins();
        Admin GetAdminByEmail(String adminemail);
        bool EmailExistsAlready(string adminEmail);
        bool CreateAdmin(String AdminName,String AdminEmail,String AdminPassword,String AdminConfirmPassword);
        string generateJWTToken(Admin admin);
        bool ValidateCrByID(int id);
        bool AddDepartment(AddDepartmentDto departmentDto);
        bool AddProgrammeSemester(ProgrammeSemesterDto programmeSemesterDto);
        bool AddCourse(CourseDto courseDto);
        bool AddExamSchedule(ExamScheduleDto examScheduleDto);
        bool AddLink(LinkDto linkDto);
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
