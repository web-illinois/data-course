# How Credentials, Requirement Sets, and Courses Work Together

[Back to Requirement Sets Knowledge Base](README.md)

## The relationships
- A **Credential** has **0 to many Requirement Sets**.
- A **Requirement Set** has **0 to many Courses**.
- A **Course** can appear in many requirement sets.
- Courses have **Sections** (term-based classes), but requirement sets point to courses, not sections.

## Types of rules
| Rule | Example |
|------|---------|
| Must take **all** of these courses | Core: MATH 101, MATH 102, MATH 201 |
| Must take **one** of these courses | Choose one: ART 200 or ART 210 |
| Must take **9 credit hours** from the following courses | Electives: 9 hours from a list of 12 courses |

A credential with all three rules would have three requirement sets, each with its own courses.

## Building a plan of study
1. Open the credential and go to **Course List**.
2. Enter the **Plan of Study Introduction**.
3. Under **Add Requirement Sets**, either:
   - search and select **Add to Credential** to reuse an existing set, or
   - select **Create New Requirement Set** to create a private one.
4. Select a set's title under **Chosen Requirement Sets** to edit it.
5. On the requirement set, fill in [General Information](02-general-information.md).
6. On [Courses](03-courses.md), add the courses and optional public notes, then **Save**.
7. Repeat for each rule in the credential.

## Private vs. shared sets
- A new set created from a credential is **private** to that credential.
- On [Technical Information](04-technical-information.md), turn on **Is This Shared?** to let other credentials reuse it. Changes to a shared set affect every credential using it.
- To make a shared set private again, only one credential may use it.

## Things to watch
- A set used by a credential cannot be deleted.
- Removing a course from a set does not delete the course.
- Deleting a credential or course elsewhere may affect sets that reference it.
- In the test environment, copying or transferring a program overwrites its credentials. Make sure matching requirement sets exist in both environments.

