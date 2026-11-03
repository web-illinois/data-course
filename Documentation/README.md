# Program Information Documentation

## Introduction
The Program Information application is where colleges, departments, and units maintain the information that appears on their public websites about **programs**, **credentials**, **courses**, **sections**, and **requirement sets**. This documentation is written for the people who edit that information. It explains each screen, what each field is for, and what happens when you save, transfer, or delete.

## Key concept: sources
A **source** is a separate workspace for one college, department, or unit. Each source has its own data, filters, note templates, settings, security, and API key.

> **Updating one source does not affect any other source.** Always confirm which source you are working in before making changes, especially before deleting.

## How the information fits together
```
Program
  └── Credential            (degree, certificate, or endorsement)
		└── Requirement Set (0 to many per credential)
			  └── Course    (0 to many per requirement set)
					└── Section (term-based class)
```

- A **program** groups one or more credentials.
- A **credential** can contain 0 to many requirement sets.
- A **requirement set** can contain 0 to many courses. It describes a rule such as "must take all of these courses", "must take one of these courses", or "must take 9 credit hours from the following courses".
- A **course** has one or more sections.

## Documentation summary
| Folder | What it covers |
|--------|----------------|
| [Program](Program/README.md) | Editing a program: general information, links and images, overview, admissions, quote, filters, credential list, related links, notes, technical information, security, and audit history. |
| [Credential](Credential/README.md) | Editing a credential: general information, filters, course list (requirement sets), transcript information, technical information, security, and audit history. |
| [Requirement Set](RequirementSet/README.md) | Creating requirement sets (groups of courses needed for a credential), writing the rule in the Description field, setting credit hours, adding courses, shared and private sets, and how credentials, requirement sets, and courses connect. |
| [Course](Course/README.md) | Editing a course: general information, faculty, location and time, section list, importing courses from the Electronic Data Warehouse, Canvas, and Coursera. |
| [Section](Section/README.md) | Editing a section: general information, location and time, faculty, and technical information. |
| [Transfer](Transfer/README.md) | Saving data to JSON files and loading them back, to copy data between sources, back up before large changes, or make bulk edits in a text editor. |
| [Configuration](Configuration/README.md) | Source-level setup: sources, filters, note templates, security and API keys, settings, deleting a source, and testing access. |
| [FieldsUsed](FieldsUsed/README.md) | Turning features on or off for your source, hiding fields you do not use, and adding instructions for each field. |
| [Audit](Audit/README.md) | Reports for reviewing changes and checking data quality, such as unused requirement sets and the course import log. |

## Where to start
- **New to the application?** Read this page, then the [Configuration](Configuration/README.md) and [FieldsUsed](FieldsUsed/README.md) overviews.
or [Course](Course/README.md).
- **Building a plan of study?** Read [Requirement Set](RequirementSet/README.md).
- **Making a backup or bulk change?** Read [Transfer](Transfer/README.md).
- **Checking for problems?** Read [Audit](Audit/README.md).

## Conventions used
- **Bold text** is a button, field, or menu item as it appears on screen.
- Routes such as `/program/general` show the page address within the application.
- Most pages have a **Save** button. Changes are not stored until you select it. Security pages are the exception and save immediately.
- Fields and pages may differ between sources because features can be turned on or off, and fields hidden, in **Fields Used**.

## Getting help
Go to https://go.illinois.edu/course-toolkit-help to report a bug, request an enhancement, or ask a question. 

## Resources
The API for this is through a function app that is hosted at the WIGG Course API, and has search and ID options. Request information through an HTTP Get or Post and get JSON back.

To report a bug or request an enhancement, go to our GitHub repository and create an issue. This will log the issue so we can review it. Note that this application is meant for campus, which means we cannot customize it to a specific unit unless others will benefit from the change.

This application is meant to be used with the Illinois Toolkit, although there's no reason why you cannot use this application alone. The Toolkit Builder will have examples of how this can be integrated with the toolkit.

### Can I get access to this?
You have three options:
* Go to the Source Code listing and create a source code. Once you create a new source code, you can start adding information through the editing interface and start pulling data using your programming language of choice by referencing the source code.
* Go to the Source Code listing and create a source code. Once you have created a new source code, go to the Security tab and add an API key. This will allow you to programatically add and edit courses, programs, etc. from your site via API calls.
* Take the existing code and point it to your own AWS OpenSearch instance. Go to our GitHub repository to see the code.

## About This Project
This started in 2019 (with Bryan Jonker, Myranda Crist, and Elaine Robbins) as a way to organize the various programs that we have in the College of Education. The first incarnation worked but was skewed towards larger programs.

In 2022, Gies College of Business needed a way to organize their online portfolio. At the same time, IT Partners @ Education needed an easier way to maintain our existing program list and make it more agile. At the same time as that, other areas on campus needed to organize their program and course information. This was built to meet those needs, with the emphasis on making it extensible to other areas of campus. Courtney Fleeger helped immensely with the user interface for this.

In 2026, we transferred the tool to WIGG and the University Website Management Center of Expertise group.