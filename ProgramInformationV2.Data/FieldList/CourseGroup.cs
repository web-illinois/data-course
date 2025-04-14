using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class CourseGroup : BaseGroup {

        public CourseGroup() {
            CategoryType = CategoryType.Course;
            Instructions = "Customize the fields used for courses. You can add custom instructions for each field based on your use case.";
            FieldTypeInstructions = new Dictionary<FieldType, string> {
                [FieldType.General] = "General information about the course.",
                [FieldType.Link] = "Control what links, images, and videos are added to the course page.",
                [FieldType.Overview] = "This information will be displayed on the credential page.",
                [FieldType.Location_Time] = "This information is time and room if sections are not available",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };
            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.Course, FieldType = FieldType.General, IsRequired = true, InitialDescription = "Course title -- do not include the rubric or course number, as it will be added to the title when displayed." },
                new() { Title = "Rubric", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Number", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Summary", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Description", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Information", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Length", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Cost", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Credit Hours", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Enrollment Date", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Summary Link (URL)", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Apply Now / Get More Information Link (URL)", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Image URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Image Alt Text", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Video URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Details", CategoryType = CategoryType.Course, FieldType = FieldType.Overview },
                new() { Title = "External Details", CategoryType = CategoryType.Course, FieldType = FieldType.Overview },
                new() { Title = "Building", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Room", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Term", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Days", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Time", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Course, FieldType = FieldType.Technical },
                new() { Title = "Id", CategoryType = CategoryType.Course, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage" },
                new() { Title = "Edit Link", CategoryType = CategoryType.Course, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly" }
            ];
        }
    }
}