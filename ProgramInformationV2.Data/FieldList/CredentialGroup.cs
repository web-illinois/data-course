using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class CredentialGroup : BaseGroup {

        public CredentialGroup() {
            CategoryType = CategoryType.Credential;
            Instructions = "Customize the fields used for credentials. You can add custom instructions for each field based on your use case.";
            FieldTypeInstructions = new Dictionary<FieldType, string> {
                [FieldType.General] = "General information about the credential.",
                [FieldType.Link] = "Control what links, images, and videos are added to the credential page.",
                [FieldType.Overview] = "This information will be displayed on the credential page.",
                [FieldType.Transcriptable] = "This information shows transcriptable information (like majors, minors, etc.).",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };
            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.Credential, FieldType = FieldType.General, IsRequired = true },
                new() { Title = "Summary Text", CategoryType = CategoryType.Credential, FieldType = FieldType.General, InitialDescription = "Text that is displayed in the program finder" },
                new() { Title = "Credit Hours", CategoryType = CategoryType.Credential, FieldType = FieldType.General, InitialDescription = "" },
                new() { Title = "Cost", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Credential Length", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Suggested Enrollment Date", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Credential Type", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Course Format", CategoryType = CategoryType.Credential, FieldType = FieldType.General },
                new() { Title = "Summary Link (URL)", CategoryType = CategoryType.Credential, FieldType = FieldType.Link },
                new() { Title = "Apply Now / Get More Information Link (URL)", CategoryType = CategoryType.Credential, FieldType = FieldType.Link },
                new() { Title = "Image URL", CategoryType = CategoryType.Credential, FieldType = FieldType.Link },
                new() { Title = "Image Alt Text", CategoryType = CategoryType.Credential, FieldType = FieldType.Link },
                new() { Title = "Description", CategoryType = CategoryType.Credential, FieldType = FieldType.Overview },
                new() { Title = "Is This Credential Transcriptable", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Transcriptable Name", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Major Title", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Minor Title", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Disclaimer Text", CategoryType = CategoryType.Credential, FieldType = FieldType.Transcriptable },
                new() { Title = "Display Order", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical },
                new() { Title = "Id", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage" },
                new() { Title = "Edit Link", CategoryType = CategoryType.Credential, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly" }
            ];
        }
    }
}