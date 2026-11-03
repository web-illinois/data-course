using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class CourseGroup : BaseGroup {

        public CourseGroup() {
            CategoryType = CategoryType.Course;
            Instructions = "Customize the fields used for courses. You can edit custom instructions for each field based on your use case.";
            FieldTypeInstructions = new Dictionary<FieldType, string> {
                [FieldType.General] = "General information about the course, including information pulled from Banner.",
                [FieldType.Link] = "Control what links, images, and videos are added to the course page.",
                [FieldType.Overview] = "This information will be displayed on the credential page.",
                [FieldType.Admissions] = "This information will be displayed as admission information on the course page.",
                [FieldType.QuoteOption] = "Controls what quote will be added to the course page.",
                [FieldType.Filters] = "Be able to create filters for the course to help search.",
                [FieldType.RelatedLinks] = "Generate a list of links associated with a course. This is intended to be displayed on the course page.",
                [FieldType.NotesList] = "Generate an FAQ-like area or individual notes for a course. This is intended to be displayed on the course page.",
                [FieldType.Location_Time] = "This information is time and room if sections are not available",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };
            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.Course, FieldType = FieldType.General, IsRequired = true, InitialDescription = "Name your course. This will appear when users search for courses. -- Do not include the rubric or course number, as it will be added to the title when displayed." },
                new() { Title = "Rubric", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Number", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Summary Title", CategoryType = CategoryType.Course, FieldType = FieldType.General, InitialDescription = "Change the title of the summary if needed. This will appear when users search programs; they will read a brief summary of the program before clicking into the program page." },
                new() { Title = "Summary Text", CategoryType = CategoryType.Course, FieldType = FieldType.General, InitialDescription = "Briefly summarize the program. This will appear when users search programs; they will read a brief summary of the program before clicking into the program page." },
                new() { Title = "Contact", CategoryType = CategoryType.Course, FieldType = FieldType.General, InitialDescription = "What is the contact name? This can be a faculty member or a department name." },
                new() { Title = "Contact Email", CategoryType = CategoryType.Course, FieldType = FieldType.General, InitialDescription = "What is the contact email? Make sure this is the full email address." },
                new() { Title = "Platform Type", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Description", CategoryType = CategoryType.Course, FieldType = FieldType.General, InitialDescription = "This text should describe the course. It will be on the course page and is pulled from Banner. If you need a rich-text version, use the Details section." },
                new() { Title = "Information", CategoryType = CategoryType.Course, FieldType = FieldType.General, InitialDescription = "For example: 3 undergraduate hours. 2 or 4 graduate hours." },
                new() { Title = "Course Length", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Course Cost", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Credit Hours", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "Enrollment Date", CategoryType = CategoryType.Course, FieldType = FieldType.General },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "Note that the URL fragment is used to make searching for this item easier and to meet SEO standards. This needs to be unique and consist of lower-case letters, numbers, dashes, and the '/' character. Do not use this if you cannot meet these requirements and rely on the ID to be a unique identifier." },
                new() { Title = "Link URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "This link is where your course page information will live, it is the live page link. You will need to copy this from your CMS (such as Sitefinity)." },
                new() { Title = "Apply Now / Get More Information Link URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "Link to campus course details or to apply/register for course." },
                new() { Title = "Course Image URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the course finder. You must add alternative text when linking an image." },
                new() { Title = "Course Image Alt Text", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "Alternative text for the image." },
                new() { Title = "Summary Image", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the course finder. You must add alternative text when uploading an image." },
                new() { Title = "Summary Image Alternative Text", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "Alternative text for the program image for the course page." },
                new() { Title = "Detail Image", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the course page. You must add alternative text when uploading an image." },
                new() { Title = "Detail Image Alternative Text", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "Alternative text for the detail image." },

                new() { Title = "Video URL", CategoryType = CategoryType.Course, FieldType = FieldType.Link, InitialDescription = "This should link to a video that you would like featured on the course page. Please be sure the video has captions and/or transcripts." },
                new() { Title = "Details", CategoryType = CategoryType.Course, FieldType = FieldType.Overview, InitialDescription = "Course details." },
                new() { Title = "External Details", CategoryType = CategoryType.Course, FieldType = FieldType.Overview, InitialDescription = "Course details from an external source such as Coursera." },
                new() { Title = "Who Should Apply", CategoryType = CategoryType.Course, FieldType = FieldType.Overview, InitialDescription = "This text should detail who is eligible and may be interested in the program. It will be on the program page." },
                new() { Title = "Job Type Information", CategoryType = CategoryType.Course, FieldType = FieldType.Overview, InitialDescription = "This text should describe what job titles could this credential prepare students for." },
                new() { Title = "Professional Information", CategoryType = CategoryType.Course, FieldType = FieldType.Overview, InitialDescription = "This text should detail what job titles could this credential prepare students for." },
                new() { Title = "Skills", CategoryType = CategoryType.Course, FieldType = FieldType.Overview, InitialDescription = "This text should detail what skills this credential prepare students for. This is distinct from the filters skills section." },
                new() { Title = "Admission Requirements", CategoryType = CategoryType.Course, FieldType = FieldType.Admissions, InitialDescription = "This text should detail what you need to do to apply, or what requirements there are for this." },
                new() { Title = "Transfer Student Requirements", CategoryType = CategoryType.Course, FieldType = FieldType.Admissions, InitialDescription = "This text should detail what you need to do to apply if you are a transfer student, or what requirements there are. Be sure to distinguish between transfers from another college at Illinois and transferring from a community college or another four-year college." },
                new() { Title = "Course Tracks", CategoryType = CategoryType.Course, FieldType = FieldType.Admissions, InitialDescription = "This text should detail course tracks or alternative versions of the course." },
                new() { Title = "Quote Title", CategoryType = CategoryType.Course, FieldType = FieldType.QuoteOption, InitialDescription = "This text should give the quote title, usually the person's name." },
                new() { Title = "Quote Subtitle", CategoryType = CategoryType.Course, FieldType = FieldType.QuoteOption, InitialDescription = "This text should give the quote sub-title, usually the person's group (alumni, new student, etc.)." },
                new() { Title = "Quote", CategoryType = CategoryType.Course, FieldType = FieldType.QuoteOption, InitialDescription = "This text should give the quote. Do not put the quote in quotes." },
                new() { Title = "Quote Image", CategoryType = CategoryType.Course, FieldType = FieldType.QuoteOption, InitialDescription = "This should link to an image that you would like featured on the quote. You must add alternative text when uploading an image." },
                new() { Title = "Quote Image Alternative Text", CategoryType = CategoryType.Course, FieldType = FieldType.QuoteOption, InitialDescription = "Alternative text for the quote image." },
                new() { Title = "Filters", CategoryType = CategoryType.Course, InitialDescription = "", FieldType = FieldType.Filters },
                new() { Title = "Related Links", CategoryType = CategoryType.Course, InitialDescription = "", FieldType = FieldType.RelatedLinks },
                new() { Title = "Note List", CategoryType = CategoryType.Course, InitialDescription = "", FieldType = FieldType.NotesList },
                new() { Title = "Building", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Room", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Term", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Days", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Time", CategoryType = CategoryType.Course, FieldType = FieldType.Location_Time },
                new() { Title = "Filters", CategoryType = CategoryType.Course, InitialDescription = "", FieldType = FieldType.Filters },
                new() { Title = "Note List", CategoryType = CategoryType.Course, InitialDescription = "", FieldType = FieldType.NotesList },
                new() { Title = "Internal Search Text", CategoryType = CategoryType.Course, InitialDescription = "Internal text used to manage search fields. Not displayed to the end user but used in search.", FieldType = FieldType.Technical },
                new() { Title = "Id", CategoryType = CategoryType.Course, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage." },
                new() { Title = "Edit Link", CategoryType = CategoryType.Course, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly." }
            ];
        }
    }
}
