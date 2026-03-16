using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Controls {
    public partial class NoteListEditor {
        [Parameter]
        public List<Note> Notes { get; set; } = default!;
        [Parameter]
        public List<string> NoteTemplateTitles { get; set; } = default!;
        [Parameter]
        public EventCallback SetDirty { get; set; }


        public string NoteTemplateTitle { get; set; } = default!;
        public string NewNoteTitle { get; set; } = "";
        public string NewNoteDescription { get; set; } = "";
        public string NewNoteLinkName { get; set; } = "";
        public string NewNoteLinkUrl { get; set; } = "";

        public async Task Add() {
            if (Notes.Select(n => n.Title).Contains(NewNoteTitle)) {
                Notes.First(n => n.Title == NewNoteTitle).Description = NewNoteDescription;
                Notes.First(n => n.Title == NewNoteTitle).LinkText = NewNoteLinkName;
                Notes.First(n => n.Title == NewNoteTitle).LinkUrl = NewNoteLinkUrl;
            } else {
                Notes.Add(new Note {
                    Title = NewNoteTitle,
                    Description = NewNoteDescription,
                    LinkText = NewNoteLinkName,
                    LinkUrl = NewNoteLinkUrl,
                });
            }
            await SetDirty.InvokeAsync();
            Clear();
        }
        public async Task ChangeNoteTemplateTitle() {
            NewNoteTitle = NoteTemplateTitle;
            NoteTemplateTitle = "";
            await SetDirty.InvokeAsync();
        }

        protected async Task MoveDown(Note item) {
            Notes.MoveItemDown(item);
            await SetDirty.InvokeAsync();
        }

        protected async Task MoveUp(Note item) {
            Notes.MoveItemUp(item);
            await SetDirty.InvokeAsync();
        }

        protected async Task Remove(Note item) {
            Notes.Remove(item);
            await SetDirty.InvokeAsync();
        }

        protected async Task SelectItem(Note item) {
            NewNoteTitle = item.Title;
            NewNoteDescription = item.Description;
            NewNoteLinkName = item.LinkText;
            NewNoteLinkUrl = item.LinkUrl;
        }

        private void Clear() {
            NewNoteDescription = "";
            NewNoteLinkName = "";
            NewNoteLinkUrl = "";
            NewNoteTitle = "";
        }
    }
}
