# Requirement Sets Knowledge Base

[Back to Documentation Home](../README.md)

End-user documentation for the **Requirement Sets** section of the Program Information application (pages under `/requirementset` and `/requirementsets`).

## What is a Requirement Set?

A Requirement Set is a group of courses that a student needs to take in order to earn a credential. The requirement can be "take all of these courses" or "take 12 hours of some of these courses." This text is added manually in the **Description** field.

## How Credentials, Requirement Sets, and Courses connect

```
Program
  └── Credential            (a degree, certificate, or endorsement)
		└── Requirement Set (0 to many per credential)
			  └── Course    (0 to many per requirement set)
					└── Section
```

- A **Credential** can contain **0 to many Requirement Sets**.
- A **Requirement Set** can contain **0 to many Courses**.
- A Requirement Set describes a rule a student must satisfy, for example:
  - **Must take all of these courses**
  - **Must take one of these courses**
  - **Must take 9 credit hours from the following courses**
- Together, a credential's requirement sets form its **plan of study** (see the Credential **Course List** page).
- A requirement set is normally **private** to one credential. It can be marked **shared** so other credentials can reuse it.

| # | Article | What it covers |
|---|---------|----------------|
| 1 | [Finding and Creating Requirement Sets](01-finding-and-creating.md) | The Requirement Sets list page |
| 2 | [General Information](02-general-information.md) | Titles, description, credit hours, and how rules are expressed |
| 3 | [Courses](03-courses.md) | Adding and removing courses |
| 4 | [Technical Information](04-technical-information.md) | Status, shared/private, credentials in use, delete |
| 5 | [How Credentials, Requirement Sets, and Courses Work Together](05-connecting-credentials-and-courses.md) | Step-by-step examples |

## Basics
- Each page has an orange **Save** button. Changes are not stored until you select it.
- A **Back** button appears when you arrived from another page, such as a credential's Course List.
- Requirement sets must be enabled in **Configuration > Fields Used > Requirement Sets**. Connecting courses also requires courses to be enabled, and using requirement sets requires credentials to be enabled.

