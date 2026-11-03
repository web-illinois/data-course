# Load JSON Information to the Server

[Back to Transfer Knowledge Base](README.md)

**Where:** Transfer sidebar > **Load JSON Information to the Server** (`/transfer/loadjson`)

Uploads a JSON file to your source. Use it to make bulk changes, restore a backup, or bring in data from another source.

> **Loading overwrites data.** If an item in the file has the same ID as an existing item, the existing item is **overwritten**. Items with new IDs are **added**. Save a backup first. See [Save Information to JSON](01-save-json.md).

## What you can load
- **Programs** (including credentials)
- **Courses** (including sections)
- **Requirement Sets**
- **Configuration**: loads the filters in the file.

> **Configuration can only be loaded into a blank source.** If the source already has any data, you cannot load configuration to it. Load configuration first, before creating or loading any programs, courses, or requirement sets.

## Steps
1. Open **Load JSON Information to the Server**.
2. In **File type**, choose the type that matches your file.
3. Choose the file with the file picker. Messages show "Starting to upload file." then "Uploaded file."
4. Select **Load JSON File**.
5. Wait. The message "JSON file being prepared -- this may take a while." appears, followed by the result.

## Rules and limits
- **Maximum file size:** the file must be under about 2 MB (2,048,000 bytes). Larger files are rejected with "File size needs to be under 2048000". Split large files into several smaller ones and load each.
- **IDs must start with the source.** Each item's ID must begin with your source code. If the data came from another source, you must **manually change the ID and source** in the file to match the source you are loading into.
- **Choose the right type.** Loading a file under the wrong type may fail or give unexpected results.
- If you do not choose a file type, you see "Please choose a file type to download."
- Loading takes a while. Wait for the result message.

## After loading
Check a few items on the Programs, Courses, or Requirement Sets pages to confirm the changes. Manual JSON loads are **not** listed in an item's Audit page.

