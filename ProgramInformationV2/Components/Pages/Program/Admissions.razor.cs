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
    public partial class Admissions {
        private IEnumerable<FieldItem> _fieldItems = default!;
        private RichTextEditor _rteAdmissions = default!;
        private RichTextEditor _rteTransferStudent = default!;
        private RichTextEditor _rteProgramTracks = default!;

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

            if (_rteAdmissions != null) {
                ProgramItem.AdmissionRequirements = await _rteAdmissions.GetValue();
            }
            if (_rteTransferStudent != null) {
                ProgramItem.TransferStudentDetails = await _rteTransferStudent.GetValue();
            }
            if (_rteProgramTracks != null) {
                ProgramItem.ProgramTracks = await _rteProgramTracks.GetValue();
            }
            _ = await ProgramSetter.SetProgram(ProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.Admissions, ProgramItem);
            await Layout.AddMessage("Program saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            ProgramItem = await ProgramGetter.GetProgram(id);
            _rteAdmissions.InitialValue = ProgramItem.AdmissionRequirements;
            _rteTransferStudent.InitialValue = ProgramItem.TransferStudentDetails;
            _rteProgramTracks.InitialValue = ProgramItem.ProgramTracks;
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Credential) ? SidebarEnum.ProgramWithCredential : SidebarEnum.Program;
            Layout.SetSidebar(sidebar, ProgramItem.Title);
            _fieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new ProgramGroup(), FieldType.Admissions);
            await base.OnInitializedAsync();
        }
    }
}
