using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class ProgramGroup : BaseGroup {

        public ProgramGroup() {
            CategoryType = CategoryType.Program;
            Instructions = "Customize the fields used for programs. You can edit custom instructions for each field based on your use case.";
            FieldTypeInstructions = new Dictionary<FieldType, string> {
                [FieldType.General] = "General information that will describe the program for users searching the site.",
                [FieldType.Link] = "Control what links, images, and videos are added to the program page.",
                [FieldType.Overview] = "Describe the program and who should apply. This will be displayed on the program page.",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };
            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.Program, FieldType = FieldType.General, IsRequired = true, InitialDescription = "Name your program. This will appear when users search for programs." },
                new() { Title = "Summary Text", CategoryType = CategoryType.Program, FieldType = FieldType.General, InitialDescription = "Briefly summarize the program. This will appear when users search programs; they will read a brief summary of the program before clicking into the program page." },
                new() { Title = "Link URL", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription= "This link is where your program page information will live, it is the live page link. You will need to copy this from your CMS (such as Sitefinity)." },
                new() { Title = "Alternate Link URL", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription = "This should link to an alternative page by another college involved in running/hosting the program. You must also add the text the link will be applied to." },
                new() { Title = "Alternate Link Text", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription = "This should be the text you will be applying the link to. It should describe the link destination." },
                new() { Title = "Program Image", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the program finder. You must add alternative text when uploading an image." },
                new() { Title = "Program Image Alternative Text", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription = "Alternative text for the program image." },
                new() { Title = "Detail Program Image", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription = "This should link to an image that you would like featured on the program page. You must add alternative text when uploading an image." },
                new() { Title = "Detail Program Image Alternative Text", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription = "Alternative text for the detail image." },
                new() { Title = "Video URL", CategoryType = CategoryType.Program, FieldType = FieldType.Link, InitialDescription = "This should link to a video that you would like featured on the program page. Please be sure the video has captions and/or transcripts." },
                new() { Title = "Description", CategoryType = CategoryType.Program, FieldType = FieldType.Overview, InitialDescription = "This text should describe the program. It will be on the program page." },
                new() { Title = "Who Should Apply", CategoryType = CategoryType.Program,FieldType = FieldType.Overview, InitialDescription = "This text should detail who is eligible and may be interested in the program. It will be on the program page." },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Program, InitialDescription = "Note that the URL fragment is used to make searching for this item easier and to meet SEO standards. This needs to be unique and consist of lower-case letters, numbers, dashes, and the '/' character. Do not use this if you cannot meet these requirements and rely on the ID to be a unique identifier.", FieldType = FieldType.Technical },
                new() { Title = "Id", CategoryType = CategoryType.Program, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage." },
                new() { Title = "Edit Link", CategoryType = CategoryType.Program, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly." }
            ];
        }
    }
}
