using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Program {

    public partial class Filters {
        public IEnumerable<TagSource>? DepartmentTags => FilterTags?.Where(f => f.Key == TagType.Department).SelectMany(x => x);
        public IEnumerable<IGrouping<TagType, TagSource>> FilterTags { get; set; } = [];

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public bool NoFiltersAvailable => FilterTags == null || FilterTags.Count() == 0;
        public Search.Models.Program ProgramItem { get; set; } = new Search.Models.Program();

        public IEnumerable<TagSource>? SkillTags => FilterTags?.Where(f => f.Key == TagType.Skill).SelectMany(x => x);

        public IEnumerable<TagSource>? Tags => FilterTags?.Where(f => f.Key == TagType.Tag).SelectMany(x => x);

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        public async Task Save() {
            ProgramItem.DepartmentList = DepartmentTags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? new List<string>();
            ProgramItem.SkillList = SkillTags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? new List<string>();
            ProgramItem.TagList = Tags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? new List<string>();
            Layout.RemoveDirty();
            await Layout.AddMessage("Program saved successfully.");
            await Layout.Log(CategoryType.Program, FieldType.Filters, ProgramItem);
            _ = await ProgramSetter.SetProgram(ProgramItem);
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            FilterTags = await FilterHelper.GetAllFilters(sourceCode);
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            ProgramItem = await ProgramGetter.GetProgram(id);
            Layout.SetSidebar(SidebarEnum.Program, ProgramItem.Title);
            foreach (var tag in FilterTags.SelectMany(x => x)) {
                if (ProgramItem.DepartmentList.Contains(tag.Title) && tag.TagType == TagType.Department) {
                    tag.EnabledBySource = true;
                }
                if (ProgramItem.TagList.Contains(tag.Title) && tag.TagType == TagType.Tag) {
                    tag.EnabledBySource = true;
                }
                if (ProgramItem.SkillList.Contains(tag.Title) && tag.TagType == TagType.Skill) {
                    tag.EnabledBySource = true;
                }
            }
        }
    }
}