const bcrypt = require("bcrypt");
const passport = require("passport");
const axios = require("axios");
const https = require("https");
const { link } = require("fs");

const agent = new https.Agent({
    rejectUnauthorized: false,
});

const getstudentlogin = (req, res) => {
    res.render("student/studentlogin");
};

const studentLogin = passport.authenticate("student", {
    successRedirect: "/student/dashboard",
    failureRedirect: "studentlogin",
    failureFlash: true,
});

const getstudentdashboard = (req, res) => {
    res.render("student/dashboard");
};

const getstudentfinalpreference = async (req, res) => {
    try {
        const apiResponse = await axios.get(
            "https://localhost:7227/api/Student/GetCourses",
            {
                httpsAgent: agent,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${req.user.token}`,
                },
            }
        );
        console.log(apiResponse.data.info);
        const courses = apiResponse.data.info;

        const apiResponse1 = await axios.get(
            "https://localhost:7227/api/Student/GetExamDates",
            {
                httpsAgent: agent,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${req.user.token}`,
                },
            }
        );
        console.log(apiResponse1.data.info);
        const coursesWithDates = apiResponse1.data.info;
        console.log(coursesWithDates);
        const combinedCourses = courses.map((course) => ({
            courseName: course,
            examDate:
                (coursesWithDates.find((cd) => cd.courseName === course) || {})
                    .examDate || null,
        }));

        const apiResponse2 = await axios.get(
            "https://localhost:7227/api/Student/GetAvailableCourses",
            {
                httpsAgent: agent,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${req.user.token}`,
                },
            }
        );
        let availableDates=[];
        availableDates=apiResponse2.data.info;

        for (let i = 0; i < combinedCourses.length; i++) {
            console.log(apiResponse2.data.info[i]);
            console.log(combinedCourses[i].examDate);
            const index = availableDates.findIndex(date => date === combinedCourses[i].examDate);
    
            if (index !== -1) {
                availableDates.splice(index, 1);
            }
        }
        console.log(availableDates);

        res.render("student/studentfinalpreference", { courses: combinedCourses, availableDates });
    } catch (err) {
        console.error(err.message);
    }
};


const submitfinalpreference = async (req, res) => {
    try {
        const { courses, dates } = req.body;
        console.log(courses, dates);
        //check if dates array has duppliacte values
        const isDuplicate = dates.some((val, i) => dates.indexOf(val) !== i);
        if(isDuplicate) {
            let error = [{ info: "Duplicate dates are not allowed" }];
            res.render("student/dashboard", { error });
        }
        else{
            let formattedData = [];
        if(Array.isArray(courses)) {
        // Assuming courses and dates are arrays of strings
            formattedData = courses.map((courseName, index) => ({
                examDate: dates[index],
                courseName: courseName,
            }));
        }
        else {
            formattedData = [{
                examDate: dates,
                courseName: courses,
            }];
        }
        // Log the formatted data for verification
        console.log(formattedData);
        const apiResponse = await axios.post(
            "https://localhost:7227/api/Student/PostStudentPreferences",
            formattedData,
            {
                httpsAgent: agent,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${req.user.token}`,
                },
            }
        );

        console.log(apiResponse.data.info);

        if (apiResponse.data.info === "Student preferences added successfully") {
            let no_err = [{ message: apiResponse.data.info }];
            res.render("student/dashboard", { no_err });
        } else {
            let error = [{ info: apiResponse.data.info }];
            res.render("student/dashboard");
        }
        }
    } catch (err) {
        console.error(err.message);
        res.status(500).json({ error: "Internal Server Error" });
    }
};


const getstudentsignup = (req, res) => {
    res.render("student/studentsignup");
};

const studentsignup = async (req, res) => {
    const {
        studentid,
        studentname,
        studentemail,
        programsemester,
        studentpassword,
        cstudentpassword,
    } = req.body;
    console.log(
        studentid,
        studentname,
        studentemail,
        programsemester,
        studentpassword,
        cstudentpassword
    );
    try {
        const apiResponse = await axios.post(
            "https://localhost:7227/api/Student/CRSignup",
            {
                studentId: studentid,
                studentName: studentname,
                studentEmail: studentemail,
                programeSemester: programsemester,
                studentPassword: studentpassword,
                studentConfirmPassword: cstudentpassword,
                salt: "string",
            },
            {
                httpsAgent: agent,
                headers: {
                    "Content-Type": "application/json",
                },
            }
        );
        console.log(apiResponse.data.message);
        if (apiResponse.data.message == "Request sent to admin") {
            let no_err = [];
            no_err.push({ message: apiResponse.data.message });
            res.render("student/studentsignup", { no_err });
        } else {
            let error = [];
            error.push({ info: apiResponse.data.message });
            res.render("student/studentsignup", { error });
        }
    } catch (err) {
        console.error(err.message);
    }
};

const getProvideDateAndPriority = async (req, res) => {
    try {
        const apiResponse3 = await axios.get(
            "https://localhost:7227/api/Student/GetIfSubmitted",
            {
                httpsAgent: agent,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${req.user.token}`,
                },
            }
        )
        console.log(apiResponse3.data.info);
        if(apiResponse3.data.info == true){
            let error = [];
            error.push({ info: "You have already submitted your preferences" });
            res.render("student/dashboard");
        }
        else{
            const apiResponse = await axios.get(
                "https://localhost:7227/api/Student/GetLinkedCourses",
                {
                    httpsAgent: agent,
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${req.user.token}`,
                    },
                }
            );
            const apiResponse2 = await axios.get(
                "https://localhost:7227/api/Student/GetAvailableCourses",
                {
                    httpsAgent: agent,
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${req.user.token}`,
                    },
                }
            );
            
            
    
            let availableDates=[];
            availableDates=apiResponse2.data.info;
            console.log(apiResponse.data.info);
            const linkWithcourses = apiResponse.data.info;
            res.render("student/providedateandpriority", { linkWithcourses ,availableDates});
        }
        
    } catch (err) {
        console.error(err.message);
    }
}

const postsubmitdateandpriority = async (req, res) => {
    try {
        const { linkname, dateInput, priorityInput } = req.body;
        console.log(linkname);
        console.log("hi");
        console.log(dateInput);
        console.log("hi");
        console.log(priorityInput);
        if(Array.isArray(linkname)){
            let isDuplicate = false;
            let dateinput=dateInput;
            for(let i=0;i<dateinput.length;i++){
                let arr=[];
                for(let j=0;j<3;j++,i++){
                    arr.push(dateinput[j]);
                }
                isDuplicate = arr.some((val, i) => arr.indexOf(val) !== i);
            }
            if(isDuplicate) {
                let error = [{ info: "Duplicate dates are not allowed" }];
                res.render("student/dashboard", { error });
            }      
            else{
                    const resultArray = [];
                let j = 0
                for (let i = 0; i < linkname.length; i++) {
                    for (let k=0; k < 3; k++,j++) {
                        const linkName = linkname[i];
                        const examDate = dateInput[j];
                        const priority = parseInt(priorityInput[j]);

                        resultArray.push({ linkName, examDate, priority });
                    }
                }

                console.log(resultArray);

                const apiResponse = await axios.post(
                    'https://localhost:7227/api/Student/PostLinkedCourses', 
                    resultArray,
                    {
                        httpsAgent: agent,
                        headers: {
                            'Content-Type': 'application/json',
                            Authorization: `Bearer ${req.user.token}`,
                        },
                    }
                );

                console.log(apiResponse.data.message);
                if (apiResponse.data.message === 'Dates with priority added successfully') {
                    let no_err = [];
                    no_err.push({ message: apiResponse.data.message });
                    res.render('student/dashboard', { no_err });

                } else {
                    let error = [];
                    error.push({ info: apiResponse.data.message });
                    res.render('student/dashboard', { error });
                }
            }
        }
        else{
            console.log("I am here");
            const resultArray = [];
                let j = 0
                const linkName = linkname;
                    for (let k=0; k < 3; k++,j++) {
                        const examDate = dateInput[j];
                        const priority = parseInt(priorityInput[j]);

                        resultArray.push({ linkName, examDate, priority });
                    }

                console.log(resultArray);

                const apiResponse = await axios.post(
                    'https://localhost:7227/api/Student/PostLinkedCourses', 
                    resultArray,
                    {
                        httpsAgent: agent,
                        headers: {
                            'Content-Type': 'application/json',
                            Authorization: `Bearer ${req.user.token}`,
                        },
                    }
                );

                console.log(apiResponse.data.message);
                if (apiResponse.data.message === 'Dates with priority added successfully') {
                    let no_err = [];
                    no_err.push({ message: apiResponse.data.message });
                    res.render('student/dashboard', { no_err });

                } else {
                    let error = [];
                    error.push({ info: apiResponse.data.message });
                    res.render('student/dashboard', { error });
                }
        }
    } catch (err) {
        console.error(err.message);
        res.status(500).json({ error: 'Internal Server Error' });
    }
}

module.exports = {
    getstudentlogin,
    studentLogin,
    getstudentdashboard,
    getstudentfinalpreference,
    getstudentsignup,
    studentsignup,
    submitfinalpreference,
    getProvideDateAndPriority,
    postsubmitdateandpriority
};
