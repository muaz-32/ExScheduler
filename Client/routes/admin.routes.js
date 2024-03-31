const express = require('express');
const router = express.Router();
const{
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
} = require('../controllers/admin.controllers');

router.get("/", getDashboard);
router.get("/admin/adminlogin",getadminlogin);
router.get("/admin/adminsignup",getadminsignup);
router.get('/api/Admin/getLinkedCoursesWithoutPriority', getLinkedCoursesWithoutPriority);
router.get('/api/Admin/getadmins', getadmins);
router.get('/admin/admindashboard', getAdminDashboard);
// router.get('/add-program-semester', getProgramSemester);   
// router.get('/add-exam-schedule', getAddExamSchedule); 
// router.get('/add-course', getAddCourse);
// router.get('/add-link',getaddlink);
// router.get('/add-department',getadddepartment);
router.get('/fetch-routine', fetchRoutine);

router.get('/generate-routine', generateRoutine);



router.get('/userlogout', (req, res) => {
    req.logout((err) => {
        if (err) {
            console.log(err);
        }
        else{
            console.log('User logged out');
        }
    });
    res.redirect('/admin/adminlogin');
});



router.post('/admin/adminlogin', adminLogin);
router.post('/admin/adminsignup', adminSignup);
router.post('/admin/adddepartment',adddepartment);
router.post('/admin/addprogramsemester',addprogramsemester);
router.post('/admin/addexamschedule',addexamschedule);
router.post('/admin/addcourse',addcourse);
router.post('/admin/addlink',addlink);


router.get('/getvalidatecr',getvalidatecr);
router.post('/admin/postvalidatecr',postvalidatecr)


router.get('/admin/tata', getTry);

router.get('/admin/deptTable', (req, res) => {
    res.render('admin/multi-form/deptTable');
});
router.get('/admin/getDepartments', getDepartments);
router.delete('/admin/deleteDepartment', deleteDepartment);


router.get('/admin/progSemTable', (req, res) => {
    res.render('admin/multi-form/progSemTable');
});
router.get('/admin/getProgramSemester', getProgramSemester);
router.delete('/admin/deleteProgramSemester', deleteProgramSemester);


router.get('/admin/courseTable', (req, res) => {
    res.render('admin/multi-form/courseTable');
});
router.get('/admin/getCourse', getCourse);
router.delete('/admin/deleteCourse', deleteCourse);

router.get('/admin/linkTable', (req, res) => {
    res.render('admin/multi-form/linkTable');
});
router.get('/admin/getLink', getLink);
router.delete('/admin/deleteLink', deleteLink);

router.get('/admin/examScheduleTable', (req, res) => {
    res.render('admin/multi-form/examScheduleTable');
});
router.get('/admin/getExamSchedule', getExamSchedule);
router.delete('/admin/deleteExamSchedule', deleteExamSchedule);

module.exports = router;