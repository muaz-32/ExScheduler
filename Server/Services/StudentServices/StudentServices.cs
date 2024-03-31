using ExScheduler_Server.Dto;
using ExScheduler_Server.Interfaces;

namespace ExScheduler_Server.Services.StudentServices
{
    public class StudentServices : IStudentServices
    {
        private readonly IStudentRepository _CRRepository;
        public StudentServices(IStudentRepository cRRepository)
        {
            _CRRepository = cRRepository;
        }

        public ICollection<GetExamDateAndCourseDto> GetExamDatesAndCourses()
        {
            return _CRRepository.GetExamDatesAndCourses();
        }

        public string postDatesWithPriority(ICollection<LinkDatesDto> linkDates)
        {
            return _CRRepository.postDatesWithPriority(linkDates);
        }

        public string postStudentPreference(ICollection<PostStudentPreferenceDto> postStudentPreferenceDtos)
        {
            return _CRRepository.postStudentPreference(postStudentPreferenceDtos);
        }

        ICollection<linkedCoursesDto> IStudentServices.GetLinkedCourses()
        {
            return _CRRepository.GetLinkedCourses();
        }

        public ICollection<string> GetCourses()
        {
            return _CRRepository.GetCourses();
        }

        bool IStudentServices.GetCheckPreferences()
        {
            return _CRRepository.GetCheckPreferences();
        }

        public ICollection<string> GetAvailableDates()
        {
            return _CRRepository.GetAvailableDates();
        }

        public bool GetIfSubmitted()
        {
            return _CRRepository.GetIfSubmitted();
        }
    }
}
