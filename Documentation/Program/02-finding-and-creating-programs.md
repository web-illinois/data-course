# Finding, Creating, and Opening a Program

[Back to Programs Knowledge Base](README.md)

**Where:** **Programs & Credentials > Programs** (`/programs`)

## Opening an existing program
1. Select **Programs** from the sidebar.
2. If your source is set to search on load, the list fills automatically. Otherwise, enter a search term and search.
3. Optionally narrow results with the **Department** filter.
4. Select a program and choose to edit it. The program's **General Information** page opens.

> If your account is restricted, the list shows only the programs you have been assigned.

## Creating a program
1. Under **Create a new program**, select **Create Program**.
2. The **General Information** page opens with blank fields.
3. Enter a **Title** and the other details, then select **Save**.
4. Add at least one credential from [Credential List](09-credential-list.md). A valid program must have one or more credentials.

The **Create Program** button is disabled if you are a restricted user.

## Copying a program from production (test environment only)
If your source is a test environment, a **Copy Program From Production** section appears.

1. Enter the **ID From Production Site**. Get it from the production program's [Technical Information](12-technical-information.md) page (Information to Copy > Id).
2. Select **Copy Program from Production**.

This overwrites the test version of that program and its credentials. If no matching production program is found, the message "No corresponding program found in production." appears. See [Technical Information](12-technical-information.md) for details.

## When the page says programs are unavailable
"Your area does not use programs" means programs are turned off for your source. A source owner can turn them on under **Configuration > Fields Used > Programs**.

