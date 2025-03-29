using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Program {

    public partial class Overview {
        private IEnumerable<FieldItem> _fieldItems = default!;

        private RichTextEditor _rteDescription = default!;

        private RichTextEditor _rteWhoShouldApply = default!;

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

        public async Task Save() {
            Layout.RemoveDirty();

            if (_rteDescription != null) {
                ProgramItem.Description = await _rteDescription.GetValue();
            }
            if (_rteWhoShouldApply != null) {
                ProgramItem.WhoShouldApply = await _rteWhoShouldApply.GetValue();
            }
            _ = await ProgramSetter.SetProgram(ProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.Overview, ProgramItem);
            await Layout.AddMessage("Program saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            ProgramItem = await ProgramGetter.GetProgram(id);
            _rteDescription.InitialValue = ProgramItem.Description;
            _rteWhoShouldApply.InitialValue = ProgramItem.WhoShouldApply;
            await Layout.SetSidebar(SidebarEnum.Program, ProgramItem.Title);
            _fieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new ProgramGroup(), FieldType.Overview);
            await base.OnInitializedAsync();
        }
    }
}