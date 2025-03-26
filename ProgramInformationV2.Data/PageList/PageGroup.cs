namespace ProgramInformationV2.Data.PageList {

    public static class PageGroup {

        private static readonly Dictionary<SidebarEnum, List<PageLink>> _breadcrumbs = new() {
            { SidebarEnum.Configuration, new() { new ("Home", "/"),
                                 new ("Configuration", "/configuration/sources") } },
            { SidebarEnum.FieldsUsed, new() { new ("Home", "/"),
                                 new ("Configuration", "/configuration/sources"),
                                 new ("Fields Used", "/configuration/fieldsused/programs") } },
            { SidebarEnum.ProgramCredential, new() { new ("Home", "/"),
                                 new ("Programs and Credentials", "/programcred") } },
            { SidebarEnum.Program, new() { new ("Home", "/"),
                                 new ("Programs and Credentials", "/programcred"),
                                 new ("Programs", "/programs") } },
            { SidebarEnum.Credential, new() { new ("Home", "/"),
                        new ("Programs and Credentials", "/programcred"),
                        new ("Credentials", "/credentials") } },
            { SidebarEnum.Courses, new() { new ("Home", "/"),
                        new ("Courses", "/courses") } },
            { SidebarEnum.Course, new() { new ("Home", "/"),
                        new ("Courses", "/courses") } },
            { SidebarEnum.RequirementSets, new() { new ("Home", "/"),
                        new ("Requirement Set", "/requirementset") } },
            { SidebarEnum.RequirementSet, new() { new ("Home", "/"),
                        new ("Requirement Set", "/requirementset") } },
            { SidebarEnum.Section, new() { new ("Home", "/"),
                        new ("Courses", "/courses"),
                        new ("Section", "/section") } }
        };

        private static readonly Dictionary<SidebarEnum, List<PageLink>> _sidebars = new() {
            { SidebarEnum.EditInformation, new() { new ("Programs & Credentials", "/programcred"),
                                     new ("Courses & Sections", "/coursesection"),
                                     new ("Requirement Sets", "/requirementset") } },
            { SidebarEnum.ProgramCredential, new() { new ("Programs And Credentials", "/programcred"),
                                     new ("Programs", "/programs"),
                                     new ("Credentials", "/credentials") } },
            { SidebarEnum.Program, new() { new ("General Information", "/program/general"),
                                 new ("Link Information", "/program/link"),
                                 new ("Overview", "/program/overview"),
                                 new ("Filters", "/program/filters"),
                                 new ("Credential List", "/program/credentiallist"),
                                 new ("Technical Details", "/program/technical") } },
            { SidebarEnum.Credential, new() { new ("General Information", "/credential/general"),
                                 new ("Link Information", "/credential/link"),
                                 new ("Overview", "/credential/overview"),
                                 new ("Filters", "/credential/filters"),
                                 new ("Course List", "/credential/courselist"),
                                 new ("Transcript Information", "/credential/transcriptable"),
                                 new ("Technical Details", "/credential/technical") } },
            { SidebarEnum.Courses, new() { new ("Courses", "/courses"),
                                     new ("Import Courses", "/courses/import") } },
            { SidebarEnum.Course, new() { new ("General Information", "/course/general"),
                                 new ("Link Information", "/course/link"),
                                 new ("Overview", "/course/overview"),
                                 new ("Filters", "/course/filters"),
                                 new ("Associated Courses", "/course/associated"),
                                 new ("Section List", "/course/sectionlist"),
                                 new ("Technical Details", "/course/technical") } },
            { SidebarEnum.Section, new() { new ("General Information", "/section/general"),
                                 new ("Location & Time", "/section/locationtime"),
                                 new ("Faculty", "/section/faculty"),
                                 new ("Technical Details", "/section/technical") } },
            { SidebarEnum.RequirementSets, new() { new ("Requirement Sets", "/requirementsets") } },
            { SidebarEnum.RequirementSet, new() { new ("Details", "/requirementset/details"),
                                 new ("Requirements", "/requirementset/requirements"),
                                 new ("Courses", "/requirementset/courses") } },
            { SidebarEnum.Configuration, new() { new ("Sources", "/configuration/sources"),
                                 new ("Fields Used", "/configuration/fieldsused/programs"),
                                 new ("Manage Filters", "/configuration/filters"),
                                 new ("Security", "/configuration/security"),
                                 new ("Request Deletion", "/configuration/requestdeletion"),
                                 new ("Testing Access", "/configuration/testing"),
                                 new ("Save Information to JSON", "/configuration/savejson"),
                                 new ("Load JSON Information to the Server", "/configuration/loadjson") } },
            { SidebarEnum.FieldsUsed, new() { new ("Programs", "/configuration/fieldsused/programs"),
                                 new ("Credentials", "/configuration/fieldsused/credentials"),
                                 new ("Courses", "/configuration/fieldsused/courses"),
                                 new ("Sections", "/configuration/fieldsused/sections") } }
        };

        public static List<PageLink>? GetBreadcrumbs(SidebarEnum s) => _breadcrumbs.ContainsKey(s) ? _breadcrumbs[s] : null;

        public static List<PageLink>? GetSidebar(SidebarEnum s) => _sidebars.ContainsKey(s) ? _sidebars[s].ToList() : null;
    }
}