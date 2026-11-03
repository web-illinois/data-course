# Security

[Back to Configuration Knowledge Base](README.md)

**Where:** Configuration > **Security** (`/configuration/security`)

Controls who can use **this source** and generates an API key for it. Access in one source does not give access to other sources.

> This page is not available to restricted users. Contact the source owner for unrestricted access.

## Existing Names
Each user is listed by email, with buttons:
| Button | Effect |
|--------|--------|
| **Deactivate User** / **Activate User** | Turns the user's access off or back on. |
| **Restrict User Access** / **Allow User To Access Everything** | Restricted users can only edit programs and credentials assigned to them (see each program's Security page). |
| **Remove User** | Removes the user from the source. |

## Adding a New Name
1. Enter the **New Net ID**. The `@illinois.edu` part is optional.
2. Select **Add Net ID**.

## API Updates
An **API key** lets a program add, update, and delete items in this source.

- If a key is active, the page shows when it was last changed.
- **Create a New API key**: generates a key and shows it once. **Record it immediately; you cannot see it again after leaving the page.**
- **Invalidate the API key**: deactivates the key.
- Creating a new key does not disable an old valid key. If a key may be compromised, **invalidate first**, then create a new one.

Changes made through the API are not listed in item audit pages.

