using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Program {
    public partial class Quotes {
        private ImageControl _imageUrl = default!;
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public Search.Models.Program ProgramItem { get; set; } = new Search.Models.Program();

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();
            ProgramItem.SetFullUrl(await Layout.GetBaseUrl());
            if (_imageUrl != null) {
                _ = await _imageUrl.SaveFileToPermanent();
            }
            _ = await ProgramSetter.SetProgram(ProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.QuoteOption, ProgramItem);
            await Layout.AddMessage("Program saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            ProgramItem = await ProgramGetter.GetProgram(id);
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Credential) ? SidebarEnum.ProgramWithCredential : SidebarEnum.Program;

            Layout.SetSidebar(sidebar, ProgramItem.Title);
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new ProgramGroup(), FieldType.QuoteOption);
            await base.OnInitializedAsync();
        }
    }
}
