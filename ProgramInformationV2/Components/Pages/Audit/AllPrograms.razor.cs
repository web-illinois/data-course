using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.AuditHelpers;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Pages.Audit {

    public partial class AllPrograms {
        public List<GenericItem> ListOfItems = [];

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        public ProgramAudits ProgramAudits { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            Layout.SetSidebar(SidebarEnum.Audit, "Audit");
            ListOfItems = await ProgramAudits.GetAllProgramsAndCredentials(await Layout.CheckSource());
        }

        protected async Task SaveAsFile() {
            var source = await Layout.CheckSource();
            ListOfItems = await ProgramAudits.GetAllProgramsAndCredentials(source);
            var fileStream = new MemoryStream(Encoding.ASCII.GetBytes(string.Join("\r\n", ListOfItems.Select(a => a.Title + "\t" + a.Id + "\t" + a.IsActive))));
            using var streamRef = new DotNetStreamReference(fileStream);
            await JsRuntime.InvokeVoidAsync("downloadFileFromStream", $"{source}_{DateTime.Now.ToString("yyyy_MM_dd")}_programs.txt", streamRef);
            await Layout.AddMessage("File downloaded successfully");
        }
    }
}