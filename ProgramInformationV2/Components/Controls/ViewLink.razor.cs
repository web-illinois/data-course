using Microsoft.AspNetCore.Components;

namespace ProgramInformationV2.Components.Controls {

    public partial class ViewLink {

        [Parameter]
        public string Title { get; set; } = "";

        [Parameter]
        public string Url { get; set; } = "";
    }
}