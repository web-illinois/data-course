# Audit Reports Knowledge Base

[Back to Documentation Home](../README.md)

End-user documentation for the **Audit** section of the Program Information application (pages under `/audit`).

The Audit reports help you review changes, check data quality, and find clean-up work. They are read only: viewing a report does not change any data, except **Remove Logs** on the Course Import Log.

> Reports show information for the **source** you are working in. A source is a separate workspace for a college, department, or unit. The one exception is a pending-import count on the Course Import Log, which is a total across all sources.

## Reports at a glance
| Sidebar item | Route | What it shows |
|--------------|-------|---------------|
| [Access Logs](01-recently-changed-items.md) | `/audit/changed` | The most recent manual changes to items in your source. |
| [Unused Requirement Sets](02-unused-requirement-sets.md) | `/audit/unusedrequirementsets` | Requirement sets that no credential uses. |
| [Public Requirement Sets](03-public-requirement-sets.md) | `/audit/publicrequirementsets` | Shared requirement sets that only one credential uses. |
| [List of Requirement Sets](04-list-of-requirement-sets.md) | `/audit/listrequirementsets` | Every public requirement set with the courses it contains. |
| [Courses By Requirement Sets](05-courses-by-requirement-sets.md) | `/audit/coursebyrequirementset` | Every course with the requirement sets that contain it. |
| [Requirement Sets with Invalid Courses](06-requirement-sets-with-invalid-courses.md) | `/audit/requirementsetswithinvalidcourses` | Requirement sets that contain courses flagged as invalid. |
| [All Programs and Credentials](07-all-programs-and-credentials.md) | `/audit/allprograms` | A downloadable list of every program and credential. |
| [Course Import Log](08-course-import-log.md) | `/audit/courseimportlog` | The status of course imports. |

> **Not every report applies to every source.** Some reports may not be used if your source does not use the associated item (for example, Programs, Credentials, Courses, or Requirement Sets). For instance, the requirement set reports have nothing to show if your source does not use requirement sets. Features are turned on or off in **Configuration > Fields Used**.

Report titles on the pages differ

## Item-level audit pages
Each program, credential, and course also has its own **Audits and Changes** page showing the change history for that single item. See the Program, Credential, and Course documentation.

> Audits cover manual changes only. Loading JSON files and API updates are not recorded in change logs.

## Clicking through
In reports that list requirement sets or courses, select a title to open that item for editing.

