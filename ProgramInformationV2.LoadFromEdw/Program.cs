// See https://aka.ms/new-console-template for more information
using ProgramInformationV2.LoadFromEdw;

Console.WriteLine("Starting load");

var searchUrl = "https://search-sitefinity-search-2022-mxlf4grqurtcjtyyk4gaar6inq.us-east-2.es.amazonaws.com/";

if (args.Length == 3 && args[0].Equals("courseload")) {
    await LoadCourses.Run(args[1], args[2], searchUrl);
}