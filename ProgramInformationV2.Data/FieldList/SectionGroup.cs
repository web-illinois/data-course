using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class SectionGroup : BaseGroup {

        public SectionGroup() {
            CategoryType = CategoryType.Section;
            Instructions = "Customize the fields used for sections. Note that you need to have courses enabled to use sections appropriately, and sections are usually used in conjunction with Electronic Data Warehouse banner data. You can add custom instructions for each field based on your use case.";
            FieldTypeInstructions = new Dictionary<FieldType, string> {
                [FieldType.General] = "General information about the source.",
                [FieldType.Location_Time] = "This information is date and room information",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };
            FieldItems = [
                new() { Title = "Section Code", CategoryType = CategoryType.Section, FieldType = FieldType.General, IsRequired = true },
                new() { Title = "Alternate Title", CategoryType = CategoryType.Section, FieldType = FieldType.General },
                new() { Title = "Alternate Description", CategoryType = CategoryType.Section, FieldType = FieldType.General },
                new() { Title = "CRN", CategoryType = CategoryType.Section, FieldType = FieldType.General },
                new() { Title = "Format Type", CategoryType = CategoryType.Section, FieldType = FieldType.General },
                new() { Title = "Credit Hours", CategoryType = CategoryType.Section, FieldType = FieldType.General },
                new() { Title = "Building", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "Room", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "Term", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "Semester Year", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "Begin Date", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "End Date", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "Days", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "Time", CategoryType = CategoryType.Section, FieldType = FieldType.Location_Time },
                new() { Title = "URL Fragment", CategoryType = CategoryType.Section, FieldType = FieldType.Technical },
                new() { Title = "Id", CategoryType = CategoryType.Section, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage" },
                new() { Title = "Edit Link", CategoryType = CategoryType.Section, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly" }
            ];
        }
    }
}