using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.FieldList {

    public class RequirementSetGroup : BaseGroup {

        public RequirementSetGroup() {
            CategoryType = CategoryType.RequirementSet;
            Instructions = "Customize the fields used for requirement sets. Note that you need to have credentials enabled to use requirement sets effectively. You can add custom instructions for each field based on your use case.";
            FieldTypeInstructions = new Dictionary<FieldType, string> {
                [FieldType.General] = "General information about the requirement set like credit hours.",
                [FieldType.Technical] = "Technical details used for internal purposes."
            };

            FieldItems = [
                new() { Title = "Title", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.General, IsRequired = true },
                new() { Title = "Internal Title", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.General },
                new() { Title = "Description", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.General },
                new() { Title = "Minimum Credit Hours", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.General },
                new() { Title = "Maximum Credit Hours", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.General },
                new() { Title = "Is This Shared?", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.Technical },
                new() { Title = "Id", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.Technical, InitialDescription = "The ID of the item, which may be used in a CMS to pull the item and display it on a webpage" },
                new() { Title = "Edit Link", CategoryType = CategoryType.RequirementSet, FieldType = FieldType.Technical, InitialDescription = "This is a quick link to edit this item directly" }
            ];
        }
    }
}