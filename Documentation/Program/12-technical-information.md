# Technical Information

[Back to Programs Knowledge Base](README.md)

**Where:** Program sidebar > **Technical Information** (`/program/technical`)

## Status on Site
Controls whether the program is active (published) on your site. Set it, then select **Save**.

## Internal Search Text
Text that helps editors find this program when searching. It is for internal use and is not meant for display. Enter synonyms, old names, or abbreviations, then select **Save**.

## Information to Copy
Read-only values you can copy for integrations or support:
- **Id**: the program's unique ID. You need this to copy the program from production.
- **Edit Link**: a direct link to edit this program.

## Test environment tools
These sections appear **only if your source is a test environment** (the source code ends with `!`).

### Copy Program From Production
Refreshes the test program from production, discarding your test changes.

1. Select **Copy Program from Production**.
2. The program **and all credentials under it** are overwritten in the test environment.
3. If production has no matching program, you see "No corresponding program found in production."
4. On success, you can start editing immediately.

### Transfer Program To Production
Sends the test program to production.

1. Make sure the same requirement sets exist in both test and production.
2. Select **Transfer Program To Production**.
3. The production program **and all credentials under it** are overwritten.

> Both actions overwrite data. Neither can be undone.

## Delete
At the bottom of the page is a delete notice for the program. Deleting is automatic and cannot be undone. After deletion you are returned to the Programs page.

Consider saving a backup first (Transfer > **Save JSON File**).

