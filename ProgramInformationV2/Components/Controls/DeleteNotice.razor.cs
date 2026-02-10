using Microsoft.AspNetCore.Components;

namespace ProgramInformationV2.Components.Controls {

    public partial class DeleteNotice {
        public bool DeleteCheckbox { get; set; } = false;

        [Parameter]
        public EventCallback<string> DeleteClicked { get; set; }

        [Parameter]
        public string ItemTitle { get; set; } = "";

        public string Label => ItemTitle.StartsWith("Source Data") ? "deletebox2" : "deletebox";

        public void Delete() {
            DeleteClicked.InvokeAsync();
        }
    }
}