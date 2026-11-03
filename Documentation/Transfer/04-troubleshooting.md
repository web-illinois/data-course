# Troubleshooting

[Back to Transfer Knowledge Base](README.md)

**"Please choose a file type to download."**
Pick a value in **File type** before selecting the button.

**The download didn't start.**
Large sources take time. Wait for the "downloaded successfully" message. Check your browser's download settings and pop-up/multiple download blocking.

**I received several files.**
The data was large and was split into parts. Keep all parts and load each one.

**"File size needs to be under 2048000."**
The file is too large to load. Split it into smaller files, each with a complete valid list of items, and load each separately.

**My items were added instead of replacing existing ones.**
IDs did not match existing items. IDs must start with your source code and must match exactly to overwrite.

**My items overwrote existing ones unexpectedly.**
Items with the same ID are always overwritten. Restore from a backup if needed.

**Loading from another source didn't work.**
Change the ID and source values in the file to the new source before loading.

**I can't load Configuration.**
Configuration can only be loaded into a blank source. If the source already has any data, the load is not allowed. Use a blank source, or create the filters manually on **Configuration > Filters**.

**Changes from a JSON load aren't in the audit.**
Audit pages only list manual edits, not JSON loads or API updates.

**The page says it is taking a while.**
Large loads and saves are slow. Wait for the result message before navigating away.

