# Configuration Knowledge Base

[Back to Documentation Home](../README.md)

End-user documentation for the **Configuration** section of the Program Information application (pages under `/configuration`).

## What is a source?
A **source** is a separate, self-contained workspace for one college, department, or unit. Each source has its own:

- Programs, credentials, courses, sections, and requirement sets
- Filters (departments, skills, lengths, tags)
- Note templates
- Fields Used choices and instructions
- Security (the list of users and their access)
- API key
- Settings and logs

Each source has a **Source Name** and a short **Source Code**. The code is the prefix used on the IDs of everything in the source.

### Updating one source does not affect other sources
> **Every change you make applies only to the source you are currently working in.** Updating, adding, or deleting data or settings in one source does **not** change any other source. The same is true for filters, note templates, fields used, security, settings, and deletions.

Because of this:
- Always confirm which source is selected before you make changes, especially before deleting.
- Choose the source you want to edit on the **home page**. Creating a new source switches your selection to that new source.
- Copying data to another source requires the Transfer pages (see the Transfer documentation).
- A test source (source code ending in `!`) is separate from its production source. Data only moves between them through the Technical Information copy and transfer actions.

| # | Article | What it covers |
|---|---------|----------------|
| 1 | [Sources](01-sources.md) | Create a source and find source owners |
| 2 | [Fields Used](../FieldsUsed/README.md) | Turn features on or off, hide unused fields, and add instructions for each field (Programs, Credentials, Courses, Sections, Requirement Sets) |
| 3 | [Manage Filters](03-manage-filters.md) | Departments, skills, lengths, tags |
| 4 | [Manage Note Templates](04-note-templates.md) | Shared FAQ-style notes |
| 5 | [Security](05-security.md) | Users, restricted access, API key |
| 6 | [Settings](06-settings.md) | Search, AI, seminar splitting, base URL |
| 7 | [Delete Source](07-delete-source.md) | Delete source data or the whole source |
| 8 | [Testing Access](08-testing-access.md) | Access to the testing area |

## Sidebar
**Sources**, **Fields Used**, **Manage Filters**, **Manage Note Templates**, **Security**, **Settings**, **Delete Source**, **Testing Access**.

Before a source is selected, only **Sources** and **Testing Access** appear.

