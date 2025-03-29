using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Configuration {

    public partial class SaveJson {
        public string FileType { get; set; } = "";

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected JsonHelper JsonHelper { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public async Task Save() {
            if (string.IsNullOrWhiteSpace(FileType)) {
                await Layout.AddMessage("Please choose a file type to download");
            } else {
                var source = await Layout.CheckSource();
                await Layout.AddMessage("JSON file being prepared -- this may take a while.");
                Enum.TryParse(FileType, out UrlTypes urlType);

                var jsonText = await JsonHelper.GetJson(source, urlType);
                var fileStream = new MemoryStream(Encoding.ASCII.GetBytes(jsonText));
                using var streamRef = new DotNetStreamReference(fileStream);
                await JsRuntime.InvokeVoidAsync("downloadFileFromStream", $"{source}_{DateTime.Now.ToString("yyyyMMddhhmmss")}_{FileType.ToLowerInvariant()}.json", streamRef);
                await Layout.AddMessage("JSON file downloaded successfully.");
            }
        }

        protected override async Task OnInitializedAsync() {
            await Layout.CheckSource();
            await Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
            base.OnInitialized();
        }
    }
}