using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Credential {

    public partial class Filters {
        public Search.Models.Credential CredentialItem { get; set; } = default!;
        public IEnumerable<TagSource>? DepartmentTags => FilterTags?.Where(f => f.Key == TagType.Department).SelectMany(x => x);
        public IEnumerable<IGrouping<TagType, TagSource>> FilterTags { get; set; } = [];

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public bool NoFiltersAvailable => FilterTags == null || FilterTags.Count() == 0;
        public IEnumerable<TagSource>? SkillTags => FilterTags?.Where(f => f.Key == TagType.Skill).SelectMany(x => x);
        public IEnumerable<TagSource>? Tags => FilterTags?.Where(f => f.Key == TagType.Tag).SelectMany(x => x);
        public bool UsePrograms { get; set; }

        [Inject]
        protected CredentialGetter CredentialGetter { get; set; } = default!;

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task BackToProgram() {
            await Layout.SetCacheId(CredentialItem?.ProgramId ?? "");
            NavigationManager.NavigateTo("/program/credentiallist", true);
        }

        public async Task Save() {
            CredentialItem.DepartmentList = DepartmentTags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? [];
            CredentialItem.SkillList = SkillTags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? [];
            CredentialItem.TagList = Tags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? [];
            Layout.RemoveDirty();
            _ = await ProgramSetter.SetCredential(CredentialItem);
            await Layout.Log(CategoryType.Credential, FieldType.Filters, CredentialItem);
            await Layout.AddMessage("Credential saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            FilterTags = await FilterHelper.GetAllFilters(sourceCode);
            var id = await Layout.GetCachedId();
            UsePrograms = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Program);
            CredentialItem = await CredentialGetter.GetCredential(id);
            Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.Title);
            foreach (var tag in FilterTags.SelectMany(x => x)) {
                if (CredentialItem.DepartmentList.Contains(tag.Title) && tag.TagType == TagType.Department) {
                    tag.EnabledBySource = true;
                }
                if (CredentialItem.TagList.Contains(tag.Title) && tag.TagType == TagType.Tag) {
                    tag.EnabledBySource = true;
                }
                if (CredentialItem.SkillList.Contains(tag.Title) && tag.TagType == TagType.Skill) {
                    tag.EnabledBySource = true;
                }
            }
        }
    }
}