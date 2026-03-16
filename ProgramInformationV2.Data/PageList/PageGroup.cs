namespace ProgramInformationV2.Data.PageList {

    public static class PageGroup {

        private static readonly Dictionary<SidebarEnum, List<PageLink>> _breadcrumbs = new() {
            { SidebarEnum.Configuration, [
                    new("Home", "/"),
                    new("Configuration", "/configuration/sources")
                ]
            },
            { SidebarEnum.ConfigurationNoSource, [
                    new("Home", "/"),
                    new("Configuration", "/configuration/sources")
                ]
            },
            { SidebarEnum.FieldsUsed, [
                    new("Home", "/"),
                    new("Configuration", "/configuration/sources"),
                    new("Fields Used", "/configuration/fieldsused/programs")
                ]
            },
            { SidebarEnum.ProgramCredential, [
                    new("Home", "/"),
                    new("Programs and Credentials", "/programcred")
                ]
            },
            { SidebarEnum.Program, [
                    new("Home", "/"),
                    new("Programs and Credentials", "/programcred"),
                    new("Programs", "/programs")
                ]
            },
            { SidebarEnum.ProgramWithCredential, [
                    new("Home", "/"),
                    new("Programs and Credentials", "/programcred"),
                    new("Programs", "/programs")
                ]
            },
            { SidebarEnum.Credential, [
                    new("Home", "/"),
                    new("Programs and Credentials", "/programcred"),
                    new("Credentials", "/credentials")
                ]
            },
            { SidebarEnum.Courses, [
                    new("Home", "/"),
                    new("Courses", "/courses")
                ]
            },
            { SidebarEnum.Course, [
                    new("Home", "/"),
                    new("Courses", "/courses")
                ]
            },
            { SidebarEnum.CourseWithSection, [
                    new("Home", "/"),
                    new("Courses", "/courses")
                ]
            },
            { SidebarEnum.RequirementSets, [
                    new("Home", "/"),
                    new("Requirement Sets", "/requirementsets")
                ]
            },
            { SidebarEnum.RequirementSet, [
                    new("Home", "/"),
                    new("Requirement Sets", "/requirementsets")
                ]
            },
            { SidebarEnum.Section, [
                    new("Home", "/"),
                    new("Courses", "/courses"),
                    new("Course", "/course/general")
                ]
            }
        };

        private static readonly Dictionary<SidebarEnum, List<PageLink>> _sidebars = new() {
            { SidebarEnum.None, [] },
            { SidebarEnum.EditInformation, [
                    new("Programs & Credentials", "/programcred"),
                    new("Courses & Sections", "/coursesection"),
                    new("Requirement Sets", "/requirementset")
                ]
            },
            { SidebarEnum.ProgramCredential, [
                    new("Programs And Credentials", "/programcred"),
                    new("Programs", "/programs"),
                    new("Credentials", "/credentials")
                ]
            },
            { SidebarEnum.Program, [
                    new("General Information", "/program/general"),
                    new("Link and Image", "/program/link"),
                    new("Overview", "/program/overview"),
                    new("Filters", "/program/filters"),
                    new("Related Links", "/program/relatedlinks"),
                    new("List of Notes", "/program/notelist"),
                    new("Technical Information", "/program/technical"),
                    new("Security", "/program/security"),
                    new("Audits and Changes", "/program/audit")
                ]
            },
            { SidebarEnum.ProgramWithCredential, [
                    new("General Information", "/program/general"),
                    new("Link and Image", "/program/link"),
                    new("Overview", "/program/overview"),
                    new("Filter List", "/program/filtersread"),
                    new("Credential List", "/program/credentiallist"),
                    new("Related Links", "/program/relatedlinks"),
                    new("List of Notes", "/program/notelist"),
                    new("Technical Information", "/program/technical"),
                    new("Security", "/program/security"),
                    new("Audits and Changes", "/program/audit")
                ]
            },
            { SidebarEnum.Credential, [
                    new("General Information", "/credential/general"),
                    new("Link and Image", "/credential/link"),
                    new("Overview", "/credential/overview"),
                    new("Filters", "/credential/filters"),
                    new("Related Links", "/credential/relatedlinks"),
                    new("List of Notes", "/credential/notelist"),
                    new("Course List", "/credential/courselist"),
                    new("Transcript Information", "/credential/transcriptable"),
                    new("Technical Information", "/credential/technical"),
                    new("Security", "/credential/security"),
                    new("Audits and Changes", "/credential/audit")
                ]
            },
            { SidebarEnum.Courses, [
                    new("Courses", "/courses"),
                    new("Import Courses", "/courses/import")
                ]
            },
            { SidebarEnum.Course, [
                    new("General Information", "/course/general"),
                    new("Link and Image", "/course/link"),
                    new("Overview", "/course/overview"),
                    new("Filters", "/course/filters"),
                    new("Related Links", "/course/relatedlinks"),
                    new("List of Notes", "/course/notelist"),
                    new("Location & Time", "/course/locationtime"),
                    new("Faculty", "/course/faculty"),
                    new("Technical Information", "/course/technical"),
                    new("Security", "/course/security"),
                    new("Audits and Changes", "/course/audit")
                ]
            },
            { SidebarEnum.CourseWithSection, [
                    new("General Information", "/course/general"),
                    new("Link and Image", "/course/link"),
                    new("Overview", "/course/overview"),
                    new("Filters", "/course/filters"),
                    new("Related Links", "/course/relatedlinks"),
                    new("List of Notes", "/course/notelist"),
                    new("Section List", "/course/sectionlist"),
                    new("Technical Information", "/course/technical"),
                    new("Security", "/course/security"),
                    new("Audits and Changes", "/course/audit")
                ]
            },
            { SidebarEnum.Section, [
                    new("General Information", "/section/general"),
                    new("Location & Time", "/section/locationtime"),
                    new("Faculty", "/section/faculty"),
                    new("Technical Information", "/section/technical")
                ]
            },
            { SidebarEnum.RequirementSets, [
                new("Requirement Sets", "/requirementsets")]
            },
            { SidebarEnum.RequirementSet, [
                new("General Information", "/requirementset/general"),
                new("Courses", "/requirementset/courses"),
                new("Technical Information", "/requirementset/technical") ]
            },
            { SidebarEnum.Configuration, [
                    new("Sources", "/configuration/sources"),
                    new("Fields Used", "/configuration/fieldsused/programs"),
                    new("Manage Filters", "/configuration/filters"),
                    new("Manage Note Templates", "/configuration/notetemplates"),
                    new("Security", "/configuration/security"),
                    new("Delete Source", "/configuration/deletion"),
                    new("Testing Access", "/configuration/testing")
                ]
            },
            { SidebarEnum.TransferInformation, [
                    new("Save Information to JSON", "/transfer/savejson"),
                    new("Load JSON Information to the Server", "/transfer/loadjson")
                ]
            },
            { SidebarEnum.ConfigurationNoSource, [
                    new("Sources", "/configuration/sources"),
                    new("Testing Access", "/configuration/testing")
                ]
            },
            { SidebarEnum.FieldsUsed, [
                    new("Programs", "/configuration/fieldsused/programs"),
                    new("Credentials", "/configuration/fieldsused/credentials"),
                    new("Courses", "/configuration/fieldsused/courses"),
                    new("Sections", "/configuration/fieldsused/sections"),
                    new("Requirement Sets", "/configuration/fieldsused/requirementsets")
                ]
            },
            { SidebarEnum.Audit, [
                    new("Access Logs", "/audit/changed"),
                    new("Unused Requirement Sets", "/audit/unusedrequirementsets"),
                    new("Public Requirement Sets", "/audit/publicrequirementsets"),
                    new("List of Requirement Sets", "/audit/listrequirementsets"),
                    new("Courses By Requirement Sets", "/audit/coursebyrequirementset"),
                    new("Requirement Sets with Invalid Courses", "/audit/requirementsetswithinvalidcourses"),
                    new("All Programs and Credentials", "/audit/allprograms"),
                    new("Course Import Log", "/audit/courseimportlog")
                ]
            }
        };

        public static List<PageLink>? GetBreadcrumbs(SidebarEnum s) => _breadcrumbs.TryGetValue(s, out var value) ? value : null;

        public static List<PageLink>? GetSidebar(SidebarEnum s) => _sidebars.TryGetValue(s, out var value) ? [.. value] : null;
    }
}