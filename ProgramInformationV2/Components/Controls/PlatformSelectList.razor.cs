using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Controls {
    public partial class PlatformSelectList {
        private PlatformTypes? _value;

        [Parameter]
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        public string Id => Title.Replace(" ", "_").ToLowerInvariant();

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Parameter]
        public string Title { get; set; } = "";

        [Parameter]
        public PlatformTypes? Value {
            get => _value;
            set {
                if (_value == value)
                    return;
                if (Layout != null)
                    Layout.SetDirty();
                _value = value;
                ValueChanged.InvokeAsync(value.HasValue ? value.Value : PlatformTypes.None);
            }
        }

        [Parameter]
        public EventCallback<PlatformTypes> ValueChanged { get; set; }

        public bool GetFieldItemActive() => FieldItems == null ? false : FieldItems.FirstOrDefault(f => f.Title == Title)?.ShowItem ?? true;

        public string GetFieldItemDescription() => FieldItems == null ? "" : FieldItems.FirstOrDefault(f => f.Title == Title)?.Description ?? "";

    }
}
