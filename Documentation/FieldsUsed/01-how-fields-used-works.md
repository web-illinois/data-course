# How Fields Used Works

[Back to Fields Used Knowledge Base](README.md)

All five Fields Used pages (Programs, Credentials, Courses, Sections, Requirement Sets) work the same way. Only the list of fields differs.

> Settings apply only to the current source and never change other sources.

## 1. Turn the feature on or off
Each page starts with a yes/no question:

| Page | Question |
|------|----------|
| Programs | Would you like to add Programs to your source? |
| Credentials | Would you like to add credentials to your source? |
| Courses | Would you like to add courses to your source? |
| Sections | Would you like to add sections to your source? |
| Requirement Sets | Would you like to add requirement sets to your source? |

- **Yes**: the feature's pages are available and the field list appears below.
- **No**: the field list is hidden. Pages for that feature show a message that your area does not use it, with a link back to the matching Fields Used page.

## 2. Hide fields that you do not use
When a feature is on, fields are listed in groups. Each group has a heading ("*Group* Fields") and a short explanation.

Each field has a **Show Item** switch:
- **On**: the field appears on the editing page.
- **Off**: the field is **hidden** from editors.

**Required fields** have no switch and always appear.

Hiding unused fields gives editors simpler pages. If a field is missing from an editing page, it was most likely hidden here.

## 3. Add instructions for each item
Next to every field is a text box. Enter **instructions** for editors, such as formatting rules, length limits, or where the information comes from.

Examples:
- Contact Email: "Use the department's shared address."
- Summary Text: "Keep under 200 characters."

Instructions can be added to every field, including required ones.

## 4. Save
Select **Save**. "Information saved" confirms the change. Unsaved changes are not applied, and the page tracks unsaved edits.

## Tips
- Turn a feature on before creating items of that type.
- Hide fields only after checking that editors do not need them. Hidden fields are not shown, but existing data stays in the item.
- Revisit Fields Used when you create a new source; each source has its own settings.
- Filters, Related Links, and Notes are controlled here as well. If an editing page says one "is not currently being used for this source", turn it on on the matching page.

## Dependencies
| Feature | Needs |
|---------|-------|
| Requirement Sets | Credentials |
| Requirement Set > Courses | Courses |
| Sections | Courses |
| Course import | Courses and Sections |

