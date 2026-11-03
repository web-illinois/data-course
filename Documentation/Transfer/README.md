# Transfer Knowledge Base

[Back to Documentation Home](../README.md)

End-user documentation for the **Transfer Information** section of the Program Information application (pages under `/transfer`).

The Transfer pages let you move data out of the application as **JSON files** and load it back in. Use them to:

- **Copy data from one source to another.**
- **Back up your data** before making a lot of changes.
- **Make many changes at once** using a text editor, then load the edited file back.

JSON (JavaScript Object Notation) is an open standard format that stores data as human-readable text.

| # | Article | What it covers |
|---|---------|----------------|
| 1 | [Save Information to JSON](01-save-json.md) | Download a backup or export |
| 2 | [Load JSON Information to the Server](02-load-json.md) | Upload an edited or backed-up file |
| 3 | [Common Tasks](03-common-tasks.md) | Backups, bulk edits, copying to another source |
| 4 | [Troubleshooting](04-troubleshooting.md) | Common problems |

## The sidebar
| Menu item | Route |
|-----------|-------|
| **Save Information to JSON** | `/transfer/savejson` |
| **Load JSON Information to the Server** | `/transfer/loadjson` |

## What can be transferred
| File type | Includes |
|-----------|----------|
| **Programs** | Program data **including credentials** |
| **Courses** | Course data **including sections** |
| **Requirement Sets** | Requirement sets and the courses they reference |
| **Configuration** | Filters. Can be **loaded only into a blank source** (see [Load JSON](02-load-json.md)) |

Programs, credentials, requirement sets, and courses are connected (a credential contains requirement sets, and a requirement set contains courses). When moving a full plan of study, save and load all of the related file types.

