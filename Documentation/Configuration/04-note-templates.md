# Manage Note Templates

[Back to Configuration Knowledge Base](README.md)

**Where:** Configuration > **Manage Note Templates** (`/configuration/notetemplates`)

"Notes" are FAQ-like entries with a heading or question, a text area, and a link to extra information. You can write notes on an individual program, credential, or course, or create **note templates** here that apply to many at once.

> Templates apply only within this source.

## Choose a type
Select **Programs**, **Credentials**, or **Courses** to see that type's templates.

## Template list
Each template has buttons to move up or down (change order), **Edit**, or **Remove**.

## Add or edit a template
| Field | Meaning |
|-------|---------|
| **Criteria: Show Only For** | Limit the template to a Department, Degree, Format, Skill, or Tag. Leave blank to apply to all. |
| **Criteria Value** | The specific value for the criteria. For Credential Degree, choose `Base_Undergraduate`, `Base_Graduate`, or `Base_Certificate` to apply to all undergraduate, graduate, or certificate credentials. |
| **Title** | The note heading. |
| **Description** | The note text. Accepts Markdown links `[name](url)`, bullet lists `*`, and converts links automatically. Use `[title]` to insert the program / credential / course title. |
| **Link Name** | Text of the link. |
| **Link URL** | Web address of the link. |

Select **Add** (or **Edit**), then **Save** at the bottom of the page.

## Overriding a template on one item
Give a note on the specific program, credential, or course the **same title** as the template. It replaces the template for that item.

Notes on an item display at the end of the list, unless they override a template. To control position, create a **placeholder** with a blank description, link text, and link URL. Notes with all three blank are never displayed.

