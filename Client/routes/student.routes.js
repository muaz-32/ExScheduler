const express = require('express');
const router = express.Router();
const {
    getstudentlogin,
    studentLogin,
    getstudentdashboard,
    getstudentfinalpreference,
    getstudentsignup,
    studentsignup,
    submitfinalpreference,
    getProvideDateAndPriority,
    postsubmitdateandpriority
} = require('../controllers/student.controllers');

router.get('/student/studentlogin', getstudentlogin);
router.get('/student/studentsignup', getstudentsignup);
router.get('/student/dashboard', getstudentdashboard);
router.get('/student/studentfinalpreference', getstudentfinalpreference);
router.get('/student/providedateandpriority', getProvideDateAndPriority);

router.post('/student/studentlogin', studentLogin);
router.post('/student/studentsignup', studentsignup);
router.post('/student/submitfinalpreference', submitfinalpreference);
router.post('/student/submitdateandpriority',postsubmitdateandpriority);


module.exports = router;
