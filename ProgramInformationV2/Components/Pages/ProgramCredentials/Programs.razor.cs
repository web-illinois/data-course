using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Data.Versioning;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.ProgramCredentials {

    public partial class Programs {
        private bool _isRestricted = false;
        private string[] _restrictedIds = [];
        private SearchGenericItem _searchGenericItem = default!;

        private string _sourceCode = "";
        private bool? _usePrograms;
        public bool IsTest { get; set; }
        public string CopyId { get; set; } = "";
        public List<string> DepartmentList { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public List<GenericItem> ProgramList { get; set; } = default!;

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SecurityHelper SecurityHelper { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        [Inject]
        protected VersionManager VersionManager { get; set; } = default!;


        protected async Task ChooseProgram() {
            if (!string.IsNullOrWhiteSpace(_searchGenericItem.SelectedItemId)) {
                await Layout.SetCacheId(_searchGenericItem.SelectedItemId);
                NavigationManager.NavigateTo("/program/general");
            }
        }

        protected async Task GetPrograms() {
            ProgramList = await ProgramGetter.GetAllProgramsBySource(_sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem, _searchGenericItem == null ? "" : _searchGenericItem.Department ?? "");
            if (_isRestricted) {
                ProgramList = [.. ProgramList.Where(pl => _restrictedIds.Contains(pl.Id))];
            }
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.ProgramCredential, "Programs and Credentials");
            _sourceCode = await Layout.CheckSource();
            IsTest = _sourceCode.EndsWith('!');
            _usePrograms = await SourceHelper.DoesSourceUseItem(_sourceCode, CategoryType.Program);
            (_isRestricted, _restrictedIds) = await SecurityHelper.GetRestrictions(await Layout.GetNetId(), _sourceCode);
            await GetPrograms();
            var (tagSources, _) = await FilterHelper.GetFilters(_sourceCode, TagType.Department);
            DepartmentList = [.. tagSources.Select(t => t.Title).OrderBy(t => t)];
            await base.OnInitializedAsync();
        }

        protected async Task CopyProgram() {
            var productionProgramItem = await ProgramGetter.GetProgram(CopyId);
            if (productionProgramItem == null || productionProgramItem.Id != CopyId) {
                await Layout.AddMessage("No corresponding program found in production.");
                return;
            }
            var newProgram = await VersionManager.CopyProgramFromProduction(productionProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.Technical, newProgram, "Copied from Production");
            await Layout.SetCacheId(newProgram.Id);
            NavigationManager.NavigateTo("/program/general");
        }

        protected async Task SetNewProgram() {
            await Layout.ClearCacheId();
            NavigationManager.NavigateTo("/program/general");
        }
    }
}