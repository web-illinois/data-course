# Save Information to JSON

[Back to Transfer Knowledge Base](README.md)

**Where:** Transfer sidebar > **Save Information to JSON** (`/transfer/savejson`)

Downloads your source's data as a JSON file. Use it to back up data or to export data for editing.

## Steps
1. Open **Save Information to JSON**.
2. In **File type**, choose one:
   - **Programs** (includes credentials)
   - **Courses** (includes sections)
   - **Requirement Sets**
   - **Configuration**
3. Select **Save JSON File**.
4. Wait for the messages: "JSON file being prepared -- this may take a while." then "JSON file downloaded successfully: *filename*".
5. Find the file in your browser's **Downloads** folder.

If you do not choose a file type, the message "Please choose a file type to download" appears.

## File names
Files are named `source_date_filetype.json`, for example `abc_2026_03_15_programs.json`, where the source is your source code and the date is the day you saved.

## Large files
If the data is very large (about 4 MB or more), it is split into several files named with a number at the end (for example `..._programs_1.json`, `..._programs_2.json`). The message says "split into parts because of size." Your browser may ask permission to download multiple files. Keep all the parts.

## Notes
- Saving does not change any data in the application.
- Saving takes a while for large sources. Do not close the page until the download message appears.

