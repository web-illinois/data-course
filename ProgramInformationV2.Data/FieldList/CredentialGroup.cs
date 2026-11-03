using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class CredentialGroup : BaseGroup {

        public CredentialGroup() {
            CategoryType = CategoryType.Credential;
            Instructions = "Customize the fields used for credentials. You can edit custom instructions for each field based on your use case.";
            FieldTypeInstructions = new Dictionary<FieldType, string> {
                [FieldType.General] = "General information about the credential.",
                [FieldType.Link] = "Control what links, images, and videos are added to the credential page.",
                [FieldType.Overview] = "This information will be displayed on the credential page.",
                [FieldType.Admissions] = "This information will be displayed as admission information on the credential page.",
                [FieldType.QuoteOption] = "Controls what quote will be added to the credential page.",
                [FieldType.Filters] = "Be able to create filters for the credential to help search and are also rolled up to the program associated with the credential if you are using programs.",
                [FieldType.RelatedLinks] = "Generate a list of links associated with a credential. This is intended to be displayed on the credential page.",
                [FieldType.NotesList] = "Generate an FAQ-like area or individual notes for a credential. This is intended to be displayed on the credential page.",
                [FieldType.CourseList] = "This information will be displayed with the typical plan of study.",
                [FieldType.Transcriptable] = "This information shows transcriptable information (like majors, minors, etc.).",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };
            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.Credential, FieldType = FieldType.General, IsRequired = true, InitialDescription = "Name your credential. This will appear when users search programs and credentials." },
                new() { Title = "Summary Title", CategoryType = CategoryType.Credential, FieldType = FieldType.General, InitialDescription = "Change the title of the summary if needed. This will appear when users search credentials; they will read a brief summary of the credential before clicking into the credential page." },
                new() { Title = "Summary Text", CategoryType = CategoryType.Credential, FieldType = FieldType.General, InitialDescription = "Briefly summarize the credential. This will appear when users search programs and credentials; they may read a brief summary of the credential before clicking into the credential page." },
                new() { Title = "Contact", CategoryType = CategoryType.Credential, FieldType = FieldType.General, InitialDescription = "What is the contact name? This can be a faculty member or a department name." },
                new() { Title = "Contact Email", CategoryType = CategoryType.Credential, FieldType = FieldType.General, InitialDescription = "What is the contact email? Make sure this is the full email address." },
                new() { Title = "Credit Hours", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Cost", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Online Cost", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Credential Length", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Online Credential Length", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Suggested Enrollment Date", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Online Suggested Enrollment Date", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Online Suggested Enrollment Date Fall", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Online Suggested Enrollment Date Spring", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Online Suggested Enrollment Date Summer", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Credential Type", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Course Format", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "Note that the URL fragment is used to make searching for this item easier and to meet SEO standards. This needs to be unique and consist of lower-case letters, numbers, dashes, and the '/' character. Do not use this if you cannot meet these requirements and rely on the ID to be a unique identifier." },
                new() { Title = "Summary Link (URL)", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "This link is where your credential page information will live, it is the live page link. You will need to copy this from your CMS (such as Sitefinity)." },
                new() { Title = "Apply Now / Get More Information Link (URL)", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "Link to campus credential details or to apply for credential." },
                new() { Title = "Credential Image", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the program finder. You must add alternative text when linking an image." },
                new() { Title = "Credential Image Alt Text", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "Alternative text for the image." },
                new() { Title = "Detail Credential Image", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the program page. You must add alternative text when uploading an image." },
                new() { Title = "Detail Credential Image Alternative Text", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "Alternative text for the detail image." },
                new() { Title = "Video URL", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "This should link to a video that you would like featured on the program page. Please be sure the video has captions and/or transcripts." },
                new() { Title = "Description", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This text should describe the credential. It will be on the credential page." },
                new() { Title = "Program Notes Title", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This is a way to overwrite the program notes title, if you need to use a more descriptive term than 'Notes'." },
                new() { Title = "Program Notes", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This text has generic notes about the program, listed at the end of the page. Use this for disclaimers or additional information." },
                new() { Title = "Who Should Apply", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This text should detail who is eligible and may be interested in the program. It will be on the program page." },
                new() { Title = "Job Type Information", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This text should describe what job titles could this credential prepare students for." },
                new() { Title = "Professional Information", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This text should detail what job titles could this credential prepare students for." },
                new() { Title = "Skills", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This text should detail what skills this credential prepare students for. This is distinct from the filters skills section." },
                new() { Title = "Admission Requirements", CategoryType = CategoryType.Credential, FieldType = FieldType.Admissions, InitialDescription = "This text should detail what you need to do to apply, or what requirements there are for this." },
                new() { Title = "Transfer Student Requirements", CategoryType = CategoryType.Credential, FieldType = FieldType.Admissions, InitialDescription = "This text should detail what you need to do to apply if you are a transfer student, or what requirements there are. Be sure to distinguish between transfers from another college at Illinois and transferring from a community college or another four-year college." },
                new() { Title = "Program Tracks", CategoryType = CategoryType.Credential, FieldType = FieldType.Admissions, InitialDescription = "This text should detail program tracks." },
                new() { Title = "Quote Title", CategoryType = CategoryType.Credential, FieldType = FieldType.QuoteOption, InitialDescription = "This text should give the quote title, usually the person's name." },
                new() { Title = "Quote Subtitle", CategoryType = CategoryType.Credential, FieldType = FieldType.QuoteOption, InitialDescription = "This text should give the quote sub-title, usually the person's group (alumni, new student, etc.)." },
                new() { Title = "Quote", CategoryType = CategoryType.Credential, FieldType = FieldType.QuoteOption, InitialDescription = "This text should give the quote. Do not put the quote in quotes." },
                new() { Title = "Quote Image", CategoryType = CategoryType.Credential, FieldType = FieldType.QuoteOption, InitialDescription = "This should link to an image that you would like featured on the quote. You must add alternative text when uploading an image." },
                new() { Title = "Quote Image Alternative Text", CategoryType = CategoryType.Credential, FieldType = FieldType.QuoteOption, InitialDescription = "Alternative text for the quote image." },
                new() { Title = "Filters", CategoryType = CategoryType.Credential, InitialDescription = "", FieldType = FieldType.Filters },
                new() { Title = "Related Links", CategoryType = CategoryType.Credential, InitialDescription = "", FieldType = FieldType.RelatedLinks },
                new() { Title = "Note List", CategoryType = CategoryType.Credential, InitialDescription = "", FieldType = FieldType.NotesList },
                new() { Title = "Plan of Study Introduction", CategoryType = CategoryType.Credential, FieldType = FieldType.CourseList, InitialDescription = "This text will be above the plan of study and contains important notes about the plan of study." },
                new() { Title = "Is This Credential Transcriptable", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Transcriptable Name", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable, InitialDescription = "The name the credential will appear as on transcripts." },
                new() { Title = "Major Title", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Minor Title", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Disclaimer Text", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable, InitialDescription = "Detail any disclaimers that are necessary to add" },
                new() { Title = "Display Order", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "Enter a number to indicate where this credential should be listed in the program finder." },
                new() { Title = "Internal Search Text", CategoryType = CategoryType.Credential, InitialDescription = "Internal text used to manage search fields. Not displayed to the end user but used in search.", FieldType = FieldType.Technical },
                new() { Title = "ID", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage." },
                new() { Title = "Edit Link", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly." }
            ];
        }
    }
}
