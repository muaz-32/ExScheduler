using ExScheduler_Server.Dto;

namespace ExScheduler_Server.Services.StudentServices
{
    public interface IStudentServices
    {
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
