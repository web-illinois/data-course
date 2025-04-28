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
                [FieldType.Transcriptable] = "This information shows transcriptable information (like majors, minors, etc.).",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };
            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.Credential, FieldType = FieldType.General, IsRequired = true, InitialDescription = "Name your credential. This will appear when users search programs and credentials." },
                new() { Title = "Summary Text", CategoryType = CategoryType.Credential, FieldType = FieldType.General, InitialDescription = "Briefly summarize the credential. This will appear when users search programs and credentials; they may read a brief summary of the credential before clicking into the credential page." },
                new() { Title = "Credit Hours", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Cost", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Credential Length", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Suggested Enrollment Date", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Credential Type", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Course Format", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Link URL", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "This link is where your credential page information will live, it is the live page link. You will need to copy this from your CMS (such as Sitefinity)." },
                new() { Title = "Apply Now / Get More Information Link URL", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "Link to campus credential details or to apply for credential." },
                new() { Title = "Credential Image", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the program finder. You must add alternative text when linking an image." },
                new() { Title = "Credential Image Alt Text", CategoryType = CategoryType.Credential, FieldType = FieldType.Link, InitialDescription = "Alternative text for the image." },
                new() { Title = "Description", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview, InitialDescription = "This text should describe the credential. It will be on the credential page." },
                new() { Title = "Is This Credential Transcriptable", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Transcriptable Name", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable, InitialDescription = "The name the credential will appear as on transcripts." },
                new() { Title = "Major Title", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable, },
                new() { Title = "Minor Title", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable, },
                new() { Title = "Disclaimer Text", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable, InitialDescription = "Detail any disclaimers that are necessary to add" },
                new() { Title = "Display Order", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "Enter a number to indicate where this credential should be listed in the program finder." },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "Note that the URL fragment is used to make searching for this item easier and to meet SEO standards. This needs to be unique and consist of lower-case letters, numbers, dashes, and the '/' character. Do not use this if you cannot meet these requirements and rely on the ID to be a unique identifier." },
                new() { Title = "ID", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage." },
                new() { Title = "Edit Link", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly." }
            ];
        }
    }
}
