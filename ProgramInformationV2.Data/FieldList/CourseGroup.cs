using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class CourseGroup : BaseGroup {

        public CourseGroup() {
            CategoryType = CategoryType.Course;
            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.Course, FieldType = FieldType.General, IsRequired = true },
                new() { Title = "Rubric", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Number", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Summary", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Description", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Information", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Length", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Cost", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Enrollment Date", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Summary Link (URL)", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Apply Now / Get More Information Link (URL)", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Image URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Image Alt Text", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Video URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link },
                new() { Title = "Details", CategoryType = CategoryType.Course, FieldType = FieldType.Overview },
                new() { Title = "External Details", CategoryType = CategoryType.Course, FieldType = FieldType.Overview },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Course, FieldType = FieldType.Technical }
            ];
        }
    }
}