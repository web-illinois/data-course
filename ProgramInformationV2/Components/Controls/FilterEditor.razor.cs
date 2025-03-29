using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Components.Controls {

    public partial class FilterEditor {
        private TagSource? _value;

        [Parameter]
        public string FilterType { get; set; } = "";

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public EventCallback<TagSource> MoveDownCallback { get; set; }

        [Parameter]
        public EventCallback<TagSource> MoveUpCallback { get; set; }

        [Parameter]
        public EventCallback<TagSource> RemoveCallback { get; set; }

        [Parameter]
        public string Title { get; set; } = default!;

        [Parameter]
        public TagSource? Value {
            get => _value;
            set {
                if (_value == value)
                    return;

                _value = value;
                ValueChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<TagSource> ValueChanged { get; set; }

        public async Task MoveDown() => await MoveDownCallback.InvokeAsync(Value);

        public async Task MoveUp() => await MoveUpCallback.InvokeAsync(Value);

        public async Task Remove() => await RemoveCallback.InvokeAsync(Value);
    }
}