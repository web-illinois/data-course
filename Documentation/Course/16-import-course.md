# Import a Course

[Back to Courses Knowledge Base](README.md)

**Where:** **Courses > Import Campus Courses** (`/courses/import`)

Runs or schedules a course import from the Electronic Data Warehouse. Requires that your area uses both courses and sections; otherwise the page links to Configuration to enable them.

## Options
1. **By Rubric**: loads all courses of a rubric. **This always overwrites existing course information.** It runs asynchronously and may take a while; view progress in the course import log (`/audit/courseimportlog`). Enter several rubrics as a comma-delimited list.
2. **By Rubric and Course Number**: loads one course immediately. Two switches appear:
   - **Include Sections?**
   - **Overwrite existing data?**: off means only new sections are loaded.

The import looks back five years and forward one year from the current date.

## Steps
1. Enter **Rubric** (and optionally **Course Number**).
2. Set the switches if shown.
3. Select **Import Course**.

