// See https://aka.ms/new-console-template for more information
using ProgramInformationV2.LoadFromEdw;

Console.WriteLine("Starting load");

/*

// College of Education values: CI,EDPR,EDUC,EOL,EPOL,EPS,ERAM,EPSY,HRD,HRE,SPED

if (args.Length == 3 && args[0].Equals("courseload")) {
    await LoadCourses.Run(args[1], args[2], searchUrl, searchKey, searchSecret, "https://education.illinois.edu/course/{rubric}/{coursenumber}");
} else {
    await LoadCourses.Run("EPOL,ERAM", "coe", searchUrl, searchKey, searchSecret, "https://education.illinois.edu/course/{rubric}/{coursenumber}");
}

// This section is a one-time translation of the old program data to the new program data

var path = "C:\\Users\\jonker\\Downloads\\";
var file = "program_coe.json";
OneTimeProgramTranslation.TranslatePrograms(path, file);

// This section is a one-time translation of the old requirement set data to the new requirement set data

var path = "C:\\Users\\jonker\\Downloads\\";
var file = "requirementset_coe.json";
var filecp = "courseprogram_coe.json";
await OneTimeRequirementTranslation.TranslateRequirementSets(path, file, filecp, searchUrl, searchKey, searchSecret);

*/

// This section is a one-time translation of the old course data

var path = "C:\\Users\\jonker\\Downloads\\";
var file = "course_gies_online.json";
OneTimeCourseTranslation.TranslateCourses(path, file);