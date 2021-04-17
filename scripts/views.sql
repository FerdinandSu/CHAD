START TRANSACTION;

CREATE VIEW `RelStudentCourses` AS
SELECT `c`.`CourseId`,`s`.`StudentId`
FROM `RelStudentClasses` AS `s`
INNER JOIN `RelCourseClasses` AS `c`
ON `c`.`ClassId`=`s`.`ClassId`;

CREATE VIEW `ResourceSummaries` AS
SELECT `r`.`Id`, `r`.`Name`, `a`.`FriendlyName` AS `Uploader`, LENGTH(`r`.`Content`) AS `Size`
FROM `Resources` AS `r` 
INNER JOIN `AspNetUsers` 
AS `a` ON `r`.`UploaderId` = `a`.`Id`
ORDER BY `r`.`UploadTime` DESC;

CREATE VIEW `RelResourceCourses` AS
SELECT `l`.`CourseId`, `r`.`ResourceId`
FROM `RelResourceLessons` AS `r`
INNER JOIN `Lessons` AS `l`
ON `l`.`Id` = `r`.`LessonId`;

COMMIT;