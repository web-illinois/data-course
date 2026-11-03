# Troubleshooting and FAQ

[Back to Programs Knowledge Base](README.md)

**The Programs page says "Your area does not use programs."**
Programs are off for your source. Ask a source owner to enable them in **Configuration > Fields Used > Programs**.

**I can't find my program in the list.**
- Run a search; the list may not load automatically.
- Clear the **Department** filter.
- If you are a restricted user, you only see programs assigned to you. Ask a source owner to assign it on the program's [Security](13-security.md) page.

**The Create Program button is disabled.**
Restricted users cannot create programs. Ask a source owner for unrestricted access.

**My changes weren't saved.**
Select **Save** before leaving a page. Look for "Program saved successfully." Only the Security page saves automatically.

**I can't edit the program's filters.**
Your source uses credentials, so the page is a read-only **Filter List**. Edit filters on each credential.

**It says no filters are available.**
Ask a source administrator to add filters in **Configuration > Manage Filters**.

**A field is missing from a page.**
It has been hidden for your source in **Configuration > Fields Used > Programs**.

**The Filters, Related Links, or Notes page says the feature is not used.**
Enable the feature in **Fields Used > Programs**.

**I don't see Copy or Transfer buttons.**
They appear only in a test environment (source code ending in `!`).

**My test changes disappeared.**
**Copy Program from Production** overwrites the test program and its credentials. Check [Audits and Changes](14-audits-and-changes.md).

**I deleted a program by mistake.**
Deletion cannot be undone. Restore it from a JSON backup, or contact IT Partners @ Education.

**The public page doesn't show my changes.**
Confirm you saved, then use **View Program**. Also check **Status on Site** in [Technical Information](12-technical-information.md).

**Who do I contact for more help?**
IT Partners @ Education.

