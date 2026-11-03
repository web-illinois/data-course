# Common Tasks

[Back to Transfer Knowledge Base](README.md)

## Back up before making many changes
1. Go to **Save Information to JSON**.
2. Save **Programs**, **Courses**, and **Requirement Sets** (and **Configuration** to keep a copy of your filters; configuration can only be loaded back into a blank source).
3. Store the files somewhere safe.
4. If something goes wrong, restore with **Load JSON Information to the Server** using the same file types. Items with the same IDs are overwritten with the backed-up versions.

> Items created after the backup remain. Loading a backup does not delete them.

## Make many changes with a text editor
1. Save the file type you want to change.
2. Open the `.json` file in a text editor (for example, Visual Studio Code or Notepad).
3. Edit the values. Keep the structure, quotation marks, commas, and brackets intact. Do **not** change IDs unless you mean to create a new item.
4. Save the file.
5. Load it back with **Load JSON Information to the Server**.

Tip: Save a backup before loading edits.

## Copy data from one source to another
1. In the original source, save the file types you need.
2. Open each file in a text editor.
3. Change every ID and source value so they begin with the **new** source code.
4. Switch to the new source.
5. Load each file with the matching file type.

The new source must be **blank** to load **Configuration**. If it already has any data, configuration cannot be loaded and the filters must be created manually.

Load in this order so connected items resolve properly:
1. **Configuration** (filters, blank source only)
2. **Courses**
3. **Requirement Sets**
4. **Programs** (including credentials)

## Restore from a backup
Use **Load JSON Information to the Server** with the saved file and matching file type. Existing items with the same IDs are overwritten.

