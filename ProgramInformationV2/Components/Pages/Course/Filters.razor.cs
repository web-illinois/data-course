using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class Filters {
        public ProgramInformationV2.Search.Models.Course CourseItem { get; set; } = default!;
        public IEnumerable<TagSource>? DepartmentTags => FilterTags?.Where(f => f.Key == TagType.Department).SelectMany(x => x);
        public IEnumerable<IGrouping<TagType, TagSource>> FilterTags { get; set; } = [];

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public bool NoFiltersAvailable => FilterTags == null || FilterTags.Count() == 0;
        public string QuickLinkUrl { get; set; } = "";
        public IEnumerable<TagSource>? SkillTags => FilterTags?.Where(f => f.Key == TagType.Skill).SelectMany(x => x);
        public IEnumerable<TagSource>? Lengths => FilterTags?.Where(f => f.Key == TagType.Length).SelectMany(x => x);
        public IEnumerable<TagSource>? Tags => FilterTags?.Where(f => f.Key == TagType.Tag).SelectMany(x => x);

        public string Instructions { get; set; } = default!;
        public bool UseItem { get; set; }

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;
        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task Save() {
            CourseItem.DepartmentList = DepartmentTags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? [];
            CourseItem.SkillList = SkillTags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? [];
            CourseItem.TagList = Tags?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? [];
            CourseItem.LengthList = Lengths?.Where(t => t.EnabledBySource).Select(t => t.Title).ToList() ?? [];
            Layout.RemoveDirty();

            await Layout.Log(CategoryType.Course, FieldType.Filters, CourseItem);
            _ = await CourseSetter.SetCourse(CourseItem);

            await Layout.AddMessage("Course saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            FilterTags = await FilterHelper.GetAllFilters(sourceCode);
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CourseItem = await CourseGetter.GetCourse(id);
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Section) ? SidebarEnum.CourseWithSection : SidebarEnum.Course;
            Layout.SetSidebar(sidebar, CourseItem.Title);
            QuickLinkUrl = await Layout.GetCachedQuickLink();
            foreach (var tag in FilterTags.SelectMany(x => x)) {
                if (CourseItem.DepartmentList != null && CourseItem.DepartmentList.Contains(tag.Title) && tag.TagType == TagType.Department) {
                    tag.EnabledBySource = true;
                }
                if (CourseItem.TagList != null && CourseItem.TagList.Contains(tag.Title) && tag.TagType == TagType.Tag) {
                    tag.EnabledBySource = true;
                }
                if (CourseItem.SkillList != null && CourseItem.SkillList.Contains(tag.Title) && tag.TagType == TagType.Skill) {
                    tag.EnabledBySource = true;
                }
                if (CourseItem.LengthList != null && CourseItem.LengthList.Contains(tag.Title) && tag.TagType == TagType.Length) {
                    tag.EnabledBySource = true;
                }
            }
            var fieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CourseGroup(), FieldType.Filters);
            Instructions = fieldItems.FirstOrDefault()?.Description ?? "";
            UseItem = fieldItems.FirstOrDefault()?.ShowItem ?? true;
        }
    }
}