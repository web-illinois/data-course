using Blazored.TextEditor;
using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.FieldList;

namespace ProgramInformationV2.Components.Controls {

    public partial class RichTextEditor {
        private BlazoredTextEditor? _quillItem = default!;

        [Parameter]
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        public string Id => Title.Replace(" ", "_").ToLowerInvariant();

        public string InitialValue { get; set; } = "";

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Parameter]
        public EventCallback<string> OnInitializedCallback { get; set; }

        [Parameter]
        public string Title { get; set; } = "";

        public bool GetFieldItemActive() => FieldItems == null ? false : FieldItems.FirstOrDefault(f => f.Title == Title)?.ShowItem ?? true;

        public string GetFieldItemDescription() => FieldItems == null ? "" : FieldItems.FirstOrDefault(f => f.Title == Title)?.Description ?? "";

        public async Task<string> GetValue() => _quillItem == null ? "" : await _quillItem.GetHTML();
    }
}