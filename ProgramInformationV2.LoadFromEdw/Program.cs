// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using ProgramInformationV2.LoadFromEdw;
using Models = ProgramInformationV2.Search.Models;

Console.WriteLine("Starting load");

var searchUrl = "https://search-sitefinity-search-2022-mxlf4grqurtcjtyyk4gaar6inq.us-east-2.es.amazonaws.com/";
if (args.Length == 3 && args[0].Equals("courseload")) {
    await LoadCourses.Run(args[1], args[2], searchUrl);
} else {
    await LoadCourses.Run("CI", "coe", searchUrl);
}
return;

// This section is a one-time translation of the old program data to the new program data

var path = "C:\\Users\\jonker\\Downloads\\";
var file = "program_coe.json";

using var reader = new StreamReader(path + file);
var items = JsonConvert.DeserializeObject<List<dynamic>>(reader.ReadToEnd()) ?? [];
var programs = new List<Models.Program>();
foreach (var item in items) {
    Console.WriteLine($"Item: {item.title}");
    programs.Add(new Models.Program {
        Id = item.id,
        Title = item.title,
        Source = item.source,
        IsActive = item.isactive ?? true, // Default to true if not specified
        Fragment = item.fragment ?? item.url ?? "",
        Description = item.description,
        Url = item.url ?? "",
        LastUpdatedBy = item.lastupdatedby ?? "",
        TagList = item.taglist != null ? item.taglist.ToObject<List<string>>() : [],
        SkillList = item.skilllist != null ? item.skilllist.ToObject<List<string>>() : [],
        DepartmentList = item.departmentlist != null ? item.departmentlist.ToObject<List<string>>() : [],
        WhoShouldApply = item.whoshouldapply,
        VideoUrl = item.videourl ?? "",
        SummaryText = item.summarytext ?? "",
        AlternateUrl = item.alternateurl ?? "",
        AlternateUrlTitle = item.alternateurltitle ?? "",
        Credentials = item.credentials == null ? [] : ((IEnumerable<dynamic>) item.credentials).Select(c => new Models.Credential {
            Title = c.title ?? "",
            Cost = c.cost ?? "",
            Description = c.description ?? "",
            Id = c.id ?? "",
            IsActive = c.isactive ?? true,
            Fragment = c.fragment ?? c.url ?? "",
            Url = c.url ?? "",
            Source = c.source ?? "",
            HourText = c.hourtext ?? "",
            FormatType = c.formattype,
            CredentialType = c.credentialtype,
            Notes = c.notes,
            IsTranscriptable = c.istranscriptable ?? false,
            SummaryText = c.summarytext ?? "",
            SummaryTitle = c.summarytitle ?? "",
            LastUpdatedBy = item.lastupdatedby ?? "",
            TranscriptableName = c.transcriptablename ?? "",
            TagList = c.taglist != null && item.taglist != null ? (c.taglist.ToObject<List<string>>().Count != 0 ? c.taglist.ToObject<List<string>>() : item.taglist.ToObject<List<string>>()) : [],
            SkillList = c.skilllist != null && item.skilllist != null ? (c.skilllist.ToObject<List<string>>().Count != 0 ? c.skilllist.ToObject<List<string>>() : item.skilllist.ToObject<List<string>>()) : [],
            DepartmentList = c.departmentlist != null && item.departmentlist != null ? (c.departmentlist.ToObject<List<string>>().Count != 0 ? c.departmentlist.ToObject<List<string>>() : item.departmentlist.ToObject<List<string>>()) : []
        }).ToList()
    });
}
programs.ForEach(p => p.Prepare());
using var writer = new StreamWriter(path + "new-" + file);
var results = JsonConvert.SerializeObject(programs, Formatting.Indented);
writer.Write(results);