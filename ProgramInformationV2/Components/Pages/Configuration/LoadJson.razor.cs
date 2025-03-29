using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Configuration {

    public partial class LoadJson {
        private const int maxFileSize = 2048000;

        private string reader = "";
        public string FileType { get; set; } = "";

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected JsonHelper JsonHelper { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public async Task Load() {
            if (string.IsNullOrWhiteSpace(FileType)) {
                await Layout.AddMessage("Please choose a file type to download");
            } else {
                var source = await Layout.CheckSource();
                await Layout.AddMessage("JSON file being prepared -- this may take a while.");
                Enum.TryParse(FileType, out UrlTypes urlType);
                await Layout.AddMessage(await JsonHelper.LoadJson(source, urlType, reader));
            }
        }

        protected override async Task OnInitializedAsync() {
            await Layout.CheckSource();
            await Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
            base.OnInitialized();
        }

        private async Task LoadFile(InputFileChangeEventArgs e) {
            if (e.File.Size > maxFileSize) {
                await Layout.AddMessage("File size needs to be under " + maxFileSize);
            } else {
                await Layout.AddMessage("Starting to upload file.");
                reader = await new StreamReader(e.File.OpenReadStream(maxFileSize)).ReadToEndAsync();
                await Layout.AddMessage("Uploaded file.");
            }
        }
    }
}