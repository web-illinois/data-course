namespace ProgramInformationV2.Data.DataModels {

    public enum CategoryType {
        None,
        Program,
        Credential,
        Course,
        Section,
        RequirementSet
    }

    public enum FieldType {
        None,
        General,
        Link,
        Overview,
        Location_Time,
        Transcriptable,
        Faculty,
        Technical,
        Filters,
        CourseList,
        RelatedLinks,
        Security
    }

    public enum ScreenType {
        None,
        Edit,
        Programs_and_Credentials,
        Programs,
        Credentials,
        Program,
        Credential,
        Courses_and_Sections,
        Courses,
        Sections,
        Course,
        Section,
        RequirementSets,
        RequirementSet,
        Audit_Screens,
        Configuration,
        Fields_Used,
        Contact_Us,
        Search_Information
    }

    public enum TagType {
        None,
        Tag,
        Skill,
        Department
    }
}