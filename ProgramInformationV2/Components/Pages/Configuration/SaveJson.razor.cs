using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Helpers;
using ProgramInformationV2.Search.Models;
using System.Text;

namespace ProgramInformationV2.Components.Pages.Configuration {

    public partial class SaveJson {
        public string FileType { get; set; } = "";

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        [Inject]
        protected JsonHelper JsonHelper { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        public async Task Save() {
            if (string.IsNullOrWhiteSpace(FileType)) {
                await Layout.AddMessage("Please choose a file type to download");
            } else {
                string source = await Layout.CheckSource();
                await Layout.AddMessage("JSON file being prepared -- this may take a while.");
                if (FileType == "Configuration") {
                    string jsonTextFull = await FilterHelper.GetFilterJsonForExport(source);
                    var fileStream = new MemoryStream(Encoding.ASCII.GetBytes(jsonTextFull));
                    using var streamRef = new DotNetStreamReference(fileStream);
                    string filename = $"{source}_{DateTime.Now.ToString("yyyy_MM_dd")}_{FileType.ToLowerInvariant()}.json";
                    await JsRuntime.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
                    await Layout.AddMessage("JSON file downloaded successfully: " + filename);
                } else {
                    Enum.TryParse(FileType, out UrlTypes urlType);
                    string jsonTextFull = await JsonHelper.GetJsonFull(source, urlType);
                    if (jsonTextFull.Length < 4000000) {
                        var fileStream = new MemoryStream(Encoding.ASCII.GetBytes(jsonTextFull));
                        using var streamRef = new DotNetStreamReference(fileStream);
                        string filename = $"{source}_{DateTime.Now.ToString("yyyy_MM_dd")}_{FileType.ToLowerInvariant()}.json";
                        await JsRuntime.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
                        await Layout.AddMessage("JSON file downloaded successfully: " + filename);
                    } else {
                        int i = 0;
                        bool continueLoop = true;
                        while (continueLoop) {
                            string jsonText = await JsonHelper.GetJson(source, urlType, i);
                            if (jsonText == "[]" || jsonText.Length < 20) {
                                continueLoop = false;
                            } else {
                                i++;
                                var fileStream = new MemoryStream(Encoding.ASCII.GetBytes(jsonText));
                                using var streamRef = new DotNetStreamReference(fileStream);
                                await JsRuntime.InvokeVoidAsync("downloadFileFromStream", $"{source}_{DateTime.Now.ToString("yyyy_MM_dd")}_{FileType.ToLowerInvariant()}_{i}.json", streamRef);
                            }
                        }
                        await Layout.AddMessage("JSON file downloaded successfully -- split into parts because of size.");
                    }
                }
            }
        }

        protected override async Task OnInitializedAsync() {
            await Layout.CheckSource();
            Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
            base.OnInitialized();
        }
    }
}