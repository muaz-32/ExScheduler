using ExScheduler_Server.Dto;
using ExScheduler_Server.Models;

namespace ExScheduler_Server.Interfaces
{
    public interface IStudentRepository
    {
        bool EmailExistsAlready(string crEmail);
        ICollection<Students> GetClassRepresentatives();
        public bool CreateClassRepresentative(StudentDto CRdto);
        string generateJWTToken(Students cr);
        Students GetClassRepresentativeByEmail(string studentemail);
        ICollection<linkedCoursesDto> GetLinkedCourses();
        string postDatesWithPriority(ICollection<LinkDatesDto> linkDates);
        ICollection<GetExamDateAndCourseDto> GetExamDatesAndCourses();
        string postStudentPreference(ICollection<PostStudentPreferenceDto> postStudentPreferenceDtos);
        ICollection<string> GetCourses();
        bool GetCheckPreferences();
        ICollection<string> GetAvailableDates();
        bool GetIfSubmitted();
    }
}
