namespace ProgramInformationV2.Data.CourseraImport {
    public class CourseraCourse {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Url { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string ImageAltText { get; set; } = "";

        public bool IsCourseFree { get; set; }

        public bool IsCreditEligible { get; set; }

        public string Id { get; set; } = "";
        public List<string> Skills { get; set; } = [];
        public List<string> Instructors { get; set; } = [];

    }
}
