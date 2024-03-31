include "globals.mzn"; 


%%%%%%%%%%%%% There is big assumption. There will always be 6 days and 6 courses       %%%%%%%%%%%%%%%%%%%%%%%%%%
int: num_days;
int: num_classes;
int: num_courses;
int: total_courses;
int: num_common_courses;

array[1..num_classes, 1..num_courses] of var 1..total_courses: data;
array [1..num_common_courses, 1..2] of var int: common_courses_count;
type Preference = record (
  var int: day,
  var int: value,
  var int: course_id
);
array[1..num_classes, 1..num_courses, 1..3] of Preference: preferences; 

% decision variables
array[1..num_days, 1..num_classes] of var 1..total_courses: s;


var int: total_preference_value = sum(c in 1..num_classes, cr in 1..num_courses, p in 1..3)(
  let { 
    var int: course_id = preferences[c, cr, p].course_id;
    var int: day = preferences[c, cr, p].day;
    var int: value = preferences[c, cr, p].value;
  } in
  value * bool2int(s[day, c] = course_id)
);

solve maximize total_preference_value;

constraint
  forall(c in 1..num_classes, course in 1..total_courses) (
    count([s[d,c] | d in 1..num_days], course) <= 1
  )
  
  /\
  
  forall(d in 1..num_days, c in 1..num_classes) (
    let {
      var 1..total_courses: class_course = s[d,c]; % Course assigned to the class
      array[1..num_courses] of var 1..total_courses: class_courses;
    } in
    class_courses = [data[c, i] | i in 1..num_courses] /\
    class_course in class_courses
  )
  
  /\
  
  forall(c in 1..num_common_courses, d in 1..num_days) (
    let {
      var int: course_id = common_courses_count[c, 1]; % Course ID
      var int: class_count = common_courses_count[c, 2]; % Number of classes taking this course
      var int: exams_on_date = count([s[d, cl] | cl in 1..num_classes], course_id);
    } in
    exams_on_date = 0 \/ exams_on_date = class_count
  )
;



output 
["s:\n"]
++
[
  show2d(s)
]
++
["\nmax: \n"]
++
[
  show(total_preference_value)
]
;

  
  
  