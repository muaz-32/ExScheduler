using System.Diagnostics;
using ExScheduler_Server.Data;
using ExScheduler_Server.Dto;
using ExScheduler_Server.Interfaces;
using ExScheduler_Server.Models;
using ExScheduler_Server.Services.AdminServices;
using ExScheduler_Server.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OpenApi.Any;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Generators;

namespace ExScheduler_Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAdminServices _adminServices;
        private readonly DataContext _context;

        public AdminController(IAuthenticationService authenticationService, IAdminServices adminServices, DataContext _context)
        {
            _authenticationService = authenticationService;
            _adminServices = adminServices;
            this._context = _context;
        }


        //AdminCotrollerStarts
        [HttpPost("adminsignup")]
        [ProducesResponseType(201, Type = typeof(Admin))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminDto adminDTO)
        {
            return Ok(new { message = await _authenticationService.Signup(adminDTO) });
        }




        [HttpPost("adminlogin")]
        [ProducesResponseType(201, Type = typeof(Admin))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> Login([FromBody] AdminLoginDto adminLogindto)
        {
            return Ok(new { token = await _authenticationService.Login(adminLogindto) });
        }



        [HttpGet("getadmins")]
        [Authorize]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Admin>))]
        public async Task<IActionResult> GetAdmins()
        {
            var admins = _adminServices.GetAdmins();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(admins);
        }




        [HttpGet("{adminemail}")]
        [ProducesResponseType(200, Type = typeof(Admin))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize]
        public async Task<IActionResult> GetAdminByEmail(string adminemail)
        {
            return Ok(new {admin = await _adminServices.GetByEmail(adminemail)});
        }



        //Update the CR validity
        [HttpPost("validatecr")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize]
        public IActionResult ValidateCR([FromBody] validateCRDto validateCRDto)
        {
            var message = _adminServices.ValidateCR(validateCRDto.id);
            return Ok(new {message});
        }

        //Add Departments
        [HttpPost("adddepartment")]
        [ProducesResponseType(201, Type = typeof(Department))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        // [Authorize]
        public IActionResult AddDepartment([FromBody] AddDepartmentDto departmentDto)
        {
            return Ok(new { message = _adminServices.AddDepartment(departmentDto) });
        }

        //Add ProgrammeSemester
        [HttpPost("addprogrammesemester")]
        [ProducesResponseType(201, Type = typeof(ProgrammeSemester))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        // [Authorize]
        public IActionResult AddProgrammeSemester([FromBody] ProgrammeSemesterDto programmeSemesterDto)
        {
            return Ok(new { message = _adminServices.AddProgrammeSemester(programmeSemesterDto) });
        }

        //Add ExamSchedule
        [HttpPost("addexamschedule")]
        [ProducesResponseType(201, Type = typeof(ExamSchedule))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        // [Authorize]
        public IActionResult AddExamSchedule([FromBody] ExamScheduleDto examScheduleDto)
        {
            return Ok(new { message = _adminServices.addExamSchedule(examScheduleDto) });
        }

        //Add Course
        [HttpPost("addcourse")]
        [ProducesResponseType(201, Type = typeof(Course))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        // [Authorize]
        public IActionResult AddCourse([FromBody] CourseDto courseDto)
        {
            return Ok(new { message = _adminServices.AddCourse(courseDto) });
        }

        //Add Links
        [HttpPost("addlink")]
        [ProducesResponseType(201, Type = typeof(Links))]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        // [Authorize]
        public IActionResult AddLink([FromBody] LinkDto linkDto)
        {
            return Ok(new { message = _adminServices.AddLink(linkDto) });
        }


        //Generate Exam Schedule
        [HttpGet("fetchexamschedule")]
        [Authorize]
        public IActionResult GenerateExamSchedule()
        {
            return Ok(new { info = _adminServices.FetchExamSchedules() });
        }

        [HttpGet("getLinkedcourses")]
        [Authorize]
        public IActionResult GetLinkedCourses()
        {
            return Ok(new { info = _adminServices.GetLinkedCourses() });
        }

        [HttpGet("getLinkedCourseDatePriority")]
        [Authorize]
        public IActionResult GetLinkedCourseDatePriority()
        {
            return Ok(new { info = _adminServices.GetLinkCourseDatePriority() });
        }

        [HttpGet("getpriorities")]
        [Authorize]
        public IActionResult GetPriorities()
        {
            return Ok(new { info = _adminServices.GetPriorities() });
        }













        // //////////////////////////////////////// CRUD Options ////////////////////////////////////////////////////
        [HttpGet("getDepartments")]
        // [Authorize]
        public IActionResult GetDepartments()
        {
            return Ok(new { info = _adminServices.GetDepartments() });
        }

        [HttpGet("getProgramSemesters")]
        // [Authorize]
        public IActionResult GetProgrammeSemesters()
        {
            return Ok(new { info = _adminServices.GetProgrammeSemesters() });
        }

        [HttpGet("getDates")]
        // [Authorize]
        public IActionResult GetDates()
        {
            return Ok(new { info = _adminServices.GetDates() });
        }

        [HttpGet("getCourses")]
        // [Authorize]
        public IActionResult GetCourses()
        {
            return Ok(new { info = _adminServices.GetCourses() });
        }

        [HttpGet("getLinks")]
        // [Authorize]
        public IActionResult GetLinks()
        {
            return Ok(new { info = _adminServices.GetLinks() });
        }

        [HttpDelete("deleteDepartment/{DeptId}")]
        // [Authorize]
        public IActionResult DeleteDepartment(string DeptId)
        {
            return Ok(new { message = _adminServices.DeleteDepartment(DeptId) });
        }

        [HttpDelete("deleteProgrammeSemester/{ProgSemId}")]
        // [Authorize]
        public IActionResult DeleteProgrammeSemester(string ProgSemId)
        {
            return Ok(new { message = _adminServices.DeleteProgrammeSemester(ProgSemId) });
        }

        [HttpDelete("deleteCourse/{CourseId}")]
        // [Authorize]
        public IActionResult DeleteCourse(string CourseId)
        {
            return Ok(new { message = _adminServices.DeleteCourse(CourseId) });
        }

        [HttpDelete("deleteExamSchedule/{ExamScheduleId}")]
        // [Authorize]
        public IActionResult DeleteExamSchedule(string ExamScheduleId)
        {
            return Ok(new { message = _adminServices.DeleteExamSchedule(ExamScheduleId) });
        }

        [HttpDelete("deleteLink/{LinkId}")]
        // [Authorize]
        public IActionResult DeleteLink(string LinkId)
        {
            return Ok(new { message = _adminServices.DeleteLink(LinkId) });
        }




        // //////////////////////////////////////// End of CRUD ////////////////////////////////////////////////////
        [HttpGet("getLinkedCoursesWithoutPriority")]
        [Authorize]
        public IActionResult GetLinkedCoursesWithoutPriority()
        {
            return Ok(new { info = _adminServices.GetLinkedCoursesWithoutPriority() });
        }
        int total_distinct_courses;

        // [HttpPost("getBlankDates")]
        // public IActionResult GetBlankDates(GetDatesDto getDatesDto)
        // {
        //     return Ok(new { info = _adminServices.GetBlankDates(getDatesDto) });
        // }

        [HttpGet("generate")]
        [Authorize]
        public IActionResult GetGenerate()
        {
            int num_classes = _context.ProgramSemesters.Count();
            int num_courses_per_class = _context.ExamSchedules.Count();
            List<linkedCoursesWithProgSem> linkedCoursesWithProgSems = getLinkedCourses();
            // return Ok(new { info = linkedCoursesWithProgSems });
            Dictionary<string, int> programmeSemesterToInt = getProgrammeSemesterDictionary();
            // foreach(var linkedCourseWithProgSem in linkedCoursesWithProgSems)
            // {
            //     Console.WriteLine(linkedCourseWithProgSem.linkID);
            //     Console.WriteLine(linkedCourseWithProgSem.courses.Count);
            //     Console.WriteLine(linkedCourseWithProgSem.classes.Count);
            // }
            
            int[,] data = getAllCourseRelatedInfo(ref linkedCoursesWithProgSems, programmeSemesterToInt, num_classes, num_courses_per_class);
            // Dictionary<int, int> commonCourseCount = getCommonCourseCount(linkedCoursesWithProgSems);
            // insert the 2d array data into a 2d list
            List<List<int>> data_list = new List<List<int>>();
            for(int i = 0; i < num_classes; i++)
            {
                List<int> temp = new List<int>();
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    temp.Add(data[i, j]);
                }
                data_list.Add(temp);
            }
            Dictionary<int, int> commonCourseCount = getCommonCourseCount(linkedCoursesWithProgSems);
            int num_days = _context.ExamSchedules.Count();
            string writeData = "num_courses = " + num_courses_per_class + ";\n" +
            "num_classes = " + num_classes + ";\n" +
            "num_days = " + num_days + ";\n" +
            "num_common_courses = " + commonCourseCount.Count + ";\n" +
            "total_courses = " + total_distinct_courses + ";\n" +
            "data = array2d(1..num_classes, 1..num_courses,\n" +
            "[\n";
            for(int i = 0; i < num_classes; i++)
            {
                writeData += "   ";
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    writeData += data[i, j] + ", ";
                }
                writeData += "\n";
            }
            writeData += "]);\n\n\n";
            writeData += "common_courses_count = array2d(1..num_common_courses, 1..2,\n" +
            "[\n";
            foreach(var kvp in commonCourseCount)
            {
                writeData += "   " + kvp.Key + ", " + kvp.Value + ",\n";
            }
            writeData += "]);\n\n\n";
            Dictionary<string, int> datesToInt = getDatesDictionary();
            int [,,] preferences = new int[num_classes, num_courses_per_class, 3];
            for(int i = 0; i < num_classes; i++)
            {
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    for(int k = 0; k < 3; k++)
                    {
                        preferences[i, j, k] = 1;
                    }
                }
            }
            var priorities = _adminServices.GetPriorities();
            foreach(var priority in priorities)
            {
                int classID = programmeSemesterToInt[priority.programmeSemesterName];
                var a = linkedCoursesWithProgSems.Find(link => link.linkID == priority.linkname);
                int b = 0;
                for(int i = 0; i < num_courses_per_class; i++)
                {
                    if(data[classID, i] == a.val)
                    {
                        b = i;
                    }
                }
                int courseID = data[classID, b];
                int dateID = datesToInt[priority.examDate];
                int priorityID = int.Parse(priority.priority);
                preferences[classID, b, int.Parse(priority.priority) - 1] = dateID;
            }
            List<List<List<int>>> preferences_list = new List<List<List<int>>>();
            for(int i = 0; i < num_classes; i++)
            {
                List<List<int>> temp = new List<List<int>>();
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    List<int> temp2 = new List<int>();
                    for(int k = 0; k < 3; k++)
                    {
                        temp2.Add(preferences[i, j, k]);
                    }
                    temp.Add(temp2);
                }
                preferences_list.Add(temp);
            }
            writeData += "preferences = array3d(1..num_classes, 1..num_courses, 1..3,\n" +
            "[\n";
            for(int i = 0; i < num_classes; i++)
            {
                writeData += "% Class: " + (i + 1) + "\n";
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    for(int k = 0; k < 3; k++)
                    {
                        if(data[i, j] <= imp_val)
                        {
                            writeData += "(day: " + preferences[i, j, k] + ", value: " + (3 - k) + ", course_id: " + data[i, j] + "), ";
                        }
                        else
                        {
                            writeData += "(day: " + preferences[i, j, k] + ", value: " + 0 + ", course_id: " + data[i, j] + "), ";
                        }
                    }
                    writeData += "\n";
                }
                writeData += "\n";
            }
            writeData += "]);";
            System.IO.File.WriteAllText(@"D:\DP-1\DP1\newServer\exschedular-server\Minizinc\data.dzn", writeData);
            string command = "minizinc";
            string arguments = "D:\\DP-1\\DP1\\newServer\\exschedular-server\\Minizinc\\model.mzn -d D:\\DP-1\\DP1\\newServer\\exschedular-server\\Minizinc\\data.dzn --solver Chuffed";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                string newoutput = process.StandardOutput.ReadToEnd();
                // string output = System.IO.File.ReadAllText(@"D:\DP-1\DP1\newServer\exschedular-server\Minizinc\solution.txt");
                string error = process.StandardError.ReadToEnd();

                System.IO.File.WriteAllText(@"D:\DP-1\DP1\newServer\exschedular-server\Minizinc\solution.txt", newoutput);
                
                process.WaitForExit();

                string output = System.IO.File.ReadAllText(@"D:\DP-1\DP1\newServer\exschedular-server\Minizinc\solution.txt");

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }
                List<LinkCourseSchedule> linkCourseSchedules = ConvertOutputToSchedule(output, datesToInt, programmeSemesterToInt, linkedCoursesWithProgSems);
                // return Ok(new { info = linkCourseSchedules });
                ICollection<PostLinkedCoursesScheduleDto> postLinkedCoursesScheduleDtos = new List<PostLinkedCoursesScheduleDto>();
                foreach(var linkCourseSchedule in linkCourseSchedules)
                {
                    PostLinkedCoursesScheduleDto postLinkedCoursesScheduleDto = new PostLinkedCoursesScheduleDto
                    {
                        courseName = linkCourseSchedule.courseName,
                        date = linkCourseSchedule.date,
                        programmeSemesterName = linkCourseSchedule.programmeSemesterName
                    };
                    postLinkedCoursesScheduleDtos.Add(postLinkedCoursesScheduleDto);
                }
                string ans = _adminServices.PostLinkedCoursesSchedule(postLinkedCoursesScheduleDtos);
                if(ans == "Exam Schedule added successfully")
                {
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
                // return Ok(ans);
            }
               
        }

        private List<LinkCourseSchedule> ConvertOutputToSchedule(string output, Dictionary<string, int> datesToInt, Dictionary<string, int> programmeSemesterToInt, List<linkedCoursesWithProgSem> linkedCoursesWithProgSems)
        {
            List<List<string>> schedule = new List<List<string>>();
            string[] lines = output.Split('\n');
            // trim the last 4 lines
            string[] trimmedLines = new string[lines.Length - 8];
            for(int i = 1; i < lines.Length - 7; i++)
            {
                trimmedLines[i - 1] = lines[i];
            }
            // replace [ with " " in the second line
            trimmedLines[0] = trimmedLines[0].Replace("[", " ");
            for(int i = 0; i < trimmedLines.Length; i++)
            {
                Console.WriteLine(trimmedLines[i][3..]);
            }            
            // trim the first 2 characters from all the lines
            for(int i = 0; i < trimmedLines.Length; i++)
            {
                trimmedLines[i] = trimmedLines[i][3..];
            }

            List<List<int>> table = new List<List<int>>();
            foreach (string line in trimmedLines)
            {
                List<int> row = line.Split(',').Select(item => int.Parse(item.Trim())).ToList();
                table.Add(row);
            }
            List<LinkCourseSchedule> linkCourseSchedules = new List<LinkCourseSchedule>();
            for(int i = 0; i < table.Count; i++)
            {
                for(int j = 0; j < table[i].Count; j++)
                {
                    if(table[i][j] <= imp_val)
                    {
                        string date = datesToInt.FirstOrDefault(x => x.Value == (i + 1)).Key;
                        string programSemesterName = programmeSemesterToInt.FirstOrDefault(x => x.Value == j).Key;
                        string courseName = linkedCoursesWithProgSems.FirstOrDefault(x => x.val == table[i][j]).courses.Find(x => x.ProgrammeSemester.programSemesterName == programSemesterName).courseName;
                        // string courseName = linkedCoursesWithProgSems.FirstOrDefault(x => x.val == table[i][j]).courses.FirstOrDefault().courseName;
                        LinkCourseSchedule linkCourseSchedule = new LinkCourseSchedule
                        {
                            val = table[i][j],
                            courseName = courseName,
                            date = date,
                            programmeSemesterName = programSemesterName
                        };
                        linkCourseSchedules.Add(linkCourseSchedule);
                    }
                }
            }
            
            return linkCourseSchedules;
        }

        struct LinkCourseSchedule
        {
            public int val;
            public string courseName;
            public string date;
            public string programmeSemesterName;
        }

        private Dictionary<string, int> getDatesDictionary()
        {
            var dates = _context.ExamSchedules.ToList();
            Dictionary<string, int> datesToInt = new Dictionary<string, int>();
            int val = 1;
            foreach (var date in dates)
            {
                datesToInt.Add(date.examDate, val);
                val++;
            }
            return datesToInt;
        }
        private Dictionary<string, int> getProgrammeSemesterDictionary()
        {
            var programmeSemesters = _context.ProgramSemesters.ToList();
            Dictionary<string, int> programmeSemesterToInt = new Dictionary<string, int>();
            int val = 0;
            foreach (var programmeSemester in programmeSemesters)
            {
                programmeSemesterToInt.Add(programmeSemester.programSemesterName, val);
                val++;
            }
            Console.WriteLine(val);
            return programmeSemesterToInt;
        }

        struct linkedCoursesWithProgSem
        {
            public int val;
            public string linkID;
            public List<Course> courses;
            public List<ProgrammeSemester> classes;
        }

        private List<linkedCoursesWithProgSem> getLinkedCourses()
        {
            var linkedCourses = _adminServices.GetLinkCourseDatePriority();
            // insert the linknames in a set
            HashSet<string> linkNames = new HashSet<string>();
            foreach (var linkedCourse in linkedCourses)
            {
                linkNames.Add(linkedCourse.LinkName);
            }
            List<linkedCoursesWithProgSem> linkedCoursesWithProgSems = new List<linkedCoursesWithProgSem>();
            foreach (var linkName in linkNames)
            {
                linkedCoursesWithProgSem linkedCoursesWithProgSem = new linkedCoursesWithProgSem
                {
                    linkID = linkName,
                    courses = new List<Course>(),
                    classes = new List<ProgrammeSemester>()
                };
                var set = linkedCourses.Where(link => link.LinkName == linkName);
                HashSet<string> set2 = new HashSet<string>();
                foreach(var s in set)
                {
                    set2.Add(s.CourseName);
                }
                foreach (var linkedCourse in set2)
                {
                    // Console.WriteLine(linkedCourse);
                    // find the course with the coursename using context
                    var course = _context.Courses.Include(c => c.ProgrammeSemester).Where(c => c.courseName == linkedCourse).FirstOrDefault();
                    linkedCoursesWithProgSem.courses.Add(course);
                    

                    var programmeSemester = _context.ProgramSemesters.Where(ps => ps.programSemesterID == course.ProgrammeSemester.programSemesterID).FirstOrDefault();
                    linkedCoursesWithProgSem.classes.Add(programmeSemester);
                }
                linkedCoursesWithProgSems.Add(linkedCoursesWithProgSem);
            }
            return linkedCoursesWithProgSems;
        }

        int imp_val = 0;
        private int[,] getAllCourseRelatedInfo(ref List<linkedCoursesWithProgSem> linkedCoursesWithProgSems, Dictionary<string, int> programmeSemesterToInt, int num_classes, int num_courses_per_class)
        {
            int[,] data = new int[num_classes, num_courses_per_class];
            for(int i = 0; i < num_classes; i++)
            {
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    data[i, j] = 0;
                }
            }
            int val = 1;
            for(int i = 0; i <  linkedCoursesWithProgSems.Count; i++)
            {
                for(int j = 0; j < linkedCoursesWithProgSems[i].classes.Count; j++)
                {
                    
                    for(int k = 0; k < num_courses_per_class; k++)
                    {
                        int test = programmeSemesterToInt[linkedCoursesWithProgSems[i].classes[j].programSemesterName];
                        if(data[test, k] == 0)
                        {
                            data[programmeSemesterToInt[linkedCoursesWithProgSems[i].classes[j].programSemesterName], k] = val;
                            var temp = linkedCoursesWithProgSems[i];
                            temp.val = val;
                            linkedCoursesWithProgSems[i] = temp;
                            break;
                            // break from this loop
                            
                        }
                    }
                    // data[programmeSemesterToInt[linkedCoursesWithProgSems[i].classes[j].programSemesterName], 0] = val;
                    
                }
                val++;
            }
            imp_val = val - 1;
            // Console.WriteLine(num_classes);
            // Console.WriteLine(num_courses_per_class);
            for(int i = 0; i < num_classes; i++)
            {
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    if(data[i, j] == 0)
                    {
                        data[i, j] = val;
                        val++;
                    }
                }
            }
            total_distinct_courses = val - 1;
            return data;
        }

        private Dictionary<int, int> getCommonCourseCount(List<linkedCoursesWithProgSem> linkedCoursesWithProgSems)
        {
            int[,] commonCourseCount = new int[linkedCoursesWithProgSems.Count, 2];
            for(int i = 0; i < linkedCoursesWithProgSems.Count; i++)
            {
                commonCourseCount[i, 0] = linkedCoursesWithProgSems[i].val;
                commonCourseCount[i, 1] = linkedCoursesWithProgSems[i].courses.Count;
            }
            Dictionary<int, int> kvp = new Dictionary<int, int>();
            for(int i = 0; i < linkedCoursesWithProgSems.Count; i++)
            {
                kvp.Add(commonCourseCount[i, 0], commonCourseCount[i, 1]);
            }
            return kvp;
        }

        private string writeData(Dictionary<int, int> commonCourseCount, int[,] data, int num_courses_per_class, int num_classes, int num_days, int total_distinct_courses, List<linkedCoursesWithProgSem> linkedCoursesWithProgSems){
            string writeData = "num_courses = " + num_courses_per_class + ";\n" +
            "num_classes = " + num_classes + ";\n" +
            "num_days = " + num_days + ";\n" +
            "num_common_courses = " + commonCourseCount.Count + ";\n" +
            "total_courses = " + total_distinct_courses + ";\n" +
            "data = array2d(1..num_classes, 1..num_courses,\n" +
            "[\n";
            for(int i = 0; i < num_classes; i++)
            {
                writeData += "   ";
                for(int j = 0; j < num_courses_per_class; j++)
                {
                    writeData += data[i, j] + ", ";
                }
                writeData += "\n";
            }
            writeData += "]);\n\n\n";
            writeData += "common_courses_count = array2d(1..num_common_courses, 1..2,\n" +
            "[\n";
            foreach(var kvp in commonCourseCount)
            {
                writeData += "   " + kvp.Key + ", " + kvp.Value + ",\n";
            }
            writeData += "]);\n\n\n";
            return writeData;
        }
        
        
    }
}
