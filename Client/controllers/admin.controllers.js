const bcrypt = require("bcrypt");
const passport = require("passport");
const axios = require("axios");
const https = require("https");
const { link } = require("fs");

const agent = new https.Agent({
  rejectUnauthorized: false,
});

const getDashboard = (req, res) => {
  res.render("index");
};
const getAdminDashboard = (req, res) => {
  console.log(req.user);
  res.render("admin/admindashboard");
};
const getadminlogin = (req, res) => {
  res.render("admin/adminlogin");
};
const getadminsignup = (req, res) => {
  res.render("admin/adminsignup");
}
// const getProgramSemester=(req,res)=>{
//   res.render("admin/add-program-semester");
// }
// const getAddExamSchedule=(req,res)=>{
//   res.render("admin/add-exam-schedule");
// }
// const getAddCourse=(req,res)=>{
//   res.render("admin/add-course");
// }
// const getaddlink=(req,res)=>{
//   res.render("admin/add-link");
// }
// const getadddepartment=(req,res)=>{
//   res.render("admin/add-department");
// }

const getLinkedCoursesWithoutPriority = async (req, res) => {
  try {
    const apiresponse = await axios.get(
      "https://localhost:7227/api/Admin/getLinkedCoursesWithoutPriority",
      { httpsAgent: agent }
    );
    res.json(apiresponse.data.info);
  } catch (err) {
    console.error(err.message);
  }
};

const adminLogin = passport.authenticate("admin", {
  successRedirect: "/admin/admindashboard",
  failureRedirect: "adminlogin",
  failureFlash: true
});

const adminSignup = async (req, res) => {
  let { adminname,adminpassword,cadminpassword,adminemail } = req.body;
  console.log(adminname);
  console.log(adminpassword);
  console.log(adminemail);
  console.log(cadminpassword);
  try {
    const apiResponse = await axios.post(
      "https://localhost:7227/api/Admin/adminsignup",
      {
        "adminName": adminname,
        "adminEmail": adminemail,
        "adminPassword": adminpassword,
        "adminConfirmPassword": cadminpassword
      },
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message=="Admin Created Successfully")
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.render("admin/adminsignup", { no_err });
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.render("admin/adminsignup",{error});
    }
  } catch (err) {
    console.error(err.message);
  }

}

const getTokenFromLocalStorage = () => {
  return localStorage.getItem("jwtToken");
};

const getadmins = async (req, res) => {
  try {
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/getadmins",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      }
    );
    res.json(apiResponse.data);
  } catch (err) {
    console.error(err.message);
  }
};

const adddepartment = async (req, res) => {
  let department=req.body.department;
  // console.log(department);
  try {
    const apiResponse = await axios.post(
      "https://localhost:7227/api/Admin/adddepartment",
      {
        "departmentName": department,
      },
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      }
    );
    console.log(req.user.message);
    // res.json(apiResponse.data);
    // console.log(apiResponse.data.message);
    if(apiResponse.data.message=="Department added successfully")
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  } catch (err) {
    console.error(err.message);
  }
}

const addprogramsemester = async (req, res) => {
  let programsemester=req.body.programsemester;
  let department=req.body.department;
  console.log(programsemester);
  console.log(department);
  try {
    const apiResponse = await axios.post(
      "https://localhost:7227/api/Admin/addprogrammesemester",
      {
        "programmeSemesterName": programsemester,
        "departmentName": department,
      },
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      }
    );
    console.log(req);
    console.log(apiResponse.data.message);
    if(apiResponse.data.message=="ProgrammeSemester added successfully")
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  } catch (err) {
    console.error(err.message);
  }
}

const addexamschedule = async (req, res) => {
  let examdate=req.body.examdate;
  console.log(examdate);
  try {
    const apiResponse = await axios.post(
      "https://localhost:7227/api/Admin/addexamschedule",
      {
        "examDate": examdate,
      },
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message=="ExamSchedule added successfully")
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  } catch (err) {
    console.error(err.message);
  }
}

const addcourse = async(req,res)=>{
  let programsemester=req.body.programsemester;
  let course=req.body.course;
  console.log(programsemester);
  console.log(course);
  try{
    const apiResponse = await axios.post(
      "https://localhost:7227/api/Admin/addcourse",
      {
        "courseName": course,
        "programSemesterName": programsemester
      },
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message=="Course added successfully")
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error})
    }
  } catch (err) {
    console.error(err.message);
  }
} 

const addlink = async(req,res)=>{
  let linkname=req.body.linkname;
  let course1=req.body.course1;
  let course2=req.body.course2;
  let course3=req.body.course3;
  console.log(linkname);
  console.log(course1);
  console.log(course2);
  console.log(course3);
  var courses=[];
  if(course1!="")
  {
    courses.push(course1);
  }
  if(course2!="")
  {
    courses.push(course2);
  }
  if(course3!="")
  {
    courses.push(course3);
  }
  console.log(courses);
  try{
    const apiResponse = await axios.post(
      "https://localhost:7227/api/Admin/addlink",
      {
        "linkname": linkname,
        "courses": courses
      },
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message=="Link added successfully")
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  } catch (err) {
    console.error(err.message);
  }
}

const getDepartments = async (req, res) => {
  try {
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/getDepartments",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    // console.log(apiResponse.data.info);
    return res.json(apiResponse.data.info);
  } catch (err) {
    console.error(err.message);
  }
}

const getProgramSemester = async (req, res) => {
  try {
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/getProgramSemesters",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    console.log(apiResponse.data.info);
    return res.json(apiResponse.data.info);
  } catch (err) {
    console.error(err.message);
  }
}

const getExamSchedule = async (req, res) => {
  try {
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/getDates",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    // console.log(apiResponse.data.info);
    return res.json(apiResponse.data.info);
  } catch (err) {
    console.error(err.message);
  }
}

const getCourse = async (req, res) => {
  console.log("getCourse");
  try {
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/getCourses",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    console.log(apiResponse.data.info);
    return res.json(apiResponse.data.info);
  } catch (err) {
    console.error(err.message);
  }
}

const getLink = async (req, res) => {
  try {
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/getLinks",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    // console.log(apiResponse.data.info);
    return res.json(apiResponse.data.info);
  } catch (err) {
    console.error(err.message);
  }
}

const deleteDepartment = async (req, res) => {
  let departmentId=req.body.departmentId;
  console.log(departmentId);
  try {
    const apiResponse = await axios.delete(
      "https://localhost:7227/api/Admin/deleteDepartment/"+departmentId,
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message==1)
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  }
  catch (err) {
    console.error(err.message);
  }
}

const deleteProgramSemester = async (req, res) => {
  let programsemesterId=req.body.programsemesterId;
  console.log(programsemesterId);
  try {
    const apiResponse = await axios.delete(
      "https://localhost:7227/api/Admin/deleteProgrammeSemester/"+programsemesterId,
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message==1)
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  }
  catch (err) {
    console.error(err.message);
  }
}

const deleteExamSchedule = async (req, res) => {
  let examscheduleId=req.body.examscheduleId;
  console.log(examscheduleId);
  try {
    const apiResponse = await axios.delete(
      "https://localhost:7227/api/Admin/deleteExamSchedule/"+examscheduleId,
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message==1)
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  }
  catch (err) {
    console.error(err.message);
  }
}

const deleteCourse = async (req, res) => {
  let courseId=req.body.courseId;
  console.log(courseId);
  try {
    const apiResponse = await axios.delete(
      "https://localhost:7227/api/Admin/deleteCourse/"+courseId,
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message==1)
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  }
  catch (err) {
    console.error(err.message);
  }
}

const deleteLink = async (req, res) => {
  let linkId=req.body.linkId;
  console.log(linkId);
  try {
    const apiResponse = await axios.delete(
      "https://localhost:7227/api/Admin/deleteLink/"+linkId,
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json"
        },
      }
    );
    console.log(apiResponse.data.message);
    if(apiResponse.data.message==1)
    {
      let no_err= [];
      no_err.push({ message: apiResponse.data.message });
      res.json({no_err});
    }
    else{
      let error=[];
      error.push({message:apiResponse.data.message});
      res.json({error});
    }
  }
  catch (err) {
    console.error(err.message);
  }
}


// const fetchRoutine = async (req, res) => {
//   try {
//     const apiResponse = await axios.get(
//       "https://localhost:7227/api/Admin/fetchexamschedule",
//       {
//         httpsAgent: agent,
//         headers: {
//           "Content-Type": "application/json",
//           Authorization: `Bearer ${req.user.token}`,
//         },
//       }
//     );
//     res.json(apiResponse.data);
//   } catch (err) {
//     console.error(err.message);
//   }
// }

// const fetchRoutine = async (req, res) => {
//   try {
//     const apiResponse = await axios.get(
//       "https://localhost:7227/api/Admin/fetchexamschedule",
//       {
//         httpsAgent: agent,
//         headers: {
//           "Content-Type": "application/json",
//           Authorization: `Bearer ${req.user.token}`,
//         },
//       }
//     );

//     const infoArray = apiResponse.data.info;

//     // Initialize the 2D array with header row
//     const scheduleArray = [["", "BSC CSE", "BSC EEE"]];

//     // Loop through each exam schedule and populate the 2D array
//     infoArray.forEach((info) => {
//       const dateRow = [info.examDate, "", ""];

//       info.examSchedules.forEach((schedule) => {
//         const programIndex = schedule.programSemesterName === "BSC CSE" ? 1 : 2;
//         dateRow[programIndex] = schedule.course.courseName;
//       });

//       scheduleArray.push(dateRow);
//     });
//     res.render('admin/fetch-routine', { scheduleArray });
//   } catch (err) {
//     console.error(err.message);
//     res.status(500).send("Internal Server Error");
//   }
// };

const generateRoutine = async (req, res) => {
  try{
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/generate",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      },
    );
    console.log(apiResponse.data);
    return res.json(apiResponse.data);
    
  }
  catch(err){
    console.error(err.message);
  }
}

const fetchRoutine = async (req, res) => {
  try {
    const apiResponse = await axios.get(
      "https://localhost:7227/api/Admin/fetchexamschedule",
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      }
    );

    const infoArray = apiResponse.data.info;

    // Initialize the 2D array with an empty header row
    const scheduleArray = [[]];

    // Dynamically fetch header row from examSchedules
    infoArray.forEach((info) => {
      info.examSchedules.forEach((schedule) => {
        const programName = schedule.programSemesterName;
        if (!scheduleArray[0].includes(programName)) {
          scheduleArray[0].push(programName);
        }
      });
    });

    // Loop through each exam schedule and populate the 2D array
    infoArray.forEach((info) => {
      const dateRow = [info.examDate];

      scheduleArray[0].forEach((program) => {
        const course = info.examSchedules.find(
          (schedule) => schedule.programSemesterName === program
        );

        dateRow.push(course ? course.course.courseName : "");
      });

      scheduleArray.push(dateRow);
    });
    scheduleArray[0].unshift("Date\ProgramSemester");
    // res.json(scheduleArray);

    res.render('admin/fetch-routine', { scheduleArray });
  } catch (err) {
    console.error(err.message);
    res.status(500).send("Internal Server Error");
  }
};

const getvalidatecr = async (req, res) => {
  res.render("admin/validatecr");
}

//new apis
const postvalidatecr = async (req, res) => {
  let studentID=req.body.studentID;
  try {
    const apiResponse = await axios.post(
      "https://localhost:7227/api/Admin/validatecr",
      {
        "id": studentID
      },
      {
        httpsAgent: agent,
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${req.user.token}`,
        },
      }
    );
    res.json(apiResponse.data.message);
    console.log(apiResponse.data.message);
  } catch (err) {
    console.error(err.message);
  }
}



const getTry = async (req, res) => {
  res.render("admin/multi-form/multiform/index");
};




module.exports = {
  getDashboard,
  getLinkedCoursesWithoutPriority,
  getadmins,
  adminLogin,
  getadminlogin,
  getadminsignup,
  adddepartment,
  addprogramsemester,
  addexamschedule,
  addcourse,
  addlink,
  getDepartments,
  getProgramSemester,
  getExamSchedule,
  getCourse,
  getLink,
  deleteDepartment,
  deleteProgramSemester,
  deleteExamSchedule,
  deleteCourse,
  deleteLink,
  fetchRoutine,
  adminSignup,
  getTry,
  getAdminDashboard,
  generateRoutine,
  getvalidatecr,
  postvalidatecr
};
